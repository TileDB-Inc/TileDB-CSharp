using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Filter : IDisposable
    {

        private readonly FilterHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;
 

        public Filter(Context ctx, FilterType filterType)
        {
            _ctx = ctx;
            _handle = new FilterHandle(_ctx.Handle, (tiledb_filter_type_t)(filterType));
        }

        internal Filter(Context ctx, FilterHandle handle)
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

        internal FilterHandle Handle => _handle;

        /// <summary>
        /// Get filter type.
        /// </summary>
        /// <returns></returns>
        public FilterType FilterType()
        {
            var tiledb_filter_type= tiledb_filter_type_t.TILEDB_FILTER_NONE;
            _ctx.handle_error(Methods.tiledb_filter_get_type(_ctx.Handle, _handle, &tiledb_filter_type));
            return (FilterType)tiledb_filter_type;
        }

        /// <summary>
        /// Check filter option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterOption"></param>
        /// <exception cref="System.NotSupportedException"></exception>
        private void check_filter_option<T>(FilterOption filterOption) 
        {
            //check for filter option 
            var is_compression_level = filterOption == FilterOption.TILEDB_COMPRESSION_LEVEL && typeof(T) == typeof(int);
            var is_bid_width_max_window = filterOption == FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW && typeof(T) == typeof(uint);
            var is_positive_delta_max_window = filterOption == FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW && typeof(T) == typeof(uint);

            if (is_compression_level || is_bid_width_max_window || is_positive_delta_max_window)
            {
                return;
            }
            throw new NotSupportedException("Filter, type:" + typeof(T) + " is not supported for filter option:" + filterOption);
        }

        /// <summary>
        /// Set filter option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterOption"></param>
        /// <param name="value"></param>
        public void SetOption<T>(FilterOption filterOption, T value) where T: struct 
        {
            //check for filter option 
            check_filter_option<T>(filterOption);

            var tiledb_filter_option = (tiledb_filter_option_t)filterOption;
            var data = new[] { value };
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            _ctx.handle_error(Methods.tiledb_filter_set_option(_ctx.Handle, _handle, tiledb_filter_option, (void*)dataGcHandle.AddrOfPinnedObject()));
            dataGcHandle.Free();
        }

        /// <summary>
        /// Get filter option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterOption"></param>
        /// <returns></returns>
        public T GetOption<T>(FilterOption filterOption) where T: struct
        {
            //check for filter option 
            check_filter_option<T>(filterOption);

            var tiledb_filter_option = (tiledb_filter_option_t)filterOption;

            var data = new T[1];
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            _ctx.handle_error(Methods.tiledb_filter_get_option(_ctx.Handle, _handle, tiledb_filter_option, (void*)dataGcHandle.AddrOfPinnedObject()));
            var result = data[0];
            dataGcHandle.Free();

            return result;
        }

    }
}
