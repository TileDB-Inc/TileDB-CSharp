using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class SubarrayHandle : SafeHandle
{
    public SubarrayHandle() : base(IntPtr.Zero, true) { }

    public SubarrayHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static SubarrayHandle Create(Context context, ArrayHandle arrayHandle)
    {
        var handle = new SubarrayHandle();
        var successful = false;
        tiledb_subarray_t* array = null;
        try
        {
            using (var ctx = context.Handle.Acquire())
            using (var arrayHandleHolder = arrayHandle.Acquire())
            {
                context.handle_error(Methods.tiledb_subarray_alloc(ctx, arrayHandleHolder, &array));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(array);
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
            Methods.tiledb_subarray_free((tiledb_subarray_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_subarray_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_subarray_t> Acquire() => new(this);
}
