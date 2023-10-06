using System;
using System.Collections.Generic;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Represents a TileDB enumeration object.
    /// </summary>
    public sealed unsafe class Enumeration : IDisposable
    {
        /// <summary>
        /// This value indicates an enumeration with variable-sized members.
        /// It may be returned from <see cref="ValuesPerMember"/>.
        /// </summary>
        /// <seealso cref="Create{T}(Context, string, bool, ReadOnlySpan{T}, ReadOnlySpan{ulong}, DataType?)"/>
        public const uint VariableSized = Constants.VariableSizedImpl;

        private readonly Context _ctx;

        private readonly EnumerationHandle _handle;

        internal Enumeration(Context ctx, EnumerationHandle handle)
        {
            _ctx = ctx;
            _handle = handle;
        }

        internal EnumerationHandle Handle => _handle;

        /// <summary>
        /// Creates an <see cref="Enumeration"/> from a collection of strings.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> to use.</param>
        /// <param name="name">The name of the enumeration.</param>
        /// <param name="ordered">Whether the enumeration should be considered as ordered.
        /// If false, prevents inequality operators in <see cref="QueryCondition"/>s from
        /// being used with this enumeration.</param>
        /// <param name="values">The collection of values for the enumeration.</param>
        /// <param name="dataType">The data type of the enumeration. Must be a string type.
        /// Optional, defaults to <see cref="DataType.StringUtf8"/>.</param>
        public static Enumeration Create(Context ctx, string name, bool ordered, IReadOnlyCollection<string> values, DataType dataType = DataType.StringUtf8)
        {
            using MarshaledContiguousStringCollection marshaledStrings = new(values, dataType);
            var handle = EnumerationHandle.Create(ctx, name, (tiledb_datatype_t)dataType, VariableSized,
                ordered, marshaledStrings.Data, marshaledStrings.DataCount, marshaledStrings.Offsets, marshaledStrings.OffsetsCount);
            return new Enumeration(ctx, handle);
        }

        /// <summary>
        /// Disposes the <see cref="Enumeration"/>.
        /// </summary>
        public void Dispose()
        {
            _handle.Dispose();
        }

        /// <summary>
        /// Returns the number of values for each member of the <see cref="Enumeration"/>.
        /// </summary>
        /// <remarks>
        /// If this property has a value of <see cref="VariableSized"/>, each value has a different size.
        /// </remarks>
        public uint ValuesPerMember
        {
            get
            {
                uint result;
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_enumeration_get_cell_val_num(ctxHandle, handle, &result));
                return result;
            }
        }

        /// <summary>
        /// The <see cref="DataType"/> of the <see cref="Enumeration"/>.
        /// </summary>
        public DataType DataType
        {
            get
            {
                tiledb_datatype_t result;
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_enumeration_get_type(ctxHandle, handle, &result));
                return (DataType)result;
            }
        }

        /// <summary>
        /// Whether the <see cref="Enumeration"/> should be considered as ordered.
        /// </summary>
        /// <remarks>
        /// If <see langword="false"/>, prevents inequality operators in
        /// <see cref="QueryCondition"/>s from being used with this enumeration.
        /// </remarks>
        public bool IsOrdered
        {
            get
            {
                int result;
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_enumeration_get_ordered(ctxHandle, handle, &result));
                return result != 0;
            }
        }

        /// <summary>
        /// Returns the name of the <see cref="Enumeration"/>.
        /// </summary>
        public string GetName()
        {
            using StringHandleHolder nameHolder = new();
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_enumeration_get_name(ctxHandle, handle, &nameHolder._handle));
            return nameHolder.ToString();
        }

        /// <summary>
        /// Returns the values of the <see cref="Enumeration"/>.
        /// </summary>
        /// <returns>
        /// An array whose type depends on the enumeration's <see cref="DataType"/>
        /// and <see cref="ValuesPerMember"/>:
        /// <list type="bullet">
        /// <item>If <see cref="DataType"/> is a string datatype, the
        /// method will return an array of <see cref="string"/>.</item>
        /// <item>If the enumeration's values have a fixed size (i.e. <see cref="ValuesPerMember"/>
        /// is not equal to <see cref="VariableSized"/>), the method will return an array of
        /// values of the underlying data type.</item>
        /// <item>If the enumeration's values have a variable size (i.e. <see cref="ValuesPerMember"/>
        /// is equal to <see cref="VariableSized"/>), the method will return an array of arrays of
        /// values of the underlying data type.</item>
        /// </list>
        /// </returns>
        public System.Array GetValues()
        {
            // Keep the handles acquired for the entire method.
            // This prevents another thread freeing the data and offsets buffers.
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();

            void* dataPtr;
            ulong* offsetsPtr;
            ulong dataSize, offsetsSize;
            _ctx.handle_error(Methods.tiledb_enumeration_get_data(ctxHandle, handle, &dataPtr, &dataSize));
            _ctx.handle_error(Methods.tiledb_enumeration_get_offsets(ctxHandle, handle, (void**)&offsetsPtr, &offsetsSize));

            DataType datatype;
            _ctx.handle_error(Methods.tiledb_enumeration_get_type(ctxHandle, handle, (tiledb_datatype_t*)&datatype));

            if (EnumUtil.IsStringType(datatype))
            {
                ReadOnlySpan<ulong> offsets = new(offsetsPtr, checked((int)(offsetsSize / sizeof(ulong))));
                string[] result = new string[offsets.Length];

                for (int i = 0; i < offsets.Length; i++)
                {
                    ulong nextOffset = i == offsets.Length - 1 ? dataSize : offsets[i + 1];
                    ReadOnlySpan<byte> bytes = new((byte*)dataPtr + offsets[i], checked((int)(nextOffset - offsets[i])));
                    result[i] = MarshaledStringOut.GetString(bytes, datatype);
                }

                return result;
            }

            Type type = EnumUtil.DataTypeToType(datatype);
            if (type == typeof(sbyte))
            {
                return GetValues<sbyte>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(byte))
            {
                return GetValues<byte>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(short))
            {
                return GetValues<short>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(ushort))
            {
                return GetValues<ushort>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(int))
            {
                return GetValues<int>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(uint))
            {
                return GetValues<uint>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(long))
            {
                return GetValues<long>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(ulong))
            {
                return GetValues<ulong>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(float))
            {
                return GetValues<float>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            if (type == typeof(double))
            {
                return GetValues<double>(dataPtr, dataSize, offsetsPtr, offsetsSize);
            }
            throw new NotSupportedException($"Unsupported enumeration type: {type}. Please open a bug report.");

            System.Array GetValues<T>(void* dataPtr, ulong dataSize, ulong* offsetsPtr, ulong offsetsSize)
            {
                uint cellValNum;
                _ctx.handle_error(Methods.tiledb_enumeration_get_cell_val_num(ctxHandle, handle, &cellValNum));
                if (cellValNum != VariableSized)
                {
                    return new ReadOnlySpan<T>(dataPtr, checked((int)(dataSize / (ulong)sizeof(T)))).ToArray();
                }

                T[][] result;
                if (cellValNum != VariableSized)
                {
                    result = new T[checked((int)offsetsSize)][];
                    int i;
                    ulong j;
                    for (i = 0, j = 0; i < result.Length; i++, j += cellValNum)
                    {
                        result[i] = new ReadOnlySpan<T>((T*)dataPtr + j, checked((int)cellValNum)).ToArray();
                    }

                    return result;
                }

                ReadOnlySpan<ulong> offsets = new(offsetsPtr, checked((int)(offsetsSize / sizeof(ulong))));
                result = new T[offsets.Length][];
                for (int i = 0; i < offsets.Length; i++)
                {
                    ulong nextOffset = i == offsets.Length - 1 ? dataSize : offsets[i + 1];
                    result[i] = new ReadOnlySpan<T>((byte*)dataPtr + offsets[i], checked((int)((nextOffset - offsets[i]) / (ulong)sizeof(T)))).ToArray();
                }

                return result;
            }
        }
    }
}
