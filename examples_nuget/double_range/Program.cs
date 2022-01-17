using System;
using System.Linq;
using TileDB;  
var ctx = new TileDB.Context();
var dom = new TileDB.Domain(ctx);
//add dimensions
dom.add_double_dimension("rows",1.0,10.0,10.0);
dom.add_double_dimension("cols",1.0,10.0,10.0);
var schema = new TileDB.ArraySchema(ctx,TileDB.ArrayType.TILEDB_SPARSE);
schema.set_domain(dom);
//add attribute
var attr1 = TileDB.Attribute.create_attribute(ctx, "a1", TileDB.DataType.TILEDB_STRING_ASCII);
//var attr1 =  TileDB.Attribute.create_attribute(ctx, "a1", TileDB.DataType.TILEDB_INT32);
 
schema.add_attribute(attr1);
string array_uri = "test_doublerange_array";
var vfs = new TileDB.VFS(ctx);
if(vfs.is_dir(array_uri))
{
    vfs.remove_dir(array_uri);

}

TileDB.Array.create(array_uri, schema);

TileDB.QueryStatus status = TileDB.QueryStatus.TILEDB_UNINITIALIZED;


TileDB.TileDBBuffer<double> tdb_dim1_buffer = new TileDBBuffer<double>();
tdb_dim1_buffer.Init(8, false, false);
for (int i = 0; i < 8; ++i)
{
    tdb_dim1_buffer.Data[i] = 1.1+ (i * 0.1);
}

TileDB.TileDBBuffer<double> tdb_dim2_buffer = new TileDBBuffer<double>();
tdb_dim2_buffer.Init(8, false, false);
for (int i = 0; i < 8; ++i)
{
    tdb_dim2_buffer.Data[i] = 2.1 + (i*0.1);
}

TileDB.TileDBBuffer<int> tdb_int_buffer = new TileDBBuffer<int>();
tdb_int_buffer.Init(8, false, false);
for (int i = 0; i < 8; ++i)
{
    tdb_int_buffer.Data[i] = i;
}

TileDB.TileDBBuffer<System.String> strbuffer = new TileDBBuffer<string>();
string[] strarr = new string[8]
{
   "a","b","c","d",
   "aa","bb","cc","dd" 
};
strbuffer.PackStringArray(strarr);

 
var array_write = new TileDB.Array(ctx,array_uri,TileDB.QueryType.TILEDB_WRITE);

TileDB.Query query_write = new TileDB.Query(ctx, array_write, TileDB.QueryType.TILEDB_WRITE);
query_write.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
 
query_write.set_buffer("rows", tdb_dim1_buffer.DataIntPtr, tdb_dim1_buffer.BufferSize, tdb_dim1_buffer.ElementDataSize);
query_write.set_buffer("cols", tdb_dim2_buffer.DataIntPtr, tdb_dim2_buffer.BufferSize, tdb_dim2_buffer.ElementDataSize);
query_write.set_buffer_with_offsets("a1", strbuffer.DataIntPtr, strbuffer.BufferSize, strbuffer.ElementDataSize, strbuffer.Offsets);
//query_write.set_buffer("a1", tdb_int_buffer.DataIntPtr, tdb_int_buffer.BufferSize, tdb_int_buffer.ElementDataSize);

query_write.submit();
query_write.finalize();
status=query_write.query_status();

array_write.close();

var array_read = new TileDB.Array(ctx, array_uri, TileDB.QueryType.TILEDB_READ);
 
//initialize big enough buffers
TileDB.TileDBBuffer<double> tdb_dim1_buffer_read = new TileDBBuffer<double>();
tdb_dim1_buffer_read.Init(8, false, false);

TileDB.TileDBBuffer<double> tdb_dim2_buffer_read = new TileDBBuffer<double>();
tdb_dim2_buffer_read.Init(8, false, false);

//Here we initialize the string buffer to have maxium number of string as 8, for each string might have several characters
//so we use 256 as buffer size for char
TileDB.TileDBBuffer<string> tdb_str_buffer_read = new TileDBBuffer<string>();
tdb_str_buffer_read.Init(8, true, false, 256);

 
TileDB.MapStringVectorUInt64 buffer_elements = new TileDB.MapStringVectorUInt64();

try {
    TileDB.Query query_read = new TileDB.Query(ctx, array_read, TileDB.QueryType.TILEDB_READ);

    query_read.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);

    query_read.set_buffer("rows", tdb_dim1_buffer_read.DataIntPtr, tdb_dim1_buffer_read.BufferSize, tdb_dim1_buffer_read.ElementDataSize);
    query_read.set_buffer("cols", tdb_dim2_buffer_read.DataIntPtr, tdb_dim2_buffer_read.BufferSize, tdb_dim2_buffer_read.ElementDataSize);
    query_read.set_buffer_with_offsets("a1", tdb_str_buffer_read.DataIntPtr, tdb_str_buffer_read.BufferSize, tdb_str_buffer_read.ElementDataSize, tdb_str_buffer_read.Offsets);

 
    //add range for rows dimension with dimension index 0
    TileDB.VectorString range1 = new TileDB.VectorString();
    range1.Add("1.1");
    range1.Add("1.5");
    query_read.add_range_from_str_vector(0, range1);

    //add range for cols dimension with dimension index 1
    TileDB.VectorString range2 = new TileDB.VectorString();
    range2.Add("2.2");
    range2.Add("2.5");
    query_read.add_range_from_str_vector(1,range2);

    query_read.submit();
    query_read.finalize();
    status = query_read.query_status();
    buffer_elements = query_read.result_buffer_elements();
 
}
catch (TileDB.TileDBError tdbe)
{
    System.Console.WriteLine(tdbe.Message);

}
catch (System.Exception e)
{
    System.Console.WriteLine(e.Message);

}

//dim1 double result
UInt64 result_el_dim1_size = buffer_elements["rows"][1];
var dim1_double = tdb_dim1_buffer_read.Data.Take((int)result_el_dim1_size);

//dim2 double result
UInt64 result_el_dim2_size = buffer_elements["cols"][1];
var dim2_double = tdb_dim2_buffer_read.Data.Take((int)result_el_dim2_size);    


// a1 string query result
UInt64 result_el_a1_data_length = buffer_elements["a1"][0];
UInt64 result_el_a1_buffer_size = buffer_elements["a1"][1];
string[] a1_str = tdb_str_buffer_read.UnPackStringArray((int)result_el_a1_buffer_size, (int)result_el_a1_data_length);

//print out 
System.Console.WriteLine("query result rows:{0}, cols:{1}, a1:{2}", String.Join(" ", dim1_double), String.Join(" ", dim2_double), String.Join(" ", a1_str));

array_read.close();

//Clean up
if(vfs.is_dir(array_uri))
{
    vfs.remove_dir(array_uri);
}


