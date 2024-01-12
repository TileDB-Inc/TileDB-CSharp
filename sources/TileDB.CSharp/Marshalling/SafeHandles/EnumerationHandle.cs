using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class EnumerationHandle : SafeHandle
{
    public EnumerationHandle() : base(IntPtr.Zero, true) { }

    public EnumerationHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static EnumerationHandle Create(Context context, string name, tiledb_datatype_t datatype, uint cellValNum, bool ordered, byte* data, ulong dataSize, ulong* offsets, ulong offsetsSize)
    {
        var handle = new EnumerationHandle();
        var successful = false;
        tiledb_enumeration_t* enumeration = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            using var ms_name = new MarshaledString(name);
            context.handle_error(Methods.tiledb_enumeration_alloc(contextHandle, ms_name, datatype, cellValNum,
                ordered ? 1 : 0, data, dataSize, offsets, offsetsSize, &enumeration));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(enumeration);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return handle;
    }

    public EnumerationHandle Extend(Context context, byte* data, ulong dataSize, ulong* offsets, ulong offsetsSize)
    {
        var handle = new EnumerationHandle();
        var successful = false;
        tiledb_enumeration_t* enumeration = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            using var old_enumeration_handle = Acquire();
            context.handle_error(Methods.tiledb_enumeration_extend(contextHandle, old_enumeration_handle, data,
                dataSize, offsets, offsetsSize, &enumeration));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(enumeration);
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
            Methods.tiledb_enumeration_free((tiledb_enumeration_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_enumeration_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_enumeration_t> Acquire() => new(this);
}
