using System;
using System.IO;

namespace TileDB.CSharp.Examples;

static class ExampleAggregateQuery
{
    private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("aggregate-array");
    private static readonly Context Ctx = Context.GetDefault();

    private static void CreateArray()
    {
        // Create array
        var dim1 = Dimension.Create(Ctx, "rows", boundLower: 1, boundUpper: 4, extent: 4);
        var dim2 = Dimension.Create(Ctx, "cols", boundLower: 1, boundUpper: 4, extent: 4);
        var domain = new Domain(Ctx);
        domain.AddDimension(dim1);
        domain.AddDimension(dim2);
        var array_schema = new ArraySchema(Ctx, ArrayType.Sparse);
        var attr = new Attribute(Ctx, "a", DataType.Int32);
        array_schema.AddAttribute(attr);
        array_schema.SetDomain(domain);
        array_schema.Check();

        Array.Create(Ctx, ArrayPath, array_schema);
    }

    private static void WriteArray()
    {
        using (var array_write = new Array(Ctx, ArrayPath))
        {
            array_write.Open(QueryType.Write);
            using (var query_write = new Query(array_write))
            {
                query_write.SetLayout(LayoutType.GlobalOrder);
                query_write.SetDataBuffer("rows", new int[] { 1, 2 });
                query_write.SetDataBuffer("cols", new int[] { 1, 4 });
                query_write.SetDataBuffer("a", new int[] { 1, 2 });
                query_write.Submit();
                query_write.SetDataBuffer("rows", new int[] { 3 });
                query_write.SetDataBuffer("cols", new int[] { 3 });
                query_write.SetDataBuffer("a", new int[] { 3 });
                query_write.SubmitAndFinalize();
            }
            array_write.Close();
        }
    }

    private static void ReadArray()
    {
        ulong[] count = [0];
        long[] sum = [0];

        using (var array_read = new Array(Ctx, ArrayPath))
        {
            array_read.Open(QueryType.Read);
            using var query_read = new Query(array_read);
            query_read.SetLayout(LayoutType.Unordered);
            using var channel = query_read.GetDefaultChannel();
            channel.ApplyAggregate(AggregateOperation.Count, "Count");
            channel.ApplyAggregate(AggregateOperation.Unary(AggregateOperator.Sum, "a"), "Sum");
            query_read.SetDataBuffer("Count", count);
            query_read.SetDataBuffer("Sum", sum);
            query_read.Submit();
            array_read.Close();
        }

        Console.WriteLine($"Count: {count[0]}");
        Console.WriteLine($"Sum: {sum[0]}");
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
