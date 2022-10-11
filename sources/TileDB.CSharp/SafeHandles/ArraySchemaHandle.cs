using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ArraySchemaHandle : SafeHandle
    {
        public ArraySchemaHandle() : base(IntPtr.Zero, true) { }

        public ArraySchemaHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static ArraySchemaHandle CreateUnowned(tiledb_array_schema_t* schema) => new((IntPtr)schema, ownsHandle: false);

        public static ArraySchemaHandle Create(Context context, tiledb_array_type_t arrayType)
        {
            var handle = new ArraySchemaHandle();
            bool successful = false;
            tiledb_array_schema_t* schema = null;
            try
            {
                using (var ctx = context.Handle.Acquire())
                {
                    context.handle_error(Methods.tiledb_array_schema_alloc(ctx, arrayType, &schema));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(schema);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_array_schema_t*)handle;
            Methods.tiledb_array_schema_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        private void InitHandle(tiledb_array_schema_t* h) { SetHandle((IntPtr)h); }
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_array_schema_t> Acquire() => new(this);
    }
}
