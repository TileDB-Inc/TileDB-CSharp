using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TileDB.CSharp.Examples;

public static class ExampleIncompleteQuery
{
    static readonly string ArrayPath = ExampleUtil.MakeExamplePath("sparse-incomplete-query");
    private static readonly Context Ctx = Context.GetDefault();

    private static void CreateArray()
    {
        var dim1 = Dimension.Create(Ctx, "rows", boundLower: 1, boundUpper: 10, extent: 4);
        var dim2 = Dimension.Create(Ctx, "cols", boundLower: 1, boundUpper: 10, extent: 4);

        var domain = new Domain(Ctx);
        domain.AddDimension(dim1);
        domain.AddDimension(dim2);

        var schema = new ArraySchema(Ctx, ArrayType.Sparse);
        schema.SetDomain(domain);

        var attr = new Attribute(Ctx, "a1", DataType.Int32);
        schema.AddAttribute(attr);
        schema.Check();

        Array.Create(Ctx, ArrayPath, schema);
    }

    private static void WriteArray()
    {
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
        using (var arrayWrite = new Array(Ctx, ArrayPath))
        {
            arrayWrite.Open(QueryType.Write);

            var queryWrite = new Query(arrayWrite);
            queryWrite.SetLayout(LayoutType.Unordered);
            queryWrite.SetDataBuffer("rows", rowsData);
            queryWrite.SetDataBuffer("cols", colsData);
            queryWrite.SetDataBuffer("a1", attrData);
            queryWrite.Submit();

            Console.WriteLine($"Write query status: {queryWrite.Status()}");
            arrayWrite.Close();
        }
    }

    private static void ReadArray()
    {
        // Allocate buffers for 10 values per batch read
        var rowsRead = new int[10];
        var colsRead = new int[10];
        var attrRead = new int[10];

        using (var arrayRead = new Array(Ctx, ArrayPath))
        {
            arrayRead.Open(QueryType.Read);
            var queryRead = new Query(arrayRead);
            queryRead.SetLayout(LayoutType.Unordered);

            queryRead.SetDataBuffer("rows", rowsRead);
            queryRead.SetDataBuffer("cols", colsRead);
            queryRead.SetDataBuffer("a1", attrRead);

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
            } while (queryRead.Status() == QueryStatus.Incomplete);

            Console.WriteLine($"Final read query status: {queryRead.Status()}");
            arrayRead.Close();
        }
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
    }
}
