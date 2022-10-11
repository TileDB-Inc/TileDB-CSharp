using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class GroupHandle : SafeHandle
    {
        public GroupHandle(ContextHandle contextHandle, sbyte* uri) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_group_t* group;
            Methods.tiledb_group_alloc(contextHandle, uri, &group);

            if (group == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(group);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_group_t*)handle;
            Methods.tiledb_group_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_group_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_group_t> Acquire() => new(this);
    }
}
