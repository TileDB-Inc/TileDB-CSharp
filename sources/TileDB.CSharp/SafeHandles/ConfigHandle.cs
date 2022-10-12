using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ConfigHandle : SafeHandle
    {
        public ConfigHandle() : base(IntPtr.Zero, true) { }

        public ConfigHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static ConfigHandle CreateUnowned(tiledb_config_t* schema) => new((IntPtr)schema, ownsHandle: false);

        public static ConfigHandle Create()
        {
            var handle = new ConfigHandle();
            bool successful = false;
            tiledb_config_t* config = null;
            try
            {
                tiledb_error_t* error;
                Methods.tiledb_config_alloc(&config, &error);
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(config);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_config_t*)handle;
            Methods.tiledb_config_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_config_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_config_t> Acquire() => new(this);
    }
}
