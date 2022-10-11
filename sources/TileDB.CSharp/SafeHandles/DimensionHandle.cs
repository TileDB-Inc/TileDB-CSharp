using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class DimensionHandle: SafeHandle
    {
        public DimensionHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype, void* dimDomain, void* tileExtent) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_dimension_t* dimension;
            var ms_name = new MarshaledString(name);
            Methods.tiledb_dimension_alloc(hcontext, ms_name, datatype, dimDomain, tileExtent, &dimension);

            if (dimension == null)
            {
                throw new Exception("Failed to allocate!");
            }
            InitHandle(dimension);
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_dimension_t*)handle;
            Methods.tiledb_dimension_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_dimension_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_dimension_t> Acquire() => new(this);
    }
}
