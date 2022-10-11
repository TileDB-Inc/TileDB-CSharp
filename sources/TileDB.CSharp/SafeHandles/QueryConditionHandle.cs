using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class QueryConditionHandle : SafeHandle
    {
        public QueryConditionHandle(ContextHandle contextHandle) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_query_condition_t* queryCondition;
            Methods.tiledb_query_condition_alloc(contextHandle, &queryCondition);

            if (queryCondition == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(queryCondition);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_query_condition_t*)handle;
            Methods.tiledb_query_condition_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_query_condition_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_query_condition_t> Acquire() => new(this);
    }
}
