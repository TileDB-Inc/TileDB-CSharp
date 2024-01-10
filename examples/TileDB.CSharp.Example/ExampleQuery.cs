using System;
using System.IO;
using TileDB.CSharp;
namespace TileDB.CSharp.Examples;

static class ExampleQuery
{
    private static readonly string ArrayPath = ExampleUtil.MakeExamplePath("sparse-array");
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
        var dim1_data_buffer = new int[3] { 1, 2, 3 };
        var dim2_data_buffer = new int[3] { 1, 3, 4 };
        var attr_data_buffer = new int[3] { 1, 2, 3 };

        using (var array_write = new Array(Ctx, ArrayPath))
        {
            array_write.Open(QueryType.Write);
            var query_write = new Query(array_write);
            query_write.SetLayout(LayoutType.Unordered);
            query_write.SetDataBuffer("rows", dim1_data_buffer);
            query_write.SetDataBuffer("cols", dim2_data_buffer);
            query_write.SetDataBuffer("a", attr_data_buffer);
            query_write.Submit();
            array_write.Close();
        }
    }

    private static void ReadArray()
    {
        var dim1_data_buffer_read = new int[3];
        var dim2_data_buffer_read = new int[3];
        var attr_data_buffer_read = new int[3];

        using (var array_read = new Array(Ctx, ArrayPath))
        {
            array_read.Open(QueryType.Read);
            var query_read = new Query(array_read);
            query_read.SetLayout(LayoutType.RowMajor);
            query_read.SetDataBuffer("rows", dim1_data_buffer_read);
            query_read.SetDataBuffer("cols", dim2_data_buffer_read);
            query_read.SetDataBuffer("a", attr_data_buffer_read);
            query_read.Submit();
            array_read.Close();
        }

        Console.WriteLine("dim1:{0},{1},{2}", dim1_data_buffer_read[0], dim1_data_buffer_read[1], dim1_data_buffer_read[2]);
        Console.WriteLine("dim2:{0},{1},{2}", dim2_data_buffer_read[0], dim2_data_buffer_read[1], dim2_data_buffer_read[2]);
        Console.WriteLine("attr:{0},{1},{2}", attr_data_buffer_read[0], attr_data_buffer_read[1], attr_data_buffer_read[2]);
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
