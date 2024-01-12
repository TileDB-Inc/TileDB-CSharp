using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class DimensionHandle : SafeHandle
{
    public DimensionHandle() : base(IntPtr.Zero, true) { }

    public DimensionHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static DimensionHandle Create(Context context, string name, tiledb_datatype_t datatype, void* dimDomain, void* tileExtent)
    {
        var handle = new DimensionHandle();
        var successful = false;
        tiledb_dimension_t* dimension = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            using var ms_name = new MarshaledString(name);
            context.handle_error(Methods.tiledb_dimension_alloc(contextHandle, ms_name, datatype, dimDomain, tileExtent, &dimension));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(dimension);
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
            Methods.tiledb_dimension_free((tiledb_dimension_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_dimension_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_dimension_t> Acquire() => new(this);
}
