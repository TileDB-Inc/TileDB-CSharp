using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
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
            var ms_uri = new MarshaledString(uri);
            tiledb_array_schema_t* array_schema_p;
            _ctx.handle_error(Methods.tiledb_filestore_schema_create(_ctx.Handle, ms_uri, &array_schema_p));
            if (array_schema_p == null)
            {
                throw new ErrorException("Array.schema, schema pointer is null");
            }
            return new ArraySchema(_ctx,array_schema_p);
        }
        
        public void URIImport(string arrayURI, string fileURI, MIMEType mimeType)
        {
            var ms_arrayURI = new MarshaledString(arrayURI);
            var ms_fileURI = new MarshaledString(fileURI);
            var tiledb_mime_type = (tiledb_mime_type_t)mimeType;
            _ctx.handle_error(Methods.tiledb_filestore_uri_import(_ctx.Handle, ms_arrayURI, ms_fileURI, tiledb_mime_type));
        }
        
        public void URIExport(string fileURI, string arrayURI)
        {
            var ms_fileURI = new MarshaledString(fileURI);
            var ms_arrayURI = new MarshaledString(arrayURI);
            _ctx.handle_error(Methods.tiledb_filestore_uri_export(_ctx.Handle, ms_fileURI, ms_arrayURI));
        }

        public void BufferImport<T>(string arrayURI, T value, ulong size, MIMEType mimeType) where T: struct
        {
            var ms_arrayURI = new MarshaledString(arrayURI);
            var data = new[] { value };
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var tiledb_mime_type = (tiledb_mime_type_t)mimeType;
            _ctx.handle_error(Methods.tiledb_filestore_buffer_import(
                _ctx.Handle, 
                ms_arrayURI, 
                (void*)dataGcHandle.AddrOfPinnedObject(),
                size,
                tiledb_mime_type));
        }
        
        public T BufferExport<T>(string arrayURI, ulong offset, ulong size) where T: struct
        {
            var ms_arrayURI = new MarshaledString(arrayURI);
            var data = new T[1];
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            _ctx.handle_error(Methods.tiledb_filestore_buffer_export(
                _ctx.Handle, 
                ms_arrayURI, 
                offset,
                (void*)dataGcHandle.AddrOfPinnedObject(),
                size));
            
            var result = data[0];
            dataGcHandle.Free();

            return result;
        }
        
        public ulong Size(string arrayURI)
        {
            var ms_arrayURI = new MarshaledString(arrayURI);
            ulong size;
            _ctx.handle_error(Methods.tiledb_filestore_size(
                _ctx.Handle, 
                ms_arrayURI,
                &size));

            return size;
        }
        
        public string MIMETypeToStr(MIMEType mimeType)
        {
            var tiledb_mime_type = (tiledb_mime_type_t)mimeType;
            var ms_result = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value)
            {
                Methods.tiledb_mime_type_to_str(tiledb_mime_type, p_result);
            }
            
            return ms_result;
        }
        
        public MIMEType MIMETypeFromStr(string str)
        {
            tiledb_mime_type_t mimeType;
            var ms_str = new MarshaledString(str);
            
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