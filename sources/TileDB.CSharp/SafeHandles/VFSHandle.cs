using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class VFSHandle : SafeHandle
    {
        public VFSHandle(ContextHandle hcontext, ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_vfs_t* vfs;
            Methods.tiledb_vfs_alloc(hcontext, hconfig, &vfs);

            if (vfs == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(vfs);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            tiledb_vfs_t* p = (tiledb_vfs_t*)handle;
            TileDB.Interop.Methods.tiledb_vfs_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private protected void InitHandle(tiledb_vfs_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => this.handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_vfs_t> Acquire() => new(this);
    }
}
