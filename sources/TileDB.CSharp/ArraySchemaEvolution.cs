using System;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class ArraySchemaEvolution : IDisposable
    {
        private readonly Context _ctx;
        private bool _disposed;
        private readonly ArraySchemaEvolutionHandle _handle;

        public ArraySchemaEvolution(Context ctx)
        {
            _ctx = ctx;
            _handle = new ArraySchemaEvolutionHandle(_ctx.Handle);
            _disposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing && !_handle.IsInvalid)
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        internal ArraySchemaEvolutionHandle Handle => _handle;

        /// <summary>
        /// Get context.
        /// </summary>
        /// <returns></returns>
        public Context Context() => _ctx;

        /// <summary>
        /// Add an attribute to an ArraySchemaEvolution object
        /// </summary>
        /// <param name="attr">Fully constructed Attribute to add to the schema</param>
        public void AddAttribute(Attribute attr)
        {
            _ctx.handle_error(Methods.tiledb_array_schema_evolution_add_attribute(_ctx.Handle, _handle, attr.Handle));
        }

        /// <summary>
        /// Drop an attribute from an ArraySchemaEvolution object
        /// </summary>
        /// <param name="attrName">String name of attribute to drop from the schema</param>
        public void DropAttribute(string attrName)
        {
            var msAttrName = new MarshaledString(attrName);
            _ctx.handle_error(Methods.tiledb_array_schema_evolution_drop_attribute(_ctx.Handle, _handle, msAttrName));
        }

        /// <summary>
        /// Set timestamp range for ArraySchemaEvolution
        /// </summary>
        /// <param name="high">High value of timestamp range</param>
        /// <param name="low">Low value of timestamp range</param>
        public void SetTimeStampRange(ulong high, ulong low)
        {
            _ctx.handle_error(
                Methods.tiledb_array_schema_evolution_set_timestamp_range(_ctx.Handle, _handle, low, high));
        }

        /// <summary>
        /// Apply the ArraySchemaEvolution to an existing array
        /// </summary>
        /// <param name="uri">Uri of existing array to apply evolution</param>
        public void EvolveArray(string uri)
        {
            var msUri = new MarshaledString(uri);
            _ctx.handle_error(Methods.tiledb_array_evolve(_ctx.Handle, msUri, _handle));
        }
    }
}