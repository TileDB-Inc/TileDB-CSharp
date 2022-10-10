using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class DimensionHandle: SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public DimensionHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype, void* dimDomain, void* tileExtent) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_dimension_t* dimension;
            var ms_name = new MarshaledString(name);
            Methods.tiledb_dimension_alloc(hcontext, ms_name, datatype, dimDomain, tileExtent, &dimension);

            if (dimension == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(dimension);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_dimension_t*)handle;
            Methods.tiledb_dimension_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_dimension_t* h) { SetHandle((IntPtr)h); }
        private protected DimensionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(DimensionHandle h) => h.handle;
        public static implicit operator tiledb_dimension_t*(DimensionHandle h) => (tiledb_dimension_t*)h.handle;
        public static implicit operator DimensionHandle(tiledb_dimension_t* value) => new DimensionHandle((IntPtr)value);
    }
}
