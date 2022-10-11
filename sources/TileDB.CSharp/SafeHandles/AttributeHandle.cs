using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class AttributeHandle : SafeHandle
    {
        public AttributeHandle() : base(IntPtr.Zero, true) { }

        public AttributeHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static AttributeHandle CreateUnowned(tiledb_attribute_t* schema) => new((IntPtr)schema, ownsHandle: false);

        public AttributeHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_attribute_t* attribute;
            var ms_name = new MarshaledString(name);
            Methods.tiledb_attribute_alloc(hcontext, ms_name, datatype, &attribute);

            if (attribute == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(attribute);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_attribute_t*)handle;
            Methods.tiledb_attribute_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_attribute_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_attribute_t> Acquire() => new(this);
    }
}
