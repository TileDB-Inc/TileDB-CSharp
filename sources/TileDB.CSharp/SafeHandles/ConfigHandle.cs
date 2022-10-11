using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ConfigHandle : SafeHandle
    {
        public ConfigHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_config_t* config;
            tiledb_error_t* error;
            Methods.tiledb_config_alloc(&config, &error);

            if (config == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(config);
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
