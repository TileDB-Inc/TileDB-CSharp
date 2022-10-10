using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class GroupHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public GroupHandle(ContextHandle contextHandle, sbyte* uri) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_group_t* group;
            Methods.tiledb_group_alloc(contextHandle, uri, &group);

            if (group == null)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(group);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_group_t*)handle;
            Methods.tiledb_group_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_group_t* h) { SetHandle((IntPtr)h); }
        private protected GroupHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(GroupHandle h) => h.handle;
        public static implicit operator tiledb_group_t*(GroupHandle h) => (tiledb_group_t*)h.handle;
        public static implicit operator GroupHandle(tiledb_group_t* value) => new GroupHandle((IntPtr)value);
    }
}
