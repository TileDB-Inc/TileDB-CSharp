using System;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class FilterList : IDisposable
    {
        private readonly FilterListHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

 

        public FilterList(Context ctx) 
        {
            _ctx = ctx;
            _handle = new FilterListHandle(_ctx.Handle);
        }

        internal FilterList(Context ctx, FilterListHandle handle) 
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

        internal FilterListHandle Handle => _handle;

        /// <summary>
        /// Add a filter.
        /// </summary>
        /// <param name="filter"></param>
        public void add_filter(Filter filter)
        {
            _ctx.handle_error(Methods.tiledb_filter_list_add_filter(_ctx.Handle, _handle, filter.Handle));
        }

        /// <summary>
        /// Set maximum chunk size.
        /// </summary>
        /// <param name="maxChunkSize"></param>
        public void set_max_chunk_size(uint maxChunkSize) 
        {
            _ctx.handle_error(Methods.tiledb_filter_list_set_max_chunk_size(_ctx.Handle,_handle,maxChunkSize));
        }

        /// <summary>
        /// Get maximum chunk size.
        /// </summary>
        /// <returns></returns>
        public uint max_chunk_size()
        {
            uint result = 0;
            _ctx.handle_error(Methods.tiledb_filter_list_get_max_chunk_size(_ctx.Handle, _handle,&result));
            return result;
        }

        /// <summary>
        /// Get number of filter.
        /// </summary>
        /// <returns></returns>
        public uint NFilters()
        {
            uint result = 0;
            _ctx.handle_error(Methods.tiledb_filter_list_get_nfilters(_ctx.Handle, _handle, &result));
            return result;
        }

        /// <summary>
        /// Get filter from index.
        /// </summary>
        /// <param name="filterIndex"></param>
        /// <returns></returns>
        /// <exception cref="ErrorException"></exception>
        public Filter Filter(uint filterIndex) 
        {
            tiledb_filter_t* filter_p = null;
            _ctx.handle_error(Methods.tiledb_filter_list_get_filter_from_index(_ctx.Handle, _handle, filterIndex, &filter_p));

            if (filter_p == null) {
                throw new ErrorException("FilterList.filter, filter pointer is null");
            }

            return new Filter(_ctx, filter_p);
        }

    }
}
