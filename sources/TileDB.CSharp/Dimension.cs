using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public unsafe class Dimension : IDisposable
    {
        private TileDB.Interop.DimensionHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;

        internal Dimension(Context ctx, TileDB.Interop.DimensionHandle handle) 
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

        internal TileDB.Interop.DimensionHandle Handle
        {
            get { return handle_; }

        }

        #region

        /// <summary>
        /// Set filter list.
        /// </summary>
        /// <param name="filterlist"></param>
        public void set_filter_list(FilterList filterlist)
        {

            ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_set_filter_list(ctx_.Handle, handle_, filterlist.Handle));
        }

        /// <summary>
        /// Set cell value number.
        /// </summary>
        /// <param name="cell_val_num"></param>
        public void set_cell_val_num(uint cell_val_num)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_set_cell_val_num(ctx_.Handle, handle_, cell_val_num));
        }

        /// <summary>
        /// Get filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList filter_list()
        {
            TileDB.Interop.tiledb_filter_list_t* filter_list_p;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_get_filter_list(ctx_.Handle, handle_, &filter_list_p));
            return new FilterList(ctx_, filter_list_p);
        }

        /// <summary>
        /// Get cell value number.
        /// </summary>
        /// <returns></returns>
        public uint cell_val_num()
        {
            uint cell_value_num = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_get_cell_val_num(ctx_.Handle, handle_, &cell_value_num));
            return cell_value_num;
        }

        /// <summary>
        /// Get name of the dimension.
        /// </summary>
        /// <returns></returns>
        public string name()
        {
            var ms_name = new TileDB.Interop.MarshaledStringOut();
            fixed (sbyte** p_result = &ms_name.Value) 
            {
                ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_get_name(ctx_.Handle, handle_, p_result));
            }
            
            return ms_name;
        }

        /// <summary>
        /// Get type of the dimension.
        /// </summary>
        /// <returns></returns>
        public TileDB.DataType type()
        {

            var tiledb_datatype = TileDB.Interop.tiledb_datatype_t.TILEDB_ANY;

            ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_get_type(ctx_.Handle, handle_, &tiledb_datatype));

            return (TileDB.DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get domain bytes array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected byte[] get_domain<T>() where T: struct 
        {
            unsafe
            {
                void* value_p;
                var temp = default(T);
                var size = (ulong)(2* System.Runtime.InteropServices.Marshal.SizeOf(temp));

                ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_get_domain(ctx_.Handle, handle_, &value_p));

                var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
                return fill_span.ToArray();
            }
        }

        /// <summary>
        /// Get domain array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] domain<T>() where T : struct
        {
            var fill_bytes = get_domain<T>();
            System.Span<byte> byteSpan = fill_bytes;
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(byteSpan);
            return span.ToArray();
        }

        /// <summary>
        /// Get tile extent bytes array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected byte[] get_tile_extent<T>() where T : struct
        {
            unsafe
            {
                void* value_p;
                var temp = default(T);
                var size = (ulong)(System.Runtime.InteropServices.Marshal.SizeOf(temp));

                ctx_.handle_error(TileDB.Interop.Methods.tiledb_dimension_get_tile_extent(ctx_.Handle, handle_, &value_p));

                var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
                return fill_span.ToArray();
            }
        }

        /// <summary>
        /// Get tile extent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T tile_extent<T>() where T : struct
        {
            var fill_bytes = get_tile_extent<T>();
            System.Span<byte> byteSpan = fill_bytes;
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(byteSpan);
            return span.ToArray()[0];
        }
        #endregion

        /// <summary>
        /// Get string of tile extent.
        /// </summary>
        /// <returns></returns>
        public string tile_extent_to_str()
        {
            var sb = new StringBuilder();

            var datatype = type();
            var t = EnumUtil.to_Type(datatype);
            switch (System.Type.GetTypeCode(t))
            {
                case System.TypeCode.Int16:
                    {
                        var extent = tile_extent<short>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.Int32:
                    {
                        var extent = tile_extent<int>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.Int64:
                    {
                        var extent = tile_extent<long>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.UInt16:
                    {
                        var extent = tile_extent<ushort>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.UInt32:
                    {
                        var extent = tile_extent<uint>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.UInt64:
                    {
                        var extent = tile_extent<ulong>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.Single:
                    {
                        var extent = tile_extent<float>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case System.TypeCode.Double:
                    {
                        var extent = tile_extent<double>();
                        sb.Append(extent.ToString());
                    }
                    break;
                default:
                    {
                        sb.Append(",");
                    }
                    break;

            }

            return sb.ToString();
        }

        /// <summary>
        /// Get string of domain.
        /// </summary>
        /// <returns></returns>
        public string domain_to_str()
        {
            var sb = new StringBuilder();

            var datatype = type();
            var t = EnumUtil.to_Type(datatype);
            switch (System.Type.GetTypeCode(t))
            {
                case System.TypeCode.Int16:
                    {
                        var domain = domain<short>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.Int32:
                    {
                        var domain = domain<int>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.Int64:
                    {
                        var domain = domain<long>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.UInt16:
                    {
                        var domain = domain<ushort>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.UInt32:
                    {
                        var domain = domain<uint>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.UInt64:
                    {
                        var domain = domain<ulong>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.Single:
                    {
                        var domain = domain<float>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                case System.TypeCode.Double:
                    {
                        var domain = domain<double>();
                        if (domain != null && domain.Length >= 2)
                        {
                            sb.Append(domain[0].ToString() + "," + domain[1].ToString());
                        }
                    }
                    break;
                default:
                    {
                        sb.Append(",");
                    }
                    break;

            }

            return sb.ToString();

        }



        /// <summary>
        /// Create a dimenson.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <param name="bound_lower"></param>
        /// <param name="bound_upper"></param>
        /// <param name="extent"></param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>

        public static Dimension create<T>(Context ctx, string name, T bound_lower, T bound_upper, T extent)
        {
            var t = typeof(T);
            var tiledb_datatype = EnumUtil.to_tiledb_datatype(typeof(T));

            if (tiledb_datatype == TileDB.Interop.tiledb_datatype_t.TILEDB_ANY) {
                throw new System.NotSupportedException("Dimension.create, not supported datatype");
            }

            if (tiledb_datatype == TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_ASCII)
            {
                var str_dim_handle = new TileDB.Interop.DimensionHandle(ctx.Handle, name, tiledb_datatype, null, null);
                return new Dimension(ctx, str_dim_handle);
            }

            var data = new T[2] { bound_lower, bound_upper};
     
            var dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
            var extent_dataGCHandle = System.Runtime.InteropServices.GCHandle.Alloc(extent, System.Runtime.InteropServices.GCHandleType.Pinned);
            var handle = new TileDB.Interop.DimensionHandle(ctx.Handle, name, tiledb_datatype, (void*)dataGCHandle.AddrOfPinnedObject(), (void*)extent_dataGCHandle.AddrOfPinnedObject());
            dataGCHandle.Free();
            extent_dataGCHandle.Free();
            return new Dimension(ctx,handle);
        }

        /// <summary>
        /// Create a dimension.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <param name="bound"></param>
        /// <param name="extent"></param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public static Dimension create<T>(Context ctx, string name, T[] bound, T extent) 
        {
            var t = typeof(T);
            var tiledb_datatype = EnumUtil.to_tiledb_datatype(typeof(T));
            if (tiledb_datatype == TileDB.Interop.tiledb_datatype_t.TILEDB_STRING_ASCII) 
            {
                throw new System.NotSupportedException("Dimension.create, use create_string for string dimensions");
            }
            if (bound.Length < 2) {
                throw new System.ArgumentException("Dimension.create, length of bound array is less than 2!");
            }
            return create(ctx, name, bound[0], bound[1], extent);
        }

        /// <summary>
        /// Create a string dimension.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dimension create_string(Context ctx, string name) 
        {
            return create<string>(ctx, name, "", "", "");
        }
    }//class

}//namespace