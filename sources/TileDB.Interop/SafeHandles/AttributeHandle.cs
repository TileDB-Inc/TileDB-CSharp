using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class AttributeHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public AttributeHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_attribute_t* attribute;
            var ms_name = new MarshaledString(name);
            Methods.tiledb_attribute_alloc(hcontext, ms_name, datatype, &attribute);

            if (attribute == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(attribute);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_attribute_t*)handle;
            Methods.tiledb_attribute_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_attribute_t* h) { SetHandle((IntPtr)h); }
        private protected AttributeHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(AttributeHandle h) => h.handle;
        public static implicit operator tiledb_attribute_t*(AttributeHandle h) => (tiledb_attribute_t*)h.handle;
        public static implicit operator AttributeHandle(tiledb_attribute_t* value) => new AttributeHandle((IntPtr)value);
    }
}
