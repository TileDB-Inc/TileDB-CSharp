using System;
using System.IO;
using System.Linq;

namespace TileDB.CSharp.Examples;

public static class ExampleIncompleteQueryVariableSize
{
    static readonly string ArrayPath = ExampleUtil.MakeExamplePath("sparse-incomplete-query-variable-size");
    private static readonly Context Ctx = Context.GetDefault();

    private static void CreateArray()
    {
        var dim1 = Dimension.Create(Ctx, "rows", boundLower: 1, boundUpper: 10, extent: 4);
        var dim2 = Dimension.Create(Ctx, "cols", boundLower: 1, boundUpper: 10, extent: 4);
        var domain = new Domain(Ctx);
        domain.AddDimension(dim1);
        domain.AddDimension(dim2);
        var array_schema = new ArraySchema(Ctx, ArrayType.Sparse);
        var attr = new Attribute(Ctx, "a", DataType.Int32);
        attr.SetCellValNum(Attribute.VariableSized);

        array_schema.AddAttribute(attr);
        array_schema.SetDomain(domain);
        array_schema.Check();

        Array.Create(Ctx, ArrayPath, array_schema);
    }

    private static void WriteArray()
    {
        var dim1_data_buffer = new int[] { 1, 2, 3, 4 };
        var dim2_data_buffer = new int[] { 1, 3, 4, 5 };
        var attr_data_buffer = new int[] { 1, 2, 20, 3, 30, 300, 4, 40, 400, 4000 };
        var attr_data_offsets_buffer = new ulong[] { 0, 1 * sizeof(int), 3 * sizeof(int), 6 * sizeof(int) };

        using (var array_write = new Array(Ctx, ArrayPath))
        {
            array_write.Open(QueryType.Write);
            var query_write = new Query(array_write);
            query_write.SetLayout(LayoutType.Unordered);
            query_write.SetDataBuffer("rows", dim1_data_buffer);
            query_write.SetDataBuffer("cols", dim2_data_buffer);
            query_write.SetDataBuffer("a", attr_data_buffer);
            query_write.SetOffsetsBuffer("a", attr_data_offsets_buffer);
            query_write.Submit();
            array_write.Close();
        }
    }

    private static void ReadArray()
    {
        var dim1_data_buffer_read = new int[1];
        var dim2_data_buffer_read = new int[1];
        var attr_data_buffer_read = new int[1];
        var attr_data_offsets_buffer = new ulong[1];

        using (var array_read = new Array(Ctx, ArrayPath))
        {
            array_read.Open(QueryType.Read);
            var query_read = new Query(array_read);
            query_read.SetLayout(LayoutType.Unordered);
            query_read.SetDataBuffer("rows", dim1_data_buffer_read);
            query_read.SetDataBuffer("cols", dim2_data_buffer_read);
            query_read.SetDataBuffer("a", attr_data_buffer_read);
            query_read.SetOffsetsBuffer("a", attr_data_offsets_buffer);

            int batchNum = 1;
            QueryStatus status;
            do
            {
                query_read.Submit();

                status = query_read.Status();

                var resultNum = (int)query_read.GetResultDataElements("rows");
                var attrNum = (int)query_read.GetResultDataElements("a");

                Console.WriteLine($"Batch #{batchNum++}");

                if (status == QueryStatus.Incomplete && resultNum == 0 && query_read.GetStatusDetails().Reason == QueryStatusDetailsReason.UserBufferSize)
                {
                    Console.WriteLine("Resizing buffers...");
                    dim1_data_buffer_read = new int[dim1_data_buffer_read.Length * 2];
                    query_read.SetDataBuffer("rows", dim1_data_buffer_read);

                    dim2_data_buffer_read = new int[dim2_data_buffer_read.Length * 2];
                    query_read.SetDataBuffer("cols", dim2_data_buffer_read);

                    attr_data_buffer_read = new int[attr_data_buffer_read.Length * 2];
                    query_read.SetDataBuffer("a", attr_data_buffer_read);

                    attr_data_offsets_buffer = new ulong[attr_data_offsets_buffer.Length * 2];
                    query_read.SetOffsetsBuffer("a", attr_data_offsets_buffer);
                }
                else
                {
                    for (int i = 0; i < resultNum; i++)
                    {
                        var attrOffsetStart = (int)attr_data_offsets_buffer[i] / sizeof(int);
                        var attrOffsetEnd = i == resultNum - 1 ? attrNum : (int)attr_data_offsets_buffer[i + 1] / sizeof(int);
                        var attrLength = attrOffsetEnd - attrOffsetStart;
                        var attrData = string.Join(", ", attr_data_buffer_read.Skip(attrOffsetStart).Take(attrLength));
                        Console.WriteLine($"Cell ({dim1_data_buffer_read[i]}, {dim2_data_buffer_read[i]}) = [{attrData}]");
                    }
                }
            }
            while (status == QueryStatus.Incomplete);

            array_read.Close();
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
