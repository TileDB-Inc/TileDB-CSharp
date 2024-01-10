using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class ArrayHandle : SafeHandle
{
    public ArrayHandle() : base(IntPtr.Zero, true) { }

    public ArrayHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static ArrayHandle Create(Context context, sbyte* uri)
    {
        var handle = new ArrayHandle();
        var successful = false;
        tiledb_array_t* array = null;
        try
        {
            using (var ctx = context.Handle.Acquire())
            {
                context.handle_error(Methods.tiledb_array_alloc(ctx, uri, &array));
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
            Methods.tiledb_array_free((tiledb_array_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_array_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_array_t> Acquire() => new(this);
}
