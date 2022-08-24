using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TileDB.CSharp.Examples
{
    public class ExampleIncompleteQueryStringDimensions
    {
        static string arrayName = "sparse-incomplete-query-string-dimensions";

        static void CreateArray()
        {
            var ctx = Context.GetDefault();

            var rows = Dimension.CreateString(ctx, "rows");
            var cols = Dimension.CreateString(ctx, "cols");

            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);

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

            var rowsData = Encoding.ASCII.GetBytes("abbcccddddeeeee");
            var colsData = Encoding.ASCII.GetBytes("fgghhhiiiijjjjj");

            // In this case the offsets happen to be the same
            // TODO: Flip colsData so they are different
            var rowsOffsets = new ulong[] { 0, 1, 3, 6, 10 };
            var colsOffsets = new ulong[] { 0, 1, 3, 6, 10 };

            // Write a1 attribute with values 1-100
            int[] attrData = Enumerable.Range(1, 5).ToArray();

            using (var arrayWrite = new Array(ctx, arrayName))
            {
                arrayWrite.Open(QueryType.TILEDB_WRITE);

                var queryWrite = new Query(ctx, arrayWrite);
                queryWrite.SetLayout(LayoutType.TILEDB_UNORDERED);
                queryWrite.SetDataBuffer("rows", rowsData);
                queryWrite.SetOffsetsBuffer("rows", rowsOffsets);
                queryWrite.SetDataBuffer("cols", colsData);
                queryWrite.SetOffsetsBuffer("cols", colsOffsets);
                queryWrite.SetDataBuffer("a1", attrData);
                queryWrite.Submit();

                Console.WriteLine($"Write query status: {queryWrite.Status()}");
                arrayWrite.Close();
            }
        }

        static void ReadArray()
        {
            var ctx = Context.GetDefault();

            using (var arrayRead = new Array(ctx, arrayName))
            {
                arrayRead.Open(QueryType.TILEDB_READ);
                var queryRead = new Query(ctx, arrayRead);
                queryRead.SetLayout(LayoutType.TILEDB_UNORDERED);
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
                    queryRead.Submit();
                    var s = queryRead.Status();
                    var resultBufferElements = queryRead.ResultBufferElements();

                    var rowDataElements = (int)resultBufferElements["rows"].Item1;
                    var rowOffsetElements = (int)resultBufferElements["rows"].Item2!;

                    var colDataElements = (int)resultBufferElements["cols"].Item1;
                    var colOffsetElements = (int)resultBufferElements["cols"].Item2!;

                    // Copy rows data into string buffer
                    List<string> rowsData = new();
                    for (int i = 0; i < rowOffsetElements; i++)
                    {
                        string result;
                        // Final offset value should be handled differently
                        // + We consume the rest of the data buffer beginning at index rowsReadOffsets[i]
                        int cellSize = i == rowOffsetElements - 1 ?
                            rowDataElements - (int)rowsReadOffsets[i]
                            : (int)rowsReadOffsets[i + 1] - (int)rowsReadOffsets[i];

                        result = new(Encoding.ASCII.GetChars(rowsRead, (int)rowsReadOffsets[i], cellSize));
                        rowsData.Add(result);
                    }

                    List<string> colsData = new();
                    for (int i = 0; i < colOffsetElements; i++)
                    {
                        string result;
                        int cellSize = i == colOffsetElements - 1 ?
                            colDataElements - (int)colsReadOffsets[i]
                            : (int)colsReadOffsets[i + 1] - (int)colsReadOffsets[i];

                        result = new(Encoding.ASCII.GetChars(colsRead, (int)colsReadOffsets[i], cellSize));
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