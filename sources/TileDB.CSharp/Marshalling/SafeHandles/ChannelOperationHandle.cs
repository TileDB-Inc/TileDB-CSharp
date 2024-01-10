using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class ChannelOperationHandle : SafeHandle
{
    // Keep the context handle around until this handle gets freed.
    private SafeHandleHolder<tiledb_ctx_t> _ctx;

    public ChannelOperationHandle() : base(IntPtr.Zero, true) { }

    public ChannelOperationHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    protected override bool ReleaseHandle()
    {
        fixed (IntPtr* p = &handle)
        {
            Methods.tiledb_aggregate_free(_ctx, (tiledb_channel_operation_t**)p);
        }
        _ctx.Dispose();
        return true;
    }

    internal void InitHandle(Context ctx, tiledb_channel_operation_t* h)
    {
        _ctx = ctx.Handle.Acquire();
        SetHandle((IntPtr)h);
    }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_channel_operation_t> Acquire() => new(this);
}
