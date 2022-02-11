using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TileDB
{
    public unsafe class Attribute : IDisposable 
    {
        private TileDB.Interop.AttributeHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;

        public Attribute(Context ctx, string name, DataType dataType) 
        {
            ctx_ = ctx;
            var tiledb_datatype = (TileDB.Interop.tiledb_datatype_t)(dataType);
            handle_ = new TileDB.Interop.AttributeHandle(ctx_.Handle, name, tiledb_datatype);
            if (EnumUtil.is_string_type(dataType))
            {
                set_cell_val_num((uint)Constants.TILEDB_VAR_NUM);
            }
        }

        internal Attribute(Context ctx, TileDB.Interop.AttributeHandle handle) 
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

        internal TileDB.Interop.AttributeHandle Handle
        {
            get { return handle_; }
        }

        #region
        /// <summary>
        /// Set nullable.
        /// </summary>
        /// <param name="nullable"></param>
        public void set_nullable(bool nullable)
        {
            var int8_nullable = nullable ? (byte)1 : (byte)0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_set_nullable(ctx_.Handle, handle_, int8_nullable));
        }

        /// <summary>
        /// Set filter list.
        /// </summary>
        /// <param name="filterlist"></param>
        public void set_filter_list(FilterList filterlist)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_set_filter_list(ctx_.Handle, handle_, filterlist.Handle));

        }

        /// <summary>
        /// Set cell value number.
        /// </summary>
        /// <param name="cell_val_num"></param>
        public void set_cell_val_num(uint cell_val_num)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_set_cell_val_num(ctx_.Handle, handle_, cell_val_num));
        }

        /// <summary>
        /// Get name of the attribute.
        /// </summary>
        /// <returns></returns>
        public string name()
        {
            var ms_result = new Interop.MarshaledStringOut();
            fixed (sbyte** p_result = &ms_result.Value) 
            {
                ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_name(ctx_.Handle, handle_, p_result));
            }

            return ms_result;
        }

        /// <summary>
        /// Get type of the attribute.
        /// </summary>
        /// <returns></returns>
        public DataType type()
        {
            var tiledb_datatype = TileDB.Interop.tiledb_datatype_t.TILEDB_ANY;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_type(ctx_.Handle, handle_, &tiledb_datatype));
            return (DataType)tiledb_datatype;
        }

        /// <summary>
        /// Get nullable of the attribute.
        /// </summary>
        /// <returns></returns>
        public bool nullable()
        {
            byte nullable = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_nullable(ctx_.Handle, handle_, &nullable));
            return nullable > 0;
        }


        /// <summary>
        /// Get filter list of the attribute.
        /// </summary>
        public FilterList filter_list()
        {
            TileDB.Interop.tiledb_filter_list_t* filter_list_p;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_filter_list(ctx_.Handle, handle_, &filter_list_p));
            return new FilterList(ctx_, filter_list_p);
        }


        /// <summary>
        /// Get cell value number.
        /// </summary>
        /// <returns></returns>
        public uint cell_val_num()
        {
            uint cell_val_num = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_cell_val_num(ctx_.Handle, handle_, &cell_val_num));
            return cell_val_num;
        }

        /// <summary>
        /// Get cell size.
        /// </summary>
        /// <returns></returns>
        public ulong cell_size()
        {

            ulong cell_size = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_cell_size(ctx_.Handle, handle_, &cell_size));
            return cell_size;
        }

 
        /// <summary>
        /// Set fill value by an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void set_fill_value<T>(T[] data) where T: struct
        {
            if (data.Length == 0) {
                throw new System.ArgumentException("Attribute.set_fill_value, data is empty!");
            }

            var cell_val_num = this.cell_val_num();
         
            if (cell_val_num != (uint)Constants.TILEDB_VAR_NUM && cell_val_num != data.Length)
            {
                throw new System.ArgumentException("Attribute.set_fill_value_nullable, data length is not equal to cell_val_num!");
            }

            ulong size;
            if (cell_val_num == (uint)Constants.TILEDB_VAR_NUM) {
                size = (ulong)(data.Length* Marshal.SizeOf(data[0]));          
            } else
            {
                size = cell_val_num * (ulong)(Marshal.SizeOf(data[0]));
            }

            var dataGCHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_set_fill_value(ctx_.Handle, handle_, (void*)dataGCHandle.AddrOfPinnedObject(), size));
            }
            finally
            {
                dataGCHandle.Free();
            }
            
        }


        /// <summary>
        /// Set fill value by a scalar value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void set_fill_value<T>(T value) where T: struct
        {
            var cell_val_num = this.cell_val_num();
            T[] data;
            if (cell_val_num == (uint)Constants.TILEDB_VAR_NUM)
            {
                data = new T[1] { value };
            }
            else { 
                data = Enumerable.Repeat(value, (int)cell_val_num).ToArray(); 
            }
            set_fill_value(data);
        }

        /// <summary>
        /// Set string fill value.
        /// </summary>
        /// <param name="value"></param>
        public void set_fill_value(string value)
        {
            var str_bytes = System.Text.Encoding.ASCII.GetBytes(value);
            set_fill_value(str_bytes);
        }
 

        /// <summary>
        /// Get fill value bytes array.
        /// </summary>
        /// <returns></returns>
        protected byte[] get_fill_value() 
        {
            unsafe {
                void* value_p;
                ulong size;
                ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_fill_value(ctx_.Handle,handle_,&value_p,&size));

                var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
                return fill_span.ToArray();
            }
        }

        /// <summary>
        /// Get fill value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] fill_value<T>() where T: struct
        {
            var fill_bytes = get_fill_value();
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(fill_bytes);
            return span.ToArray();
        }

        /// <summary>
        /// Get string fill value.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string fill_value() 
        {
            var datatype = type();
            if (!EnumUtil.is_string_type(datatype))
            {
                throw new System.NotSupportedException("Attribute.fill_value, please use fill_value<T> for non-string attribute!");
            }
            var fill_bytes = get_fill_value();
            return System.Text.Encoding.ASCII.GetString(fill_bytes);
        }

        /// <summary>
        /// Set fill array value and validity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="valid"></param>
        public void set_fill_value_nullable<T>(T[] data, bool valid) where T: struct
        {
            if (data.Length == 0)
            {
                throw new System.ArgumentException("Attribute.set_fill_value_nullable, data is empty!");
            }
           
            var cell_val_num = this.cell_val_num();
            if (cell_val_num != (uint)Constants.TILEDB_VAR_NUM && cell_val_num != data.Length) 
            {
                throw new System.ArgumentException("Attribute.set_fill_value_nullable, data length is not equal to cell_val_num!");
            }

            ulong size;
            if (cell_val_num == (uint)Constants.TILEDB_VAR_NUM)
            {
                size = (ulong)(data.Length * Marshal.SizeOf(data[0]));
            }
            else
            {
                size = cell_val_num * (ulong)(Marshal.SizeOf(data[0]));
            }
           
            var validity = valid ? (byte)1 : (byte)0;
           
            var dataGCHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_set_fill_value_nullable(ctx_.Handle, handle_, (void*)dataGCHandle.AddrOfPinnedObject(), size, validity));
            }
            finally
            {
                dataGCHandle.Free();
            }
        }

        /// <summary>
        /// Set fill scalar value and validity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valid"></param>
        public void set_fill_value_nullable<T>(T value, bool valid) where T: struct
        {
            var cell_val_num = this.cell_val_num();
            T[] data;
            if (cell_val_num == (uint)Constants.TILEDB_VAR_NUM)
            {
                data = new T[1] { value };
            }
            else
            {
                data = Enumerable.Repeat(value, (int)cell_val_num).ToArray();
            }
            set_fill_value_nullable(data, valid);
        }

        /// <summary>
        /// Set string fill value and validity.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valid"></param>
        public void set_fill_value_nullable(string value, bool valid)
        {
            var str_bytes = System.Text.Encoding.ASCII.GetBytes(value);
            set_fill_value_nullable(str_bytes, valid);
        }

        /// <summary>
        /// Get fill value bytes array and validity.
        /// </summary>
        /// <returns></returns>
        protected (byte[] bytearray, bool valid) get_fill_value_nullable()
        {
            unsafe
            {
                void* value_p;
                ulong size;
                var byte_valid = default(byte);
                ctx_.handle_error(TileDB.Interop.Methods.tiledb_attribute_get_fill_value_nullable(ctx_.Handle, handle_, &value_p, &size, &byte_valid));

                var fill_span = new ReadOnlySpan<byte>(value_p, (int)size);
                return (fill_span.ToArray(), byte_valid>0);
            }
        }

        /// <summary>
        /// Get fill scalar value and validity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public System.Tuple<T[],bool> fill_value_nullable<T>() where T : struct
        {

            var fill_value = get_fill_value_nullable();
            System.Span<byte> byteSpan = fill_value.bytearray;
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(byteSpan);
            return new Tuple<T[], bool>(span.ToArray(),fill_value.valid);
        }

        /// <summary>
        /// Get string fill value and validity.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string fill_value_nullable()
        {
            var datatype = type();
            if (!EnumUtil.is_string_type(datatype))
            {
                throw new System.NotSupportedException("Attribute.fill_value_nullable, please use fill_value<T> for non-string attribute!");
            }
            var(fill_bytes,valid) = get_fill_value_nullable();
            return System.Text.Encoding.ASCII.GetString(fill_bytes);
        }

        #endregion

        /// <summary>
        /// Create an attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Attribute create<T>(Context ctx, string name) 
        {
            var t = typeof(T);
            var datatype = EnumUtil.to_DataType(typeof(T));
            return new Attribute(ctx, name, datatype); 
        }

    }

}//namespace