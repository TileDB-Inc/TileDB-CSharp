using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ConfigIteratorHandle : SafeHandle
    {
        public ConfigIteratorHandle(ConfigHandle hconfig, string prefix) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_config_iter_t* config_iter;
            tiledb_error_t* error;
            var ms_prefix = new MarshaledString(prefix);
            Methods.tiledb_config_iter_alloc(hconfig, ms_prefix, &config_iter, &error);

            if (config_iter == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(config_iter);
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
