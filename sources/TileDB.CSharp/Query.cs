using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

    [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
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
            if (_disposed) return;
            _handle.Dispose();
            FreeAllBufferHandles();
            _disposed = true;
        }

        internal QueryHandle Handle => _handle;

        /// <summary>
        /// Gets a JSON string with statistics about the query.
        /// </summary>
        public string Stats()
        {
            sbyte* result = null;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                ErrorHandling.ThrowOnError(Methods.tiledb_query_get_stats(ctxHandle, handle, &result));

                return MarshaledStringOut.GetStringFromNullTerminated(result);
            }
            finally
            {
                if (result is not null)
                {
                    ErrorHandling.ThrowOnError(Methods.tiledb_stats_free_str(&result));
                }
            }
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
        /// Indicates that the query will write to or read from a <see cref="Subarray"/>, and provides the appropriate information.
        /// </summary>
        /// <param name="subarray">The subarray to use.</param>
        public void SetSubarray(Subarray subarray)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var subarrayHandle = subarray.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_subarray_t(ctxHandle, handle, subarrayHandle));
        }

        /// <summary>
        /// Creates a <see cref="Subarray"/> corresponding to this query.
        /// </summary>
        public Subarray GetSubarray()
        {
            var handle = new SubarrayHandle();
            var successful = false;
            tiledb_subarray_t* subarray = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var queryHandle = _handle.Acquire())
                {
                    _ctx.handle_error(Methods.tiledb_query_get_subarray_t(ctxHandle, queryHandle, &subarray));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(subarray);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }

            return new Subarray(_ctx, _array, handle);
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.SetSubarray{T}(T[])"/> instead and assign the subarray with <see cref="SetSubarray(Subarray)"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.SetSubarray on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetSubarray<T>(T[] data) where T : struct
        {
            var dim_datatype = _array.Schema().Domain().Type();
            if (EnumUtil.TypeToDataType(typeof(T)) != dim_datatype)
            {
                throw new ArgumentException("Query.SetSubarray, datatype mismatch!");
            }

            var expected_size = _array.Schema().Domain().NDim() * 2;
            if (data == null || expected_size != data.Length)
            {
                throw new ArgumentException("Query.SetSubarray, the length of data is not equal to num_dims*2!");
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
        public void SetDataBuffer<T>(string name, T[] data) where T : struct
        {
            // check datatype
            using (var schema = _array.Schema())
            using (var domain = schema.Domain())
            {
                CheckDataType<T>(GetDataType(name, schema, domain));
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Query.SetDataBuffer, buffer is null or empty!");
            }

            if (data is bool[] boolData && QueryType() == CSharp.QueryType.Write)
            {
                SetDataBuffer(name, System.Array.ConvertAll(boolData, d => d ? (byte)1 : (byte)0));
                return;
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var bufferHandle = AddDataBufferHandle(name, GCHandle.Alloc(data, GCHandleType.Pinned), size);
            _ctx.handle_error(Methods.tiledb_query_set_data_buffer(ctxHandle, handle, ms_name,
                bufferHandle.DataPointer,
                bufferHandle.SizePointer));
        }

        /// <summary>
        /// Sets the offset buffer for a var-sized attribute/dimension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SetOffsetsBuffer(string name, ulong[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Query.set_offsets_buffer, buffer is null or empty!");
            }
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var bufferHandle = AddOffsetsBufferHandle(name, GCHandle.Alloc(data, GCHandleType.Pinned), size);

            _ctx.handle_error(Methods.tiledb_query_set_offsets_buffer(ctxHandle, handle, ms_name,
                (ulong*)bufferHandle.DataPointer,
                bufferHandle.SizePointer));
        }

        /// <summary>
        /// Sets the validity buffer for nullable attribute/dimension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
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
            var bufferHandle = AddValidityBufferHandle(name, GCHandle.Alloc(data, GCHandleType.Pinned), size);
            _ctx.handle_error(Methods.tiledb_query_set_validity_buffer(ctxHandle, handle, ms_name,
                (byte*)bufferHandle.DataPointer,
                bufferHandle.SizePointer));
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

        /// <summary>
        /// Sets the query condition to be applied on a read.
        /// </summary>
        /// <param name="condition"></param>
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

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
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
        [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
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
        [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
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
            return ret > 0;
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
        /// Deprecated, use <see cref="Subarray.AddRange{T}(uint, T, T)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRange<T>(uint index, T start, T end) where T : struct
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
        /// Deprecated, use <see cref="Subarray.AddRange{T}(string, T, T)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        /// Deprecated, use <see cref="Subarray.AddRange(uint, string, string)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRange(uint index, string start, string end)
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
        /// Deprecated, use <see cref="Subarray.AddRange(string, string, string)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        /// Deprecated, use <see cref="Subarray.GetRangeCount(uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRangeCount on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ulong RangeNum(uint index)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong range_num;
            _ctx.handle_error(Methods.tiledb_query_get_range_num(ctxHandle, handle, index, &range_num));
            return range_num;
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetRangeCount(string)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRangeCount on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ulong RangeNumFromName(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong range_num;
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_get_range_num_from_name(ctxHandle, handle, ms_name, &range_num));
            return range_num;
        }

        [Obsolete(Obsoletions.QuerySubarrayMessage, DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(uint dim_idx, uint range_idx) where T : struct
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
        /// Deprecated, use <see cref="Subarray.GetRange{T}(uint, uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRange on the query's assigned Subarray instead. Note that it does not return a stride.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<T, T, T> Range<T>(uint dim_idx, uint range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_idx, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }

        [Obsolete(Obsoletions.QuerySubarrayMessage, DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(string dim_name, uint range_idx) where T : struct
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
        /// Deprecated, use <see cref="Subarray.GetRange{T}(string, uint)"/> instead and assign the subarray with <see cref="SetSubarray(Subarray)"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRange on the query's assigned Subarray instead. Note that it does not return a stride.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<T, T, T> Range<T>(string dim_name, uint range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_name, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetStringRange(uint, uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetStringRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<string, string> RangeVar(uint dim_idx, uint range_idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong start_size;
            ulong end_size;
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

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetStringRange(string, uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetStringRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<string, string> RangeVar(string dim_name, uint range_idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(dim_name);
            ulong start_size;
            ulong end_size;
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
        private ulong est_result_size(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size(ctxHandle, handle, ms_name, &size));
            return size;
        }

        /// <summary>
        /// Retrieves the estimated result size for a variable-size attribute tuple(size_off,size_val).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<ulong, ulong> est_result_size_var(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size_off;
            ulong size_val;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var(ctxHandle, handle, ms_name, &size_off, &size_val));

            return new Tuple<ulong, ulong>(size_off, size_val);
        }

        /// <summary>
        /// Retrieves the estimated result size for a fixed-size, nullable attribute tuple(size_val,size_validity).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<ulong, ulong> est_result_size_nullable(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size_val;
            ulong size_validity;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_nullable(ctxHandle, handle, ms_name, &size_val, &size_validity));
            return new Tuple<ulong, ulong>(size_val, size_validity);
        }

        /// <summary>
        /// Retrieves the estimated result size for a variable-size, nullable attribute tuple(size_off,size_val,size_validity).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<ulong, ulong, ulong> est_result_size_var_nullable(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size_off;
            ulong size_val;
            ulong size_validity;

            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var_nullable(ctxHandle, handle, ms_name, &size_off, &size_val, &size_validity));

            return new Tuple<ulong, ulong, ulong>(size_off, size_val, size_validity);
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
        public Tuple<ulong, ulong> FragmentTimestampRange(ulong idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong t1;
            ulong t2;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_timestamp_range(ctxHandle, handle, idx, &t1, &t2));
            return new Tuple<ulong, ulong>(t1, t2);
        }

        private void CheckDataType<T>(DataType dataType)
        {
            if (EnumUtil.TypeToDataType(typeof(T)) != dataType)
            {
                if (!(dataType== DataType.StringAscii && (typeof(T)==typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(string)))
                   && !(dataType == DataType.Boolean && typeof(T) == typeof(byte)))
                {
                    throw new ArgumentException("T " + typeof(T).Name + " doesnot match " + dataType.ToString());
                }
            }
        }

        private static DataType GetDataType(string name, ArraySchema schema, Domain domain)
        {
            if (schema.HasAttribute(name))
            {
                using var attribute = schema.Attribute(name);
                return attribute.Type();
            }
            else if (domain.HasDimension(name))
            {
                using var dimension = domain.Dimension(name);
                return dimension.Type();
            }

            if (name == "__coords")
            {
                return domain.Type();
            }

            throw new ArgumentException("No datatype for " + name);
        }

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
            using var schema = _array.Schema();
            using var domain = schema.Domain();
            var buffers = new Dictionary<string, Tuple<ulong, ulong?, ulong?>>();
            foreach ((string key, BufferHandle dataHandle) in _dataBufferHandles)
            {
                ulong? offsetNum = null;
                ulong? validityNum = null;

                ulong typeSize = EnumUtil.DataTypeSize(GetDataType(key, schema, domain));
                ulong dataNum = dataHandle.Size / typeSize;

                if (_offsetsBufferHandles.TryGetValue(key, out BufferHandle? offsetHandle))
                {
                    offsetNum = offsetHandle.Size / sizeof(ulong);
                }
                if (_validityBufferHandles.TryGetValue(key, out BufferHandle? validityHandle))
                {
                    validityNum = validityHandle.Size;
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
            DisposeValuesAndClear(_dataBufferHandles);
            DisposeValuesAndClear(_offsetsBufferHandles);
            DisposeValuesAndClear(_validityBufferHandles);

            static void DisposeValuesAndClear(Dictionary<string, BufferHandle> handles)
            {
                foreach (var bh in handles)
                {
                    bh.Value.Dispose();
                }
                handles.Clear();
            }
        }

        private static BufferHandle AddBufferHandle(Dictionary<string, BufferHandle> dict, string name, GCHandle handle, ulong size)
        {
            if (dict.TryGetValue(name, out var bufferHandle))
            {
                bufferHandle.Dispose();
            }
            bufferHandle = new BufferHandle(handle, size);
            dict[name] = bufferHandle;
            return bufferHandle;
        }

        private BufferHandle AddDataBufferHandle(string name, GCHandle handle, ulong size) =>
            AddBufferHandle(_dataBufferHandles, name, handle, size);

        private BufferHandle AddOffsetsBufferHandle(string name, GCHandle handle, ulong size) =>
            AddBufferHandle(_offsetsBufferHandles, name, handle, size);

        private BufferHandle AddValidityBufferHandle(string name, GCHandle handle, ulong size) =>
            AddBufferHandle(_validityBufferHandles, name, handle, size);

        private sealed class BufferHandle : IDisposable
        {
            private GCHandle DataHandle;
            public ulong* SizePointer { get; private set; }

            public void* DataPointer => (void*)DataHandle.AddrOfPinnedObject();

            public ulong Size
            {
                get
                {
                    Debug.Assert(SizePointer is not null);
                    return *SizePointer;
                }
                set
                {
                    Debug.Assert(SizePointer is not null);
                    *SizePointer = value;
                }
            }

            public BufferHandle(GCHandle handle, ulong size)
            {
                DataHandle = handle;
                SizePointer = (ulong*)Marshal.AllocHGlobal(sizeof(ulong));
                Size = size;
            }

            public void Dispose()
            {
                DataHandle.Free();
                if (SizePointer != null)
                {
                    Marshal.FreeHGlobal((IntPtr)SizePointer);
                }
                SizePointer = null;
            }
        }
    }
}
