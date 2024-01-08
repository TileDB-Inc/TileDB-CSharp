using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class GroupHandle : SafeHandle
{
    public GroupHandle() : base(IntPtr.Zero, true) { }

    public GroupHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static GroupHandle Create(Context context, sbyte* uri)
    {
        var handle = new GroupHandle();
        var successful = false;
        tiledb_group_t* group = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            context.handle_error(Methods.tiledb_group_alloc(contextHandle, uri, &group));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(group);
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
            Methods.tiledb_group_free((tiledb_group_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_group_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_group_t> Acquire() => new(this);
}
