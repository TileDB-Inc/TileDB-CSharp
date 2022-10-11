using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp
{ 
    public unsafe class ArrayMetadata : IDictionary<string,byte[]>
    {
        protected Array _array;

        #region Constructors
        public ArrayMetadata(Array array)
        {
            _array = array;
        }
        #endregion

        #region User api
        /// <summary>
        /// Put metadata array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void PutMetadata<T>(string key, T[] data) where T : struct
        {
            var tiledb_datatype = EnumUtil.to_tiledb_datatype(typeof(T));
            put_metadata<T>(key, data, tiledb_datatype, (uint)data.Length);
        }

        /// <summary>
        /// Put a single value metadata.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="v"></param>
        public void PutMetadata<T>(string key, T v) where T : struct
        {
            T[] data = new T[1] { v };
            PutMetadata<T>(key, data);
        }

        /// <summary>
        /// Put string metadata.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void PutMetadata(string key, string value)
        {
            var tiledb_datatype = tiledb_datatype_t.TILEDB_STRING_ASCII;
            var data = Encoding.ASCII.GetBytes(value);
            var val_num = (uint)data.Length;
            put_metadata<byte>(key, data, tiledb_datatype, val_num); 
        }

        /// <summary>
        /// Delete metadata.
        /// </summary>
        /// <param name="key"></param>
        public void DeleteMetadata(string key)
        {
            var ctx = _array.Context();
            using var ctxHandle = ctx.Handle.Acquire();
            using var arrayHandle = _array.Handle.Acquire();
            var ms_key = new MarshaledString(key);
            ctx.handle_error(Methods.tiledb_array_delete_metadata(ctxHandle, arrayHandle, ms_key));
        }

        /// <summary>
        /// Get metadata list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T[] GetMetadata<T>(string key) where T : struct
        {
            var (k, value, dataType, valueNum) = get_metadata(key);
            Span<byte> valueSpan = value;

            var span = MemoryMarshal.Cast<byte, T>(valueSpan);
            return span.ToArray();
        }

        /// <summary>
        /// Get string metadata
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetMetadata(string key)
        {
            var (_, byte_array, _, _) = get_metadata(key);
            return Encoding.ASCII.GetString(byte_array);
        }

        /// <summary>
        /// Get number of metadata.
        /// </summary>
        /// <returns></returns>
        public ulong MetadataNum()
        {
            var ctx = _array.Context();
            ulong num;
            using var ctxHandle = ctx.Handle.Acquire();
            using var arrayHandle = _array.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_get_metadata_num(ctxHandle, arrayHandle, &num));
            return num;
        }

        /// <summary>
        /// Get metadata from index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public (string key, T[] data) GetMetadataFromIndex<T>(ulong index) where T : struct
        {
            var (key, value, dataType, valueNum) = get_metadata_from_index(index);
            Span<byte> valueSpan = value;

            var span = MemoryMarshal.Cast<byte, T>(valueSpan);
            return (key, span.ToArray());

        }

        /// <summary>
        /// Get metadata keys.
        /// </summary>
        /// <returns></returns>
        public string[] MetadataKeys()
        {
            var num = MetadataNum();
            var ret = new string[(int)num];
            for (ulong i = 0; i < num; ++i)
            {
                var (key, _, _,  _) = get_metadata_from_index(i);
                ret[i] = key;
            }
            return ret;
        }

        /// <summary>
        /// Test if a metadata with key exists or not.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (bool has_key, DataType datatype) HasMetadata(string key)
        {
            var ctx = _array.Context();
            using var ctxHandle = ctx.Handle.Acquire();
            using var arrayHandle = _array.Handle.Acquire();
            var ms_key = new MarshaledString(key);
            tiledb_datatype_t tiledb_datatype = tiledb_datatype_t.TILEDB_ANY;
            var has_key = 0;
            ctx.handle_error(Methods.tiledb_array_has_metadata_key(ctxHandle, arrayHandle, ms_key, &tiledb_datatype, &has_key));

            return (has_key > 0, (DataType)tiledb_datatype);
        }

        /// <summary>
        /// Consolidate metadata.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="config"></param>
        public static void ConsolidateMetadata(Context ctx, string uri, Config config)
        {
            using var ctxHandle = ctx.Handle.Acquire();
            var ms_uri = new MarshaledString(uri);
            using var configHandle = config.Handle.Acquire();
            ctx.handle_error(Methods.tiledb_array_consolidate_metadata(ctxHandle, ms_uri, configHandle));
        }
        #endregion User api

        #region IDictionary interface
        public byte[] this[string key] 
        {
            get 
            {
                var ctx = _array.Context();
                void* value_p;
                var ms_key = new MarshaledString(key);
                var datatype = tiledb_datatype_t.TILEDB_ANY;

                int has_key = 0;

                using (var ctxHandle = ctx.Handle.Acquire())
                using (var arrayHandle = _array.Handle.Acquire())
                {
                    ctx.handle_error(Methods.tiledb_array_has_metadata_key(ctxHandle, arrayHandle, ms_key, &datatype, &has_key));
                }
                if (has_key <= 0)
                {
                     
                    return new byte[0];
                }

                uint value_num = 0;
                using (var ctxHandle = ctx.Handle.Acquire())
                using (var arrayHandle = _array.Handle.Acquire())
                {
                    ctx.handle_error(Methods.tiledb_array_get_metadata(ctxHandle, arrayHandle, ms_key, &datatype, &value_num, &value_p));
                }
                var size = (int)(value_num * EnumUtil.TileDBDataTypeSize(datatype));
                var fill_span = new ReadOnlySpan<byte>(value_p, size);
                return fill_span.ToArray();
            }
            set 
            {
                Add(key, value);
            }   
        }

        public ICollection<string> Keys 
        {
            get
            {
                List<string> ret = new List<string>();
                var ctx = _array.Context();
                ulong num;

                using (var ctxHandle = ctx.Handle.Acquire())
                using (var arrayHandle = _array.Handle.Acquire())
                {
                    ctx.handle_error(Methods.tiledb_array_get_metadata_num(ctxHandle, arrayHandle, &num));
                }

                for (ulong i = 0; i < num; ++i)
                {
                    var metadata = get_metadata_from_index(i);
                    ret.Add(metadata.key);
                }
                return ret;
            }
        }

        public ICollection<byte[]> Values
        {
            get
            {
                List<byte[]> ret = new List<byte[]>();
                ulong num = MetadataNum(); 
                for (ulong i = 0; i < num; ++i)
                {
                    var metadata = get_metadata_from_index(i);
                    ret.Add(metadata.data);
                }

                return ret;
            }
        }

        public int Count 
        {
            get
            {
                ulong num = MetadataNum();
                return (int)num;
            }
        }

        public bool IsReadOnly 
        {
            get
            {
                QueryType querytype = _array.QueryType();
                return querytype == QueryType.TILEDB_READ;
            }
        }

        public void Add(string key, byte[] value)
        {
            put_metadata<byte>(key, value, tiledb_datatype_t.TILEDB_UINT8, (uint)value.Length);
        }

        public void Add(KeyValuePair<string, byte[]> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, byte[]> item)
        {
            return ContainsKey(item.Key); 
        }

        public bool ContainsKey(string key)
        {
            var (has_key, _) = HasMetadata(key);
            return has_key; 
        }

        public void CopyTo(KeyValuePair<string, byte[]>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            DeleteMetadata(key);
            return true;
        }

        public bool Remove(KeyValuePair<string, byte[]> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out byte[] value)
        {
            if (!ContainsKey(key))
            {
                value = null;
                return false;
            }
            var metadata = get_metadata(key);
            value = metadata.data;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion IDictionary interface

        #region Private methods
        private void put_metadata<T>(string key, T[] value, tiledb_datatype_t tiledb_datatype, uint value_num) where T: struct
        {
            if (string.IsNullOrEmpty(key) || value.Length == 0)
            {
                throw new ArgumentException("ArrayMetadata.put_metadata, null or empty key-value!");
            }
            var ctx = _array.Context(); 
            var ms_key = new MarshaledString(key);
            var dataGcHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
            try
            {
                using var ctxHandle = ctx.Handle.Acquire();
                using var arrayHandle = _array.Handle.Acquire();
                ctx.handle_error(Methods.tiledb_array_put_metadata(ctxHandle, arrayHandle, ms_key, tiledb_datatype, value_num,
                    (void*)dataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        private (string key, byte[] data, tiledb_datatype_t tiledb_datatype, uint value_num) get_metadata(string key)
        {
            var ctx = _array.Context();
            void* value_p;
            var ms_key = new MarshaledString(key);
            var datatype = tiledb_datatype_t.TILEDB_ANY;
            uint value_num = 0;
            using (var ctxHandle = ctx.Handle.Acquire())
            using (var arrayHandle = _array.Handle.Acquire())
            {
                ctx.handle_error(Methods.tiledb_array_get_metadata(ctxHandle, arrayHandle, ms_key, &datatype, &value_num, &value_p));
            }
            var size = (int)(value_num * EnumUtil.TileDBDataTypeSize(datatype));
            var fill_span = new ReadOnlySpan<byte>(value_p, size);
            return (key, fill_span.ToArray(), datatype, value_num);
        }

        private (string key, byte[] data, tiledb_datatype_t tiledb_datatype, uint value_num) get_metadata_from_index(ulong index)
        {
            var ctx = _array.Context();
            void* value_p;
            var ms_key = new MarshaledStringOut();
            var dataType = tiledb_datatype_t.TILEDB_ANY;
            uint valueNum = 0;
            fixed (sbyte** p_key = &ms_key.Value)
            {
                uint key_len;
                using var ctxHandle = ctx.Handle.Acquire();
                using var arrayHandle = _array.Handle.Acquire();
                ctx.handle_error(Methods.tiledb_array_get_metadata_from_index(ctxHandle, arrayHandle, index, p_key, &key_len, &dataType, &valueNum, &value_p));
            }
            var size = (int)(valueNum * EnumUtil.TileDBDataTypeSize(dataType));
            var fill_span = new ReadOnlySpan<byte>(value_p, size);
            return (ms_key, fill_span.ToArray(), dataType, valueNum);
        }
        #endregion
    }
}
