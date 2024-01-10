using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class FilterHandle : SafeHandle
{
    public FilterHandle() : base(IntPtr.Zero, true) { }

    public FilterHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static FilterHandle Create(Context context, tiledb_filter_type_t filterType)
    {
        var handle = new FilterHandle();
        var successful = false;
        tiledb_filter_t* filter = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            context.handle_error(Methods.tiledb_filter_alloc(contextHandle, filterType, &filter));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(filter);
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
            Methods.tiledb_filter_free((tiledb_filter_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_filter_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_filter_t> Acquire() => new(this);
}
