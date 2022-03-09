using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public ref struct ArrayMetadata<T>
    {
        public string Key;
        public uint KeyLen;
        public tiledb_datatype_t Datatype;
        public uint ValueNum;
        public Span<T> Value;
    }

    public class NonEmptyDomain
    {
        private readonly Dictionary<string, object> _dict = new();

        public void Add<T>(string key, T value)
        {
            if (value != null) _dict.Add(key, value);
        }

        public T Get<T>(string key)
        {
            return (T)_dict[key];
        }

        public bool TryGet<T>(string key, out T? value)
        {
            if (_dict.TryGetValue(key, out var result) && result is T value1) {
                value = value1;
                return true;
            }
            value = default(T);
            return false;
        }
    }

    public sealed unsafe class Array : IDisposable
    {
        private readonly ArrayHandle _handle;
        private readonly Context _ctx;
        private readonly string _uri;
        private bool _disposed;


        public Array(Context ctx, string uri)
        {
            _ctx = ctx;
            _uri = uri;
            var ms_uri = new MarshaledString(_uri);
            _handle = new ArrayHandle(_ctx.Handle, ms_uri);
        }

        internal Array(Context ctx, ArrayHandle handle)
        {
            _ctx = ctx;
            _handle = handle;
            _uri = Uri();
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

            _disposed = true;

        }

        internal ArrayHandle Handle => _handle;


        #region capi functions
        /// <summary>
        /// Set open timestamp start, sets the subsequent Open call to use the start_timestamp of the passed value.
        /// </summary>
        /// <param name="timestampStart"></param>
        public void set_open_timestamp_start(ulong timestampStart)
        {
            _ctx.handle_error(Methods.tiledb_array_set_open_timestamp_start(_ctx.Handle, _handle, timestampStart));
        }

        /// <summary>
        /// Set open timestamp end.
        /// </summary>
        /// <param name="timestampEnd"></param>
        public void set_open_timestamp_end(ulong timestampEnd)
        {
            _ctx.handle_error(Methods.tiledb_array_set_open_timestamp_end(_ctx.Handle, _handle, timestampEnd));
        }

        /// <summary>
        /// Get open timestamp start.
        /// </summary>
        /// <returns></returns>
        public ulong open_timestamp_start()
        {
            ulong timestamp;
            _ctx.handle_error(Methods.tiledb_array_get_open_timestamp_start(_ctx.Handle, _handle, &timestamp));
            return timestamp;
        }

        /// <summary>
        /// Get timestamp end.
        /// </summary>
        /// <returns></returns>
        public ulong open_timestamp_end()
        {
            ulong timestamp;
            _ctx.handle_error(Methods.tiledb_array_get_open_timestamp_end(_ctx.Handle, _handle, &timestamp));
            return timestamp;
        }

        /// <summary>
        /// Open the array.
        /// </summary>
        /// <param name="queryType"></param>
        public void Open(QueryType queryType)
        {
            var tiledb_query_type = (tiledb_query_type_t)queryType;
            _ctx.handle_error(Methods.tiledb_array_open(_ctx.Handle, _handle, tiledb_query_type));
        }
        /// <summary>
        /// ReOpen the array.
        /// </summary>
        public void Reopen()
        {
            _ctx.handle_error(Methods.tiledb_array_reopen(_ctx.Handle, _handle));
        }

        /// <summary>
        /// Tes if the array is open or not.
        /// </summary>
        /// <returns></returns>
        public bool is_open()
        {
            int int_open;
            _ctx.handle_error(Methods.tiledb_array_is_open(_ctx.Handle, _handle, &int_open));
            return int_open > 0;
        }

        /// <summary>
        /// Set config.
        /// </summary>
        /// <param name="config"></param>
        public void set_config(Config config)
        {
            _ctx.handle_error(Methods.tiledb_array_set_config(_ctx.Handle, _handle, config.Handle));
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
        /// Close the array.
        /// </summary>
        public void Close()
        {
            _ctx.handle_error(Methods.tiledb_array_close(_ctx.Handle, _handle));
        }

        /// <summary>
        /// Get array schema.
        /// </summary>
        /// <returns></returns>
        public ArraySchema Schema()
        {
            tiledb_array_schema_t* array_schema_p;
            _ctx.handle_error(Methods.tiledb_array_get_schema(_ctx.Handle, _handle, &array_schema_p));
            if (array_schema_p == null)
            {
                throw new ErrorException("Array.schema, schema pointer is null");
            }
            return new ArraySchema(_ctx,array_schema_p);
        }

        /// <summary>
        /// Get query type.
        /// </summary>
        /// <returns></returns>
        public QueryType query_type()
        {
            tiledb_query_type_t tiledb_query_type;
            _ctx.handle_error(Methods.tiledb_array_get_query_type(_ctx.Handle, _handle, &tiledb_query_type));
            return (QueryType)tiledb_query_type;
        }

        /// <summary>
        /// Create an array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="schema"></param>
        public static void Create(Context ctx, string uri, ArraySchema schema)
        {
            var ms_uri = new MarshaledString(uri);
            ctx.handle_error(Methods.tiledb_array_create(ctx.Handle, ms_uri, schema.Handle));
        }

        /// <summary>
        /// Create an array.
        /// </summary>
        /// <param name="schema"></param>
        public void Create(ArraySchema schema)
        {
            var ms_uri = new MarshaledString(_uri);
            _ctx.handle_error(Methods.tiledb_array_create(_ctx.Handle, ms_uri, schema.Handle));
        }

        /// <summary>
        /// Consolidate an array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="config"></param>
        public static void Consolidate(Context ctx, string uri, Config config)
        {
            var ms_uri = new MarshaledString(uri);
            ctx.handle_error(Methods.tiledb_array_consolidate(ctx.Handle, ms_uri, config.Handle));
        }

        /// <summary>
        /// Vacuum the array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="config"></param>
        public static void Vacuum(Context ctx, string uri, Config config)
        {
            var ms_uri = new MarshaledString(uri);
            ctx.handle_error(Methods.tiledb_array_vacuum(ctx.Handle, ms_uri, config.Handle));
        }

        public (NonEmptyDomain, bool) non_empty_domain()
        {
            NonEmptyDomain nonEmptyDomain = new();

            var ndim = Schema().Domain().NDim();
            if (ndim == 0)
            {
                return (nonEmptyDomain, true);
            }

            bool isEmptyDomain = true;
            for (uint i = 0; i < ndim; ++i)
            {
                var dim = Schema().Domain().Dimension(i);
                var dimName = dim.Name();
                var dimType = EnumUtil.to_Type(dim.Type());

                switch (Type.GetTypeCode(dimType))
                {
                    case TypeCode.Int16:
                        {
                            (short data0, short data1, bool isEmpty) = non_empty_domain<short>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<short, short>(data0, data1));
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            (int data0, int data1, bool isEmpty) = non_empty_domain<int>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<int, int>(data0, data1));
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            (long data0, long data1, bool isEmpty) = non_empty_domain<long>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<long, long>(data0, data1));
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            (ushort data0, ushort data1, bool isEmpty) = non_empty_domain<ushort>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<ushort, ushort>(data0, data1));
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            (uint data0, uint data1, bool isEmpty) = non_empty_domain<uint>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<uint, uint>(data0, data1));
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            (ulong data0, ulong data1, bool isEmpty) = non_empty_domain<ulong>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<ulong, ulong>(data0, data1));
                        }
                        break;
                    case TypeCode.Single:
                        {
                            (float data0, float data1, bool isEmpty) = non_empty_domain<float>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<float, float>(data0, data1));
                        }
                        break;
                    case TypeCode.Double:
                        {
                            (double data0, double data1, bool isEmpty) = non_empty_domain<double>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<double, double>(data0, data1));
                        }
                        break;
                    case TypeCode.String:
                    {
                        (string data0, string data1, bool isEmpty) = non_empty_domain_var(i);
                        if (isEmpty == false)
                        {
                            isEmptyDomain = false;
                        }

                        nonEmptyDomain.Add(dimName, new Tuple<string, string>(data0, data1));
                    }
                        break;
                    default:
                        {
                        }
                        break;

                }
            }

            return (nonEmptyDomain, isEmptyDomain);
        }

        /// <summary>
        /// Get non-empty domain from index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public (T, T, bool) non_empty_domain<T>(uint index) where T : struct
        {
            var datatype = EnumUtil.to_DataType(typeof(T));
            if (datatype != Schema().Domain().Dimension(index).Type())
            {
                throw new ArgumentException("Array.non_empty_domain, not valid datatype!");
            }

            var data = new[] { default(T), default(T) };
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var int_empty = 1;
            try
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_index(_ctx.Handle, _handle, index,
                    (void*)dataGcHandle.AddrOfPinnedObject(), &int_empty));
            }
            finally
            {
                dataGcHandle.Free();
            }

            return int_empty > 0 ? (default(T), default(T), true) : (data[0], data[1], false);
        }

        /// <summary>
        /// Get non-empty domain from name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public (T, T, bool) non_empty_domain<T>(string name) where T : struct
        {
            var datatype = EnumUtil.to_DataType(typeof(T));
            if (datatype != Schema().Domain().Dimension(name).Type())
            {
                throw new ArgumentException("Array.non_empty_domain, not valid datatype!");
            }

            var ms_name = new MarshaledString(name);
            var data = new[] { default(T), default(T) };
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var int_empty = 1;
            try
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_name(_ctx.Handle, _handle, ms_name, (void*)dataGcHandle.AddrOfPinnedObject(), &int_empty));
            }
            finally
            {
                dataGcHandle.Free();
            }

            return int_empty > 0 ? (default(T), default(T), true) : (data[0], data[1], false);
        }
        /// <summary>
        /// Get string dimension domain from index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public (string, string, bool) non_empty_domain_var(uint index)
        {
            var dim = Schema().Domain().Dimension(index);
            if(!EnumUtil.is_string_type(dim.Type()))
            {
                throw new ErrorException("Array.non_empty_domain_var, not string dimension for index:" + index);
            }
            ulong start_size;
            ulong end_size;
            var int_empty = 1;
            _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_index(_ctx.Handle, _handle, index, &start_size, &end_size, &int_empty));
            if (int_empty > 0) {
                return (string.Empty, string.Empty, true);
            }

            var start = new byte[(int)start_size];
            var end = new byte[(int)end_size];

            var startGcHandle = GCHandle.Alloc(start, GCHandleType.Pinned);
            var endGcHandle = GCHandle.Alloc(end, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_index(_ctx.Handle, _handle, index,
                    (void*)startGcHandle.AddrOfPinnedObject(), (void*)endGcHandle.AddrOfPinnedObject(), &int_empty));
            }
            finally
            {
                startGcHandle.Free();
                endGcHandle.Free();
            }

            return (Encoding.ASCII.GetString(start),Encoding.ASCII.GetString(end), false);
        }

        public (string, string, bool) non_empty_domain_var(string name)
        {
            var dim = Schema().Domain().Dimension(name);
            if (!EnumUtil.is_string_type(dim.Type()))
            {
                throw new ErrorException("Array.non_empty_domain_var, not string dimension for name:" + name);
            }
            ulong start_size;
            ulong end_size;
            var int_empty = 1;
            var ms_name = new MarshaledString(name);

            _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_name(_ctx.Handle, _handle, ms_name, &start_size, &end_size, &int_empty));

            if (int_empty > 0)
            {
                return (string.Empty,string.Empty, true);
            }

            var start = new byte[(int)start_size];
            var end = new byte[(int)end_size];

            var startGcHandle = GCHandle.Alloc(start, GCHandleType.Pinned);
            var endGcHandle = GCHandle.Alloc(end, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_name(_ctx.Handle, _handle, ms_name,
                    (void*)startGcHandle.AddrOfPinnedObject(), (void*)endGcHandle.AddrOfPinnedObject(), &int_empty));
            }
            finally
            {
                startGcHandle.Free();
                endGcHandle.Free();
            }

            return new (Encoding.ASCII.GetString(start), Encoding.ASCII.GetString(end), false);
        }

        /// <summary>
        /// Get uri.
        /// </summary>
        /// <returns></returns>
        public string Uri()
        {
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                _ctx.handle_error(Methods.tiledb_array_get_uri(_ctx.Handle, _handle, p_result));
            }

            return ms_result;
        }

        /// <summary>
        /// Get encryption type of an array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static EncryptionType encryption_type(Context ctx, string uri)
        {
            var ms_uri = new MarshaledString(uri);
            tiledb_encryption_type_t tiledb_encryption_type;
            ctx.handle_error(Methods.tiledb_array_encryption_type(ctx.Handle, ms_uri, &tiledb_encryption_type));
            return (EncryptionType)tiledb_encryption_type;
        }

        /// <summary>
        /// Put metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void put_metadata<T>(string key, T[] data) where T: struct
        {
            if (string.IsNullOrEmpty(key) || data == null || data.Length == 0)
            {
                throw new ArgumentException("Array.put_metadata, null or empty key-value!");
            }
            var tiledb_datatype = EnumUtil.to_tiledb_datatype(typeof(T));
            if (tiledb_datatype == tiledb_datatype_t.TILEDB_ANY)
            {
                throw new ArgumentException("Array.put_metadata, not supported datatype!");
            }
            var val_num = (uint)data.Length;
            var ms_key = new MarshaledString(key);
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_array_put_metadata(_ctx.Handle, _handle, ms_key, tiledb_datatype, val_num,
                    (void*)dataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Put string metadata.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void put_metadata(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Array.put_metadata, null or empty key-value!");
            }
            var tiledb_datatype = tiledb_datatype_t.TILEDB_STRING_ASCII;
            var data = Encoding.ASCII.GetBytes(value);
            var val_num = (uint)data.Length;
            var ms_key = new MarshaledString(key);
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_array_put_metadata(_ctx.Handle, _handle, ms_key, tiledb_datatype, val_num,
                    (void*)dataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Delete metadata.
        /// </summary>
        /// <param name="key"></param>
        public void delete_metadata(string key)
        {
            var ms_key = new MarshaledString(key);
            _ctx.handle_error(Methods.tiledb_array_delete_metadata(_ctx.Handle, _handle, ms_key));
        }

        private (tiledb_datatype_t, uint, byte[]) get_metadata(string key)
        {
            void* value_p;
            var ms_key = new MarshaledString(key);
            var datatype = tiledb_datatype_t.TILEDB_ANY;
            uint value_num = 0;
            _ctx.handle_error(Methods.tiledb_array_get_metadata(_ctx.Handle, _handle, ms_key, &datatype, &value_num,
                &value_p));
            var size = (int)(value_num * EnumUtil.tiledb_datatype_size(datatype));
            var fill_span = new ReadOnlySpan<byte>(value_p, size);
            return (datatype, value_num, fill_span.ToArray());
        }

        /// <summary>
        /// Get metadata list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public (tiledb_datatype_t, uint, T[]) Metadata<T>(string key) where T : struct
        {
            var (dataType, valueNum, value) = get_metadata(key);
            Span<byte> valueSpan = value;
            var span = MemoryMarshal.Cast<byte, T>(valueSpan);
            return (dataType, valueNum, span.ToArray());
        }

        /// <summary>
        /// Get string metadata
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Metadata(string key)
        {
            var (_, _, byte_array) = get_metadata(key);
            return Encoding.ASCII.GetString(byte_array);
        }

        /// <summary>
        /// Get number of metadata.
        /// </summary>
        /// <returns></returns>
        public ulong metadata_num()
        {
            ulong num;
            _ctx.handle_error(Methods.tiledb_array_get_metadata_num(_ctx.Handle, _handle, &num));
            return num;
        }

        private (tiledb_datatype_t, uint, string, byte[]) get_metadata_from_index(ulong index)
        {
            void* value_p;
            var ms_key = new MarshaledStringOut();
            var dataType = tiledb_datatype_t.TILEDB_ANY;
            uint valueNum = 0;
            fixed (sbyte** p_key = &ms_key.Value)
            {
                uint key_len;
                _ctx.handle_error(Methods.tiledb_array_get_metadata_from_index(_ctx.Handle, _handle, index, p_key,
                    &key_len, &dataType, &valueNum, &value_p));
            }
            var size = (int)(valueNum * EnumUtil.tiledb_datatype_size(dataType));
            var fill_span = new ReadOnlySpan<byte>(value_p, size);
            return (dataType, valueNum, ms_key, fill_span.ToArray());
        }

        /// <summary>
        /// Get metadata from index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public ArrayMetadata<T> metadata_from_index<T>(ulong index) where T : struct
        {
            var (dataType, valueNum, key, value) = get_metadata_from_index(index);
            Span<byte> valueSpan = value;
            return new ArrayMetadata<T>()
            {
                Key = key,
                KeyLen = (uint)key.Length,
                Datatype = dataType,
                ValueNum = valueNum,
                Value = MemoryMarshal.Cast<byte, T>(valueSpan)
            };
        }

        /// <summary>
        /// Get metadata keys.
        /// </summary>
        /// <returns></returns>
        public string[] metadata_keys()
        {
            var num = metadata_num();
            var ret = new string[(int)num];
            for (ulong i = 0; i < num; ++i)
            {
                var (_, _, key, _) = get_metadata_from_index(i);
                ret[i] = key;
            }
            return ret;
        }

        /// <summary>
        /// Test if a metadata with key exists or not.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Tuple<bool, DataType> has_metadata(string key)
        {
            var ms_key = new MarshaledString(key);
            tiledb_datatype_t tiledb_datatype;
            var has_key = 0;
            _ctx.handle_error(Methods.tiledb_array_has_metadata_key(_ctx.Handle, _handle, ms_key, &tiledb_datatype, &has_key));

            return new Tuple<bool,DataType>(has_key>0 , (DataType)tiledb_datatype);
        }

        /// <summary>
        /// Consolidate metadata.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="config"></param>
        public static void consolidate_metadata(Context ctx, string uri, Config config)
        {
            var ms_uri = new MarshaledString(uri);
            ctx.handle_error(Methods.tiledb_array_consolidate_metadata(ctx.Handle, ms_uri, config.Handle));
        }
        /// <summary>
        /// Vacuum the array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="path"></param>
        public static ArraySchema load_array_schema(Context ctx, string path)
        {
            var ms_path = new MarshaledString(path);
            tiledb_array_schema_t* tiledb_array_schema_p;
            ctx.handle_error(Methods.tiledb_array_schema_load(ctx.Handle, ms_path, &tiledb_array_schema_p));
            return new ArraySchema(ctx, tiledb_array_schema_p);
        }
        #endregion capi functions

    }//class

}//namespace