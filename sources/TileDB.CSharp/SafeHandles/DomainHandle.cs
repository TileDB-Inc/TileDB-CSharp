using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    internal unsafe class DomainHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public DomainHandle(ContextHandle hcontext) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_domain_t* domain;
            Methods.tiledb_domain_alloc(hcontext, &domain);

            if (domain == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(null);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_domain_t*)handle;
            Methods.tiledb_domain_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_domain_t* h) { SetHandle((IntPtr)h); }
        private protected DomainHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(DomainHandle h) => h.handle;
        public static implicit operator tiledb_domain_t*(DomainHandle h) => (tiledb_domain_t*)h.handle;
        public static implicit operator DomainHandle(tiledb_domain_t* value) => new DomainHandle((IntPtr)value);
    }
}
