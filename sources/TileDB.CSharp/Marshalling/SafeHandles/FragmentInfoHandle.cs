using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class FragmentInfoHandle : SafeHandle
{
    public FragmentInfoHandle() : base(IntPtr.Zero, true) { }

    public FragmentInfoHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static FragmentInfoHandle Create(Context context, sbyte* array_uri)
    {
        var handle = new FragmentInfoHandle();
        bool successful = false;
        tiledb_fragment_info_t* fragmentInfo = null;
        try
        {
            using (var ctx = context.Handle.Acquire())
            {
                context.handle_error(Methods.tiledb_fragment_info_alloc(ctx, array_uri, &fragmentInfo));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(fragmentInfo);
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
        fixed (IntPtr* h = &handle)
        {
            Methods.tiledb_fragment_info_free((tiledb_fragment_info_t**)h);
        }
        return true;
    }

    private void InitHandle(tiledb_fragment_info_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_fragment_info_t> Acquire() => new(this);
}
