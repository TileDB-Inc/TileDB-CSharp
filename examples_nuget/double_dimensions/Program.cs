using System;

namespace TileDB.CSharp.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateArray();

            WriteArray();

            int num_results = 0;
            double row_start = -8.08;
            double row_end = 5.96;
            double col_start = -5.32;
            double col_end = 5.28;

            num_results = ReadArray(row_start, row_end, col_start, col_end);
            System.Console.WriteLine("row:[{0},{1}],col:[{2},{3}],num_results:{4}", row_start, row_end, col_start, col_end, num_results);

            col_end = 5.27;
            num_results = ReadArray(row_start, row_end, col_start, col_end);
            System.Console.WriteLine("row:[{0},{1}],col:[{2},{3}],num_results:{4}", row_start, row_end, col_start, col_end, num_results);           
 

        }

        static void CreateArray()
        {
            var ctx = new TileDB.Context();
            var dom = new TileDB.Domain(ctx);

            //add dimensions
            dom.add_double_dimension("rows", -10.0, 10.0, 10.0);
            dom.add_double_dimension("cols", -10.0, 10.0, 10.0);
            var schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_SPARSE);
            schema.set_domain(dom);
            //add attribute
            var attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_INT32);
            schema.add_attribute(attr1);
            string array_uri = "test_doubledimensions_array";
            var vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri))
            {
                vfs.remove_dir(array_uri);

            }

            TileDB.Array.create(array_uri, schema);

        }

        static void WriteArray()
        {
            var ctx = new TileDB.Context();
            string array_uri = "test_doubledimensions_array";

            TileDB.VectorDouble data_rows = new TileDB.VectorDouble { -8.074, -2.234, 3.352, 5.952 };
            TileDB.VectorDouble data_cols = new TileDB.VectorDouble { -5.313, -3.522, 2.333, 5.277 };
            TileDB.VectorInt32 data_a = new TileDB.VectorInt32 { 1, 2, 3, 4 };

            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri, TileDB.QueryType.TILEDB_WRITE);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
            query.set_double_vector_buffer("rows", data_rows);
            query.set_double_vector_buffer("cols", data_cols);
            query.set_int32_vector_buffer("a", data_a);

            query.submit();
            query.finalize();
            array.close();

        }

        static int ReadArray(double row_start, double row_end, double col_start, double col_end)
        {
            int num_results = 0;
            var ctx = new TileDB.Context();
            string array_uri = "test_doubledimensions_array";

            TileDB.VectorDouble data_rows = TileDB.VectorDouble.Repeat(0, 4);
            TileDB.VectorDouble data_cols = TileDB.VectorDouble.Repeat(0, 4);
            TileDB.VectorInt32 data_a = TileDB.VectorInt32.Repeat(0, 4);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri, TileDB.QueryType.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
            query.set_layout(TileDB.LayoutType.TILEDB_UNORDERED);
            query.set_double_vector_buffer("rows", data_rows);
            query.set_double_vector_buffer("cols", data_cols);
            query.set_int32_vector_buffer("a", data_a);

            TileDB.VectorString row_range = new TileDB.VectorString();
            row_range.Add(row_start.ToString());
            row_range.Add(row_end.ToString());
            query.add_range_from_str_vector(0, row_range);

            TileDB.VectorString col_range = new TileDB.VectorString();
            col_range.Add(col_start.ToString());
            col_range.Add(col_end.ToString());
            query.add_range_from_str_vector(1, col_range);

            query.submit();
            query.finalize();

            var resultbufferelements = query.result_buffer_elements();
            if (resultbufferelements.ContainsKey("a")) { 
                if(resultbufferelements["a"].Count>1)
                {
                    num_results = (int)resultbufferelements["a"][1];
                }
            }
            
            array.close();

            return num_results;
        }

    }
}


 
  
