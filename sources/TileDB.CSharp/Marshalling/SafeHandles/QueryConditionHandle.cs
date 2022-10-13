using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles
{
    internal unsafe class QueryConditionHandle : SafeHandle
    {
        public QueryConditionHandle() : base(IntPtr.Zero, true) { }

        public QueryConditionHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static QueryConditionHandle CreateUnowned(tiledb_query_condition_t* filterList) => new((IntPtr)filterList, ownsHandle: false);

        public static QueryConditionHandle Create(Context context)
        {
            var handle = new QueryConditionHandle();
            var successful = false;
            tiledb_query_condition_t* queryCondition = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                context.handle_error(Methods.tiledb_query_condition_alloc(contextHandle, &queryCondition));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(queryCondition);
                }
            }

            return handle;
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
