using System.ComponentModel;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class File
    {
        private readonly Context _ctx;

        public File(Context ctx)
        {
            _ctx = ctx;
        }

        public ArraySchema SchemaCreate(string uri)
        {
            var handle = new ArraySchemaHandle();
            var successful = false;
            tiledb_array_schema_t* array_schema_p = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var ms_uri = new MarshaledString(uri))
                {
                    _ctx.handle_error(Methods.tiledb_filestore_schema_create(ctxHandle, ms_uri, &array_schema_p));
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

        public void URIImport(string arrayURI, string fileURI, MIMEType mimeType)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var ms_arrayURI = new MarshaledString(arrayURI);
            using var ms_fileURI = new MarshaledString(fileURI);
            var tiledb_mime_type = (tiledb_mime_type_t)mimeType;
            _ctx.handle_error(Methods.tiledb_filestore_uri_import(ctxHandle, ms_arrayURI, ms_fileURI, tiledb_mime_type));
        }

        public void URIExport(string fileURI, string arrayURI)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var ms_fileURI = new MarshaledString(fileURI);
            using var ms_arrayURI = new MarshaledString(arrayURI);
            _ctx.handle_error(Methods.tiledb_filestore_uri_export(ctxHandle, ms_fileURI, ms_arrayURI));
        }

        public void BufferImport<T>(string arrayURI, T value, ulong size, MIMEType mimeType) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            if (size > (ulong)sizeof(T))
            {
                ThrowHelpers.ThrowTooBigSize(size);
            }
            using var ctxHandle = _ctx.Handle.Acquire();
            using var ms_arrayURI = new MarshaledString(arrayURI);
            var tiledb_mime_type = (tiledb_mime_type_t)mimeType;
            _ctx.handle_error(Methods.tiledb_filestore_buffer_import(
                ctxHandle,
                ms_arrayURI,
                &value,
                checked((nuint)size),
                tiledb_mime_type));
        }

        public T BufferExport<T>(string arrayURI, ulong offset, ulong size) where T : struct
        {
            ErrorHandling.ThrowIfManagedType<T>();
            if (size > (ulong)sizeof(T))
            {
                ThrowHelpers.ThrowTooBigSize(size);
            }
            using var ctxHandle = _ctx.Handle.Acquire();
            using var ms_arrayURI = new MarshaledString(arrayURI);
            T result;
            _ctx.handle_error(Methods.tiledb_filestore_buffer_export(
                ctxHandle,
                ms_arrayURI,
                checked((nuint)offset),
                &result,
                checked((nuint)size)));

            return result;
        }

        public ulong Size(string arrayURI)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var ms_arrayURI = new MarshaledString(arrayURI);
            nuint size;
            _ctx.handle_error(Methods.tiledb_filestore_size(
                ctxHandle,
                ms_arrayURI,
                &size));

            return size;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MIMETypeToStr(MIMEType mimeType)
        {
            var tiledb_mime_type = (tiledb_mime_type_t)mimeType;
            sbyte* result;
            Methods.tiledb_mime_type_to_str(tiledb_mime_type, &result);

            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static MIMEType MIMETypeFromStr(string str)
        {
            tiledb_mime_type_t mimeType;
            using var ms_str = new MarshaledString(str);

            unsafe
            {
                int status = TileDB.Interop.Methods.tiledb_mime_type_from_str(ms_str, &mimeType);
                if (status != (int)Status.TILEDB_OK)
                {
                    throw new System.ArgumentException("MIMETypeFromStr, Invalid string:" + str);
                }
            }
            return (MIMEType)mimeType;
        }
    }
}
