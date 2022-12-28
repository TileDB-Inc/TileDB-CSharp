using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Domain : IDisposable 
    {
        private readonly DomainHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

        public Domain(Context ctx) 
        {
            _ctx = ctx;
            _handle = DomainHandle.Create(_ctx);
        }

        internal Domain(Context ctx, DomainHandle handle) 
        {
            _ctx = ctx;
            _handle = handle;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing && (!_handle.IsInvalid))
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        internal DomainHandle Handle => _handle;

        #region capi functions
        /// <summary>
        /// Get type of homogenous dimensions. Will throw exception for heterogeneous dimensions.
        /// </summary>
        /// <returns></returns>
        public DataType Type()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_datatype_t tiledb_datatype;
            _ctx.handle_error(Methods.tiledb_domain_get_type(ctxHandle, handle, &tiledb_datatype));
            return (DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get number of dimensions.
        /// </summary>
        /// <returns></returns>
        public uint NDim()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint ndim;
            _ctx.handle_error(Methods.tiledb_domain_get_ndim(ctxHandle, handle, &ndim));
            return ndim;
        }

        /// <summary>
        /// Add a dimension.
        /// </summary>
        /// <param name="d"></param>
        public void AddDimension(Dimension d)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var dHandle = d.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_domain_add_dimension(ctxHandle, handle, dHandle));
        }

        /// <summary>
        /// Add dimensions.
        /// </summary>
        /// <param name="d"></param>
        public void AddDimensions(params Dimension[] d)
        {
            foreach (var t in d)
            {
                AddDimension(t);
            }
        }

        /// <summary>
        /// Get a dimension from index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Dimension Dimension(uint index)
        {
            var handle = new DimensionHandle();
            var successful = false;
            tiledb_dimension_t* dimension_p = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var domainHandle = _handle.Acquire())
                {
                    _ctx.handle_error(Methods.tiledb_domain_get_dimension_from_index(ctxHandle, domainHandle, index, &dimension_p));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(dimension_p);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }

            return new Dimension(_ctx, handle);
        }

        /// <summary>
        /// Get a dimension from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dimension Dimension(string name)
        {
            var handle = new DimensionHandle();
            var successful = false;
            tiledb_dimension_t* dimension_p = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var domainHandle = _handle.Acquire())
                using (var ms_name = new MarshaledString(name))
                {
                    _ctx.handle_error(Methods.tiledb_domain_get_dimension_from_name(ctxHandle, domainHandle, ms_name, &dimension_p));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(dimension_p);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }

            return new Dimension(_ctx, handle);
        }

        /// <summary>
        /// Test if a dimension with name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasDimension(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int has_dim;
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_domain_has_dimension(ctxHandle, handle, ms_name, &has_dim));
            return has_dim > 0;
        }
        #endregion
    }
}
