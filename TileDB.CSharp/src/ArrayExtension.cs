using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB
{
    public static class ArrayExtension
    {
        public static Newtonsoft.Json.Linq.JObject GetArraySchemaJson(this TileDB.Array array) {
            if (array == null)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                string jsonstr = array.schema().to_json_str();
                return Newtonsoft.Json.Linq.JObject.Parse(jsonstr);
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (Newtonsoft.Json.JsonReaderException je)
            {
                System.Console.WriteLine("caught JsonReaderException:");
                System.Console.WriteLine(je.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
            return new Newtonsoft.Json.Linq.JObject();
        }

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJson(this TileDB.Array array)
        {
            if (array == null)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                string jsonstr = array.get_metadata_json_str();
                return Newtonsoft.Json.Linq.JObject.Parse(jsonstr);
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (Newtonsoft.Json.JsonReaderException je)
            {
                System.Console.WriteLine("caught JsonReaderException:");
                System.Console.WriteLine(je.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
            return new Newtonsoft.Json.Linq.JObject();
        }

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJsonForKey(this TileDB.Array array, string key)
        {
            if (array == null || string.IsNullOrEmpty(key))
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                string jsonstr = array.get_metadata_json_str_for_key(key);
                return Newtonsoft.Json.Linq.JObject.Parse(jsonstr);
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (Newtonsoft.Json.JsonReaderException je)
            {
                System.Console.WriteLine("caught JsonReaderException:");
                System.Console.WriteLine(je.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
            return new Newtonsoft.Json.Linq.JObject();
        }

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJsonFromIndex(this TileDB.Array array, ulong index)
        {
            if (array == null)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                string jsonstr = array.get_metadata_json_str_from_index(index);
                return Newtonsoft.Json.Linq.JObject.Parse(jsonstr);
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (Newtonsoft.Json.JsonReaderException je)
            {
                System.Console.WriteLine("caught JsonReaderException:");
                System.Console.WriteLine(je.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
            return new Newtonsoft.Json.Linq.JObject();
        }


        public static void AddArrayMetadataByJson(this TileDB.Array array, Newtonsoft.Json.Linq.JObject j)
        {
            if (array == null || j == null)
            {
                return;
            }
            try
            {
                string jsonstr = j.ToString();
                array.put_metadata_by_json_str(jsonstr);
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }

        public static void AddArrayMetadataByJsonForKey(this TileDB.Array array, string key, Newtonsoft.Json.Linq.JObject j)
        {
            //key can be null or empty as long as key is in json object
            if (array == null || j == null)
            {
                return;
            }

            try
            {
                string jsonstr = j.ToString();
                array.put_metadata_by_json_str_for_key(key, jsonstr);
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }

        }

        public static void AddArrayMetadataByStringKeyValue(this TileDB.Array array, string key, string value)
        {
            if (array == null || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return;
            }
            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{");

                sb.AppendFormat("\"key\":\"{0}\"", key);

                sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_STRING_ASCII);

                sb.AppendFormat(",\"value\":\"{0}\"", value);

                sb.Append("}");

                array.put_metadata_by_json_str_for_key(key, sb.ToString());

            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }
        public static void AddArrayMetadataByStringMap(this TileDB.Array array, System.Collections.Generic.Dictionary<string, string> strmap)
        {
            if (array == null || strmap == null || strmap.Count == 0)
            {
                return;
            }
            try
            {
 
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{");

                sb.AppendFormat("\"metadata_num\":{0}", strmap.Count);
                sb.Append(",\"metadata\":[");

                int count = 0;
                foreach (var item in strmap)
                {
                    if (count > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append("{");

                    sb.AppendFormat("\"key\":\"{0}\"", item.Key);

                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_STRING_ASCII);

                    sb.AppendFormat(",\"value\":\"{0}\"", item.Value);

                    sb.Append("}");


                    ++count;
                }

                sb.Append("]");

                sb.Append("}");

                array.put_metadata_by_json_str(sb.ToString());
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }

        public static void AddArrayMetadataByList<T>(this TileDB.Array array, string uri, string key, System.Collections.Generic.List<T> list) where T : struct
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(key) || list == null || list.Count == 0)
            {
                return;
            }
            try
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{");

                sb.AppendFormat("\"key\":\"{0}\"", key);

                System.Type t = typeof(T);
                if (t == typeof(System.Int32))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_INT32);
                }
                else if (t == typeof(System.Int64))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_INT64);
                }
                else if (t == typeof(float))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_FLOAT32);

                }
                else if (t == typeof(System.Double))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_FLOAT64);
                }
                else if (t == typeof(System.Byte))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_STRING_ASCII);

                }
                else if (t == typeof(System.Int16))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_INT16);

                }
                else if (t == typeof(System.UInt16))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_UINT16);

                }
                else if (t == typeof(System.UInt32))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_UINT32);

                }
                else if (t == typeof(System.UInt64))
                {
                    sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_UINT64);
                }
                else
                {
                    System.Console.WriteLine("AddArrayMetadataByList, unsupported type:{0}", t);
                }

                sb.AppendFormat(",\"value_num\":{0}", list.Count);

                sb.Append(",\"value\":[");

                for (int i = 0; i < list.Count; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }
                    sb.AppendFormat("{0}", list[i]);
                }

                sb.Append("]");

                sb.Append("}");

                array.put_metadata_by_json_str_for_key(key, sb.ToString());
            }
            catch (TileDB.TileDBError tdbe)
            {
                System.Console.WriteLine("caught TileDBError:");
                System.Console.WriteLine(tdbe.Message);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("caught exception:");
                System.Console.WriteLine(e.Message);
            }
        }



    }
}
