using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TileDB.CSharp.Test;

[TestClass]
public class IncompleteQueryTest
{
    private static readonly Context Ctx = Context.GetDefault();

    private static void CreateArray(string path)
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

        Array.Create(Ctx, path, array_schema);
    }

    private static void WriteArray(string path, Entry[] data)
    {
        var dim1_data_buffer = data.Select(x => x.Row).ToArray();
        var dim2_data_buffer = data.Select(x => x.Column).ToArray();
        var attr_data_buffer = data.SelectMany(x => x.Data).ToArray();
        var attr_offsets_buffer = new ulong[data.Length];
        for (int i = 1; i < data.Length; i++)
        {
            attr_offsets_buffer[i] = attr_offsets_buffer[i - 1] + (ulong)data[i - 1].Data.Length * sizeof(int);
        }

        using (var array_write = new Array(Ctx, path))
        {
            array_write.Open(QueryType.Write);
            var query_write = new Query(array_write);
            query_write.SetLayout(LayoutType.Unordered);
            query_write.SetDataBuffer("rows", dim1_data_buffer);
            query_write.SetDataBuffer("cols", dim2_data_buffer);
            query_write.SetDataBuffer("a", attr_data_buffer);
            query_write.SetOffsetsBuffer("a", attr_offsets_buffer);
            query_write.Submit();
            array_write.Close();
        }
    }

    private static List<Entry> ReadArray(string path)
    {
        var result = new List<Entry>();

        var dim1_data_buffer_read = new int[1];
        var dim2_data_buffer_read = new int[1];
        var attr_data_buffer_read = new int[1];
        var attr_offsets_buffer = new ulong[1];

        using (var array_read = new Array(Ctx, path))
        {
            array_read.Open(QueryType.Read);
            var query_read = new Query(array_read);
            query_read.SetLayout(LayoutType.Unordered);
            query_read.SetDataBuffer("rows", dim1_data_buffer_read);
            query_read.SetDataBuffer("cols", dim2_data_buffer_read);
            query_read.SetDataBuffer("a", attr_data_buffer_read);
            query_read.SetOffsetsBuffer("a", attr_offsets_buffer);

            QueryStatus status;
            do
            {
                query_read.Submit();

                status = query_read.Status();

                var resultNum = (int)query_read.GetResultDataElements("rows");
                var attrNum = (int)query_read.GetResultDataElements("a");

                if (status == QueryStatus.Incomplete && resultNum == 0 && query_read.GetStatusDetails().Reason == QueryStatusDetailsReason.UserBufferSize)
                {
                    dim1_data_buffer_read = new int[dim1_data_buffer_read.Length * 2];
                    query_read.SetDataBuffer("rows", dim1_data_buffer_read);

                    dim2_data_buffer_read = new int[dim2_data_buffer_read.Length * 2];
                    query_read.SetDataBuffer("cols", dim2_data_buffer_read);

                    attr_data_buffer_read = new int[attr_data_buffer_read.Length * 2];
                    query_read.SetDataBuffer("a", attr_data_buffer_read);

                    attr_offsets_buffer = new ulong[attr_offsets_buffer.Length * 2];
                    query_read.SetOffsetsBuffer("a", attr_offsets_buffer);
                }
                else
                {
                    for (int i = 0; i < resultNum; i++)
                    {
                        var attrOffsetStart = (int)attr_offsets_buffer[i] / sizeof(int);
                        var attrOffsetEnd = i == resultNum - 1 ? attrNum : (int)attr_offsets_buffer[i + 1] / sizeof(int);
                        var attrLength = attrOffsetEnd - attrOffsetStart;
                        var attrData = attr_data_buffer_read.AsSpan(attrOffsetStart, attrLength).ToArray();
                        result.Add(new(Row: dim1_data_buffer_read[i], Column: dim2_data_buffer_read[i], Data: attrData));
                    }
                }
            }
            while (status == QueryStatus.Incomplete);

            array_read.Close();
        }

        return result;
    }

    [TestMethod]
    public void TestVariableSizedAttributes()
    {
        using var path = new TemporaryDirectory("incomplete-query-variable");

        Entry[] entries =
        [
            new(1, 1, [1]),
            new(2, 3, [2, 20]),
            new(3, 4, [3, 30, 300]),
            new(4, 5, [4, 40, 400, 4000])
        ];

        CreateArray(path);
        WriteArray(path, entries);
        List<Entry> readEntries = ReadArray(path);

        CollectionAssert.AreEqual(entries, readEntries);
    }

    private record struct Entry(int Row, int Column, int[] Data) : IEquatable<Entry>
    {
        public bool Equals(Entry other) =>
            Row == other.Row && Column == other.Column && Data.AsSpan().SequenceEqual(other.Data);
    }
}
