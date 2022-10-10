using System;
using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public unsafe class ArraySchemaEvolutionHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArraySchemaEvolutionHandle(ContextHandle contextHandle) : base(IntPtr.Zero, ownsHandle: true)
        {
            tiledb_array_schema_evolution_t* evolution;
            Methods.tiledb_array_schema_evolution_alloc(contextHandle, &evolution);

            if (evolution == null)
            {
                throw new Exception("Failed to allocate ArraySchemaEvolutionHandle!");
            }
            SetHandle(evolution);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            var p = (tiledb_array_schema_evolution_t*)handle;
            Methods.tiledb_array_schema_evolution_free(&p);
            SetHandle(IntPtr.Zero);
            return true;
        }

        // Conversions, getters, operators
        public ulong Get() => (ulong)handle;
        private protected void SetHandle(tiledb_array_schema_evolution_t* h) => SetHandle((IntPtr)h);
        private protected ArraySchemaEvolutionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArraySchemaEvolutionHandle h) => h.handle;
        public static implicit operator tiledb_array_schema_evolution_t*(ArraySchemaEvolutionHandle h) =>
            (tiledb_array_schema_evolution_t*)h.handle;
        public static implicit operator ArraySchemaEvolutionHandle(tiledb_array_schema_evolution_t* value) =>
            new ArraySchemaEvolutionHandle((IntPtr)value);
    }
}
