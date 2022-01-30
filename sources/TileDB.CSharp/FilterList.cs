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
            if (!handle_.IsInvalid)
            {
                handle_.Dispose();
            }

            System.GC.SuppressFinalize(this);
        }

        public void add_filter(Filter filter)
        {
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("FilterList.add_filter, invalid handle!");
            }

            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_add_filter(ctx_.Handle, handle_, filter.Handle));

        }

        public void set_max_chunk_size(UInt32 max_chunk_size) 
        {
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("FilterList.set_max_chunk_size, invalid handle!");
            }

            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_set_max_chunk_size(ctx_.Handle,handle_,max_chunk_size));

        }

        public UInt32 max_chunk_size()
        {
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("FilterList.max_chunk_size, invalid handle!");
            }
            UInt32 result = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_max_chunk_size(ctx_.Handle, handle_,&result));
            return result;
        }

        public UInt32 nfilters()
        {
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("FilterList.nfilters, invalid handle!");
            }
            UInt32 result = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_nfilters(ctx_.Handle, handle_, &result));
            return result;
        }

        public Filter filter(UInt32 filter_index) 
        {
            if (handle_.IsInvalid)
            {
                throw new System.InvalidOperationException("FilterList.filter, invalid handle!");
            }

            TileDB.Interop.FilterHandle filter_handle = new TileDB.Interop.FilterHandle(TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_NONE);
         //   TileDB.Interop.tiledb_filter_t* p = (TileDB.Interop.tiledb_filter_t*)filter_handle;
           
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_list_get_filter_from_index(ctx_.Handle, handle_, filter_index, filter_handle));

            return new Filter(ctx_, filter_handle);
        }

    }
}
