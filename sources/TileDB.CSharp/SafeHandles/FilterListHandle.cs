using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class FilterListHandle : SafeHandle
    {
        public FilterListHandle() : base(IntPtr.Zero, true) { }

        public FilterListHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static FilterListHandle CreateUnowned(tiledb_filter_list_t* filterList) => new((IntPtr)filterList, ownsHandle: false);

        public static FilterListHandle Create(Context context)
        {
            var handle = new FilterListHandle();
            bool successful = false;
            tiledb_filter_list_t* filterList = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                context.handle_error(Methods.tiledb_filter_list_alloc(contextHandle, &filterList));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(filterList);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_filter_list_t*)handle;
            Methods.tiledb_filter_list_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_filter_list_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_filter_list_t> Acquire() => new(this);
    }
}
