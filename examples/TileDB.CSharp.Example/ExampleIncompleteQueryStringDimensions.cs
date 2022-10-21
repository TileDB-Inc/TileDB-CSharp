using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileDB.CSharp.Examples
{
    public static class ExampleIncompleteQueryStringDimensions
    {
        static readonly string ArrayPath = ExampleUtil.MakeExamplePath("sparse-incomplete-query-string-dimensions");
        private static readonly Context Ctx = Context.GetDefault();

        private static void CreateArray()
        {
            var rows = Dimension.CreateString(Ctx, "rows");
            var cols = Dimension.CreateString(Ctx, "cols");

            var domain = new Domain(Ctx);
            domain.AddDimensions(rows, cols);

            var schema = new ArraySchema(Ctx, ArrayType.Sparse);
            schema.SetDomain(domain);

            var attr = new Attribute(Ctx, "a1", DataType.Int32);
            schema.AddAttribute(attr);
            schema.Check();

            Array.Create(Ctx, ArrayPath, schema);
        }

        static async Task WriteArrayAsync()
        {
            var rowsData = Encoding.ASCII.GetBytes("abbcccddddeeeee");
            var colsData = Encoding.ASCII.GetBytes("jjjjjiiiihhhggf");

            var rowsOffsets = new ulong[] { 0, 1, 3, 6, 10 };
            var colsOffsets = new ulong[] { 0, 5, 9, 12, 14 };

            int[] attrData = Enumerable.Range(1, 5).ToArray();

            using (var arrayWrite = new Array(Ctx, ArrayPath))
            {
                arrayWrite.Open(QueryType.Write);

                var queryWrite = new Query(Ctx, arrayWrite);
                queryWrite.SetLayout(LayoutType.Unordered);
                queryWrite.SetDataBuffer("rows", rowsData);
                queryWrite.SetOffsetsBuffer("rows", rowsOffsets);
                queryWrite.SetDataBuffer("cols", colsData);
                queryWrite.SetOffsetsBuffer("cols", colsOffsets);
                queryWrite.SetDataBuffer("a1", attrData);
                await queryWrite.SubmitAsync();

                Console.WriteLine($"Write query status: {queryWrite.Status()}");
                arrayWrite.Close();
            }
        }

        static async Task ReadArrayAsync()
        {
            using (var arrayRead = new Array(Ctx, ArrayPath))
            {
                arrayRead.Open(QueryType.Read);
                var queryRead = new Query(Ctx, arrayRead);
                queryRead.SetLayout(LayoutType.Unordered);
                queryRead.AddRange("rows", "a", "eeeee");
                queryRead.AddRange("cols", "f", "jjjjj");

                var attrRead = new int[5];
                queryRead.SetDataBuffer("a1", attrRead);

                var rowsRead = new byte[15];
                var rowsReadOffsets = new ulong[2];
                queryRead.SetDataBuffer("rows", rowsRead);
                queryRead.SetOffsetsBuffer("rows", rowsReadOffsets);

                var colsRead = new byte[15];
                var colsReadOffsets = new ulong[2];
                queryRead.SetDataBuffer("cols", colsRead);
                queryRead.SetOffsetsBuffer("cols", colsReadOffsets);

                int batchNum = 1;
                do
                {
                    await queryRead.SubmitAsync();
                    var resultBufferElements = queryRead.ResultBufferElements();

                    var rowDataElements = (int)resultBufferElements["rows"].Item1;
                    var rowOffsetElements = (int)resultBufferElements["rows"].Item2!;

                    var colDataElements = (int)resultBufferElements["cols"].Item1;
                    var colOffsetElements = (int)resultBufferElements["cols"].Item2!;

                    // Copy rows data into string buffer
                    List<string> rowsData = new();
                    for (int i = 0; i < rowOffsetElements; i++)
                    {
                        // Final offset value should be handled differently
                        // + We consume the rest of the data buffer beginning at index rowsReadOffsets[i]
                        int cellSize = i == rowOffsetElements - 1 ?
                            rowDataElements - (int)rowsReadOffsets[i]
                            : (int)rowsReadOffsets[i + 1] - (int)rowsReadOffsets[i];

                        string result = new(Encoding.ASCII.GetChars(rowsRead, (int)rowsReadOffsets[i], cellSize));
                        rowsData.Add(result);
                    }

                    List<string> colsData = new();
                    for (int i = 0; i < colOffsetElements; i++)
                    {
                        int cellSize = i == colOffsetElements - 1 ?
                            colDataElements - (int)colsReadOffsets[i]
                            : (int)colsReadOffsets[i + 1] - (int)colsReadOffsets[i];

                        string result = new(Encoding.ASCII.GetChars(colsRead, (int)colsReadOffsets[i], cellSize));
                        colsData.Add(result);
                    }

                    // Zip (row, col) together
                    var coordList = rowsData.Zip(colsData, (row, col) =>
                        new Tuple<string, string>(row, col));
                    // Zip (coordinate, value) together
                    var resultList = coordList.Zip(attrRead, (coord, val) =>
                            new Tuple<Tuple<string, string>, int>(coord, val));
                    // Output read data as list of ((row, col), value)
                    Console.WriteLine($"Batch #{batchNum++}: {string.Join(", ", resultList)}");

                    // If read is still incomplete, submit query again using allocated buffers
                } while (queryRead.Status() == QueryStatus.Incomplete);

                Console.WriteLine($"Final read query status: {queryRead.Status()}");
                arrayRead.Close();
            }
        }

        public static async Task RunAsync()
        {
             if (Directory.Exists(ArrayPath))
             {
                 Directory.Delete(ArrayPath, true);
             }

             CreateArray();
             await WriteArrayAsync();
             await ReadArrayAsync();
        }
    }
}
