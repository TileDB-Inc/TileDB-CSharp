using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class VFSHandle : SafeHandle
{
    public VFSHandle() : base(IntPtr.Zero, true) { }

    public VFSHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static VFSHandle Create(Context context, ConfigHandle? configHandle)
    {
        var handle = new VFSHandle();
        var successful = false;
        tiledb_vfs_t* vfs = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            using var configHandleHolder = configHandle?.Acquire() ?? default;
            context.handle_error(Methods.tiledb_vfs_alloc(contextHandle, configHandleHolder, &vfs));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(vfs);
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
            Methods.tiledb_vfs_free((tiledb_vfs_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_vfs_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_vfs_t> Acquire() => new(this);
}
