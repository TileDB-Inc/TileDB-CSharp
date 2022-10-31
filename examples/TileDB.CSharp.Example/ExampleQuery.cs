using System;
using System.IO;
using TileDB.CSharp;
namespace TileDB.CSharp.Examples
{
    static class ExampleQuery
    {
        private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("sparse-array");
        private static readonly Context Ctx = Context.GetDefault();

        private static void CreateArray()
        {
            // Create array
            int[] dim1_bound = new int[] { 1, 4 };
            int dim1_extent = 4;
            var dim1 = Dimension.Create<int>(Ctx, "rows", dim1_bound, dim1_extent);
            int[] dim2_bound = new int[] { 1, 4 };
            int dim2_extent = 4;
            var dim2 = Dimension.Create<int>(Ctx, "cols", dim2_bound, dim2_extent);
            var domain = new Domain(Ctx);
            domain.AddDimension(dim1);
            domain.AddDimension(dim2);
            var array_schema = new ArraySchema(Ctx, ArrayType.TILEDB_SPARSE);
            var attr = new Attribute(Ctx, "a", DataType.TILEDB_INT32);
            array_schema.AddAttribute(attr);
            array_schema.SetDomain(domain);
            array_schema.Check();

            Array.Create(Ctx, ArrayPath, array_schema);

        } //private void CreateArray()

        private static void WriteArray()
        {
            var dim1_data_buffer = new int[3] { 1, 2, 3 };
            var dim2_data_buffer = new int[3] { 1, 3, 4 };
            var attr_data_buffer = new int[3] { 1, 2, 3 };

            using (var array_write = new Array(Ctx, ArrayPath))
            {
                array_write.Open(QueryType.TILEDB_WRITE);
                var query_write = new Query(Ctx, array_write);
                query_write.SetLayout(LayoutType.TILEDB_UNORDERED);
                query_write.SetDataBuffer<int>("rows", dim1_data_buffer);
                query_write.SetDataBuffer<int>("cols", dim2_data_buffer);
                query_write.SetDataBuffer<int>("a", attr_data_buffer);
                query_write.Submit();
                array_write.Close();
            }//array_write

        }

        private static void ReadArray()
        {
            var dim1_data_buffer_read = new int[3];
            var dim2_data_buffer_read = new int[3];
            var attr_data_buffer_read = new int[3];

            using (var array_read = new Array(Ctx, ArrayPath))
            {
                array_read.Open(QueryType.TILEDB_READ);
                var query_read = new Query(Ctx, array_read);
                query_read.SetLayout(LayoutType.TILEDB_ROW_MAJOR);
                query_read.SetDataBuffer<int>("rows", dim1_data_buffer_read);
                query_read.SetDataBuffer<int>("cols", dim2_data_buffer_read);
                query_read.SetDataBuffer<int>("a", attr_data_buffer_read);
                query_read.Submit();
                array_read.Close();
            }

            System.Console.WriteLine("dim1:{0},{1},{2}", dim1_data_buffer_read[0], dim1_data_buffer_read[1], dim1_data_buffer_read[2]);
            System.Console.WriteLine("dim2:{0},{1},{2}", dim2_data_buffer_read[0], dim2_data_buffer_read[1], dim2_data_buffer_read[2]);
            System.Console.WriteLine("attr:{0},{1},{2}", attr_data_buffer_read[0], attr_data_buffer_read[1], attr_data_buffer_read[2]);
        }

        private static void OnReadCompleted(object sender, QueryEventArgs args)
        {
            System.Console.WriteLine("Read completed!");
        }

        private static void ReadArrayAsync()
        {
            var dim1_data_buffer_read = new int[3];
            var dim2_data_buffer_read = new int[3];
            var attr_data_buffer_read = new int[3];

            var array_read = new Array(Ctx, ArrayPath);
            array_read.Open(QueryType.TILEDB_READ);
            var query_read = new Query(Ctx, array_read);
            query_read.SetLayout(LayoutType.TILEDB_ROW_MAJOR);
            query_read.SetDataBuffer<int>("rows", dim1_data_buffer_read);
            query_read.SetDataBuffer<int>("cols", dim2_data_buffer_read);
            query_read.SetDataBuffer<int>("a", attr_data_buffer_read);
            query_read.QueryCompleted += OnReadCompleted!;
            query_read.SubmitAsync();
        }

        public static void Run()
        {
            if (Directory.Exists(ArrayPath))
            {
                Directory.Delete(ArrayPath, true);
            }

            CreateArray();
            WriteArray();
            ReadArray();
            ReadArrayAsync();
        }
    }//class
}//namespace