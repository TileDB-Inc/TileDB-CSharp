using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class Filter : IDisposable
    {

        private TileDB.Interop.FilterHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;
 

        public Filter(Context ctx, FilterType filter_type)
        {
            ctx_ = ctx;
            handle_ = new TileDB.Interop.FilterHandle(ctx_.Handle, (TileDB.Interop.tiledb_filter_type_t)(filter_type));
        }

        internal Filter(Context ctx, TileDB.Interop.FilterHandle handle)
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

        internal Interop.FilterHandle Handle
        {
            get { return handle_; }
        }

        /// <summary>
        /// Get filter type.
        /// </summary>
        /// <returns></returns>
        public FilterType filter_type()
        {
            var tiledb_filter_type= TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_NONE;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_get_type(ctx_.Handle, handle_, &tiledb_filter_type));
            return (FilterType)tiledb_filter_type;
        }

        /// <summary>
        /// Check filter option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter_option"></param>
        /// <exception cref="System.NotSupportedException"></exception>
        protected void check_filter_option<T>(FilterOption filter_option) 
        {
            //check for filter option 
            var is_compression_level = filter_option == FilterOption.TILEDB_COMPRESSION_LEVEL && typeof(T) == typeof(int);
            var is_bid_width_max_window = filter_option == FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW && typeof(T) == typeof(uint);
            var is_positive_delta_max_window = filter_option == FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW && typeof(T) == typeof(uint);

            if (is_compression_level || is_bid_width_max_window || is_positive_delta_max_window)
            {
                return;
            }
            throw new System.NotSupportedException("Filter, type:" + typeof(T).ToString() + " is not supported for filter option:" + filter_option.ToString());
        }

        /// <summary>
        /// Set filter option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter_option"></param>
        /// <param name="value"></param>
        public void set_option<T>(FilterOption filter_option, T value) where T: struct 
        {
            //check for filter option 
            check_filter_option<T>(filter_option);

            var tiledb_filter_option = (TileDB.Interop.tiledb_filter_option_t)filter_option;
            var data = new T[1] { value };
            var dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_set_option(ctx_.Handle, handle_, tiledb_filter_option, (void*)dataGCHandle.AddrOfPinnedObject()));
            dataGCHandle.Free();
        }

        /// <summary>
        /// Get filter option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter_option"></param>
        /// <returns></returns>
        public T get_option<T>(FilterOption filter_option) where T: struct
        {
            //check for filter option 
            check_filter_option<T>(filter_option);
       
            T result;
            var tiledb_filter_option = (TileDB.Interop.tiledb_filter_option_t)filter_option;

            var data = new T[1];
            var dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_filter_get_option(ctx_.Handle, handle_, tiledb_filter_option, (void*)dataGCHandle.AddrOfPinnedObject()));
            result = data[0];
            dataGCHandle.Free();

            return result;
        }

    }
}
