using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class ArrayHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArrayHandle(ContextHandle contextHandle, sbyte* uri) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_array_t* array;
            Methods.tiledb_array_alloc(contextHandle, uri, &array);

            if (array == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(array);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_array_t*)handle;
            Methods.tiledb_array_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_array_t* h) { SetHandle((IntPtr)h); }
        private protected ArrayHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArrayHandle h) => h.handle;
        public static implicit operator tiledb_array_t*(ArrayHandle h) => (tiledb_array_t*)h.handle;
        public static implicit operator ArrayHandle(tiledb_array_t* value) => new ArrayHandle((IntPtr)value);
    }
}
