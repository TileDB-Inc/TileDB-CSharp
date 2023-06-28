using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TileDB.CSharp.Marshalling.SafeHandles;
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
            _handle = FilterHandle.Create(_ctx, (tiledb_filter_type_t)(filterType));
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
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_filter_get_type(ctxHandle, handle, &tiledb_filter_type));
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
            var ok =
                (filterOption == FilterOption.CompressionLevel && typeof(T) == typeof(int)) ||
                (filterOption == FilterOption.BitWidthMaxWindow && typeof(T) == typeof(uint)) ||
                (filterOption == FilterOption.PositiveDeltaMaxWindow && typeof(T) == typeof(uint)) ||
                (filterOption == FilterOption.ScaleFloatByteWidth && typeof(T) == typeof(ulong)) ||
                (filterOption == FilterOption.ScaleFloatFactor && typeof(T) == typeof(double)) ||
                (filterOption == FilterOption.ScaleFloatOffset && typeof(T) == typeof(double)) ||
                (filterOption == FilterOption.WebpQuality && typeof(T) == typeof(float)) ||
                (filterOption == FilterOption.WebpInputFormat && typeof(T) == typeof(WebpInputFormat)) ||
                (filterOption == FilterOption.WebpLossless && typeof(T) == typeof(bool)) ||
                (filterOption == FilterOption.CompressionReinterpretDatatype && typeof(T) == typeof(DataType));

            if (!ok)
            {
                throw new NotSupportedException($"Unsupported type for filter option: {filterOption}");
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

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var tiledb_filter_option = (tiledb_filter_option_t)filterOption;
            _ctx.handle_error(Methods.tiledb_filter_set_option(ctxHandle, handle, tiledb_filter_option, &value));
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

            T result;
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_filter_get_option(ctxHandle, handle, tiledb_filter_option, &result));

            return result;
        }
    }
}
