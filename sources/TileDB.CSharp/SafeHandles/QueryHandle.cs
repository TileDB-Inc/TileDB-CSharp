using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class QueryHandle : SafeHandle
    {
        public QueryHandle(ContextHandle contextHandle, ArrayHandle arrayHandle, tiledb_query_type_t queryType) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_query_t* query;
            Methods.tiledb_query_alloc(contextHandle, arrayHandle, queryType, &query);

            if (query == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(query);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_query_t*)handle;
            Methods.tiledb_query_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_query_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_query_t> Acquire() => new(this);
    }
}
