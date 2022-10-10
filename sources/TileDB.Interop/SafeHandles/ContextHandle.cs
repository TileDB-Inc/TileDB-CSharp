using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class ContextHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ContextHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_ctx_t* context;
            var hconfig = new ConfigHandle();
            Methods.tiledb_ctx_alloc(hconfig, &context);

            if (context == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(context);
        }

        public ContextHandle(ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_ctx_t* context;
            Methods.tiledb_ctx_alloc(hconfig, &context);

            if (context == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(context);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_ctx_t*)handle;
            Methods.tiledb_ctx_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_ctx_t* h) { SetHandle((IntPtr)h); }
        private protected ContextHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ContextHandle h) => h.handle;
        public static implicit operator tiledb_ctx_t*(ContextHandle h) => (tiledb_ctx_t*)h.handle;
        public static implicit operator ContextHandle(tiledb_ctx_t* value) => new ContextHandle((IntPtr)value);
    }
}
