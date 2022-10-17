using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles
{
    internal unsafe sealed class ContextHandle : SafeHandle
    {
        public ContextHandle() : base(IntPtr.Zero, true) { }

        public ContextHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static ContextHandle CreateUnowned(tiledb_ctx_t* context) => new((IntPtr)context, ownsHandle: false);

        public static ContextHandle Create()
        {
            var handle = new ContextHandle();
            var successful = false;
            tiledb_ctx_t* context = null;
            try
            {
                using var configHandle = ConfigHandle.Create();
                using var configHandleHolder = configHandle.Acquire();
                Methods.tiledb_ctx_alloc(configHandleHolder, &context);
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(context);
                }
            }

            return handle;
        }

        public static ContextHandle Create(ConfigHandle configHandle)
        {
            var handle = new ContextHandle();
            var successful = false;
            tiledb_ctx_t* context = null;
            try
            {
                using var configHandleHolder = configHandle.Acquire();
                Methods.tiledb_ctx_alloc(configHandleHolder, &context);
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(context);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            fixed (IntPtr* p = &handle)
            {
                Methods.tiledb_ctx_free((tiledb_ctx_t**)p);
            }
            return true;
        }

        private void InitHandle(tiledb_ctx_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_ctx_t> Acquire() => new(this);
    }
}
