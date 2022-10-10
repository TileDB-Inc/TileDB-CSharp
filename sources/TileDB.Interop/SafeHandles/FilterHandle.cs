using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class FilterHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public FilterHandle(ContextHandle hcontext, tiledb_filter_type_t filterType) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_filter_t* filter;
            Methods.tiledb_filter_alloc(hcontext, filterType, &filter);

            if (filter == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(filter);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_filter_t*)handle;
            Methods.tiledb_filter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_filter_t* h) { SetHandle((IntPtr)h); }
        private protected FilterHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(FilterHandle h) => h.handle;
        public static implicit operator tiledb_filter_t*(FilterHandle h) => (tiledb_filter_t*)h.handle;
        public static implicit operator FilterHandle(tiledb_filter_t* value) => new FilterHandle((IntPtr)value);
    }
}
