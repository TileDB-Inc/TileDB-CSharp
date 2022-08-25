using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TileDB.CSharp.Examples
{
    public class ExampleIncompleteQuery
    {
        static string arrayName = "sparse-incomplete-query";

        static void CreateArray()
        {
            var ctx = Context.GetDefault();

            int[] dim1_bound = { 1, 10 };
            int dim1_ext = 4;
            var dim1 = Dimension.Create<int>(ctx, "rows", dim1_bound, dim1_ext);

            int[] dim2_bound = { 1, 10 };
            int dim2_ext = 4;
            var dim2 = Dimension.Create<int>(ctx, "cols", dim2_bound, dim2_ext);

            var domain = new Domain(ctx);
            domain.AddDimension(dim1);
            domain.AddDimension(dim2);

            var schema = new ArraySchema(ctx, ArrayType.TILEDB_SPARSE);
            schema.SetDomain(domain);

            var attr = new Attribute(ctx, "a1", DataType.TILEDB_INT32);
            schema.AddAttribute(attr);
            schema.Check();

            Array.Create(ctx, arrayName, schema);
        }

        static void WriteArray()
        {
            var ctx = Context.GetDefault();

            // Produce columns iteratively
            List<int> colsList = new();
            for (int i = 0; i < 10; i++)
            {
                colsList.AddRange(Enumerable.Range(1, 10));
            }
            int[] colsData = colsList.ToArray();

            // Produce rows using LINQ
            int[] rowsData =
                (from a in Enumerable.Range(1, 10)
                from b in Enumerable.Range(1, 10)
                select a).ToArray();

            // Write a1 attribute with values 1-100
            int[] attrData = Enumerable.Range(1, 100).ToArray();
            using (var arrayWrite = new Array(ctx, arrayName))
            {
                arrayWrite.Open(QueryType.TILEDB_WRITE);

                var queryWrite = new Query(ctx, arrayWrite);
                queryWrite.SetLayout(LayoutType.TILEDB_UNORDERED);
                queryWrite.SetDataBuffer<int>("rows", rowsData);
                queryWrite.SetDataBuffer<int>("cols", colsData);
                queryWrite.SetDataBuffer<int>("a1", attrData);
                queryWrite.Submit();

                Console.WriteLine($"Write query status: {queryWrite.Status()}");
                arrayWrite.Close();
            }
        }

        static void ReadArray()
        {
            var ctx = Context.GetDefault();

            // Allocate buffers for 10 values per batch read
            var rowsRead = new int[10];
            var colsRead = new int[10];
            var attrRead = new int[10];

            using (var arrayRead = new Array(ctx, arrayName))
            {
                arrayRead.Open(QueryType.TILEDB_READ);
                var queryRead = new Query(ctx, arrayRead);
                queryRead.SetLayout(LayoutType.TILEDB_UNORDERED);
                queryRead.SetSubarray(new[] { 1, 100, 1, 100 });

                queryRead.SetDataBuffer<int>("rows", rowsRead);
                queryRead.SetDataBuffer<int>("cols", colsRead);
                queryRead.SetDataBuffer<int>("a1", attrRead);

                int batchNum = 1;
                do
                {
                    queryRead.Submit();

                    // Zip (row, col) together for pretty printing
                    var coordList =
                        rowsRead.Zip(colsRead, (row, col) => new Tuple<int, int>(row, col));
                    // Zip (coordinate, value) together for pretty printing
                    var resultList =
                        coordList.Zip(attrRead, (coord, val) => new Tuple<Tuple<int, int>, int>(coord, val));
                    // Output read data as list of ((row, col), value)
                    Console.WriteLine($"Batch #{batchNum++}: {string.Join(", ", resultList)}");

                    // If read is still incomplete, submit query again using allocated buffers
                } while (queryRead.Status() == QueryStatus.TILEDB_INCOMPLETE);

                Console.WriteLine($"Final read query status: {queryRead.Status()}");
                arrayRead.Close();
            }
        }

        public static void Run()
        {
             if (Directory.Exists(arrayName))
             {
                 Directory.Delete(arrayName, true);
             }

             CreateArray();
             WriteArray();
             ReadArray();
        }
    }
}