using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ContextHandle : SafeHandle
    {
        public ContextHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_ctx_t* context;
            var hconfig = new ConfigHandle();
            Methods.tiledb_ctx_alloc(hconfig, &context);

            if (context == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(context);
        }

        public ContextHandle(ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_ctx_t* context;
            Methods.tiledb_ctx_alloc(hconfig, &context);

            if (context == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(context);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_ctx_t*)handle;
            Methods.tiledb_ctx_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_ctx_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_ctx_t> Acquire() => new(this);
    }
}
