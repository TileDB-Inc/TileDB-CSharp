using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class FilterList : IDisposable
    {
        private TileDB.Interop.FilterListHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;

 

        public FilterList(Context ctx) 
        {
            ctx_ = ctx;
            handle_ = new TileDB.Interop.FilterListHandle(ctx_.Handle);
        }

        internal FilterList(Context ctx, TileDB.Interop.FilterListHandle handle) 
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

        internal Interop.FilterListHandle Handle
        {
            get { return handle_; }
        }

        /// <summary>
        /// Add a filter.
        /// </summary>
        /// <param name="filter"></param>
        public void add_filter(Filter filter)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_add_filter(ctx_.Handle, handle_, filter.Handle));
        }

        /// <summary>
        /// Set maximum chunk size.
        /// </summary>
        /// <param name="max_chunk_size"></param>
        public void set_max_chunk_size(uint max_chunk_size) 
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_set_max_chunk_size(ctx_.Handle,handle_,max_chunk_size));
        }

        /// <summary>
        /// Get maximum chunk size.
        /// </summary>
        /// <returns></returns>
        public uint max_chunk_size()
        {
            uint result = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_max_chunk_size(ctx_.Handle, handle_,&result));
            return result;
        }

        /// <summary>
        /// Get number of filter.
        /// </summary>
        /// <returns></returns>
        public uint nfilters()
        {
            uint result = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_nfilters(ctx_.Handle, handle_, &result));
            return result;
        }

        /// <summary>
        /// Get filter from index.
        /// </summary>
        /// <param name="filter_index"></param>
        /// <returns></returns>
        /// <exception cref="TileDB.ErrorException"></exception>
        public Filter filter(uint filter_index) 
        {
            TileDB.Interop.tiledb_filter_t* filter_p = null;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_filter_from_index(ctx_.Handle, handle_, filter_index, &filter_p));

            if (filter_p == null) {
                throw new TileDB.ErrorException("Filter.filter, filter pointer is null");
            }

            return new TileDB.Filter(ctx_, filter_p);
        }

    }
}
