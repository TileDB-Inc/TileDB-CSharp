using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles
{
    internal unsafe class QueryHandle : SafeHandle
    {
        public QueryHandle() : base(IntPtr.Zero, true) { }

        public QueryHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static QueryHandle CreateUnowned(tiledb_query_t* filterList) => new((IntPtr)filterList, ownsHandle: false);

        public static QueryHandle Create(Context context, ArrayHandle arrayHandle, tiledb_query_type_t queryType)
        {
            var handle = new QueryHandle();
            var successful = false;
            tiledb_query_t* query = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                using var arrayHandleHolder = arrayHandle.Acquire();
                context.handle_error(Methods.tiledb_query_alloc(contextHandle, arrayHandleHolder, queryType, &query));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(query);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            fixed (IntPtr* p = &handle)
            {
                Methods.tiledb_query_free((tiledb_query_t**)p);
            }
            return true;
        }

        private void InitHandle(tiledb_query_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_query_t> Acquire() => new(this);
    }
}
