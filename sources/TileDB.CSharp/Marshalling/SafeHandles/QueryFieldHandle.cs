using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class QueryFieldHandle : SafeHandle
{
    // Keep the context handle around until this handle gets freed.
    private SafeHandleHolder<tiledb_ctx_t> _ctx;

    public QueryFieldHandle() : base(IntPtr.Zero, true) { }

    public QueryFieldHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    protected override bool ReleaseHandle()
    {
        fixed (IntPtr* p = &handle)
        {
            Methods.tiledb_query_field_free(_ctx, (tiledb_query_field_t**)p);
        }
        _ctx.Dispose();
        return true;
    }

    internal void InitHandle(Context ctx, tiledb_query_field_t* h)
    {
        _ctx = ctx.Handle.Acquire();
        SetHandle((IntPtr)h);
    }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_query_field_t> Acquire() => new(this);
}
