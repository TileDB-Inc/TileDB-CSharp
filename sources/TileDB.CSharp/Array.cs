using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
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

        private ArrayMetadata _metadata;

        public Array(Context ctx, string uri)
        {
            _ctx = ctx;
            _uri = uri;
            var ms_uri = new MarshaledString(_uri);
            _handle = ArrayHandle.Create(_ctx, ms_uri);
            _disposed = false;
            _metadata = new ArrayMetadata(this);
        }

        internal Array(Context ctx, ArrayHandle handle)
        {
            _ctx = ctx;
            _handle = handle;
            _uri = Uri();
            _disposed = false;
            _metadata = new ArrayMetadata(this);
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

        /// <summary>
        /// Get context.
        /// </summary>
        /// <returns></returns>
        public Context Context()
        {
            return _ctx;
        }

        /// <summary>
        /// Get metadata.
        /// </summary>
        /// <returns></returns>
        public ArrayMetadata Metadata()
        {
            return _metadata;
        }

        #region capi functions
        /// <summary>
        /// Set open timestamp start, sets the subsequent Open call to use the start_timestamp of the passed value.
        /// </summary>
        /// <param name="timestampStart"></param>
        public void SetOpenTimestampStart(ulong timestampStart)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_set_open_timestamp_start(ctxHandle, handle, timestampStart));
        }

        /// <summary>
        /// Set open timestamp end.
        /// </summary>
        /// <param name="timestampEnd"></param>
        public void SetOpenTimestampEnd(ulong timestampEnd)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_set_open_timestamp_end(ctxHandle, handle, timestampEnd));
        }

        /// <summary>
        /// Get open timestamp start.
        /// </summary>
        /// <returns></returns>
        public ulong OpenTimestampStart()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong timestamp;
            _ctx.handle_error(Methods.tiledb_array_get_open_timestamp_start(ctxHandle, handle, &timestamp));
            return timestamp;
        }

        /// <summary>
        /// Get timestamp end.
        /// </summary>
        /// <returns></returns>
        public ulong OpenTimestampEnd()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong timestamp;
            _ctx.handle_error(Methods.tiledb_array_get_open_timestamp_end(ctxHandle, handle, &timestamp));
            return timestamp;
        }

        /// <summary>
        /// Open the array.
        /// </summary>
        /// <param name="queryType"></param>
        public void Open(QueryType queryType)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var tiledb_query_type = (tiledb_query_type_t)queryType;
            _ctx.handle_error(Methods.tiledb_array_open(ctxHandle, handle, tiledb_query_type));
        }
        /// <summary>
        /// ReOpen the array.
        /// </summary>
        public void Reopen()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_reopen(ctxHandle, handle));
        }

        /// <summary>
        /// Tes if the array is open or not.
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int int_open;
            _ctx.handle_error(Methods.tiledb_array_is_open(ctxHandle, handle, &int_open));
            return int_open > 0;
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
            _ctx.handle_error(Methods.tiledb_array_set_config(ctxHandle, handle, configHandle));
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
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_close(ctxHandle, handle));
        }

        /// <summary>
        /// Get array schema.
        /// </summary>
        /// <returns></returns>
        public ArraySchema Schema()
        {
            tiledb_array_schema_t* array_schema_p;
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_get_schema(ctxHandle, handle, &array_schema_p));
            }
            if (array_schema_p == null)
            {
                throw new ErrorException("Array.schema, schema pointer is null");
            }
            return new ArraySchema(_ctx, ArraySchemaHandle.CreateUnowned(array_schema_p));
        }

        /// <summary>
        /// Get query type.
        /// </summary>
        /// <returns></returns>
        public QueryType QueryType()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_query_type_t tiledb_query_type;
            _ctx.handle_error(Methods.tiledb_array_get_query_type(ctxHandle, handle, &tiledb_query_type));
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
            using var ctxHandle = ctx.Handle.Acquire();
            using var schemaHandle = schema.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_create(ctxHandle, ms_uri, schemaHandle));
        }

        /// <summary>
        /// Create an array.
        /// </summary>
        /// <param name="schema"></param>
        public void Create(ArraySchema schema)
        {
            var ms_uri = new MarshaledString(_uri);
            using var ctxHandle = _ctx.Handle.Acquire();
            using var schemaHandle = schema.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_create(ctxHandle, ms_uri, schemaHandle));
        }

        /// <summary>
        /// Apply an ArraySchemaEvolution to the schema of an array
        /// </summary>
        /// <param name="ctx">Current TileDB Context</param>
        /// <param name="schemaEvolution">Fully constructed ArraySchemaEvolution to apply</param>
        public void Evolve(Context ctx, ArraySchemaEvolution schemaEvolution)
        {
            var msUri = new MarshaledString(_uri);
            using var ctxHandle = ctx.Handle.Acquire();
            using var schemaEvolutionHandle = schemaEvolution.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_evolve(ctxHandle, msUri, schemaEvolutionHandle));
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
            using var ctxHandle = ctx.Handle.Acquire();
            using var configHandle = config.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_consolidate(ctxHandle, ms_uri, configHandle));
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
            using var ctxHandle = ctx.Handle.Acquire();
            using var configHandle = config.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_vacuum(ctxHandle, ms_uri, configHandle));
        }

        public (NonEmptyDomain, bool) NonEmptyDomain()
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
                var dimType = EnumUtil.DataTypeToType(dim.Type());

                switch (Type.GetTypeCode(dimType))
                {
                    case TypeCode.Int16:
                        {
                            (short data0, short data1, bool isEmpty) = NonEmptyDomain<short>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<short, short>(data0, data1));
                        }
                        break;
                    case TypeCode.Int32:
                        {
                            (int data0, int data1, bool isEmpty) = NonEmptyDomain<int>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<int, int>(data0, data1));
                        }
                        break;
                    case TypeCode.Int64:
                        {
                            (long data0, long data1, bool isEmpty) = NonEmptyDomain<long>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<long, long>(data0, data1));
                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            (ushort data0, ushort data1, bool isEmpty) = NonEmptyDomain<ushort>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<ushort, ushort>(data0, data1));
                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            (uint data0, uint data1, bool isEmpty) = NonEmptyDomain<uint>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<uint, uint>(data0, data1));
                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            (ulong data0, ulong data1, bool isEmpty) = NonEmptyDomain<ulong>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<ulong, ulong>(data0, data1));
                        }
                        break;
                    case TypeCode.Single:
                        {
                            (float data0, float data1, bool isEmpty) = NonEmptyDomain<float>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<float, float>(data0, data1));
                        }
                        break;
                    case TypeCode.Double:
                        {
                            (double data0, double data1, bool isEmpty) = NonEmptyDomain<double>(i);
                            if (isEmpty == false)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<double, double>(data0, data1));
                        }
                        break;
                    case TypeCode.String:
                    {
                        (string data0, string data1, bool isEmpty) = NonEmptyDomainVar(i);
                        if (isEmpty == false)
                        {
                            isEmptyDomain = false;
                        }

                        nonEmptyDomain.Add(dimName, new Tuple<string, string>(data0, data1));
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
        public (T, T, bool) NonEmptyDomain<T>(uint index) where T : struct
        {
            var datatype = EnumUtil.TypeToDataType(typeof(T));
            if (datatype != Schema().Domain().Dimension(index).Type())
            {
                throw new ArgumentException("Array.NonEmptyDomain, not valid datatype!");
            }

            var data = new[] { default(T), default(T) };
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var int_empty = 1;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_index(ctxHandle, handle, index,
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
        public (T, T, bool) NonEmptyDomain<T>(string name) where T : struct
        {
            var datatype = EnumUtil.TypeToDataType(typeof(T));
            if (datatype != Schema().Domain().Dimension(name).Type())
            {
                throw new ArgumentException("Array.NonEmptyDomain, not valid datatype!");
            }

            var ms_name = new MarshaledString(name);
            var data = new[] { default(T), default(T) };
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var int_empty = 1;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_name(ctxHandle, handle, ms_name, (void*)dataGcHandle.AddrOfPinnedObject(), &int_empty));
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
        public (string, string, bool) NonEmptyDomainVar(uint index)
        {
            var dim = Schema().Domain().Dimension(index);
            if(!EnumUtil.IsStringType(dim.Type()))
            {
                throw new ErrorException("Array.NonEmptyDomainVar, not string dimension for index:" + index);
            }
            ulong start_size;
            ulong end_size;
            var int_empty = 1;
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_index(ctxHandle, handle, index, &start_size, &end_size, &int_empty));
            }
            if (int_empty > 0) {
                return (string.Empty, string.Empty, true);
            }

            var start = new byte[(int)start_size];
            var end = new byte[(int)end_size];

            var startGcHandle = GCHandle.Alloc(start, GCHandleType.Pinned);
            var endGcHandle = GCHandle.Alloc(end, GCHandleType.Pinned);

            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_index(ctxHandle, handle, index,
                    (void*)startGcHandle.AddrOfPinnedObject(), (void*)endGcHandle.AddrOfPinnedObject(), &int_empty));
            }
            finally
            {
                startGcHandle.Free();
                endGcHandle.Free();
            }

            return (Encoding.ASCII.GetString(start),Encoding.ASCII.GetString(end), false);
        }

        public (string, string, bool) NonEmptyDomainVar(string name)
        {
            var dim = Schema().Domain().Dimension(name);
            if (!EnumUtil.IsStringType(dim.Type()))
            {
                throw new ErrorException("Array.NonEmptyDomainVar, not string dimension for name:" + name);
            }
            ulong start_size;
            ulong end_size;
            var int_empty = 1;
            var ms_name = new MarshaledString(name);

            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_name(ctxHandle, handle, ms_name, &start_size, &end_size, &int_empty));
            }

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
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_name(ctxHandle, handle, ms_name,
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
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_array_get_uri(ctxHandle, handle, p_result));
            }

            return ms_result;
        }

        /// <summary>
        /// Get encryption type of an array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static EncryptionType EncryptionType(Context ctx, string uri)
        {
            var ms_uri = new MarshaledString(uri);
            tiledb_encryption_type_t tiledb_encryption_type;
            using var ctxHandle = ctx.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_encryption_type(ctxHandle, ms_uri, &tiledb_encryption_type));
            return (EncryptionType)tiledb_encryption_type;
        }

        /// <summary>
        /// Put metadata array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void PutMetadata<T>(string key, T[] data) where T: struct
        {
            _metadata.PutMetadata<T>(key, data);
        }

        /// <summary>
        /// Put a sigle value metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="v"></param>
        public void PutMetadata<T>(string key, T v) where T : struct
        {
            _metadata.PutMetadata<T>(key, v);
        }

        /// <summary>
        /// Put string metadata.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutMetadata(string key, string value)
        {
            _metadata.PutMetadata(key, value);
        }

        /// <summary>
        /// Delete metadata.
        /// </summary>
        /// <param name="key"></param>
        public void DeleteMetadata(string key)
        {
            _metadata.DeleteMetadata(key);
        }

        /// <summary>
        /// Get metadata list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T[] GetMetadata<T>(string key) where T : struct
        {
            return _metadata.GetMetadata<T>(key);
        }

        /// <summary>
        /// Get string metadata
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetMetadata(string key)
        {
            return _metadata.GetMetadata(key);
        }

        /// <summary>
        /// Get number of metadata.
        /// </summary>
        /// <returns></returns>
        public ulong MetadataNum()
        {
            return _metadata.MetadataNum();
        }


        /// <summary>
        /// Get metadata from index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public (string key, T[] data) GetMetadataFromIndex<T>(ulong index) where T : struct
        {
            return _metadata.GetMetadataFromIndex<T>(index);
        }

        /// <summary>
        /// Get metadata keys.
        /// </summary>
        /// <returns></returns>
        public string[] MetadataKeys()
        {
            return _metadata.MetadataKeys();
        }

        /// <summary>
        /// Test if a metadata with key exists or not.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (bool has_key, DataType datatype) HasMetadata(string key)
        {
            return _metadata.HasMetadata(key);
        }

        /// <summary>
        /// Consolidate metadata.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="config"></param>
        public static void ConsolidateMetadata(Context ctx, string uri, Config config)
        {
            ArrayMetadata.ConsolidateMetadata(ctx, uri, config);
        }

        /// <summary>
        /// Load array schema at a given path
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="path"></param>
        public static ArraySchema LoadArraySchema(Context ctx, string path)
        {
            var ms_path = new MarshaledString(path);
            tiledb_array_schema_t* array_schema_p;
            using var ctxHandle = ctx.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_schema_load(ctxHandle, ms_path, &array_schema_p));
            return new ArraySchema(ctx, ArraySchemaHandle.CreateUnowned(array_schema_p));
        }
        #endregion
    }
}
