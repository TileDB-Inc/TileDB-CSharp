using System;
using System.Collections.Generic;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
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
            if (_dict.TryGetValue(key, out var result) && result is T value1)
            {
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

        private readonly ArrayMetadata _metadata;

        public Array(Context ctx, string uri)
        {
            _ctx = ctx;
            _uri = uri;
            using var ms_uri = new MarshaledString(_uri);
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
            var handle = new ConfigHandle();
            tiledb_config_t* config = null;
            var successful = false;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var arrayHandle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_array_get_config(ctxHandle, arrayHandle, &config));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(config);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }
            return new Config(handle);
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
            var handle = new ArraySchemaHandle();
            var successful = false;
            tiledb_array_schema_t* array_schema_p = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var arrayHandle = _handle.Acquire())
                {
                    _ctx.handle_error(Methods.tiledb_array_get_schema(ctxHandle, arrayHandle, &array_schema_p));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(array_schema_p);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }
            return new ArraySchema(_ctx, handle);
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
            using var ms_uri = new MarshaledString(uri);
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
            Create(_ctx, _uri, schema);
        }

        /// <summary>
        /// Applies an <see cref="ArraySchemaEvolution"/> to the schema of an array.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> to use.</param>
        /// <param name="uri">The array's URI.</param>
        /// <param name="schemaEvolution">The <see cref="ArraySchemaEvolution"/> to use.</param>
        public static void Evolve(Context ctx, string uri, ArraySchemaEvolution schemaEvolution)
        {
            using var msUri = new MarshaledString(uri);
            using var ctxHandle = ctx.Handle.Acquire();
            using var schemaEvolutionHandle = schemaEvolution.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_evolve(ctxHandle, msUri, schemaEvolutionHandle));
        }

        /// <summary>
        /// Applies an <see cref="ArraySchemaEvolution"/> to the schema of this array.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> to use.</param>
        /// <param name="schemaEvolution">The <see cref="ArraySchemaEvolution"/> to use.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Evolve(Context ctx, ArraySchemaEvolution schemaEvolution)
        {
            Evolve(ctx, _uri, schemaEvolution);
        }

        /// <summary>
        /// Applies an <see cref="ArraySchemaEvolution"/> to the schema of this array.
        /// </summary>
        /// <param name="schemaEvolution">The <see cref="ArraySchemaEvolution"/> to use.</param>
        public void Evolve(ArraySchemaEvolution schemaEvolution)
        {
            Evolve(_ctx, _uri, schemaEvolution);
        }

        /// <summary>
        /// Consolidates an array.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> to use.</param>
        /// <param name="uri">The array's URI.</param>
        /// <param name="config">Configuration parameters for the consolidation. Optional.</param>
        public static void Consolidate(Context ctx, string uri, Config? config = null)
        {
            using var ms_uri = new MarshaledString(uri);
            using var ctxHandle = ctx.Handle.Acquire();
            using var configHandle = config?.Handle.Acquire() ?? default;
            ctx.handle_error(Methods.tiledb_array_consolidate(ctxHandle, ms_uri, configHandle));
        }

        /// <summary>
        /// Consolidates the given fragments of an array into a single fragment.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> to use.</param>
        /// <param name="uri">The array's URI.</param>
        /// <param name="fragments">The names of the fragments to consolidate. They can be retrieved using <see cref="FragmentInfo.GetFragmentName"/>.</param>
        /// <param name="config">Configuration parameters for the consolidation. Optional.</param>
        public static void ConsolidateFragments(Context ctx, string uri, IReadOnlyList<string> fragments, Config? config = null)
        {
            using var ctxHandle = ctx.Handle.Acquire();
            using var ms_uri = new MarshaledString(uri);
            using var msc_fragments = new MarshaledStringCollection(fragments);
            using var configHandle = config?.Handle.Acquire() ?? default;
            ctx.handle_error(Methods.tiledb_array_consolidate_fragments(ctxHandle, ms_uri, (sbyte**)msc_fragments.Strings, (ulong)msc_fragments.Count, configHandle));
        }

        /// <summary>
        /// Vacuums an array.
        /// </summary>
        /// <param name="ctx">The <see cref="Context"/> to use.</param>
        /// <param name="uri">The array's URI.</param>
        /// <param name="config">Configuration parameters for the consolidation. Optional.</param>
        public static void Vacuum(Context ctx, string uri, Config? config = null)
        {
            using var ms_uri = new MarshaledString(uri);
            using var ctxHandle = ctx.Handle.Acquire();
            using var configHandle = config?.Handle.Acquire() ?? default;
            ctx.handle_error(Methods.tiledb_array_vacuum(ctxHandle, ms_uri, configHandle));
        }

        public (NonEmptyDomain, bool) NonEmptyDomain()
        {
            NonEmptyDomain nonEmptyDomain = new();
            using ArraySchema schema = Schema();
            using Domain domain = schema.Domain();

            var ndim = domain.NDim();
            if (ndim == 0)
            {
                return (nonEmptyDomain, true);
            }

            bool isEmptyDomain = true;
            for (uint i = 0; i < ndim; ++i)
            {
                using var dim = domain.Dimension(i);
                var dimName = dim.Name();
                var dimType = EnumUtil.DataTypeToType(dim.Type());

                switch (Type.GetTypeCode(dimType))
                {
                    case TypeCode.SByte:
                        GetDomain<sbyte>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.Int16:
                        GetDomain<short>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.Int32:
                        GetDomain<int>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.Int64:
                        GetDomain<long>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.Byte:
                        GetDomain<byte>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.UInt16:
                        GetDomain<ushort>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.UInt32:
                        GetDomain<uint>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.UInt64:
                        GetDomain<ulong>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.Single:
                        GetDomain<float>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.Double:
                        GetDomain<double>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                        break;
                    case TypeCode.String:
                        {
                            (string data0, string data1, bool isEmpty) = NonEmptyDomainVar(i);
                            if (!isEmpty)
                            {
                                isEmptyDomain = false;
                            }

                            nonEmptyDomain.Add(dimName, new Tuple<string, string>(data0, data1));
                        }
                        break;
                }
            }

            return (nonEmptyDomain, isEmptyDomain);

            static void GetDomain<T>(Array array, string dimName, uint i, NonEmptyDomain nonEmptyDomain, ref bool isEmptyDomain) where T : struct
            {
                (T data0, T data1, bool isEmpty) = array.NonEmptyDomain<T>(i);
                if (!isEmpty)
                {
                    isEmptyDomain = false;
                }
                nonEmptyDomain.Add(dimName, new Tuple<T, T>(data0, data1));
            }
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
            using (var schema = Schema())
            using (var domain = schema.Domain())
            using (var dimension = domain.Dimension(index))
            {
                if (datatype != dimension.Type())
                {
                    throw new ArgumentException("Array.NonEmptyDomain, not valid datatype!");
                }
            }

            SequentialPair<T> data;
            int int_empty;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_index(ctxHandle, handle, index,
                &data, &int_empty));

            return int_empty > 0 ? (default(T), default(T), true) : (data.First, data.Second, false);
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
            using (var schema = Schema())
            using (var domain = schema.Domain())
            using (var dimension = domain.Dimension(name))
            {
                if (datatype != dimension.Type())
                {
                    throw new ArgumentException("Array.NonEmptyDomain, not valid datatype!");
                }
            }

            using var ms_name = new MarshaledString(name);
            SequentialPair<T> data;
            int int_empty;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_name(ctxHandle, handle, ms_name, &data, &int_empty));

            return int_empty > 0 ? (default(T), default(T), true) : (data.First, data.Second, false);
        }

        /// <summary>
        /// Get string dimension domain from index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public (string, string, bool) NonEmptyDomainVar(uint index)
        {
            ulong start_size64;
            ulong end_size64;
            int int_empty;
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_index(ctxHandle, handle, index, &start_size64, &end_size64, &int_empty));
            }
            if (int_empty > 0)
            {
                return (string.Empty, string.Empty, true);
            }

            int start_size = checked((int)start_size64);
            int end_size = checked((int)end_size64);

            using var start = new ScratchBuffer<byte>(start_size, stackalloc byte[128], exactSize: true);
            using var end = new ScratchBuffer<byte>(end_size, stackalloc byte[128], exactSize: true);

            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                fixed (byte* startPtr = start, endPtr = end)
                    _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_index(ctxHandle, handle, index,
                        startPtr, endPtr, &int_empty));
            }

            return (MarshaledStringOut.GetString(start.Span), MarshaledStringOut.GetString(end.Span), false);
        }

        public (string, string, bool) NonEmptyDomainVar(string name)
        {
            ulong start_size64;
            ulong end_size64;
            int int_empty;
            using var ms_name = new MarshaledString(name);

            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_name(ctxHandle, handle, ms_name, &start_size64, &end_size64, &int_empty));
            }

            if (int_empty > 0)
            {
                return (string.Empty, string.Empty, true);
            }

            int start_size = checked((int)start_size64);
            int end_size = checked((int)end_size64);

            using var start = new ScratchBuffer<byte>(start_size, stackalloc byte[128], exactSize: true);
            using var end = new ScratchBuffer<byte>(end_size, stackalloc byte[128], exactSize: true);

            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var handle = _handle.Acquire())
            {
                fixed (byte* startPtr = start, endPtr = end)
                    _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_name(ctxHandle, handle, ms_name,
                        startPtr, endPtr, &int_empty));
            }

            return new(MarshaledStringOut.GetString(start.Span), MarshaledStringOut.GetString(end.Span), false);
        }

        /// <summary>
        /// Get uri.
        /// </summary>
        /// <returns></returns>
        public string Uri()
        {
            sbyte* result;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_get_uri(ctxHandle, handle, &result));

            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Get encryption type of an array.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static EncryptionType EncryptionType(Context ctx, string uri)
        {
            using var ms_uri = new MarshaledString(uri);
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
        public void PutMetadata<T>(string key, T[] data) where T : struct
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
        [Obsolete(Obsoletions.ConsolidateMetadataMessage, DiagnosticId = Obsoletions.ConsolidateMetadataDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        public static void ConsolidateMetadata(Context ctx, string uri, Config config)
        {
            ArrayMetadata.ConsolidateMetadata(ctx, uri, config);
        }

        /// <summary>
        /// Load array schema at a given path
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="path"></param>
        public static ArraySchema LoadArraySchema(Context ctx, string path) => ArraySchema.Load(ctx, path);
    }
}
