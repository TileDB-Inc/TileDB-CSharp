using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles
{
    internal unsafe class DomainHandle : SafeHandle
    {
        public DomainHandle() : base(IntPtr.Zero, true) { }

        public DomainHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static DomainHandle CreateUnowned(tiledb_domain_t* schema) => new((IntPtr)schema, ownsHandle: false);

        public static DomainHandle Create(Context context)
        {
            var handle = new DomainHandle();
            var successful = false;
            tiledb_domain_t* domain = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                context.handle_error(Methods.tiledb_domain_alloc(contextHandle, &domain));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(domain);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            fixed (IntPtr* p = &handle)
            {
                Methods.tiledb_domain_free((tiledb_domain_t**)p);
            }
            return true;
        }

        private void InitHandle(tiledb_domain_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_domain_t> Acquire() => new(this);
    }
}
