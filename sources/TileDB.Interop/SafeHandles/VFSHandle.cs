using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class VFSHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public VFSHandle(ContextHandle hcontext, ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_vfs_t* vfs;
            Methods.tiledb_vfs_alloc(hcontext, hconfig, &vfs);

            if (vfs == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(vfs);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            tiledb_vfs_t* p = (tiledb_vfs_t*)handle;
            TileDB.Interop.Methods.tiledb_vfs_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_vfs_t* h) { SetHandle((IntPtr)h); }
        private protected VFSHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(VFSHandle h) => h.handle;
        public static implicit operator tiledb_vfs_t*(VFSHandle h) => (tiledb_vfs_t*)h.handle;
        public static implicit operator VFSHandle(tiledb_vfs_t* value) => new VFSHandle((IntPtr)value);
    }
}
