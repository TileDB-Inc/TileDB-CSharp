using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

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
    /// Creates an <see cref="Enumeration"/> with fixed-sized members.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration's members.</typeparam>
    /// <param name="ctx">The <see cref="Context"/> to use.</param>
    /// <param name="name">The name of the enumeration.</param>
    /// <param name="ordered">The value of <see cref="IsOrdered"/>.</param>
    /// <param name="values">A <see cref="ReadOnlySpan{T}"/> of the enumeration's values.</param>
    /// <param name="cellValNum">The number of values per member in the enumeration. Optional,
    /// defaults to 1. If specified, its value must divide the length of <paramref name="values"/>.</param>
    /// <param name="dataType">The data type of the enumeration. Defaults to the
    /// default data type of <typeparamref name="T"/>.</param>
    /// <exception cref="InvalidOperationException"><paramref name="dataType"/> does not
    /// match <typeparamref name="T"/>.</exception>
    public static Enumeration Create<T>(Context ctx, string name, bool ordered, ReadOnlySpan<T> values, uint cellValNum = 1, DataType? dataType = null) where T : struct
    {
        DataType dataTypeActual = dataType ??= EnumUtil.TypeToDataType(typeof(T));
        ErrorHandling.CheckDataType<T>(dataTypeActual);

        fixed (T* valuesPtr = &MemoryMarshal.GetReference(values))
        {
            EnumerationHandle handle = EnumerationHandle.Create(ctx, name, (tiledb_datatype_t)dataTypeActual, cellValNum, ordered, (byte*)valuesPtr, (ulong)values.Length * (ulong)sizeof(T), null, 0);
            return new Enumeration(ctx, handle);
        }
    }

    /// <summary>
    /// Creates an <see cref="Enumeration"/> with variable-sized members.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration's members.</typeparam>
    /// <param name="ctx">The <see cref="Context"/> to use.</param>
    /// <param name="name">The name of the enumeration.</param>
    /// <param name="ordered">The value of <see cref="IsOrdered"/>.</param>
    /// <param name="values">A <see cref="ReadOnlySpan{T}"/> of all values of each member of the enumeration, concatenated.</param>
    /// <param name="offsets">A <see cref="ReadOnlySpan{T}"/> of the offsets to the first byte of each member of the enumeration.</param>
    /// <param name="dataType">The data type of the enumeration. Defaults to the
    /// default data type of <typeparamref name="T"/>.</param>
    /// <exception cref="InvalidOperationException"><paramref name="dataType"/> does not
    /// match <typeparamref name="T"/>.</exception>
    public static Enumeration Create<T>(Context ctx, string name, bool ordered, ReadOnlySpan<T> values, ReadOnlySpan<ulong> offsets, DataType? dataType = null) where T : struct
    {
        DataType dataTypeActual = dataType ??= EnumUtil.TypeToDataType(typeof(T));
        ErrorHandling.CheckDataType<T>(dataTypeActual);

        EnumerationHandle handle;
        fixed (T* valuesPtr = &MemoryMarshal.GetReference(values))
        fixed (ulong* offsetsPtr = &MemoryMarshal.GetReference(offsets))
        {
            handle = EnumerationHandle.Create(ctx, name, (tiledb_datatype_t)dataTypeActual, VariableSized, ordered, (byte*)valuesPtr, (ulong)values.Length * (ulong)sizeof(T), offsetsPtr, (ulong)offsets.Length * sizeof(ulong));
        }
        return new Enumeration(ctx, handle);
    }

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
    /// Adds additional values to an <see cref="Enumeration"/> with fixed-sized members.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration's members.</typeparam>
    /// <param name="values">A <see cref="ReadOnlySpan{T}"/> of the enumeration's values.</param>
    /// <exception cref="InvalidOperationException">The enumeration's
    /// <see cref="DataType"/> does not match <typeparamref name="T"/>.</exception>
    public Enumeration Extend<T>(ReadOnlySpan<T> values) where T : struct
    {
        ErrorHandling.CheckDataType<T>(DataType);

        fixed (T* valuesPtr = &MemoryMarshal.GetReference(values))
        {
            EnumerationHandle handle = _handle.Extend(_ctx, (byte*)valuesPtr, (ulong)values.Length * (ulong)sizeof(T), null, 0);
            return new Enumeration(_ctx, handle);
        }
    }

    /// <summary>
    /// Extends an <see cref="Enumeration"/> with variable-sized members.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration's members.</typeparam>
    /// <param name="values">A <see cref="ReadOnlySpan{T}"/> of all values of each additional member of the enumeration, concatenated.</param>
    /// <param name="offsets">A <see cref="ReadOnlySpan{T}"/> of the offsets to the first byte of each member of the enumeration.</param>
    /// <exception cref="InvalidOperationException">The enumeration's
    /// <see cref="DataType"/> does not match <typeparamref name="T"/>.</exception>
    public Enumeration Extend<T>(ReadOnlySpan<T> values, ReadOnlySpan<ulong> offsets) where T : struct
    {
        ErrorHandling.CheckDataType<T>(DataType);

        EnumerationHandle handle;
        fixed (T* valuesPtr = &MemoryMarshal.GetReference(values))
        fixed (ulong* offsetsPtr = &MemoryMarshal.GetReference(offsets))
        {
            handle = _handle.Extend(_ctx, (byte*)valuesPtr, (ulong)values.Length * (ulong)sizeof(T), offsetsPtr, (ulong)offsets.Length * sizeof(ulong));
        }
        return new Enumeration(_ctx, handle);
    }

    /// <summary>
    /// Extends an <see cref="Enumeration"/> of strings.
    /// </summary>
    /// <param name="values">The strings to add to the enumeration.</param>
    /// <exception cref="InvalidOperationException">The enumeration has a non-string data
    /// type, or its members are fixed-size.</exception>
    public Enumeration Extend(IReadOnlyCollection<string> values)
    {
        if (ValuesPerMember != VariableSized)
        {
            throw new InvalidOperationException("Enumeration must have variable-sized members.");
        }
        DataType dataType = DataType;
        if (!EnumUtil.IsStringType(dataType))
        {
            throw new InvalidOperationException($"Cannot extend enumeration with non-string data type, got {dataType}.");
        }

        using MarshaledContiguousStringCollection marshaledStrings = new(values, dataType);
        var handle = _handle.Extend(_ctx, marshaledStrings.Data, marshaledStrings.DataCount, marshaledStrings.Offsets, marshaledStrings.OffsetsCount);
        return new Enumeration(_ctx, handle);
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
    /// <para>If this property has a value of <see cref="VariableSized"/>,
    /// each value has a different size.</para>
    /// <para>This method exposes the <c>tiledb_enumeration_get_cell_val_num</c> function of the TileDB Embedded C API.</para>
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

            ReadOnlySpan<ulong> offsets = new(offsetsPtr, checked((int)(offsetsSize / sizeof(ulong))));
            T[][] result = new T[offsets.Length][];
            for (int i = 0; i < offsets.Length; i++)
            {
                ulong nextOffset = i == offsets.Length - 1 ? dataSize : offsets[i + 1];
                result[i] = new ReadOnlySpan<T>((byte*)dataPtr + offsets[i], checked((int)((nextOffset - offsets[i]) / (ulong)sizeof(T)))).ToArray();
            }

            return result;
        }
    }

    /// <summary>
    /// Returns a byte array with the raw data of the <see cref="Enumeration"/>'s members.
    /// </summary>
    /// <seealso cref="GetValues"/>
    /// <seealso cref="GetRawOffsets"/>
    public byte[] GetRawData()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        void* data;
        ulong dataSize;
        _ctx.handle_error(Methods.tiledb_enumeration_get_data(ctxHandle, handle, &data, &dataSize));
        return new ReadOnlySpan<byte>(data, checked((int)dataSize)).ToArray();
    }

    /// <summary>
    /// Returns an array with the offsets to the first byte of each member of the <see cref="Enumeration"/>.
    /// </summary>
    /// <remarks>
    /// In enumerations with fixed-size members (i.e. <see cref="ValuesPerMember"/> is not equal
    /// to <see cref="VariableSized"/>, this method will return an empty array, as each member has the same size.
    /// </remarks>
    /// <seealso cref="GetValues"/>
    /// <seealso cref="GetRawData"/>
    public ulong[] GetRawOffsets()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        void* data;
        ulong dataSize;
        _ctx.handle_error(Methods.tiledb_enumeration_get_offsets(ctxHandle, handle, &data, &dataSize));
        return new ReadOnlySpan<ulong>(data, checked((int)(dataSize / sizeof(ulong)))).ToArray();
    }
}
