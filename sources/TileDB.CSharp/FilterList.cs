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

        public FilterList() 
        {
            ctx_ = new TileDB.Context();
            handle_ = new TileDB.Interop.FilterListHandle(ctx_.Handle);
        }

        public FilterList(Context ctx) 
        {
            ctx_ = ctx;
            handle_ = new TileDB.Interop.FilterListHandle(ctx_.Handle);
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

        public void add_filter(Filter filter)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_add_filter(ctx_.Handle, handle_, filter.Handle));
        }

        public void set_max_chunk_size(UInt32 max_chunk_size) 
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_set_max_chunk_size(ctx_.Handle,handle_,max_chunk_size));
        }

        public UInt32 max_chunk_size()
        {
            UInt32 result = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_max_chunk_size(ctx_.Handle, handle_,&result));
            return result;
        }

        public UInt32 nfilters()
        {
            UInt32 result = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_nfilters(ctx_.Handle, handle_, &result));
            return result;
        }

        public Filter filter(UInt32 filter_index) 
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
