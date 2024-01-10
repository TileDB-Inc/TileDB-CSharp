using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class ConfigIteratorHandle : SafeHandle
{
    public ConfigIteratorHandle() : base(IntPtr.Zero, true) { }

    public ConfigIteratorHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static ConfigIteratorHandle Create(ConfigHandle hconfig, string prefix)
    {
        var handle = new ConfigIteratorHandle();
        var successful = false;
        tiledb_config_iter_t* config_iter = null;
        try
        {
            using var config = hconfig.Acquire();
            tiledb_error_t* error;
            using var ms_prefix = new MarshaledString(prefix);
            ErrorHandling.ThrowOnError(Methods.tiledb_config_iter_alloc(config, ms_prefix, &config_iter, &error));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(config_iter);
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
            Methods.tiledb_config_iter_free((tiledb_config_iter_t**)p);
        }
        return true;
    }

    internal void InitHandle(tiledb_config_iter_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_config_iter_t> Acquire() => new(this);
}
