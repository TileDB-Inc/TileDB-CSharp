using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public class ResultSize
    {
        public ulong DataBytesSize;
        public ulong? OffsetsBytesSize;
        public ulong? ValidityBytesSize;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSize"></param>
        /// <param name="offsetsSize"></param>
        /// <param name="validitySize"></param>
        public ResultSize(ulong dataSize, ulong? offsetsSize = null, ulong? validitySize = null)
        {
            DataBytesSize = dataSize;
            OffsetsBytesSize = offsetsSize;
            ValidityBytesSize = validitySize;
        }

        /// <summary>
        /// Test if it is variable length.
        /// </summary>
        /// <returns></returns>
        public bool IsVarSize()
        {
            return OffsetsBytesSize.HasValue;
        }

        /// <summary>
        /// Test if it is nullable.
        /// </summary>
        /// <returns></returns>
        public bool IsNullable()
        {
            return ValidityBytesSize.HasValue;
        }

        /// <summary>
        /// Get number of data elements.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public ulong DataSize(DataType dataType)
        {
            tiledb_datatype_t tiledb_datatype = (tiledb_datatype_t)dataType;
            return DataBytesSize / Methods.tiledb_datatype_size(tiledb_datatype);
        }

        /// <summary>
        /// Get number of offsets.
        /// </summary>
        /// <returns></returns>
        public ulong OffsetsSize()
        {
            return OffsetsBytesSize.HasValue ? OffsetsBytesSize.Value/Methods.tiledb_datatype_size(tiledb_datatype_t.TILEDB_UINT64) : 0;
        }

        /// <summary>
        /// Get number of validities.
        /// </summary>
        /// <returns></returns>
        public ulong ValiditySize()
        {
            return ValidityBytesSize.HasValue ? ValidityBytesSize.Value / Methods.tiledb_datatype_size(tiledb_datatype_t.TILEDB_UINT8) : 0;
        }
    }

    public class QueryEventArgs : EventArgs
    {
        public QueryEventArgs(int status, string message)
        {
            Status = status;
            Message = message;
        }
        public int Status { get; set; }
        public string Message { get; set; }
    }

    internal class BufferHandle
    {
        public GCHandle DataHandle;
        public ulong BytesSize;
        public GCHandle SizeHandle;

        public BufferHandle(GCHandle handle, ulong size)
        {
            DataHandle = handle;
            BytesSize = size;
            SizeHandle = GCHandle.Alloc(BytesSize, GCHandleType.Pinned);
        }

        public void Free()
        {
            if (DataHandle.IsAllocated)
            {
                DataHandle.Free();
            }
            if (SizeHandle.IsAllocated)
            {
                SizeHandle.Free();
            }
        }
    }

    public sealed unsafe class Query : IDisposable
    {
        private readonly Array _array;
        private readonly Context _ctx;
        private readonly QueryHandle _handle;
        private bool _disposed;

        private readonly Dictionary<string, BufferHandle> _dataBufferHandles = new Dictionary<string, BufferHandle>();
        private readonly Dictionary<string, BufferHandle> _offsetsBufferHandles = new Dictionary<string, BufferHandle>();
        private readonly Dictionary<string, BufferHandle> _validityBufferHandles = new Dictionary<string, BufferHandle>();

        public Query(Context ctx, Array array, QueryType queryType)
        {
            _ctx = ctx;
            _array = array;
            _handle = QueryHandle.Create(ctx, array.Handle, (tiledb_query_type_t)queryType);
        }

        public Query(Context ctx, Array array)
        {
            _ctx = ctx;
            _array = array;
            _handle = QueryHandle.Create(ctx, array.Handle, (tiledb_query_type_t)array.QueryType());
        }

        internal Query(Context ctx, Array array, QueryHandle handle)
        {
            _ctx = ctx;
            _array = array;
            _handle = handle;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing && (!_handle.IsInvalid))
            {
                _handle.Dispose();
            }
            FreeAllBufferHandles();

            _disposed = true;

        }

        internal QueryHandle Handle => _handle;

        #region capi functions
        /// <summary>
        /// Get statistic string.
        /// </summary>
        /// <returns></returns>
        public string Stats()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_query_get_stats(ctxHandle, handle, &result));

            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }
        /// <summary>
        /// Set config.
        /// </summary>
        /// <param name="config"></param>
        public void SetConfig(Config config)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var configHandle = config.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_config(ctxHandle, handle, configHandle));
        }

        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns></returns>
        public Config Config()
        {
            return _ctx.Config();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subarray"></param>
        public void SetSubarray<T>(T[] data) where T : struct
        {
            var dim_datatype = _array.Schema().Domain().Type();
            if (EnumUtil.TypeToDataType(typeof(T)) != dim_datatype)
            {
                throw new System.ArgumentException("Query.SetSubarray, datatype mismatch!");
            }

            var expected_size = _array.Schema().Domain().NDim() * 2;
            if (data == null || expected_size != data.Length)
            {
                throw new System.ArgumentException("Query.SetSubarray, the length of data is not equal to num_dims*2!");
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_set_subarray(ctxHandle, handle,
                    (void*)dataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                dataGcHandle.Free();
            }

        }

        /// <summary>
        /// Sets the data for a fixed/var-sized attribute/dimension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void SetDataBuffer<T>(string name, T[] data) where T : struct
        {
            // check datatype
            CheckDataType<T>(GetDataType(name));

            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Query.SetDataBuffer, buffer is null or empty!");
            }

            if (data is bool[] boolData && QueryType() == CSharp.QueryType.Write)
            {
                SetDataBuffer<byte>(name, System.Array.ConvertAll(boolData as bool[], d => d ? (byte)1 : (byte)0));
                return;
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            AddDataBufferHandle(name, GCHandle.Alloc(data, GCHandleType.Pinned), size);
            _ctx.handle_error(Methods.tiledb_query_set_data_buffer(ctxHandle, handle, ms_name,
                _dataBufferHandles[name].DataHandle.AddrOfPinnedObject().ToPointer(),
                (ulong*)_dataBufferHandles[name].SizeHandle.AddrOfPinnedObject().ToPointer()));
        }

        /// <summary>
        /// Sets the offset buffer for a var-sized attribute/dimension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void SetOffsetsBuffer(string name, UInt64[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Query.set_offsets_buffer, buffer is null or empty!");
            }
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var bufferHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            AddOffsetsBufferHandle(name, bufferHandle, size);

            _ctx.handle_error(Methods.tiledb_query_set_offsets_buffer(ctxHandle, handle, ms_name,
                (ulong*)_offsetsBufferHandles[name].DataHandle.AddrOfPinnedObject().ToPointer(),
                (ulong*)_offsetsBufferHandles[name].SizeHandle.AddrOfPinnedObject().ToPointer()));
        }

        /// <summary>
        /// Sets the validity buffer for nullable attribute/dimension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void SetValidityBuffer(string name, byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Query.set_validity_buffer, buffer is null or empty!");
            }
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var bufferHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            AddValidityBufferHandle(name, bufferHandle, size);
            _ctx.handle_error(Methods.tiledb_query_set_validity_buffer(ctxHandle, handle, ms_name,
                (byte*)_validityBufferHandles[name].DataHandle.AddrOfPinnedObject().ToPointer(),
                (ulong*)_validityBufferHandles[name].SizeHandle.AddrOfPinnedObject().ToPointer()));
        }

        /// <summary>
        /// Sets the layout of the cells to be written or read.
        /// </summary>
        /// <param name="layouttype"></param>
        public void SetLayout(LayoutType layouttype)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_layout(ctxHandle, handle, (tiledb_layout_t)layouttype));
        }

        /// <summary>
        /// Sets the layout of the cells to be written or read.
        /// </summary>
        /// <returns></returns>
        public LayoutType QueryLayout()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_layout_t layout;
            _ctx.handle_error(Methods.tiledb_query_get_layout(ctxHandle, handle, &layout));
            return (LayoutType)layout;
        }

        ///// <summary>
        ///// Sets the query condition to be applied on a read.
        ///// </summary>
        ///// <param name="condition"></param>
        public void SetCondition(QueryCondition condition)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var conditionHandle = condition.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_condition(ctxHandle, handle, conditionHandle));
        }

        /// <summary>
        /// Flushes all internal state of a query object and finalizes the query, only applicable to global layout writes.
        /// </summary>
        public void FinalizeQuery()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_finalize(ctxHandle, handle));
        }

        /// <summary>
        /// Submits the query. Call will block until query is complete.
        /// </summary>
        /// <returns></returns>
        public QueryStatus Submit()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_submit(ctxHandle, handle));
            return QueryStatus.Completed;
        }

        /// <summary>
        /// Submits and finalizes the query.
        /// </summary>
        /// <seealso cref="Submit"/>
        /// <seealso cref="FinalizeQuery"/>
        public void SubmitAndFinalize()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_submit_and_finalize(ctxHandle, handle));
        }

        [UnmanagedCallersOnly(CallConvs = new [] {typeof(CallConvCdecl)})]
        private static void QueryCallback(void* ptr)
        {
            GCHandle gcHandle = GCHandle.FromIntPtr((IntPtr)ptr);
            var query = (Query)gcHandle.Target!;
            gcHandle.Free();
            // Fire the event on the thread pool.
            // This yields the TileDB native thread and prevents any excpetions by the callback to reach it.
            Task.Run(() =>
            {
                var args = new QueryEventArgs((int)QueryStatus.Completed, "query completed");
                query.QueryCompleted?.Invoke(query, args);
            });
        }

        /// <summary>
        /// Submits the query asynchronously.
        /// </summary>
        public void SubmitAsync()
        {
            GCHandle gcHandle = GCHandle.Alloc(this);
            bool successful = false;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_query_submit_async(ctxHandle, handle, &QueryCallback, (void*)GCHandle.ToIntPtr(gcHandle)));
                successful = true;
            }
            finally
            {
                if (!successful)
                {
                    gcHandle.Free();
                }
            }
        }

        /// <summary>
        /// Default event handler is empty
        /// </summary>
        public event EventHandler<QueryEventArgs>? QueryCompleted;

        /// <summary>
        /// Returns `true` if the query has results. Applicable only to read; false for write queries.
        /// </summary>
        /// <returns></returns>
        public bool HasResults()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int ret;
            _ctx.handle_error(Methods.tiledb_query_has_results(ctxHandle, handle, &ret));
            return (ret > 0);
        }

        /// <summary>
        /// Returns the query status.
        /// </summary>
        /// <returns></returns>
        public QueryStatus Status()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_query_status_t query_status;
            _ctx.handle_error(Methods.tiledb_query_get_status(ctxHandle, handle, &query_status));
            return (QueryStatus)query_status;
        }

        /// <summary>
        /// Returns the query type (read or write).
        /// </summary>
        /// <returns></returns>
        public QueryType QueryType()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_query_type_t query_type;
            _ctx.handle_error(Methods.tiledb_query_get_type(ctxHandle, handle, &query_type));
            return (QueryType)query_type;
        }

        /// <summary>
        /// Returns the array of the query.
        /// </summary>
        /// <returns></returns>
        public Array Array()
        {
            return _array;
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension index, in the form (start, end).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="index">Index of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        public void AddRange<T>(UInt32 index, T start, T end) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range(ctxHandle, handle, index,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject(),
                    null
                ));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
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
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_by_name(ctxHandle, handle, ms_name,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject(),
                    null
                ));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension index, in the form (start, end, stride).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="index">Index of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        /// <param name="stride">Stride between dimension range values</param>
        private void AddRange<T>(UInt32 index, T start, T end, T stride) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            T[] strideData = new T[1] { stride };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            var strideDataGcHandle = GCHandle.Alloc(strideData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range(ctxHandle, handle, index,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject(),
                    (void*)strideDataGcHandle.AddrOfPinnedObject()
                ));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
                strideDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension name, specified by its name, in the form(start, end, stride).
        /// </summary>
        /// <typeparam name="T">Dimension range type</typeparam>
        /// <param name="name">Name of dimension</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        /// <param name="stride">Stride between dimension range values</param>
        private void AddRange<T>(string name, T start, T end, T stride) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            T[] strideData = new T[1] { stride };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            var strideDataGcHandle = GCHandle.Alloc(strideData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_by_name(ctxHandle, handle, ms_name,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject(),
                    (void*)strideDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
                strideDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Adds a 1D string range along a subarray dimension index, in the form (start, end).
        /// </summary>
        /// <param name="index">Index of dimension to add range</param>
        /// <param name="start">Dimension range start</param>
        /// <param name="end">Dimension range end</param>
        public void AddRange(UInt32 index, string start, string end)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            byte[] startData = Encoding.ASCII.GetBytes(start);
            byte[] endData = Encoding.ASCII.GetBytes(end);
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_var(ctxHandle, handle, index,
                    (void*)startDataGcHandle.AddrOfPinnedObject(), (ulong)startData.Length,
                    (void*)endDataGcHandle.AddrOfPinnedObject(), (ulong)endData.Length));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
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
            byte[] startData = Encoding.ASCII.GetBytes(start);
            byte[] endData = Encoding.ASCII.GetBytes(end);
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_var_by_name(ctxHandle, handle, ms_name,
                    (void*)startDataGcHandle.AddrOfPinnedObject(), (ulong)startData.Length,
                    (void*)endDataGcHandle.AddrOfPinnedObject(), (ulong)endData.Length));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Retrieves the number of ranges for a given dimension index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UInt64 RangeNum(UInt32 index)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            UInt64 range_num;
            _ctx.handle_error(Methods.tiledb_query_get_range_num(ctxHandle, handle, index, &range_num));
            return range_num;
        }

        /// <summary>
        /// Retrieves the number of ranges for a given dimension name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UInt64 RangeNumFromName(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            UInt64 range_num;
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_get_range_num_from_name(ctxHandle, handle, ms_name, &range_num));
            return range_num;
        }

        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(UInt32 dim_idx, UInt32 range_idx) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            void* start_p;
            void* end_p;
            void* stride_p;
            ulong size = EnumUtil.DataTypeSize(EnumUtil.TypeToDataType(typeof(T)));
            _ctx.handle_error(Methods.tiledb_query_get_range(ctxHandle, handle, dim_idx, range_idx, &start_p, &end_p, &stride_p));

            var start_span = new ReadOnlySpan<byte>(start_p, (int)size);
            var end_span = new ReadOnlySpan<byte>(end_p, (int)size);
            var stride_span = new ReadOnlySpan<byte>(stride_p, (int)size);
            return (start_span.ToArray(), end_span.ToArray(), stride_span.ToArray());
        }

        /// <summary>
        /// Retrieves a range for a given dimension index and range id in tuple(start,end,stride).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dim_idx"></param>
        /// <param name="range_idx"></param>
        /// <returns></returns>
        public System.Tuple<T, T, T> Range<T>(UInt32 dim_idx, UInt32 range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_idx, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }

        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(string dim_name, UInt32 range_idx) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(dim_name);
            void* start_p;
            void* end_p;
            void* stride_p;
            ulong size = EnumUtil.DataTypeSize(EnumUtil.TypeToDataType(typeof(T)));
            _ctx.handle_error(Methods.tiledb_query_get_range_from_name(ctxHandle, handle, ms_name, range_idx, &start_p, &end_p, &stride_p));

            var start_span = new ReadOnlySpan<byte>(start_p, (int)size);
            var end_span = new ReadOnlySpan<byte>(end_p, (int)size);
            var stride_span = new ReadOnlySpan<byte>(stride_p, (int)size);
            return (start_span.ToArray(), end_span.ToArray(), stride_span.ToArray());
        }

        /// <summary>
        /// Retrieves a range for a given dimension name and range id in tuple(start,end,stride).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="range_idx"></param>
        /// <returns></returns>
        public System.Tuple<T, T, T> Range<T>(string dim_name, UInt32 range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_name, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }


        /// <summary>
        /// Retrieves a range for a given variable length string dimension index and range id.
        /// </summary>
        /// <param name="dim_idx"></param>
        /// <param name="range_idx"></param>
        /// <returns></returns>
        public System.Tuple<string, string> RangeVar(UInt32 dim_idx, UInt32 range_idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            UInt64 start_size = 0;
            UInt64 end_size = 0;
            _ctx.handle_error(Methods.tiledb_query_get_range_var_size(ctxHandle, handle, dim_idx, range_idx, &start_size, &end_size));

            byte[] startData = Enumerable.Repeat(default(byte), (int)start_size).ToArray();
            byte[] endData = Enumerable.Repeat(default(byte), (int)end_size).ToArray();

            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_query_get_range_var(ctxHandle, handle, dim_idx, range_idx,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }

            return new Tuple<string, string>(MarshaledStringOut.GetString(startData), MarshaledStringOut.GetString(endData));

        }

        public System.Tuple<string, string> RangeVar(string dim_name, UInt32 range_idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(dim_name);
            UInt64 start_size = 0;
            UInt64 end_size = 0;
            _ctx.handle_error(Methods.tiledb_query_get_range_var_size_from_name(ctxHandle, handle, ms_name, range_idx, &start_size, &end_size));

            byte[] startData = Enumerable.Repeat(default(byte), (int)start_size).ToArray();
            byte[] endData = Enumerable.Repeat(default(byte), (int)end_size).ToArray();

            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_query_get_range_var_from_name(ctxHandle, handle, ms_name, range_idx,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }

            return new Tuple<string, string>(MarshaledStringOut.GetString(startData), MarshaledStringOut.GetString(endData));

        }

        /// <summary>
        /// Retrieves the estimated result size for a fixed-size attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private UInt64 est_result_size(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            UInt64 size = 0;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size(ctxHandle, handle, ms_name, &size));
            return size;
        }

        /// <summary>
        /// Retrieves the estimated result size for a variable-size attribute tuple(size_off,size_val).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<UInt64, UInt64> est_result_size_var(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            UInt64 size_off = 0;
            UInt64 size_val = 0;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var(ctxHandle, handle, ms_name, &size_off, &size_val));

            return new Tuple<UInt64, UInt64>(size_off, size_val);
        }

        /// <summary>
        /// Retrieves the estimated result size for a fixed-size, nullable attribute tuple(size_val,size_validity).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<UInt64, UInt64> est_result_size_nullable(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            UInt64 size_val = 0;
            UInt64 size_validity = 0;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_nullable(ctxHandle, handle, ms_name, &size_val, &size_validity));
            List<UInt64> ret = new List<ulong>();
            return new Tuple<UInt64, UInt64>(size_val, size_validity);
        }

        /// <summary>
        /// Retrieves the estimated result size for a variable-size, nullable attribute tuple(size_off,size_val,size_validity).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<UInt64, UInt64, UInt64> est_result_size_var_nullable(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            UInt64 size_off = 0;
            UInt64 size_val = 0;
            UInt64 size_validity = 0;

            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var_nullable(ctxHandle, handle, ms_name, &size_off, &size_val, &size_validity));

            return new Tuple<UInt64, UInt64, UInt64>(size_off, size_val, size_validity);
        }

        /// <summary>
        /// Get estimated result size.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultSize EstResultSize(string name)
        {
            bool isVar = _array.Schema().IsVarSize(name);
            bool isNullable = _array.Schema().IsNullable(name);
            if (isVar)
            {
                if (isNullable)
                {
                    var t = est_result_size_var_nullable(name);
                    return new ResultSize(t.Item2, t.Item1, t.Item3);
                }
                else
                {
                    var t = est_result_size_var(name);
                    return new ResultSize(t.Item2, t.Item1, 0);

                }
            }
            else if (isNullable)
            {
                var t = est_result_size_nullable(name);
                return new ResultSize(t.Item1, 0, t.Item2);
            }
            else
            {
                var t = est_result_size(name);
                return new ResultSize(t, 0, 0);
            }
        }

        /// <summary>
        /// Returns the number of written fragments. Applicable only to WRITE queries.
        /// </summary>
        /// <returns></returns>
        public uint FragmentNum()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint num;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_num(ctxHandle, handle, &num));
            return num;
        }

        /// <summary>
        /// Returns the URI of the written fragment with the input index. Applicable only to WRITE queries.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string FragmentUri(ulong idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_uri(ctxHandle, handle, idx, &result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Retrieves the timestamp range of the written fragment with the input index. Applicable only to WRITE queries.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public System.Tuple<UInt64, UInt64> FragmentTimestampRange(ulong idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong t1 = 0;
            ulong t2 = 0;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_timestamp_range(ctxHandle, handle, idx, &t1, &t2));
            return new Tuple<ulong, ulong>(t1, t2);
        }
        #endregion

        private void CheckDataType<T>(DataType dataType)
        {
            if (EnumUtil.TypeToDataType(typeof(T)) != dataType)
            {
                if (!(dataType== DataType.StringAscii && (typeof(T)==typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(string)))
                   && !(dataType == DataType.Boolean && typeof(T) == typeof(byte)))
                {
                    throw new System.ArgumentException("T " + typeof(T).Name + " doesnot match " + dataType.ToString());
                }
            }
        }

        private DataType GetDataType(string name)
        {
            bool is_attr = _array.Schema().HasAttribute(name);
            bool is_dim = _array.Schema().Domain().HasDimension(name);
            if (_array.Schema().HasAttribute(name))
            {
                return _array.Schema().Attribute(name).Type();
            }
            else if (_array.Schema().Domain().HasDimension(name))
            {
                return _array.Schema().Domain().Dimension(name).Type();
            }
            else if (name == "__coords")
            {
                return _array.Schema().Domain().Type();
            }
            else
            {
                throw new ArgumentException("No datatype for " + name);
            }
        }

        #region buffers
        /// <summary>
        /// Returns the number of elements read into result buffers from a read query.
        ///
        /// Dictionary key: Name of buffer used in call to SetDataBuffer, SetOffsetBuffer, or SetValidityBuffer
        /// Tuple Item1: Number of elements read by the query
        /// Tuple Item2: Number of offset elements read by the query
        /// Tuple Item3: Number of validity bytes read by the query
        ///
        /// If the buffer is not variable-sized, Tuple.Item2 will be set to null
        /// If the buffer is not nullable, Tuple.Item3 will be set to null
        /// </summary>
        /// <returns>Dictionary mapping buffer name to number of results</returns>
        public Dictionary<string, Tuple<ulong, ulong?, ulong?>> ResultBufferElements()
        {
            var buffers = new Dictionary<string, Tuple<ulong, ulong?, ulong?>>();
            foreach (var key in _dataBufferHandles.Keys)
            {
                ulong? offsetNum = null;
                ulong? validityNum = null;

                ulong typeSize = EnumUtil.DataTypeSize(GetDataType(key));
                ulong dataNum = (ulong)_dataBufferHandles[key].SizeHandle.Target! / typeSize;

                if (_offsetsBufferHandles.ContainsKey(key))
                {
                    offsetNum = (ulong)_offsetsBufferHandles[key].SizeHandle.Target! / sizeof(ulong);
                }
                if (_validityBufferHandles.ContainsKey(key))
                {
                    validityNum = (ulong)_validityBufferHandles[key].SizeHandle.Target!;
                }

                buffers.Add(key, new(dataNum, offsetNum, validityNum));
            }

            return buffers;
        }

        /// <summary>
        /// Free all handles.
        /// </summary>
        private void FreeAllBufferHandles()
        {
            foreach (var bh in _dataBufferHandles)
            {
                bh.Value.Free();
            }

            foreach (var bh in _offsetsBufferHandles)
            {
                bh.Value.Free();
            }

            foreach (var bh in _validityBufferHandles)
            {
                bh.Value.Free();
            }
        }

        /// <summary>
        /// Add data buffer handle.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        private void AddDataBufferHandle(string name, GCHandle handle, ulong size)
        {
            if (_dataBufferHandles.ContainsKey(name))
            {
                _dataBufferHandles[name].Free();
            }
            _dataBufferHandles[name] = new BufferHandle(handle, size);
        }

        /// <summary>
        /// Add offset buffer handle.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        private void AddOffsetsBufferHandle(string name, GCHandle handle, ulong size)
        {
            if (_offsetsBufferHandles.ContainsKey(name))
            {
                _offsetsBufferHandles[name].Free();
            }
            _offsetsBufferHandles[name] = new BufferHandle(handle, size);
        }

        /// <summary>
        /// Add validity buffer handle.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handle"></param>
        /// <param name="size"></param>
        private void AddValidityBufferHandle(string name, GCHandle handle, ulong size)
        {
            if (_validityBufferHandles.ContainsKey(name))
            {
                _validityBufferHandles[name].Free();
            }
            _validityBufferHandles[name] = new BufferHandle(handle, size);
        }
        #endregion
    }
}
