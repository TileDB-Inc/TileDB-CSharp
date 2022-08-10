using System;
using System.Collections.Generic;
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
        /// Check expected type for filter option matches type of T
        /// </summary>
        /// <typeparam name="T">Type of filter option value</typeparam>
        /// <param name="filterOption">Filter option to run type check against</param>
        /// <exception cref="System.NotSupportedException">Filter option does not support values of type T</exception>
        private void check_filter_option<T>(FilterOption filterOption)
        {
            //check for filter option
            var filters = new List<bool> {
                filterOption == FilterOption.TILEDB_COMPRESSION_LEVEL && typeof(T) == typeof(int),
                filterOption == FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW && typeof(T) == typeof(uint),
                filterOption == FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW && typeof(T) == typeof(uint),
                filterOption == FilterOption.TILEDB_SCALE_FLOAT_BYTEWIDTH && typeof(T) == typeof(int),
                filterOption == FilterOption.TILEDB_SCALE_FLOAT_FACTOR && typeof(T) == typeof(int),
                filterOption == FilterOption.TILEDB_SCALE_FLOAT_OFFSET && typeof(T) == typeof(int),
            };

            if (!filters.Contains(true))
            {
                throw new NotSupportedException("Filter, type:" + typeof(T) + " is not supported for filter option:" + filterOption);
            }
        }

        /// <summary>
        /// Set filter option.
        /// </summary>
        /// <typeparam name="T">Type of option value we intend to set</typeparam>
        /// <param name="filterOption">Filter option to set</param>
        /// <param name="value">Filter option value</param>
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
        /// <typeparam name="T">Type of option value we intend to get</typeparam>
        /// <param name="filterOption">Filter option to get</param>
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
