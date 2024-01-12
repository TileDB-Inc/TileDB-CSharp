using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class VFSFileHandle : SafeHandle
{
    public VFSFileHandle() : base(IntPtr.Zero, true) { }

    public VFSFileHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static VFSFileHandle Open(Context context, VFSHandle vfsHandle, string uri, tiledb_vfs_mode_t mode)
    {
        var handle = new VFSFileHandle();
        var successful = false;
        tiledb_vfs_fh_t* vfsFh = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            using var vfsHandleHolder = vfsHandle.Acquire();
            using var ms_uri = new MarshaledString(uri);
            context.handle_error(Methods.tiledb_vfs_open(contextHandle, vfsHandleHolder, ms_uri, mode, &vfsFh));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(vfsFh);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return handle;
    }

    protected override bool ReleaseHandle()
    {
        fixed (IntPtr* p = &handle)
        {
            Methods.tiledb_vfs_fh_free((tiledb_vfs_fh_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_vfs_fh_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_vfs_fh_t> Acquire() => new(this);
}
