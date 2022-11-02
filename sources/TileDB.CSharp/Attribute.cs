using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class Attribute : IDisposable
    {
        private readonly AttributeHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

        public Attribute(Context ctx, string name, DataType dataType)
        {
            _ctx = ctx;
            var tiledb_datatype = (tiledb_datatype_t)dataType;
            _handle = AttributeHandle.Create(_ctx, name, tiledb_datatype);
            if (EnumUtil.IsStringType(dataType))
            {
                SetCellValNum(Constants.TILEDB_VAR_NUM);
            }
        }

        internal Attribute(Context ctx, AttributeHandle handle)
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

        internal AttributeHandle Handle => _handle;

        #region
        /// <summary>
        /// Set nullable.
        /// </summary>
        /// <param name="nullable"></param>
        public void SetNullable(bool nullable)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var int8_nullable = nullable ? (byte)1 : (byte)0;
            _ctx.handle_error(Methods.tiledb_attribute_set_nullable(ctxHandle, handle, int8_nullable));
        }

        /// <summary>
        /// Set filter list.
        /// </summary>
        /// <param name="filterList"></param>
        public void SetFilterList(FilterList filterList)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var filterListHandle = filterList.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_attribute_set_filter_list(ctxHandle, handle, filterListHandle));
        }

        /// <summary>
        /// Set cell value number.
        /// </summary>
        /// <param name="cellValNum"></param>
        public void SetCellValNum(uint cellValNum)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_attribute_set_cell_val_num(ctxHandle, handle, cellValNum));
        }

        /// <summary>
        /// Get name of the attribute.
        /// </summary>
        /// <returns></returns>
        public string Name()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_attribute_get_name(ctxHandle, handle, &result));

            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Get type of the attribute.
        /// </summary>
        /// <returns></returns>
        public DataType Type()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_datatype_t tiledb_datatype;
            _ctx.handle_error(Methods.tiledb_attribute_get_type(ctxHandle, handle, &tiledb_datatype));
            return (DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get nullable of the attribute.
        /// </summary>
        /// <returns></returns>
        public bool Nullable()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            byte nullable;
            _ctx.handle_error(Methods.tiledb_attribute_get_nullable(ctxHandle, handle, &nullable));
            return nullable > 0;
        }


        /// <summary>
        /// Get filter list of the attribute.
        /// </summary>
        public FilterList FilterList()
        {
            var handle = new FilterListHandle();
            var successful = false;
            tiledb_filter_list_t* filter_list_p = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var attributeHandle = _handle.Acquire())
                {
                    _ctx.handle_error(Methods.tiledb_attribute_get_filter_list(ctxHandle, attributeHandle, &filter_list_p));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(filter_list_p);
                }
            }

            return new FilterList(_ctx, handle);
        }

        /// <summary>
        /// Get cell value number.
        /// </summary>
        /// <returns></returns>
        public uint CellValNum()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint cell_val_num;
            _ctx.handle_error(Methods.tiledb_attribute_get_cell_val_num(ctxHandle, handle, &cell_val_num));
            return cell_val_num;
        }

        /// <summary>
        /// Get cell size.
        /// </summary>
        /// <returns></returns>
        public ulong CellSize()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong cell_size;
            _ctx.handle_error(Methods.tiledb_attribute_get_cell_size(ctxHandle, handle, &cell_size));
            return cell_size;
        }


        /// <summary>
        /// Set fill value by an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        private void SetFillValue<T>(T[] data) where T: struct
        {
            if (data.Length == 0) {
                throw new ArgumentException("Attribute.SetFillValue, data is empty!");
            }

            var cell_val_num = this.CellValNum();

            if (cell_val_num != Constants.TILEDB_VAR_NUM && cell_val_num != data.Length)
            {
                throw new ArgumentException("Attribute.SetFillValue_nullable, data length is not equal to cell_val_num!");
            }

            ulong size;
            if (cell_val_num == Constants.TILEDB_VAR_NUM) {
                size = (ulong)(data.Length* Marshal.SizeOf(data[0]));
            } else
            {
                size = cell_val_num * (ulong)(Marshal.SizeOf(data[0]));
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_attribute_set_fill_value(ctxHandle, handle,
                    (void*)dataGcHandle.AddrOfPinnedObject(), size));
            }
            finally
            {
                dataGcHandle.Free();
            }

        }

        /// <summary>
        /// Set fill value by a scalar value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SetFillValue<T>(T value) where T: struct
        {
            if (typeof(T) == typeof(bool))
            {
                SetFillValue<byte>(Convert.ToByte(value));
                return;
            }

            var cell_val_num = this.CellValNum();
            var data = cell_val_num == Constants.TILEDB_VAR_NUM
                ? new[] { value }
                : Enumerable.Repeat(value, (int)cell_val_num).ToArray();
            SetFillValue(data);
        }

        /// <summary>
        /// Set string fill value.
        /// </summary>
        /// <param name="value"></param>
        public void SetFillValue(string value)
        {
            var str_bytes = Encoding.ASCII.GetBytes(value);
            SetFillValue(str_bytes);
        }

        /// <summary>
        /// Get fill value bytes array.
        /// </summary>
        /// <returns></returns>
        private byte[] get_fill_value()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            void* value_p;
            ulong size;
            _ctx.handle_error(Methods.tiledb_attribute_get_fill_value(ctxHandle, handle, &value_p, &size));

            var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
            return fill_span.ToArray();
        }

        /// <summary>
        /// Get fill value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] FillValue<T>() where T: struct
        {
            if (typeof(T) == typeof(char))
            {
                throw new NotSupportedException("Attribute.FillValue, please use FillValue<byte> for TILEDB_CHAR attributes!");
            }
            var fill_bytes = get_fill_value();
            var span = MemoryMarshal.Cast<byte, T>(fill_bytes);
            return span.ToArray();
        }

        /// <summary>
        /// Get string fill value.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string FillValue()
        {
            var datatype = Type();
            if (!EnumUtil.IsStringType(datatype))
            {
                throw new NotSupportedException("Attribute.FillValue, please use FillValue<T> for non-string attribute!");
            }
            var fill_bytes = get_fill_value();
            return MarshaledStringOut.GetString(fill_bytes);
        }

        /// <summary>
        /// Set fill array value and validity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="valid"></param>
        private void SetFillValueNullable<T>(T[] data, bool valid) where T: struct
        {
            if (data.Length == 0)
            {
                throw new ArgumentException("Attribute.SetFillValueNullable, data is empty!");
            }

            var cell_val_num = this.CellValNum();
            if (cell_val_num != Constants.TILEDB_VAR_NUM && cell_val_num != data.Length)
            {
                throw new ArgumentException("Attribute.SetFillValueNullable, data length is not equal to cell_val_num!");
            }

            ulong size;
            if (cell_val_num == Constants.TILEDB_VAR_NUM)
            {
                size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            }
            else
            {
                size = cell_val_num * (ulong)(Marshal.SizeOf(data[0]));
            }

            var validity = valid ? (byte)1 : (byte)0;

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_attribute_set_fill_value_nullable(ctxHandle, handle, (void*)dataGcHandle.AddrOfPinnedObject(), size, validity));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Set fill scalar value and validity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valid"></param>
        public void SetFillValueNullable<T>(T value, bool valid) where T: struct
        {
            var cell_val_num = this.CellValNum();
            var data = cell_val_num == Constants.TILEDB_VAR_NUM ? new[] { value } : Enumerable.Repeat(value, (int)cell_val_num).ToArray();
            SetFillValueNullable(data, valid);
        }

        /// <summary>
        /// Set string fill value and validity.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valid"></param>
        public void SetFillValueNullable(string value, bool valid)
        {
            var str_bytes = Encoding.ASCII.GetBytes(value);
            SetFillValueNullable(str_bytes, valid);
        }

        /// <summary>
        /// Get fill value bytes array and validity.
        /// </summary>
        /// <returns></returns>
        private (byte[] bytearray, bool valid) get_fill_value_nullable()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            void* value_p;
            ulong size;
            var byte_valid = default(byte);
            _ctx.handle_error(Methods.tiledb_attribute_get_fill_value_nullable(ctxHandle, handle, &value_p, &size, &byte_valid));

            var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
            return (fill_span.ToArray(), byte_valid>0);
        }

        /// <summary>
        /// Get fill scalar value and validity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Tuple<T[],bool> FillValueNullable<T>() where T : struct
        {
            var fill_value = get_fill_value_nullable();
            Span<byte> byteSpan = fill_value.bytearray;
            var span = MemoryMarshal.Cast<byte, T>(byteSpan);
            return new Tuple<T[], bool>(span.ToArray(),fill_value.valid);
        }

        /// <summary>
        /// Get string fill value and validity.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public string FillValueNullable()
        {
            var datatype = Type();
            if (!EnumUtil.IsStringType(datatype))
            {
                throw new NotSupportedException("Attribute.FillValueNullable, please use fill_value<T> for non-string attribute!");
            }
            var(fill_bytes,_) = get_fill_value_nullable();
            return MarshaledStringOut.GetString(fill_bytes);
        }
        #endregion

        /// <summary>
        /// Create an attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Attribute Create<T>(Context ctx, string name)
        {
            var datatype = EnumUtil.TypeToDataType(typeof(T));
            return new Attribute(ctx, name, datatype);
        }
    }
}
