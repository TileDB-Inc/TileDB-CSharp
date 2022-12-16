using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Subarray : IDisposable
    {
        private readonly Array _array;
        private readonly Context _ctx;
        private readonly SubarrayHandle _handle;

        public Subarray(Array array)
        {
            _array = array;
            _ctx = array.Context();
            _handle = SubarrayHandle.Create(_ctx, array.Handle);
        }

        public Subarray(Array array, bool coalesceRanges) : this(array)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_subarray_set_coalesce_ranges(ctxHandle, handle, coalesceRanges ? 1 : 0));
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        public void SetConfig(Config config)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var configHandle = config.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_subarray_set_config(ctxHandle, handle, configHandle));
        }

        private (DataType DataType, uint NumberOfDimensions) GetDomainInfo()
        {
            using var schema = _array.Schema();
            using var domain = schema.Domain();
            return (domain.Type(), domain.NDim());
        }

        /// <summary>
        /// Sets a subarray, defined in the order dimensions were added.
        /// </summary>
        /// <typeparam name="T">The type of the dimensions.</typeparam>
        /// <param name="data">An array of <c>[start,stop]</c> values per dimension.</param>
        /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="T"/> does not match the dimension's type.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> does not contain exactly twice as many items as the number of dimensions.</exception>
        public void SetSubarray<T>(params T[] data) where T : struct
        {
            ErrorHandling.ThrowIfNull(data);
            SetSubarray<T>(data.AsSpan());
        }

        public void SetSubarray<T>(ReadOnlySpan<T> data) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            var dataType = EnumUtil.TypeToDataType(typeof(T));
            (var domainType, var nDim) = GetDomainInfo();

            if (dataType != domainType)
            {
                ThrowHelpers.ThrowTypeMismatch(dataType);
            }
            if (data.Length != nDim * 2)
            {
                ThrowHelpers.ThrowSubarrayLengthMismatch(nameof(data));
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            fixed (T* dataPtr = &MemoryMarshal.GetReference(data))
            {
                _ctx.handle_error(Methods.tiledb_subarray_set_subarray(ctxHandle, handle, dataPtr));
            }
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension index, in the form (start, end).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="index">Index of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        public void AddRange<T>(uint index, T start, T end) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            AddRange(index, &start, &end, null);
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension name, specified by its name, in the form (start, end).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="name">Name of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        public void AddRange<T>(string name, T start, T end) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            AddRange(name, &start, &end, null);
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension index, in the form (start, end, stride).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="index">Index of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        /// <param name="stride">Stride between dimension range values</param>
        public void AddRange<T>(uint index, T start, T end, T stride) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            AddRange(index, &start, &end, &stride);
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension name, specified by its name, in the form(start, end, stride).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="name">Name of dimension</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        /// <param name="stride">Stride between dimension range values</param>
        public void AddRange<T>(string name, T start, T end, T stride) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            AddRange(name, &start, &end, &stride);
        }

        /// <summary>
        /// Adds a 1D string range along a subarray dimension index, in the form (start, end).
        /// </summary>
        /// <param name="index">Index of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        public void AddRange(uint index, string start, string end)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_start = new MarshaledString(start);
            using var ms_end = new MarshaledString(end);
            _ctx.handle_error(Methods.tiledb_subarray_add_range_var(ctxHandle, handle, index, ms_start, (ulong)ms_start.Length, ms_end, (ulong)ms_end.Length));
        }

        /// <summary>
        /// Adds a 1D string range along a subarray dimension name, in the form (start, end).
        /// </summary>
        /// <param name="name">Name of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        public void AddRange(string name, string start, string end)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            using var ms_start = new MarshaledString(start);
            using var ms_end = new MarshaledString(end);
            _ctx.handle_error(Methods.tiledb_subarray_add_range_var_by_name(ctxHandle, handle, ms_name, ms_start, (ulong)ms_start.Length, ms_end, (ulong)ms_end.Length));
        }

        private void AddRange(uint index, void* start, void* end, void* stride)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_subarray_add_range(ctxHandle, handle, index, start, end, stride));
        }

        private void AddRange(string name, void* start, void* end, void* stride)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_subarray_add_range_by_name(ctxHandle, handle, ms_name, start, end, stride));
        }

        /// <summary>
        /// Retrieves the number of ranges for a given dimension index.
        /// </summary>
        /// <param name="dimensionIndex"></param>
        /// <returns></returns>
        public ulong GetRangeCount(uint dimensionIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong result;
            _ctx.handle_error(Methods.tiledb_subarray_get_range_num(ctxHandle, handle, dimensionIndex, &result));
            return result;
        }

        /// <summary>
        /// Retrieves the number of ranges for a given dimension name.
        /// </summary>
        /// <param name="dimensionName"></param>
        /// <returns></returns>
        public ulong GetRangeCount(string dimensionName)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_dimensionName = new MarshaledString(dimensionName);
            ulong result;
            _ctx.handle_error(Methods.tiledb_subarray_get_range_num_from_name(ctxHandle, handle, ms_dimensionName, &result));
            return result;
        }

        public (T Start, T End, T Stride) GetRange<T>(uint dimensionIndex, uint rangeIndex) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            void* startPtr, endPtr, stridePtr;
            _ctx.handle_error(Methods.tiledb_subarray_get_range(ctxHandle, handle, dimensionIndex, rangeIndex, &startPtr, &endPtr, &stridePtr));
            return (Unsafe.ReadUnaligned<T>(startPtr), Unsafe.ReadUnaligned<T>(endPtr), Unsafe.ReadUnaligned<T>(stridePtr));
        }

        public (T Start, T End, T Stride) GetRange<T>(string dimensionName, uint rangeIndex) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_dimensionName = new MarshaledString(dimensionName);
            void* startPtr, endPtr, stridePtr;
            _ctx.handle_error(Methods.tiledb_subarray_get_range_from_name(ctxHandle, handle, ms_dimensionName, rangeIndex, &startPtr, &endPtr, &stridePtr));
            return (Unsafe.ReadUnaligned<T>(startPtr), Unsafe.ReadUnaligned<T>(endPtr), Unsafe.ReadUnaligned<T>(stridePtr));
        }

        public (string Start, string End) GetStringRange(uint dimensionIndex, uint rangeIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong start_size;
            ulong end_size;
            _ctx.handle_error(Methods.tiledb_subarray_get_range_var_size(ctxHandle, handle, dimensionIndex, rangeIndex, &start_size, &end_size));

            using var startBuffer = new ScratchBuffer<byte>(checked((int)start_size), stackalloc byte[128]);
            using var endBuffer = new ScratchBuffer<byte>(checked((int)end_size), stackalloc byte[128]);
            fixed (byte* startPtr = startBuffer, endPtr = endBuffer)
            {
                _ctx.handle_error(Methods.tiledb_subarray_get_range_var(ctxHandle, handle, dimensionIndex, rangeIndex, startPtr, endPtr));
            }

            var startStr = MarshaledStringOut.GetString(startBuffer.Span);
            var endStr = MarshaledStringOut.GetString(endBuffer.Span);
            return (startStr, endStr);
        }

        public (string Start, string End) GetStringRange(string dimensionName, uint rangeIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_dimensionName = new MarshaledString(dimensionName);
            ulong start_size;
            ulong end_size;
            _ctx.handle_error(Methods.tiledb_subarray_get_range_var_size_from_name(ctxHandle, handle, ms_dimensionName, rangeIndex, &start_size, &end_size));

            using var startBuffer = new ScratchBuffer<byte>(checked((int)start_size), stackalloc byte[128]);
            using var endBuffer = new ScratchBuffer<byte>(checked((int)end_size), stackalloc byte[128]);
            fixed (byte* startPtr = startBuffer, endPtr = endBuffer)
            {
                _ctx.handle_error(Methods.tiledb_subarray_get_range_var_from_name(ctxHandle, handle, ms_dimensionName, rangeIndex, startPtr, endPtr));
            }

            var startStr = MarshaledStringOut.GetString(startBuffer.Span);
            var endStr = MarshaledStringOut.GetString(endBuffer.Span);
            return (startStr, endStr);
        }
    }
}
