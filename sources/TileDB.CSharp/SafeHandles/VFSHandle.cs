using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class VFSHandle : SafeHandle
    {
        public VFSHandle() : base(IntPtr.Zero, true) { }

        public VFSHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static VFSHandle CreateUnowned(tiledb_vfs_t* filterList) => new((IntPtr)filterList, ownsHandle: false);

        public static VFSHandle Create(Context context, ConfigHandle configHandle)
        {
            var handle = new VFSHandle();
            bool successful = false;
            tiledb_vfs_t* vfs = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                using var configHandleHolder = configHandle.Acquire();
                context.handle_error(Methods.tiledb_vfs_alloc(contextHandle, configHandleHolder, &vfs));
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(vfs);
                }
            }

            return handle;
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
