using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class FilterListHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public FilterListHandle(ContextHandle hcontext) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_filter_list_t* filterList;
            Methods.tiledb_filter_list_alloc(hcontext, &filterList);

            if (filterList == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(filterList);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_filter_list_t*)handle;
            Methods.tiledb_filter_list_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_filter_list_t* h) { SetHandle((IntPtr)h); }
        private protected FilterListHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(FilterListHandle h) => h.handle;
        public static implicit operator tiledb_filter_list_t*(FilterListHandle h) => (tiledb_filter_list_t*)h.handle;
        public static implicit operator FilterListHandle(tiledb_filter_list_t* value) => new FilterListHandle((IntPtr)value);
    }
}
