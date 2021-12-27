 
// TileDB Core lib helper functions
namespace TileDB {

    public class CoreUtil {

        public static string GetCoreLibVersion() {
            return ArrayUtil.get_tiledb_version();
        }

        public static TileDB.Array OpenArray(Context ctx = null, string uri="", TileDB.QueryType querytype = TileDB.QueryType.TILEDB_READ)
        {
            if (string.IsNullOrEmpty(uri)) {
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

        public static Newtonsoft.Json.Linq.JObject GetArraySchemaJson(Context ctx = null, string uri = "")
        {
            if (string.IsNullOrEmpty(uri))
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {
                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }
                string jsonstr = ArrayUtil.get_array_schema_json_str(ctx,uri);
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

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJson(Context ctx, string uri)
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

                string jsonstr = ArrayUtil.get_array_metadata_json_str(ctx, uri);
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

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJsonForKey(Context ctx, string uri, string key)
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(key))
            {
                return new Newtonsoft.Json.Linq.JObject();
            }
            try
            {

                if (ctx == null)
                {
                    ctx = new TileDB.Context();
                }

                string jsonstr = ArrayUtil.get_array_metadata_json_str_for_key(ctx, uri, key);
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

        public static Newtonsoft.Json.Linq.JObject GetArrayMetadataJsonFromIndex(Context ctx, string uri, ulong index)
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

                string jsonstr = ArrayUtil.get_array_metadata_json_str_from_index(ctx, uri, index);
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

        public static void AddArrayMetadataByJson(Context ctx, string uri, Newtonsoft.Json.Linq.JObject j)
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
                ArrayUtil.add_array_metadata_by_json_str(ctx, uri, jsonstr);
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

        public static void AddArrayMetadataByJsonForKey(Context ctx, string uri, string key, Newtonsoft.Json.Linq.JObject j)
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
                ArrayUtil.add_array_metadata_by_json_str_for_key(ctx, uri, key, jsonstr);
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
                    ctx = new TileDB.Context();
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{");

                sb.AppendFormat("\"key\":\"{0}\"", key);

                sb.AppendFormat(",\"value_type\":{0}", (int)TileDB.DataType.TILEDB_STRING_ASCII);

                sb.AppendFormat(",\"value\":\"{0}\"", value);

                sb.Append("}");

                ArrayUtil.add_array_metadata_by_json_str_for_key(ctx, uri, key, sb.ToString());
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

                ArrayUtil.add_array_metadata_by_json_str(ctx, uri, sb.ToString());
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

                ArrayUtil.add_array_metadata_by_json_str_for_key(ctx, uri, key, sb.ToString());
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

        public static void SaveFileToArray(TileDB.Context ctx, string array_uri, string file, string mime_type, string mime_coding)
        {
            try {
                if (ctx == null)
                {
                    TileDB.Config cfg = new TileDB.Config();
                    ctx = new TileDB.Context(cfg);
                }
                TileDB.VFS vfs = new TileDB.VFS(ctx);
                if (vfs.is_dir(array_uri))
                {
                    vfs.remove_dir(array_uri);
                }
                TileDB.ArrayUtil.save_file_from_path(ctx, array_uri, file, mime_type, mime_coding);
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

        public static void ExportArrayToFile(TileDB.Context ctx, string array_uri, string file)
        {
            try
            {
                if (ctx == null)
                {
                    TileDB.Config cfg = new TileDB.Config();
                    ctx = new TileDB.Context(cfg);
                }
                TileDB.ArrayUtil.export_file_to_path(ctx, array_uri, file, 0);
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