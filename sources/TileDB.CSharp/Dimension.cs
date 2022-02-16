using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Dimension : IDisposable
    {
        private readonly DimensionHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

        internal Dimension(Context ctx, DimensionHandle handle) 
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

        internal DimensionHandle Handle => _handle;

        #region

        /// <summary>
        /// Set filter list.
        /// </summary>
        /// <param name="filterList"></param>
        public void set_filter_list(FilterList filterList)
        {

            _ctx.handle_error(Methods.tiledb_dimension_set_filter_list(_ctx.Handle, _handle, filterList.Handle));
        }

        /// <summary>
        /// Set cell value number.
        /// </summary>
        /// <param name="cellValNum"></param>
        public void set_cell_val_num(uint cellValNum)
        {
            _ctx.handle_error(Methods.tiledb_dimension_set_cell_val_num(_ctx.Handle, _handle, cellValNum));
        }

        /// <summary>
        /// Get filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList filter_list()
        {
            tiledb_filter_list_t* filter_list_p;
            _ctx.handle_error(Methods.tiledb_dimension_get_filter_list(_ctx.Handle, _handle, &filter_list_p));
            return new FilterList(_ctx, filter_list_p);
        }

        /// <summary>
        /// Get cell value number.
        /// </summary>
        /// <returns></returns>
        public uint cell_val_num()
        {
            uint cell_value_num = 0;
            _ctx.handle_error(Methods.tiledb_dimension_get_cell_val_num(_ctx.Handle, _handle, &cell_value_num));
            return cell_value_num;
        }

        /// <summary>
        /// Get name of the dimension.
        /// </summary>
        /// <returns></returns>
        public string Name()
        {
            var ms_name = new MarshaledStringOut();
            fixed (sbyte** p_result = &ms_name.Value) 
            {
                _ctx.handle_error(Methods.tiledb_dimension_get_name(_ctx.Handle, _handle, p_result));
            }
            
            return ms_name;
        }

        /// <summary>
        /// Get type of the dimension.
        /// </summary>
        /// <returns></returns>
        public DataType Type()
        {

            var tiledb_datatype = tiledb_datatype_t.TILEDB_ANY;

            _ctx.handle_error(Methods.tiledb_dimension_get_type(_ctx.Handle, _handle, &tiledb_datatype));

            return (DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get domain bytes array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private byte[] get_domain<T>() where T: struct
        {
            void* value_p;
            var temp = default(T);
            var size = (ulong)(2* Marshal.SizeOf(temp));

            _ctx.handle_error(Methods.tiledb_dimension_get_domain(_ctx.Handle, _handle, &value_p));

            var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
            return fill_span.ToArray();
        }

        /// <summary>
        /// Get domain array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] Domain<T>() where T : struct
        {
            var fill_bytes = get_domain<T>();
            Span<byte> byteSpan = fill_bytes;
            var span = MemoryMarshal.Cast<byte, T>(byteSpan);
            return span.ToArray();
        }

        /// <summary>
        /// Get tile extent bytes array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private byte[] get_tile_extent<T>() where T : struct
        {
            void* value_p;
            var temp = default(T);
            var size = (ulong)(Marshal.SizeOf(temp));

            _ctx.handle_error(Methods.tiledb_dimension_get_tile_extent(_ctx.Handle, _handle, &value_p));

            var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
            return fill_span.ToArray();
        }

        /// <summary>
        /// Get tile extent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T tile_extent<T>() where T : struct
        {
            var fill_bytes = get_tile_extent<T>();
            Span<byte> byteSpan = fill_bytes;
            var span = MemoryMarshal.Cast<byte, T>(byteSpan);
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

            var datatype = Type();
            var t = EnumUtil.to_Type(datatype);
            switch (System.Type.GetTypeCode(t))
            {
                case TypeCode.Int16:
                    {
                        var extent = tile_extent<short>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case TypeCode.Int32:
                    {
                        var extent = tile_extent<int>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case TypeCode.Int64:
                    {
                        var extent = tile_extent<long>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case TypeCode.UInt16:
                    {
                        var extent = tile_extent<ushort>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case TypeCode.UInt32:
                    {
                        var extent = tile_extent<uint>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case TypeCode.UInt64:
                    {
                        var extent = tile_extent<ulong>();
                        sb.Append(extent.ToString());
                    }
                    break;
                case TypeCode.Single:
                    {
                        var extent = tile_extent<float>();
                        sb.Append(extent.ToString(CultureInfo.InvariantCulture));
                    }
                    break;
                case TypeCode.Double:
                    {
                        var extent = tile_extent<double>();
                        sb.Append(extent.ToString(CultureInfo.InvariantCulture));
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

            var datatype = Type();
            var t = EnumUtil.to_Type(datatype);
            switch (System.Type.GetTypeCode(t))
            {
                case TypeCode.Int16:
                    {
                        var domain = Domain<short>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.Int32:
                    {
                        var domain = Domain<int>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.Int64:
                    {
                        var domain = Domain<long>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.UInt16:
                    {
                        var domain = Domain<ushort>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.UInt32:
                    {
                        var domain = Domain<uint>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.UInt64:
                    {
                        var domain = Domain<ulong>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.Single:
                    {
                        var domain = Domain<float>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
                        }
                    }
                    break;
                case TypeCode.Double:
                    {
                        var domain = Domain<double>();
                        if (domain.Length >= 2)
                        {
                            sb.Append(domain[0] + "," + domain[1]);
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
        /// Create a Dimension
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <param name="boundLower"></param>
        /// <param name="boundUpper"></param>
        /// <param name="extent"></param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>

        public static Dimension Create<T>(Context ctx, string name, T boundLower, T boundUpper, T extent)
        {
            var tiledb_datatype = EnumUtil.to_tiledb_datatype(typeof(T));

            if (tiledb_datatype == tiledb_datatype_t.TILEDB_ANY) {
                throw new NotSupportedException("Dimension.create, not supported datatype");
            }

            if (tiledb_datatype == tiledb_datatype_t.TILEDB_STRING_ASCII)
            {
                var str_dim_handle = new DimensionHandle(ctx.Handle, name, tiledb_datatype, null, null);
                return new Dimension(ctx, str_dim_handle);
            }

            var data = new[] { boundLower, boundUpper};
     
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            var extent_dataGcHandle = GCHandle.Alloc(extent, GCHandleType.Pinned);
            var handle = new DimensionHandle(ctx.Handle, name, tiledb_datatype, (void*)dataGcHandle.AddrOfPinnedObject(), (void*)extent_dataGcHandle.AddrOfPinnedObject());
            dataGcHandle.Free();
            extent_dataGcHandle.Free();
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
        public static Dimension Create<T>(Context ctx, string name, T[] bound, T extent) 
        {
            var tiledb_datatype = EnumUtil.to_tiledb_datatype(typeof(T));
            if (tiledb_datatype == tiledb_datatype_t.TILEDB_STRING_ASCII) 
            {
                throw new NotSupportedException("Dimension.create, use create_string for string dimensions");
            }
            if (bound.Length < 2) {
                throw new ArgumentException("Dimension.create, length of bound array is less than 2!");
            }
            return Create(ctx, name, bound[0], bound[1], extent);
        }

        /// <summary>
        /// Create a string dimension.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dimension create_string(Context ctx, string name) 
        {
            return Create<string>(ctx, name, "", "", "");
        }
    }//class

}//namespace