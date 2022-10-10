using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class ArraySchemaHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArraySchemaHandle(ContextHandle contextHandle, tiledb_array_type_t arrayType) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_array_schema_t* schema;
            Methods.tiledb_array_schema_alloc(contextHandle, arrayType, &schema);

            if (schema == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(schema);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_array_schema_t*)handle;
            Methods.tiledb_array_schema_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_array_schema_t* h) { SetHandle((IntPtr)h); }
        private protected ArraySchemaHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArraySchemaHandle h) => h.handle;
        public static implicit operator tiledb_array_schema_t*(ArraySchemaHandle h) => (tiledb_array_schema_t*)h.handle;
        public static implicit operator ArraySchemaHandle(tiledb_array_schema_t* value) => new ArraySchemaHandle((IntPtr)value);
    }
}
