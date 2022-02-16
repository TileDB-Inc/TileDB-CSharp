using System;
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
            _handle = new DomainHandle(_ctx.Handle);
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
            var tiledb_datatype = tiledb_datatype_t.TILEDB_ANY;
            _ctx.handle_error(Methods.tiledb_domain_get_type(_ctx.Handle, _handle, &tiledb_datatype));
            return (DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get number of dimensions.
        /// </summary>
        /// <returns></returns>
        public uint NDim()
        { 
            uint ndim = 0;
            _ctx.handle_error(Methods.tiledb_domain_get_ndim(_ctx.Handle, _handle, &ndim));
            return ndim;
        }

        /// <summary>
        /// Add a dimension.
        /// </summary>
        /// <param name="d"></param>
        public void add_dimension(Dimension d)
        {
            _ctx.handle_error(Methods.tiledb_domain_add_dimension(_ctx.Handle, _handle, d.Handle));
        }

        /// <summary>
        /// Add dimensions.
        /// </summary>
        /// <param name="d"></param>
        public void add_dimensions(params Dimension[] d)
        {
            foreach (var t in d)
            {
                add_dimension(t);
            }
        }

        /// <summary>
        /// Get a dimension from index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Dimension Dimension(uint index)
        {
            tiledb_dimension_t* dimension_p = null;
            _ctx.handle_error(Methods.tiledb_domain_get_dimension_from_index(_ctx.Handle, _handle, index, &dimension_p));

            if (dimension_p == null)
            {
                throw new ErrorException("Dimension.dimension_from_index, dimension pointer is null");
            }

            return new Dimension(_ctx, dimension_p);
        }

        /// <summary>
        /// Get a dimension from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dimension Dimension(string name)
        {
            tiledb_dimension_t* dimension_p = null;
            var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_domain_get_dimension_from_name(_ctx.Handle, _handle, ms_name, &dimension_p));

            if (dimension_p == null)
            {
                throw new ErrorException("Dimension.dimension_from_name, dimension pointer is null");
            }

            return new Dimension(_ctx, dimension_p);
        }

        /// <summary>
        /// Test if a dimension with name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool has_dimension(string name)
        {
            var has_dim = 0;
            var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_domain_has_dimension(_ctx.Handle, _handle, ms_name, &has_dim));
            return has_dim > 0;
        }

        #endregion capi functions
    }//class
}//namespace