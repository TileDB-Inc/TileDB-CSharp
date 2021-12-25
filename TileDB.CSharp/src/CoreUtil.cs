 
// TileDB Core lib helper functions
namespace TileDB {

    public class CoreUtil {

        public static string GetCoreLibVersion() {
            return ArrayUtil.get_tiledb_version();
        }

        public static TileDB.Array OpenArray(string uri, TileDB.QueryType querytype = TileDB.QueryType.TILEDB_READ, Context ctx = null)
        {
            if (uri == null || uri.Length == 0) {
                return null;
            }
            try
            {
                if(ctx==null)
                {
                    ctx = new TileDB.Context();
                }
                return new TileDB.Array(ctx, uri, querytype);
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

            return null;
        }

        public static Newtonsoft.Json.Linq.JObject GetArraySchemaJson(string uri, Context ctx=null)
        {
            if (uri == null || uri.Length == 0)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }
                string jsonstr = ArrayUtil.get_array_schema_json_str(uri, ctx);
                Newtonsoft.Json.Linq.JObject j = Newtonsoft.Json.Linq.JObject.Parse(jsonstr);
                return j;


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

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJson(string uri, Context ctx)
        {
            if (uri == null || uri.Length == 0)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                string jsonstr = ArrayUtil.get_array_metadata_json_str(uri, ctx);
                return  Newtonsoft.Json.Linq.JObject.Parse(jsonstr);

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

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJsonForKey(string uri, string key, Context ctx)
        {
            if (uri == null || uri.Length == 0 || key == null || key.Length==0)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {

                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                string jsonstr = ArrayUtil.get_array_metadata_json_str_for_key(uri, key, ctx);
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

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJsonFromIndex(string uri, ulong index, Context ctx)
        {
            if (uri == null || uri.Length == 0)
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                string jsonstr = ArrayUtil.get_array_metadata_json_str_from_index(uri, index, ctx);
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

        public static void AddArrayMetadataByJson(string uri, Newtonsoft.Json.Linq.JObject j, Context ctx)
        {
            if (uri == null || uri.Length == 0 || j==null)
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                string jsonstr = j.ToString();
                ArrayUtil.add_array_metadata_by_json_str(uri, jsonstr, ctx);
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

        public static void AddArrayMetadataByJsonForKey(string uri, string key, Newtonsoft.Json.Linq.JObject j, Context ctx)
        {
            if (uri == null || uri.Length == 0 || j==null)
            {
                return;
            }

            try
            {

                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                string jsonstr = j.ToString();
                ArrayUtil.add_array_metadata_by_json_str_for_key(uri, key, jsonstr, ctx);
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

        public static void AddArrayMetadataByStringKeyValue(string uri, string key, string value, Context ctx)
        {
            if (uri==null || uri.Length == 0 || key == null || key.Length == 0 || value==null || value.Length==0)
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{");

                sb.AppendFormat("\"key\":\"{0}\"", key);

                sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_STRING_ASCII);

                sb.AppendFormat(",\"value\":\"{0}\"", value);

                sb.Append("}");

                ArrayUtil.add_array_metadata_by_json_str_for_key(uri, key, sb.ToString(), ctx);
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
        public static void AddArrayMetadataByStringMap(string uri, System.Collections.Generic.Dictionary<string,string> strmap, Context ctx)
        {
            if (uri == null || uri.Length ==0 || strmap == null || strmap.Count == 0) 
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }
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

                ArrayUtil.add_array_metadata_by_json_str(uri, sb.ToString(), ctx);
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

        public static void AddArrayMetadataByList<T>(string uri, string key, System.Collections.Generic.List<T> list, Context ctx) where T: struct
        {
            if (uri==null || uri.Length ==0 || key == null || key.Length == 0 || list == null || list.Count == 0)
            {
                return;
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

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

                ArrayUtil.add_array_metadata_by_json_str_for_key(uri, key, sb.ToString(), ctx);
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




    }//class 

}//namespace