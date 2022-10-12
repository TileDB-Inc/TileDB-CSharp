using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ArrayHandle : SafeHandle
    {
        public ArrayHandle() : base(IntPtr.Zero, true) { }

        public ArrayHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static ArrayHandle CreateUnowned(tiledb_array_t* array) => new((IntPtr)array, ownsHandle: false);

        public static ArrayHandle Create(Context context, sbyte* uri)
        {
            var handle = new ArrayHandle();
            bool successful = false;
            tiledb_array_t* array = null;
            try
            {
                using (var ctx = context.Handle.Acquire())
                {
                    context.handle_error(Methods.tiledb_array_alloc(ctx, uri, &array));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(array);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_array_t*)handle;
            Methods.tiledb_array_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_array_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_array_t> Acquire() => new(this);
    }
}
