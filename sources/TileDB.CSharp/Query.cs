using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public class ResultSize
    {
        public ulong DataBytesSize = 0;
        public ulong OffsetsBytesSize = 0;
        public ulong ValidityBytesSize = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSize"></param>
        /// <param name="offsetsSize"></param>
        /// <param name="validitySize"></param>
        public ResultSize(ulong dataSize, ulong offsetsSize, ulong validitySize)
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
            return OffsetsBytesSize > 0;
        }

        /// <summary>
        /// Test if it is nullable.
        /// </summary>
        /// <returns></returns>
        public bool IsNullable()
        {
            return ValidityBytesSize > 0;
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
            return OffsetsBytesSize/Methods.tiledb_datatype_size(tiledb_datatype_t.TILEDB_UINT64);
        }

        /// <summary>
        /// Get number of validities.
        /// </summary>
        /// <returns></returns>
        public ulong ValiditySize()
        {
            return ValidityBytesSize / Methods.tiledb_datatype_size(tiledb_datatype_t.TILEDB_UINT8);
        }
        
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

        private Dictionary<string, BufferHandle> _dataBufferHandles = new Dictionary<string, BufferHandle>();
        private Dictionary<string, BufferHandle> _offsetsBufferHandles = new Dictionary<string, BufferHandle>();
        private Dictionary<string, BufferHandle> _validityBufferHandles = new Dictionary<string, BufferHandle>();

        public Query(Context ctx, Array array, QueryType queryType)
        {
            _ctx = ctx;
            _array = array;
            _handle = new QueryHandle(ctx.Handle, array.Handle, (tiledb_query_type_t)queryType);
        }

        public Query(Context ctx, Array array)
        {
            _ctx = ctx;
            _array = array;
            _handle = new QueryHandle(ctx.Handle, array.Handle, (tiledb_query_type_t)array.query_type());
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
        public string stats()
        {
            var result_out = new MarshaledStringOut();
            fixed (sbyte** p_result = &result_out.Value)
            {
                _ctx.handle_error(Methods.tiledb_query_get_stats(_ctx.Handle, _handle, p_result));
            }

            return result_out;
        }
        /// <summary>
        /// Set config.
        /// </summary>
        /// <param name="config"></param>
        public void set_config(Config config)
        {
            _ctx.handle_error(Methods.tiledb_query_set_config(_ctx.Handle, _handle, config.Handle));
        }

        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns></returns>
        public Config config()
        {
            return _ctx.Config();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subarray"></param>
        public void set_subarray<T>(T[] data) where T : struct
        {
            var dim_datatype = _array.Schema().Domain().Type();
            if (EnumUtil.to_DataType(typeof(T)) != dim_datatype)
            {
                throw new System.ArgumentException("Query.set_subarray, datatype mismatch!");
            }

            var expected_size = _array.Schema().Domain().NDim() * 2;
            if (data == null || expected_size != data.Length)
            {
                throw new System.ArgumentException("Query.set_subarray, the length of data is not equal to num_dims*2!");
            }

            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_set_subarray(_ctx.Handle, _handle,
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
        public void set_data_buffer<T>(string name, T[] data) where T : struct
        {
            // check datatype
            CheckDataType<T>(GetDataType(name));

            if (data == null || data.Length == 0)
            {
                throw new System.ArgumentException("Query.set_data_buffer, buffer is null or empty!");
            }
            var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            AddDataBufferHandle(name, GCHandle.Alloc(data, GCHandleType.Pinned), size);
            _ctx.handle_error(Methods.tiledb_query_set_data_buffer(_ctx.Handle, _handle, ms_name,
                _dataBufferHandles[name].DataHandle.AddrOfPinnedObject().ToPointer(),
                (ulong*)_dataBufferHandles[name].SizeHandle.AddrOfPinnedObject().ToPointer()));

        }

        /// <summary>
        /// Sets the offset buffer for a var-sized attribute/dimension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void set_offsets_buffer(string name, UInt64[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new System.ArgumentException("Query.set_offsets_buffer, buffer is null or empty!");
            }
            var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            AddOffsetsBufferHandle(name, handle, size);

            _ctx.handle_error(Methods.tiledb_query_set_offsets_buffer(_ctx.Handle, _handle, ms_name,
                (ulong*)_offsetsBufferHandles[name].DataHandle.AddrOfPinnedObject().ToPointer(), 
                (ulong*)_offsetsBufferHandles[name].SizeHandle.AddrOfPinnedObject().ToPointer()));

        }

        /// <summary>
        /// Sets the validity buffer for nullable attribute/dimension.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public void set_validity_buffer(string name, byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new System.ArgumentException("Query.set_validity_buffer, buffer is null or empty!");
            }
            var ms_name = new MarshaledString(name);
            ulong size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            AddValidityBufferHandle(name, handle, size);
            _ctx.handle_error(Methods.tiledb_query_set_validity_buffer(_ctx.Handle, _handle, ms_name,
                (byte*)_validityBufferHandles[name].DataHandle.AddrOfPinnedObject().ToPointer(),
                (ulong*)_validityBufferHandles[name].SizeHandle.AddrOfPinnedObject().ToPointer()));

        }


 
        /// <summary>
        /// Sets the layout of the cells to be written or read.
        /// </summary>
        /// <param name="layouttype"></param>
        public void set_layout(LayoutType layouttype)
        {
            _ctx.handle_error(Methods.tiledb_query_set_layout(_ctx.Handle, _handle, (tiledb_layout_t)layouttype));
        }

        /// <summary>
        /// Sets the layout of the cells to be written or read.
        /// </summary>
        /// <returns></returns>
        public LayoutType query_layout()
        {
            tiledb_layout_t layout;
            _ctx.handle_error(Methods.tiledb_query_get_layout(_ctx.Handle, _handle, &layout));
            return (LayoutType)layout;
        }

         
        ///// <summary>
        ///// Sets the query condition to be applied on a read.
        ///// </summary>
        ///// <param name="condition"></param>
        public void set_condition(QueryCondition condition)
        {
            _ctx.handle_error(Methods.tiledb_query_set_condition(_ctx.Handle, _handle, condition.Handle));
        }

        /// <summary>
        /// Flushes all internal state of a query object and finalizes the query, only applicable to global layout writes.
        /// </summary>
        public void finalize()
        {
            _ctx.handle_error(Methods.tiledb_query_finalize(_ctx.Handle, _handle));
        }

        /// <summary>
        /// Submits the query. Call will block until query is complete.
        /// </summary>
        /// <returns></returns>
        public QueryStatus submit()
        {
            _ctx.handle_error(Methods.tiledb_query_submit(_ctx.Handle, _handle));
            return QueryStatus.TILEDB_COMPLETED;
        }

        /// <summary>
        /// Returns `true` if the query has results. Applicable only to read; false for write queries.
        /// </summary>
        /// <returns></returns>
        public bool has_results()
        {
            int ret;
            _ctx.handle_error(Methods.tiledb_query_has_results(_ctx.Handle, _handle, &ret));
            return (ret > 0);
        }

        /// <summary>
        /// Returns the query status.
        /// </summary>
        /// <returns></returns>
        public QueryStatus status()
        {
            tiledb_query_status_t query_status;
            _ctx.handle_error(Methods.tiledb_query_get_status(_ctx.Handle, _handle, &query_status));
            return (QueryStatus)query_status;
        }

        /// <summary>
        /// Returns the query type (read or write).
        /// </summary>
        /// <returns></returns>
        public QueryType query_type()
        {
            tiledb_query_type_t query_type;
            _ctx.handle_error(Methods.tiledb_query_get_type(_ctx.Handle, _handle, &query_type));
            return (QueryType)query_type;
        }

        /// <summary>
        /// Returns the array of the query.
        /// </summary>
        /// <returns></returns>
        public Array array()
        {
            return _array;
        }

        /// <summary>
        /// Adds a 1D range along a subarray dimension name, specified by its name, in the form (start, end, stride).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stride"></param>
        public void add_range<T>(UInt32 index, T start, T end, T stride) where T : struct
        {
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            T[] strideData = new T[1] { stride };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            var strideDataGcHandle = GCHandle.Alloc(strideData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range(_ctx.Handle, _handle, index,
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
        /// <param name="index"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void add_range(UInt32 index, string start, string end)
        {

            byte[] startData = Encoding.ASCII.GetBytes(start);
            byte[] endData = Encoding.ASCII.GetBytes(end);
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_var(_ctx.Handle, _handle, index,
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
        /// Adds a 1D range along a subarray dimension name, specified by its name, in the form(start, end, stride).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stride"></param>
        public void add_range<T>(string name, T start, T end, T stride) where T : struct
        {
            var ms_name = new MarshaledString(name);
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            T[] strideData = new T[1] { stride };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            var strideDataGcHandle = GCHandle.Alloc(strideData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_by_name(_ctx.Handle, _handle, ms_name,
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

        public void add_range(string name, string start, string end)
        {
            var ms_name = new MarshaledString(name);
            byte[] startData = Encoding.ASCII.GetBytes(start);
            byte[] endData = Encoding.ASCII.GetBytes(end);
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_var_by_name(_ctx.Handle, _handle, ms_name,
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
        public UInt64 range_num(UInt32 index)
        {
            UInt64 range_num;
            _ctx.handle_error(Methods.tiledb_query_get_range_num(_ctx.Handle, _handle, index, &range_num));
            return range_num;
        }

        /// <summary>
        /// Retrieves the number of ranges for a given dimension name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UInt64 range_num_from_name(string name)
        {
            UInt64 range_num;
            var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_get_range_num_from_name(_ctx.Handle, _handle, ms_name, &range_num));
            return range_num;
        }

        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(UInt32 dim_idx, UInt32 range_idx) where T : struct
        {
            void* start_p;
            void* end_p;
            void* stride_p;
            ulong size = EnumUtil.datatype_size(EnumUtil.to_DataType(typeof(T)));
            _ctx.handle_error(Methods.tiledb_query_get_range(_ctx.Handle, _handle, dim_idx, range_idx, &start_p, &end_p, &stride_p));

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
        public System.Tuple<T, T, T> range<T>(UInt32 dim_idx, UInt32 range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_idx, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }

        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(string dim_name, UInt32 range_idx) where T : struct
        {
            var ms_name = new MarshaledString(dim_name);
            void* start_p;
            void* end_p;
            void* stride_p;
            ulong size = EnumUtil.datatype_size(EnumUtil.to_DataType(typeof(T)));
            _ctx.handle_error(Methods.tiledb_query_get_range_from_name(_ctx.Handle, _handle, ms_name, range_idx, &start_p, &end_p, &stride_p));

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
        public System.Tuple<T, T, T> range<T>(string dim_name, UInt32 range_idx) where T : struct
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
        public System.Tuple<string, string> range_var(UInt32 dim_idx, UInt32 range_idx)
        {

            UInt64 start_size = 0;
            UInt64 end_size = 0;
            _ctx.handle_error(Methods.tiledb_query_get_range_var_size(_ctx.Handle, _handle, dim_idx, range_idx, &start_size, &end_size));

            byte[] startData = Enumerable.Repeat(default(byte), (int)start_size).ToArray();
            byte[] endData = Enumerable.Repeat(default(byte), (int)end_size).ToArray();

            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_query_get_range_var(_ctx.Handle, _handle, dim_idx, range_idx,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }

            return new Tuple<string, string>(Encoding.ASCII.GetString(startData), Encoding.ASCII.GetString(endData));

        }

        public System.Tuple<string, string> range_var(string dim_name, UInt32 range_idx)
        {
            var ms_name = new MarshaledString(dim_name);
            UInt64 start_size = 0;
            UInt64 end_size = 0;
            _ctx.handle_error(Methods.tiledb_query_get_range_var_size_from_name(_ctx.Handle, _handle, ms_name, range_idx, &start_size, &end_size));

            byte[] startData = Enumerable.Repeat(default(byte), (int)start_size).ToArray();
            byte[] endData = Enumerable.Repeat(default(byte), (int)end_size).ToArray();

            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_query_get_range_var_from_name(_ctx.Handle, _handle, ms_name, range_idx,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }

            return new Tuple<string, string>(Encoding.ASCII.GetString(startData), Encoding.ASCII.GetString(endData));

        }

        /// <summary>
        /// Retrieves the estimated result size for a fixed-size attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private UInt64 est_result_size(string name)
        {
            var ms_name = new MarshaledString(name);
            UInt64 size = 0;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size(_ctx.Handle, _handle, ms_name, &size));
            return size;
        }

        /// <summary>
        /// Retrieves the estimated result size for a variable-size attribute tuple(size_off,size_val).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<UInt64, UInt64> est_result_size_var(string name)
        {
            var ms_name = new MarshaledString(name);
            UInt64 size_off = 0;
            UInt64 size_val = 0;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var(_ctx.Handle, _handle, ms_name, &size_off, &size_val));

            return new Tuple<UInt64, UInt64>(size_off, size_val);
        }

        /// <summary>
        /// Retrieves the estimated result size for a fixed-size, nullable attribute tuple(size_val,size_validity).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Tuple<UInt64, UInt64> est_result_size_nullable(string name)
        {
            var ms_name = new MarshaledString(name);
            UInt64 size_val = 0;
            UInt64 size_validity = 0;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_nullable(_ctx.Handle, _handle, ms_name, &size_val, &size_validity));
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
            var ms_name = new MarshaledString(name);
            UInt64 size_off = 0;
            UInt64 size_val = 0;
            UInt64 size_validity = 0;

            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var_nullable(_ctx.Handle, _handle, ms_name, &size_off, &size_val, &size_validity));

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
        public uint fragment_num()
        {
            uint num;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_num(_ctx.Handle, _handle, &num));
            return num;
        }

        /// <summary>
        /// Returns the URI of the written fragment with the input index. Applicable only to WRITE queries.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string fragment_uri(ulong idx)
        {
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                _ctx.handle_error(Methods.tiledb_query_get_fragment_uri(_ctx.Handle, _handle, idx, p_result));
            }
            return ms_result;
        }

        /// <summary>
        /// Retrieves the timestamp range of the written fragment with the input index. Applicable only to WRITE queries.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public System.Tuple<UInt64, UInt64> fragment_timestamp_range(ulong idx)
        {
            ulong t1 = 0;
            ulong t2 = 0;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_timestamp_range(_ctx.Handle, _handle, idx, &t1, &t2));
            return new Tuple<ulong, ulong>(t1, t2);
        }


        #endregion capi functions


        private void CheckDataType<T>(DataType dataType) 
        {
            if (EnumUtil.to_DataType(typeof(T)) != dataType)
            {
                if(!(dataType== DataType.TILEDB_STRING_ASCII && (typeof(T)==typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(string) ))) {
                    throw new System.ArgumentException("T " + typeof(T).Name + " doesnot match " + dataType.ToString());
                }
                
            }
        }

        private DataType GetDataType(string name)
        {
            bool is_attr = _array.Schema().has_attribute(name);
            bool is_dim = _array.Schema().Domain().has_dimension(name);
            if (_array.Schema().has_attribute(name))
            {
                return _array.Schema().Attribute(name).Type();
            }
            else if (_array.Schema().Domain().has_dimension(name))
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
            _dataBufferHandles[name] = new BufferHandle(handle,size);
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

 
        #endregion buffers

    }//class

}
