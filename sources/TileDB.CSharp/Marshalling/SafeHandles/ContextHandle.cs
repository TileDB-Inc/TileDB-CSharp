using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class ContextHandle : SafeHandle
{
    public ContextHandle() : base(IntPtr.Zero, true) { }

    public ContextHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static ContextHandle Create(ConfigHandle? configHandle = null)
    {
        var handle = new ContextHandle();
        var successful = false;
        tiledb_ctx_t* context = null;
        try
        {
            tiledb_error_t* p_tiledb_error;
            int status;
            using (var configHandleHolder = configHandle?.Acquire() ?? default)
            {
                status = Methods.tiledb_ctx_alloc_with_error(configHandleHolder, &context, &p_tiledb_error);
            }
            ErrorHandling.CheckLastError(&p_tiledb_error, status);
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(context);
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
            Methods.tiledb_ctx_free((tiledb_ctx_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_ctx_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_ctx_t> Acquire() => new(this);
}
