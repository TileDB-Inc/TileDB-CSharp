using TileDB;  
var ctx = new TileDB.Context();
var dom = new TileDB.Domain(ctx);
//add dimensions
dom.add_int32_dimension("rows",1,4,4);
dom.add_int32_dimension("cols",1,4,4);
var schema = new TileDB.ArraySchema(ctx,TileDB.ArrayType.TILEDB_DENSE);
schema.set_domain(dom);
//add attribute
var attr1 = TileDB.Attribute.create_attribute(ctx,"a",TileDB.DataType.TILEDB_INT32);
schema.add_attribute(attr1);
string array_uri = "test_metadata_array";
var vfs = new TileDB.VFS(ctx);
if(vfs.is_dir(array_uri))
{
    vfs.remove_dir(array_uri);

}

TileDB.Array.create(array_uri,schema);


//if you donot use using statement block, please call array_write.close() at the end to materialize it on disk
using (var array_write = new TileDB.Array(ctx,array_uri,TileDB.QueryType.TILEDB_WRITE))
{
    // add int metadata
    System.Collections.Generic.List<int> int_metadata = new System.Collections.Generic.List<int>() {1,100};
    array_write.AddArrayMetadataByList<int>("int_metadata",int_metadata);
    // add double metadata
    var double_metadata = new System.Collections.Generic.List<double>() {1.0,100.0,1000.0};
    array_write.AddArrayMetadataByList<double>("double_metadata", double_metadata);
    // add string key value pair 
    array_write.AddArrayMetadataByStringKeyValue("key1","value1");
    // add multiple string key value pairs 
    var key_value_map = new System.Collections.Generic.Dictionary<string,string>();
    key_value_map["key2"] = "value2";
    key_value_map["key3"] = "value3";
    array_write.AddArrayMetadataByStringMap(key_value_map);
}

//get metadata from index
var array_read = new TileDB.Array(ctx,array_uri,TileDB.QueryType.TILEDB_READ);
var metadata_json = array_read.GetArrayMetadataJsonFromIndex(0);
System.Console.WriteLine("{0}", metadata_json.ToString());

//get metadata from key
var double_metadata_json = array_read.GetArrayMetadataJsonForKey("double_metadata");
System.Console.WriteLine("{0}", double_metadata_json.ToString());
var double_list = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<double>>(double_metadata_json["value"].ToString());
System.Console.WriteLine("type of double_list:{0}, value of double_list:{1}",double_list.GetType(),string.Join(",",double_list));

// get string metadata
var string_metadata = array_read.GetArrayMetadataJsonForKey("key2");
System.Console.WriteLine("{0}", string_metadata.ToString());
// access key and value
System.Console.WriteLine("{0}:{1}",string_metadata["key"],string_metadata["value"]);

//get all metadata
var all_metadata_json = array_read.GetArrayMetadataJson();
System.Console.WriteLine("{0}", all_metadata_json.ToString());


//clean up
array_read.close();
if(vfs.is_dir(array_uri))
{
    vfs.remove_dir(array_uri);
}


