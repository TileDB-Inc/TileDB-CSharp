using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class Domain : IDisposable 
    {
        private TileDB.Interop.DomainHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;


        public Domain(Context ctx) 
        {
            ctx_ = ctx;
            handle_ = new TileDB.Interop.DomainHandle(ctx_.Handle);
        }

        internal Domain(Context ctx, TileDB.Interop.DomainHandle handle) 
        {
            ctx_ = ctx;
            handle_ = handle;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!disposed_)
            {
                if (disposing && (!handle_.IsInvalid))
                {
                    handle_.Dispose();
                }

                disposed_ = true;
            }

        }

        internal TileDB.Interop.DomainHandle Handle
        {
            get { return handle_; }
        }

        #region capi functions

        /// <summary>
        /// Get type of homogenous dimensions. Will throw exception for heterogenous dimensions.
        /// </summary>
        /// <returns></returns>
        public TileDB.DataType type()
        {
            var tiledb_datatype = TileDB.Interop.tiledb_datatype_t.TILEDB_ANY;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_domain_get_type(ctx_.Handle, handle_, &tiledb_datatype));
            return (TileDB.DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get number of dimensions.
        /// </summary>
        /// <returns></returns>
        public uint ndim()
        { 
            uint ndim = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_domain_get_ndim(ctx_.Handle, handle_, &ndim));
            return ndim;
        }

        /// <summary>
        /// Add a dimension.
        /// </summary>
        /// <param name="d"></param>
        public void add_dimension(Dimension d)
        {
            //        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
            //        [return: NativeTypeName("int32_t")]
            //        public static extern int tiledb_domain_add_dimension(tiledb_ctx_t* ctx, tiledb_domain_t* domain, tiledb_dimension_t* dim);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_domain_add_dimension(ctx_.Handle, handle_, d.Handle));
            return;
        }

        /// <summary>
        /// Add dimensions.
        /// </summary>
        /// <param name="d"></param>
        public void add_dimensions(params Dimension[] d)
        {
            for (int idx = 0; idx < d.Length; idx++)
            {
                add_dimension(d[idx]);
            }
        }

 


        /// <summary>
        /// Get a dimension from index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Dimension dimension(uint index)
        {
            TileDB.Interop.tiledb_dimension_t* dimension_p = null;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_domain_get_dimension_from_index(ctx_.Handle, handle_, index, &dimension_p));

            if (dimension_p == null)
            {
                throw new TileDB.ErrorException("Dimension.dimension_from_index, dimension pointer is null");
            }

            return new Dimension(ctx_, dimension_p);
        }

        /// <summary>
        /// Get a dimension from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Dimension dimension(string name)
        {
            TileDB.Interop.tiledb_dimension_t* dimension_p = null;
            var ms_name = new TileDB.Interop.MarshaledString(name);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_domain_get_dimension_from_name(ctx_.Handle, handle_, ms_name, &dimension_p));

            if (dimension_p == null)
            {
                throw new TileDB.ErrorException("Dimension.dimension_from_name, dimension pointer is null");
            }

            return new Dimension(ctx_, dimension_p);
        }

        /// <summary>
        /// Test if a dimension with name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool has_dimension(string name)
        {
            var has_dim = 0;
            var ms_name = new TileDB.Interop.MarshaledString(name);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_domain_has_dimension(ctx_.Handle, handle_, ms_name, &has_dim));
            return has_dim > 0;
        }

        #endregion capi functions

    
  

    }//class
}//namespace