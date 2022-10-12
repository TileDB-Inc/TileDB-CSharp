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

        public static AttributeHandle Create(Context context, string name, tiledb_datatype_t datatype)
        {
            var handle = new AttributeHandle();
            bool successful = false;
            tiledb_attribute_t* attribute = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                var ms_name = new MarshaledString(name);
                context.handle_error(Methods.tiledb_attribute_alloc(contextHandle, ms_name, datatype, &attribute));
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(attribute);
                }
            }

            return handle;
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
