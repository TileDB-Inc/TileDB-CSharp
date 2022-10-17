using System;
using System.Runtime.CompilerServices;
using System.Text;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public unsafe sealed class FragmentInfo : IDisposable
    {
        private readonly Context _ctx;
        private readonly FragmentInfoHandle _handle;

        internal FragmentInfoHandle Handle => _handle;

        public FragmentInfo(Context ctx, string uri)
        {
            _ctx = ctx;
            using var uri_ms = new MarshaledString(uri);
            _handle = FragmentInfoHandle.Create(ctx, uri_ms);
        }

        public void Dispose()
        {
            _handle.Dispose();
        }

        public void Load()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_fragment_info_load(ctxHandle, handle));
        }

        public void LoadWithKey(EncryptionType encryptionType, ReadOnlySpan<byte> key)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            fixed (byte* keyPtr = key)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_load_with_key(ctxHandle, handle, (tiledb_encryption_type_t)encryptionType, keyPtr, (uint)key.Length));
            }
        }

        public void SetConfig(Config config)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var configHandle = config.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_fragment_info_set_config(ctxHandle, handle, configHandle));
        }

        public uint FragmentCount
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                uint result;
                _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_num(ctxHandle, handle, &result));
                return result;
            }
        }

        public uint FragmentToVacuumCount
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                uint result;
                _ctx.handle_error(Methods.tiledb_fragment_info_get_to_vacuum_num(ctxHandle, handle, &result));
                return result;
            }
        }

        public uint FragmentWithUnconsolidatedMetadataCount
        {
            get
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                uint result;
                _ctx.handle_error(Methods.tiledb_fragment_info_get_unconsolidated_metadata_num(ctxHandle, handle, &result));
                return result;
            }
        }

        public ArraySchema GetSchema(uint fragmentIndex)
        {
            var ctx = _ctx;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_array_schema_t* schema;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_array_schema(ctxHandle, handle, fragmentIndex, &schema));
            return new ArraySchema(ctx, ArraySchemaHandle.CreateUnowned(schema));
        }

        public string GetSchemaName(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var name = new MarshaledStringOut();
            fixed (sbyte** name_ptr = &name.Value)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_array_schema_name(ctxHandle, handle, fragmentIndex, name_ptr));
            }
            return name;
        }

        public ulong GetCellsWritten(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_cell_num(ctxHandle, handle, fragmentIndex, &result));
            return result;
        }

        public string GetFragmentToVacuumUri(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var uri = new MarshaledStringOut();
            fixed (sbyte** uri_ptr = &uri.Value)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_to_vacuum_uri(ctxHandle, handle, fragmentIndex, uri_ptr));
            }
            return uri;
        }

        public string GetFragmentName(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var uri = new MarshaledStringOut();
            fixed (sbyte** uri_ptr = &uri.Value)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_name(ctxHandle, handle, fragmentIndex, uri_ptr));
            }
            return uri;
        }

        public string GetFragmentUri(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var uri = new MarshaledStringOut();
            fixed (sbyte** uri_ptr = &uri.Value)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_uri(ctxHandle, handle, fragmentIndex, uri_ptr));
            }
            return uri;
        }

        public uint GetFormatVersion(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_version(ctxHandle, handle, fragmentIndex, &result));
            return result;
        }

        public ulong GetFragmentSize(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_size(ctxHandle, handle, fragmentIndex, &result));
            return result;
        }

        public bool HasConsolidatedMetadata(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int result;
            _ctx.handle_error(Methods.tiledb_fragment_info_has_consolidated_metadata(ctxHandle, handle, fragmentIndex, &result));
            return result == 1;
        }

        public bool IsDense(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_dense(ctxHandle, handle, fragmentIndex, &result));
            return result == 1;
        }

        public bool IsSparse(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_sparse(ctxHandle, handle, fragmentIndex, &result));
            return result == 1;
        }

        public (ulong Start, ulong End) GetTimestampRange(uint fragmentIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong start, end;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_timestamp_range(ctxHandle, handle, fragmentIndex, &start, &end));
            return (start, end);
        }

        public (T Start, T End) GetNonEmptyDomain<T>(uint fragmentIndex, uint dimensionIndex)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (typeof(T) == typeof(string))
                {
                    (string startStr, string endStr) = GetStringNonEmptyDomain(fragmentIndex, dimensionIndex);
                    return ((T)(object)startStr, (T)(object)endStr);
                }
                ThrowHelpers.ThrowTypeNotSupported();
                return default;
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            byte* domain = stackalloc byte[Unsafe.SizeOf<T>() * 2];
            _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_from_index(ctxHandle, handle, fragmentIndex, dimensionIndex, domain));

            var start = Unsafe.ReadUnaligned<T>(domain);
            var end = Unsafe.ReadUnaligned<T>(domain + Unsafe.SizeOf<T>());
            return (start, end);
        }

        public (T Start, T End) GetNonEmptyDomain<T>(uint fragmentIndex, string dimensionName)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (typeof(T) == typeof(string))
                {
                    (string startStr, string endStr) = GetStringNonEmptyDomain(fragmentIndex, dimensionName);
                    return ((T)(object)startStr, (T)(object)endStr);
                }
                ThrowHelpers.ThrowTypeNotSupported();
                return default;
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_dimensionName = new MarshaledString(dimensionName);
            byte* domain = stackalloc byte[Unsafe.SizeOf<T>() * 2];
            _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_from_name(ctxHandle, handle, fragmentIndex, ms_dimensionName, domain));

            var start = Unsafe.ReadUnaligned<T>(domain);
            var end = Unsafe.ReadUnaligned<T>(domain + Unsafe.SizeOf<T>());
            return (start, end);
        }

        private (string Start, string End) GetStringNonEmptyDomain(uint fragmentIndex, uint dimensionIndex)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong startSize64, endSize64;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_size_from_index(ctxHandle, handle, fragmentIndex, dimensionIndex, &startSize64, &endSize64));
            int startSize = checked((int)startSize64);
            int endSize = checked((int)endSize64);
            using var startBuf = new ScratchBuffer<byte>(startSize, stackalloc byte[128]);
            using var endBuf = new ScratchBuffer<byte>(endSize, stackalloc byte[128]);
            fixed (byte* startBufPtr = startBuf, endBufPtr = endBuf)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_from_index(ctxHandle, handle, fragmentIndex, dimensionIndex, startBufPtr, endBufPtr));
            }

            var start = Encoding.ASCII.GetString(startBuf.Span);
            var end = Encoding.ASCII.GetString(endBuf.Span);
            return (start, end);
        }

        private (string Start, string End) GetStringNonEmptyDomain(uint fragmentIndex, string dimensionName)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_dimensionName = new MarshaledString(dimensionName);
            ulong startSize64, endSize64;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_size_from_name(ctxHandle, handle, fragmentIndex, ms_dimensionName, &startSize64, &endSize64));
            int startSize = checked((int)startSize64);
            int endSize = checked((int)endSize64);
            using var startBuf = new ScratchBuffer<byte>(startSize, stackalloc byte[128]);
            using var endBuf = new ScratchBuffer<byte>(endSize, stackalloc byte[128]);
            fixed (byte* startBufPtr = startBuf, endBufPtr = endBuf)
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_from_name(ctxHandle, handle, fragmentIndex, ms_dimensionName, startBufPtr, endBufPtr));
            }

            var start = Encoding.ASCII.GetString(startBuf.Span);
            var end = Encoding.ASCII.GetString(endBuf.Span);
            return (start, end);
        }
    }
}
