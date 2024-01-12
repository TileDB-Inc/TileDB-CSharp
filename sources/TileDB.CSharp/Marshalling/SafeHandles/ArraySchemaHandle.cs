using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class ArraySchemaHandle : SafeHandle
{
    public ArraySchemaHandle() : base(IntPtr.Zero, true) { }

    public ArraySchemaHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static ArraySchemaHandle Create(Context context, tiledb_array_type_t arrayType)
    {
        var handle = new ArraySchemaHandle();
        var successful = false;
        tiledb_array_schema_t* schema = null;
        try
        {
            using (var ctx = context.Handle.Acquire())
            {
                context.handle_error(Methods.tiledb_array_schema_alloc(ctx, arrayType, &schema));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(schema);
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
            Methods.tiledb_array_schema_free((tiledb_array_schema_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_array_schema_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_array_schema_t> Acquire() => new(this);
}
