using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    internal unsafe class ConfigIteratorHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

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
            SetHandle(config_iter);
        }

        // Deallocator: call native free with CER guarantees from SafeHTha andle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_config_iter_t*)handle;
            Methods.tiledb_config_iter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_config_iter_t* h) { SetHandle((IntPtr)h); }
        private protected ConfigIteratorHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ConfigIteratorHandle h) => h.handle;
        public static implicit operator tiledb_config_iter_t*(ConfigIteratorHandle h) => (tiledb_config_iter_t*)h.handle;
        public static implicit operator ConfigIteratorHandle(tiledb_config_iter_t* value) => new ConfigIteratorHandle((IntPtr)value);
    }
}
