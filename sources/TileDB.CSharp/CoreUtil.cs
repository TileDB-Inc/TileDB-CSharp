
// TileDB Core lib helper functions
using System;
using System.Text;
using System.Collections.Generic;
namespace TileDB.CSharp {

    public class CoreUtil {

        /// <summary>
        /// Get core library version.
        /// </summary>
        /// <returns></returns>
        public static Version GetCoreLibVersion() {
            int major;
            int minor;
            int rev;
            unsafe
            {
                TileDB.Interop.Methods.tiledb_version(&major, &minor, &rev);
            }

            return new Version(major, minor, rev);   
        }

        #region Metadata
        /// <summary>
        /// Add array metadata string key value pair.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [Obsolete("Will be deprecated. Please use Metadata class.", false)]
        public static void AddArrayMetadataByStringKeyValue(Context ctx, string uri, string key, string value)
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = Context.GetDefault();
                }
                using (var array = new Array(ctx, uri))
                {
                    array.Open(QueryType.TILEDB_WRITE);
                    array.PutMetadata(key, value);
                    array.Close();
                }
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Add string dictionary metadata.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="strmap"></param>
        [Obsolete("Will be deprecated. Please use Metadata class.", false)]
        public static void AddArrayMetadataByStringMap(Context ctx, string uri, System.Collections.Generic.Dictionary<string,string> strmap)
        {
            if (string.IsNullOrEmpty(uri) || strmap == null || strmap.Count == 0) 
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = Context.GetDefault();
                }

                using (var array = new Array(ctx, uri))
                {
                    array.Open(QueryType.TILEDB_WRITE);
                    foreach(var item in strmap)
                    {
                        array.PutMetadata(item.Key, item.Value);
                    }
                    array.Close();
                } 
            } 
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Add multiple values for a metadata key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <param name="key"></param>
        /// <param name="list"></param>
        [Obsolete("Will be deprecated. Please use Metadata class.", false)]
        public static void AddArrayMetadataByList<T>(Context ctx, string uri, string key, System.Collections.Generic.List<T> list) where T: struct
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(key) || list == null || list.Count == 0)
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = Context.GetDefault();
                }

                using (var array = new Array(ctx, uri))
                {
                    array.Open(QueryType.TILEDB_WRITE);

                    T[] data = list.ToArray();
                    array.PutMetadata<T>(key, data);
 
                    array.Close();
                }
                 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }

        #endregion Metadata

        #region  Buffer

        /// <summary>
        /// Convert element offsets to byte offsets.
        /// </summary>
        /// <param name="element_offsets"></param>
        /// <returns></returns>
        public static ulong[] ElementOffsetsToByteOffsets(ulong[] element_offsets, DataType dataType)
        {
            ulong[] ret = new ulong[element_offsets.Length];
            for (var i = 0; i < element_offsets.Length; ++i)
            {
                ret[i] = element_offsets[i] * EnumUtil.DataTypeSize(dataType);
            }
            return ret;
        }

        /// <summary>
        /// Convert byte offsets to element offsets
        /// </summary>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public static ulong[] ByteOffsetsToElementOffsets(ulong[] offsets, DataType dataType)
        {
            ulong[] ret = new ulong[offsets.Length];
            for (var i = 0; i < offsets.Length; ++i)
            {
                ret[i] = offsets[i] / EnumUtil.DataTypeSize(dataType);
            }
            return ret;
        }


        /// <summary>
        /// Pack string array.
        /// </summary>
        /// <param name="strarray"></param>
        /// <param name="encodingmethod"></param>
        /// <returns></returns>
        public static (byte[] data, ulong[] offsets) PackStringArray(string[] strarray, string encodingmethod = "ASCII")
        {
            if (strarray.Length == 0)
            {
                return (new byte[0], new ulong[0]);
            }
 
            int totsize = 0;
            for (int i = 0; i < strarray.Length; ++i)
            {
                int bytes_size = 0;
                if (encodingmethod == "ASCII")
                {
                    bytes_size = Encoding.ASCII.GetByteCount(strarray[i]);
                }
                else if (encodingmethod == "UTF16")
                {
                    bytes_size = Encoding.Unicode.GetByteCount(strarray[i]);
                }
                else if (encodingmethod == "UTF32")
                {
                    bytes_size = Encoding.UTF32.GetByteCount(strarray[i]);
                }
                else
                {
                    bytes_size = Encoding.ASCII.GetByteCount(strarray[i]);
                }
                totsize += bytes_size;
            }

            byte[] data = new byte[totsize];
            ulong[] offsets = new ulong[strarray.Length];
            offsets[0] = 0;

            int idx = 0;
            for (int i = 0; i < strarray.Length; ++i)
            {
                byte[] bytes;
                if (encodingmethod == "ASCII")
                {
                    bytes = Encoding.ASCII.GetBytes(strarray[i]);

                }
                else if (encodingmethod == "UTF16")
                {
                    bytes = Encoding.Unicode.GetBytes(strarray[i]);
                }
                else if (encodingmethod == "UTF32")
                {
                    bytes = Encoding.UTF32.GetBytes(strarray[i]);
                }
                else
                {
                    bytes = Encoding.ASCII.GetBytes(strarray[i]);
                }
                bytes.CopyTo(data, idx);
                idx += bytes.Length;


                if (i < (strarray.Length - 1))
                {
                    offsets[i + 1] = offsets[i] + (ulong)bytes.Length;
                }

            }
            return (data, offsets);
        }


        /// <summary>
        /// Unpack string array. 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offsets"></param>
        /// <param name="validities"></param>
        /// <param name="encodingmethod"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static List<string> UnPackStringArray(byte[] data, ulong[] offsets, byte[]? validities = null, string encodingmethod = "ASCII") 
        {
            List<string> result = new List<string>();
            int tot_bytes_size = data.Length;
            int length = offsets.Length;

            bool is_nullable = (validities != null);
            if (is_nullable && (validities.Length != offsets.Length))
            {
                throw new System.ArgumentException("CoreUtil.UnpackStringArray, the lengths of offsets and validities are not equal!");
            }

            int totsize = 0;
            int indexFrom = 0;
            for (int i = 0; i < length; ++i)
            {
                if (is_nullable && validities[i] == 0)
                {
                    continue;
                }
                int bytesize = 0;
                if (i < (length - 1))
                {
                    bytesize = (int)(offsets[i + 1] - offsets[i]);

                }
                else
                {
                    bytesize = tot_bytes_size - (int)offsets[length - 1];
                }

                totsize += bytesize;
                if (totsize <= tot_bytes_size && bytesize > 0)
                {
                    byte[] bytes = new byte[bytesize];// 
                    System.Array.Copy(data, indexFrom, bytes, 0, bytesize);
                    if (encodingmethod == "ASCII")
                    {
                        result.Add(Encoding.ASCII.GetString(bytes));
                    }
                    else if (encodingmethod == "UTF16")
                    {
                        result.Add(Encoding.Unicode.GetString(bytes));
                    }
                    else if (encodingmethod == "UTF32")
                    {
                        result.Add(Encoding.UTF32.GetString(bytes));
                    }
                    else
                    {
                        result.Add(Encoding.UTF8.GetString(bytes));
                    }

                    indexFrom = totsize;
                }

            } //for

            return result;
        }

        #endregion Buffer



    }//class 

}//namespace