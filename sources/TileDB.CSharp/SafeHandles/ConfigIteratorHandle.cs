using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ConfigIteratorHandle : SafeHandle
    {
        public ConfigIteratorHandle() : base(IntPtr.Zero, true) { }

        public ConfigIteratorHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static ConfigIteratorHandle CreateUnowned(tiledb_config_iter_t* schema) => new((IntPtr)schema, ownsHandle: false);

        public static ConfigIteratorHandle Create(ConfigHandle hconfig, string prefix)
        {
            var handle = new ConfigIteratorHandle();
            bool successful = false;
            tiledb_config_iter_t* config_iter = null;
            try
            {
                using var config = hconfig.Acquire();
                tiledb_error_t* error;
                var ms_prefix = new MarshaledString(prefix);
                Methods.tiledb_config_iter_alloc(config, ms_prefix, &config_iter, &error);
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(config_iter);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_config_iter_t*)handle;
            Methods.tiledb_config_iter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_config_iter_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_config_iter_t> Acquire() => new(this);
    }
}
