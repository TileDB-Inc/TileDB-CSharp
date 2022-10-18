using System;
using TileDB.CSharp.Marshalling.SafeHandles;
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
            _handle = ArraySchemaEvolutionHandle.Create(_ctx);
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
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var attrHandle = attr.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_schema_evolution_add_attribute(ctxHandle, handle, attrHandle));
        }

        /// <summary>
        /// Drop an attribute from an ArraySchemaEvolution object
        /// </summary>
        /// <param name="attrName">String name of attribute to drop from the schema</param>
        public void DropAttribute(string attrName)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var msAttrName = new MarshaledString(attrName);
            _ctx.handle_error(Methods.tiledb_array_schema_evolution_drop_attribute(ctxHandle, handle, msAttrName));
        }

        /// <summary>
        /// Drop an attribute from ArraySchemaEvolution.
        /// Calls DropAttribute(string) using Name property of attr
        /// </summary>
        /// <param name="attr">Fully constructed attribute to drop</param>
        public void DropAttribute(Attribute attr) => DropAttribute(attr.Name());

        /// <summary>
        /// Set timestamp range for ArraySchemaEvolution
        /// </summary>
        /// <param name="high">High value of timestamp range</param>
        /// <param name="low">Low value of timestamp range</param>
        public void SetTimeStampRange(ulong high, ulong low)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(
                Methods.tiledb_array_schema_evolution_set_timestamp_range(ctxHandle, handle, low, high));
        }

        /// <summary>
        /// Apply the ArraySchemaEvolution to an existing array
        /// </summary>
        /// <param name="uri">Uri of existing array to apply evolution</param>
        public void EvolveArray(string uri)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var msUri = new MarshaledString(uri);
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_evolve(ctxHandle, msUri, handle));
        }
    }
}
