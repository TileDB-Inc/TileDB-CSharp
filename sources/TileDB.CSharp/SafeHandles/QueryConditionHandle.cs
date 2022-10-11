using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    internal unsafe class QueryConditionHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public QueryConditionHandle(ContextHandle contextHandle) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_query_condition_t* queryCondition;
            Methods.tiledb_query_condition_alloc(contextHandle, &queryCondition);

            if (queryCondition == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(queryCondition);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_query_condition_t*)handle;
            Methods.tiledb_query_condition_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_query_condition_t* h) { SetHandle((IntPtr)h); }
        private protected QueryConditionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(QueryConditionHandle h) => h.handle;
        public static implicit operator tiledb_query_condition_t*(QueryConditionHandle h) => (tiledb_query_condition_t*)h.handle;
        public static implicit operator QueryConditionHandle(tiledb_query_condition_t* value) => new QueryConditionHandle((IntPtr)value);
    }
}
