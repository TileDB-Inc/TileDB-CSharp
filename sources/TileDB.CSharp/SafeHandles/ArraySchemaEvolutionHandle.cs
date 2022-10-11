using System;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    internal unsafe class ArraySchemaEvolutionHandle : SafeHandle
    {
        public ArraySchemaEvolutionHandle(ContextHandle contextHandle) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_array_schema_evolution_t* evolution;
            Methods.tiledb_array_schema_evolution_alloc(contextHandle, &evolution);

            if (evolution == null)
            {
                throw new Exception("Failed to allocate ArraySchemaEvolutionHandle!");
            }
            InitHandle(evolution);
        }

        protected override bool ReleaseHandle()
        {
            var p = (tiledb_array_schema_evolution_t*)handle;
            Methods.tiledb_array_schema_evolution_free(&p);
            SetHandle(IntPtr.Zero);
            return true;
        }

        private void InitHandle(tiledb_array_schema_evolution_t* h) => SetHandle((IntPtr)h);
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeHandleHolder<tiledb_array_schema_evolution_t> Acquire() => new(this);
    }
}
