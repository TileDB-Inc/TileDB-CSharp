using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class FilterHandle : SafeHandle
    {
        public FilterHandle() : base(IntPtr.Zero, true) { }

        public FilterHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static FilterHandle CreateUnowned(tiledb_filter_t* array) => new((IntPtr)array, ownsHandle: false);

        public static FilterHandle Create(Context context, tiledb_filter_type_t filterType)
        {
            var handle = new FilterHandle();
            bool successful = false;
            tiledb_filter_t* filter = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                context.handle_error(Methods.tiledb_filter_alloc(contextHandle, filterType, &filter));
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(filter);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_filter_t*)handle;
            Methods.tiledb_filter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_filter_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_filter_t> Acquire() => new(this);
    }
}
