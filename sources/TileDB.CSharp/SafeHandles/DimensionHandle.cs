using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class DimensionHandle : SafeHandle
    {
        public DimensionHandle() : base(IntPtr.Zero, true) { }

        public DimensionHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static DimensionHandle CreateUnowned(tiledb_dimension_t* schema) => new((IntPtr)schema, ownsHandle: false);

        public static DimensionHandle Create(Context context, string name, tiledb_datatype_t datatype, void* dimDomain, void* tileExtent)
        {
            var handle = new DimensionHandle();
            bool successful = false;
            tiledb_dimension_t* dimension = null;
            try
            {
                using var contextHandle = context.Handle.Acquire();
                var ms_name = new MarshaledString(name);
                context.handle_error(Methods.tiledb_dimension_alloc(contextHandle, ms_name, datatype, dimDomain, tileExtent, &dimension));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(dimension);
                }
            }

            return handle;
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
