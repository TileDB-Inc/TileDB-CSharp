using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TileDB.CSharp.Examples;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S6562:Always set the \"DateTimeKind\" when creating new \"DateTime\" instances", Justification = "Excessive for this example; the dates' kind will not be converted")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S6588:Use the \"UnixEpoch\" field instead of creating \"DateTime\" instances that point to the beginning of the Unix epoch", Justification = "DateTime.UnixEpoch is unavailable in .NET 5")]
internal static class ExampleDataframe
{
    public static void Run()
    {
        using var ctx = new Context();
        const string arrayUri = "dataframe_array";
        if (Directory.Exists(arrayUri))
        {
            Directory.Delete(arrayUri, true);
        }
        CreateArray(ctx, arrayUri);

        string[] names = ["Adam", "Bree", "Charles", "Dianna", "Evan", "Fiona", "Gabe", "Hannah", "Isidore", "Julia"];
        DateTime[] dobs = [new DateTime(1990, 1, 1),
            new DateTime(1991, 2, 2),
            new DateTime(1992, 3, 3),
            new DateTime(1993, 4, 4),
            new DateTime(1994, 5, 5),
            new DateTime(1995, 6, 6),
            new DateTime(1996, 7, 7),
            new DateTime(1997, 8, 8),
            new DateTime(1998, 9, 9),
            new DateTime(1999, 10, 10)];
        Write(ctx, arrayUri, 0, names, dobs);

        foreach (var x in Read(ctx, arrayUri, 0, 9))
        {
            Console.WriteLine($"ID: {x.Id}, Name: {x.Name}, Date of birth: {x.DateOfBirth:d}");
        }
    }

    static void CreateArray(Context ctx, string uri)
    {
        using var schema = new ArraySchema(ctx, ArrayType.Dense);
        using var domain = new Domain(ctx);
        using var dimension = Dimension.Create<ulong>(ctx, "id", 0, ulong.MaxValue - 1, 1);
        domain.AddDimension(dimension);
        schema.SetDomain(domain);
        using var attrName = new Attribute(ctx, "name", DataType.StringUtf8);
        attrName.SetCellValNum(Attribute.VariableSized);
        using var attrDob = new Attribute(ctx, "dob", DataType.DateTimeDay);
        schema.AddAttributes(attrName);
        schema.AddAttributes(attrDob);
        Array.Create(ctx, uri, schema);
    }

    static void Write(Context ctx, string uri, ulong idStart, string[] names, DateTime[] dobs)
    {
        using var array = new Array(ctx, uri);
        array.Open(QueryType.Write);
        using var query = new Query(array);
        query.SetLayout(LayoutType.RowMajor);
        using var subarray = new Subarray(array);
        subarray.AddRange(0, idStart, idStart + (ulong)names.Length - 1);
        query.SetSubarray(subarray);
        var (nameData, nameOffsets) = CoreUtil.PackStringArray(names);
        query.SetDataBuffer("name", nameData);
        query.SetOffsetsBuffer("name", nameOffsets);
        query.SetDataBuffer("dob", dobs.Select(x => (long)(x - new DateTime(1970, 1, 1)).Days).ToArray());
        query.Submit();
    }

    static IEnumerable<(ulong Id, string Name, DateTime DateOfBirth)> Read(Context ctx, string uri, ulong idStart, ulong count)
    {
        using var array = new Array(ctx, uri);
        array.Open(QueryType.Read);
        (ulong minElement, ulong maxElement, bool isEmpty) = array.NonEmptyDomain<ulong>("id");
        if (isEmpty)
        {
            yield break;
        }
        idStart = Math.Max(idStart, minElement);
        ulong idEnd = Math.Min(idStart + count, maxElement);
        using var query = new Query(array);
        query.SetLayout(LayoutType.RowMajor);
        using var subarray = new Subarray(array);
        subarray.AddRange(0, idStart, idEnd);
        query.SetSubarray(subarray);

        byte[] nameData = new byte[512];
        ulong[] nameOffsets = new ulong[16];
        long[] dobs = new long[16];
        query.SetDataBuffer("name", nameData);
        query.SetOffsetsBuffer("name", nameOffsets);
        query.SetDataBuffer("dob", dobs);
        ulong currentId = idStart;
        // Loop until the query completes or fails.
        while (query.Status() is not (QueryStatus.Failed or QueryStatus.Completed))
        {
            query.Submit();
            var dataCount = (int)query.GetResultDataBytes("name");
            // The count of offsets for the variable-length attribute "name"
            // essentially tells us how many cells were returned.
            var offsetCount = (int)query.GetResultOffsets("name");
            // If zero cells were returned, check if the reason was that the
            // buffers were too small, resize them, and try again.
            if (offsetCount == 0 && query.GetStatusDetails().Reason == QueryStatusDetailsReason.UserBufferSize)
            {
                System.Array.Resize(ref nameData, nameData.Length * 2);
                System.Array.Resize(ref nameOffsets, nameOffsets.Length * 2);
                System.Array.Resize(ref dobs, dobs.Length * 2);
                query.SetDataBuffer("name", nameData);
                query.SetOffsetsBuffer("name", nameOffsets);
                query.SetDataBuffer("dob", dobs);
                continue;
            }
            for (int i = 0; i < offsetCount; i++)
            {
                var offset = (int)nameOffsets[i];
                var offsetOfNext = i < offsetCount - 1 ? (int)nameOffsets[i + 1] : dataCount;
                var name = Encoding.ASCII.GetString(nameData.AsSpan(offset, offsetOfNext - offset));
                var dob = new DateTime(1970, 1, 1).AddDays(dobs[i]);
                yield return (currentId++, name, dob);
            }
        }
    }
}
