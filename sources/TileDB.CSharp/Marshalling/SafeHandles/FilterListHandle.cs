using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class FilterListHandle : SafeHandle
{
    public FilterListHandle() : base(IntPtr.Zero, true) { }

    public FilterListHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static FilterListHandle Create(Context context)
    {
        var handle = new FilterListHandle();
        var successful = false;
        tiledb_filter_list_t* filterList = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            context.handle_error(Methods.tiledb_filter_list_alloc(contextHandle, &filterList));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(filterList);
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
            Methods.tiledb_filter_list_free((tiledb_filter_list_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_filter_list_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_filter_list_t> Acquire() => new(this);
}
