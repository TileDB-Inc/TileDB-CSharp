using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles
{
    internal unsafe sealed class ArraySchemaEvolutionHandle : SafeHandle
    {
        public ArraySchemaEvolutionHandle() : base(IntPtr.Zero, true) { }

        public ArraySchemaEvolutionHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

        public static ArraySchemaEvolutionHandle CreateUnowned(tiledb_array_schema_t* array) => new((IntPtr)array, ownsHandle: false);

        public static ArraySchemaEvolutionHandle Create(Context context)
        {
            var handle = new ArraySchemaEvolutionHandle();
            var successful = false;
            tiledb_array_schema_evolution_t* evolution = null;
            try
            {
                using (var contextHandle = context.Handle.Acquire())
                {
                    context.handle_error(Methods.tiledb_array_schema_evolution_alloc(contextHandle, &evolution));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(evolution);
                }
            }

            return handle;
        }

        protected override bool ReleaseHandle()
        {
            fixed (IntPtr* p = &handle)
            {
                Methods.tiledb_array_schema_evolution_free((tiledb_array_schema_evolution_t**)p);
            }
            return true;
        }

        private void InitHandle(tiledb_array_schema_evolution_t* h) => SetHandle((IntPtr)h);
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_array_schema_evolution_t> Acquire() => new(this);
    }
}
