using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles
{
    internal unsafe sealed class DimensionLabelHandle : SafeHandle
    {
        public DimensionLabelHandle() : base(IntPtr.Zero, true) { }

        public DimensionLabelHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        protected override bool ReleaseHandle()
        {
            fixed (IntPtr* p = &handle)
            {
                Methods.tiledb_dimension_label_free((tiledb_dimension_label_t**)p);
            }
            return true;
        }

        internal void InitHandle(tiledb_dimension_label_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_dimension_label_t> Acquire() => new(this);
    }
}
