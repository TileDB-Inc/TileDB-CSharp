using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class ConfigHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public ConfigHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_config_t* config;
            tiledb_error_t* error;
            Methods.tiledb_config_alloc(&config, &error);

            if (config == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(config);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_config_t*)handle;
            Methods.tiledb_config_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_config_t* h) { SetHandle((IntPtr)h); }
        private protected ConfigHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ConfigHandle h) => h.handle;
        public static implicit operator tiledb_config_t*(ConfigHandle h) => (tiledb_config_t*)h.handle;
        public static implicit operator ConfigHandle(tiledb_config_t* value) => new ConfigHandle((IntPtr)value);
    }
}
