using System;
using System.IO;
using TileDB.CSharp;
namespace TileDB.CSharp.Examples
{
    class ExampleQuery
    {
        protected static void CreateArray()
        {
            var context = Context.GetDefault();

            // Create array
            int[] dim1_bound = new int[] { 1, 4 };
            int dim1_extent = 4;
            var dim1 = Dimension.Create<int>(context, "rows", dim1_bound, dim1_extent);
            int[] dim2_bound = new int[] { 1, 4 };
            int dim2_extent = 4;
            var dim2 = Dimension.Create<int>(context, "cols", dim2_bound, dim2_extent);
            var domain = new Domain(context);
            domain.AddDimension(dim1);
            domain.AddDimension(dim2);
            var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
            var attr = new Attribute(context, "a", DataType.TILEDB_INT32);
            array_schema.AddAttribute(attr);
            array_schema.SetDomain(domain);
            array_schema.Check();

            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_example_sparse_array");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            Array.Create(context, tmpArrayPath, array_schema);

        } //protected void CreateArray()

        protected static void WriteArray()
        {
            var context = Context.GetDefault();
            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_example_sparse_array");

            var dim1_data_buffer = new int[3] { 1, 2, 3 };
            var dim2_data_buffer = new int[3] { 1, 3, 4 };
            var attr_data_buffer = new int[3] { 1, 2, 3 };

            using (var array_write = new Array(context, tmpArrayPath))
            {
                array_write.Open(QueryType.TILEDB_WRITE);
                var query_write = new Query(context, array_write);
                query_write.SetLayout(LayoutType.TILEDB_UNORDERED);
                query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
                query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
                query_write.SetDataBuffer<int>("a", attr_data_buffer);
                query_write.Submit();
                var status = query_write.Status();
                array_write.Close();
            }//array_write

        }

        protected static void ReadArray()
        {
            var context = Context.GetDefault();
            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_example_sparse_array");
            var dim1_data_buffer_read = new int[3];
            var dim2_data_buffer_read = new int[3];
            var attr_data_buffer_read = new int[3];

            using (var array_read = new Array(context, tmpArrayPath))
            {
                array_read.Open(QueryType.TILEDB_READ);
                var query_read = new Query(context, array_read);
                query_read.SetLayout(LayoutType.TILEDB_ROW_MAJOR);
                query_read.SetDataBuffer<int>("rows", dim1_data_buffer_read);
                query_read.SetDataBuffer<int>("cols", dim2_data_buffer_read);
                query_read.SetDataBuffer<int>("a", attr_data_buffer_read);
                query_read.Submit();
                var status_read = query_read.Status();
                array_read.Close();
            }

            System.Console.WriteLine("dim1:{0},{1},{2}", dim1_data_buffer_read[0], dim1_data_buffer_read[1], dim1_data_buffer_read[2]);
            System.Console.WriteLine("dim2:{0},{1},{2}", dim2_data_buffer_read[0], dim2_data_buffer_read[1], dim2_data_buffer_read[2]);
            System.Console.WriteLine("attr:{0},{1},{2}", attr_data_buffer_read[0], attr_data_buffer_read[1], attr_data_buffer_read[2]);
        }

        protected static void OnReadCompleted(object sender, QueryEventArgs args)
        {
            System.Console.WriteLine("Read completed!");
        }

        protected static void ReadArrayAsync()
        {
            var context = Context.GetDefault();
            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_example_sparse_array");
            var dim1_data_buffer_read = new int[3];
            var dim2_data_buffer_read = new int[3];
            var attr_data_buffer_read = new int[3];

            var array_read = new Array(context, tmpArrayPath);
            array_read.Open(QueryType.TILEDB_READ);
            var query_read = new Query(context, array_read);
            query_read.SetLayout(LayoutType.TILEDB_ROW_MAJOR);
            query_read.SetDataBuffer<int>("rows", dim1_data_buffer_read);
            query_read.SetDataBuffer<int>("cols", dim2_data_buffer_read);
            query_read.SetDataBuffer<int>("a", attr_data_buffer_read);
            query_read.QueryCompleted += OnReadCompleted!;
            query_read.SubmitAsync();
        }

        public static void Run()
        {
            CreateArray();
            WriteArray();
            ReadArray();
            ReadArrayAsync();
        }
    }//class
}//namespace