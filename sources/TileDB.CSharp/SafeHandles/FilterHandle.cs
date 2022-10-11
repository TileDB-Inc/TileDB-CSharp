using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class FilterHandle : SafeHandle
    {
        public FilterHandle(ContextHandle hcontext, tiledb_filter_type_t filterType) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_filter_t* filter;
            Methods.tiledb_filter_alloc(hcontext, filterType, &filter);

            if (filter == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(filter);
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
