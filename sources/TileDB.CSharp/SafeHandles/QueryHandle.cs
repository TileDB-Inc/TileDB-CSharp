using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    internal unsafe class QueryHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public QueryHandle(ContextHandle contextHandle, ArrayHandle arrayHandle, tiledb_query_type_t queryType) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_query_t* query;
            Methods.tiledb_query_alloc(contextHandle, arrayHandle, queryType, &query);

            if (query == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(query);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_query_t*)handle;
            Methods.tiledb_query_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_query_t* h) { SetHandle((IntPtr)h); }
        private protected QueryHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(QueryHandle h) => h.handle;
        public static implicit operator tiledb_query_t*(QueryHandle h) => (tiledb_query_t*)h.handle;
        public static implicit operator QueryHandle(tiledb_query_t* value) => new QueryHandle((IntPtr)value);
    }
}
