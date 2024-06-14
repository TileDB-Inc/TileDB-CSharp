using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class ArrayTest
{
    [TestMethod]
    public void TestDenseArray()
    {
        var context = Context.GetDefault();

        var tmpArrayPath = TestUtil.MakeTestPath("dense_array_test");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        var array_schema = ArrayTest.BuildDenseArraySchema(context);
        Assert.IsNotNull(array_schema);

        array.Create(array_schema);

        Assert.AreEqual(("file://" + tmpArrayPath).Replace('\\', '/').Replace("///","//"), array.Uri().Replace("///","//"));

        array.Open(QueryType.Read);

        array.Reopen();

        array.Close();

        var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        array.SetOpenTimestampStart((ulong)(unixTimestamp / 1000000));

        array.Open(QueryType.Read);

        array_schema = array.Schema();
        Assert.IsNotNull(array_schema);

        Assert.AreEqual(LayoutType.RowMajor, array_schema.TileOrder());

        Assert.AreEqual(QueryType.Read, array.QueryType());

        (_, _, bool isEmpty) = array.NonEmptyDomain<short>("dim1");
        Assert.IsTrue(isEmpty);

        (_, _, isEmpty) = array.NonEmptyDomain<short>(0);
        Assert.IsTrue(isEmpty);

        array.Close();

        var array_schema_loaded = Array.LoadArraySchema(context, tmpArrayPath);
        Assert.IsNotNull(array_schema_loaded);
    }

    [TestMethod]
    public void TestSparseArray()
    {
        var context = Context.GetDefault();

        var tmpArrayPath = TestUtil.MakeTestPath("sparse_array_test");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        var array_schema = ArrayTest.BuildSparseArraySchema(context);
        Assert.IsNotNull(array_schema);

        array.Create(array_schema);

        Assert.AreEqual(("file://" + tmpArrayPath).Replace('\\','/').Replace("///","//"), array.Uri().Replace("///","//"));

        array.Open(QueryType.Read);

        array.Reopen();

        array.Close();

        var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        array.SetOpenTimestampStart((ulong)(unixTimestamp / 1000000));

        array.Open(QueryType.Read);

        array_schema = array.Schema();
        Assert.IsNotNull(array_schema);

        Assert.AreEqual(LayoutType.RowMajor, array_schema.TileOrder());

        Assert.AreEqual(QueryType.Read, array.QueryType());

        (_, _, bool isEmpty) = array.NonEmptyDomain<short>("dim1");
        Assert.IsTrue(isEmpty);

        (_, _, isEmpty) = array.NonEmptyDomain<short>(0);
        Assert.IsTrue(isEmpty);

        Assert.ThrowsException<ArgumentException>(()=>array.NonEmptyDomain<short>("dim2"));
        Assert.ThrowsException<ArgumentException>(()=>array.NonEmptyDomain<short>(1));

        (_, _, isEmpty) = array.NonEmptyDomain<float>("dim2");
        Assert.IsTrue(isEmpty);

        (_, _, isEmpty) = array.NonEmptyDomain<float>(1);
        Assert.IsTrue(isEmpty);

        (_, isEmpty) = array.NonEmptyDomain();
        Assert.IsTrue(isEmpty);

        array.Close();

        var array_schema_loaded = Array.LoadArraySchema(context, tmpArrayPath);
        Assert.IsNotNull(array_schema_loaded);
    }

    [TestMethod]
    public void TestConsolidateFragments()
    {
        const uint FragmentCount = 10;

        var context = Context.GetDefault();

        using var uri = new TemporaryDirectory("array_consolidate_fragments");

        using (var schema = ArrayTest.BuildDenseArraySchema(context))
        using (var array = new Array(context, uri))
        {
            array.Create(schema);
        }

        int[] a1Data = Enumerable.Range(1, 10).Select(x => x * x).ToArray();
        byte[] a2Data = "aabbcccdeffggghhhhijj"u8.ToArray();
        ulong[] a2Offsets = [0, 2, 4, 7, 8, 9, 11, 14, 17, 18];
        for (uint i = 0; i < FragmentCount; i++)
        {
            using var array = new Array(context, uri);
            array.Open(QueryType.Write);

            using var query = new Query(array, QueryType.Write);
            query.SetDataBuffer("a1", a1Data);
            query.SetDataBuffer("a2", a2Data);
            query.SetOffsetsBuffer("a2", a2Offsets);

            query.Submit();
        }

        using var fragmentInfo = new FragmentInfo(context, uri);
        fragmentInfo.Load();

        Assert.AreEqual(FragmentCount, fragmentInfo.FragmentCount);

        string[] fragments = Enumerable.Range(0, (int)FragmentCount).Select(x => fragmentInfo.GetFragmentName((uint)x)).ToArray();

        Array.ConsolidateFragments(context, uri, fragments);

        fragmentInfo.Load();

        Assert.AreEqual(1u, fragmentInfo.FragmentCount);
        Assert.AreEqual(FragmentCount, fragmentInfo.FragmentToVacuumCount);
    }

    [TestMethod]
    public void TestDelete()
    {
        var context = Context.GetDefault();

        using var uri = new TemporaryDirectory("array_delete");

        using (var schema = BuildDenseArraySchema(context))
        {
            Array.Create(context, uri, schema);
        }

        Assert.AreEqual(ObjectType.Array, context.GetObjectType(uri));

        Array.Delete(context, uri);

        Assert.AreEqual(ObjectType.Invalid, context.GetObjectType(uri));
    }

    /// <summary>
    /// Base-64 encoded zip file of an array with an old format version.
    /// </summary>
    /// <remarks>
    /// The array was taken from <see href="https://github.com/TileDB-Inc/TileDB-Py/blob/3ea00dbc2eabce972bea8af1ea8a4dc420e00a72/tiledb/tests/test_libtiledb.py#L311-L341"/>
    /// and repackaged to a zip file, because <c>System.Formats.Tar</c> is available only in .NET 7+.
    /// </remarks>
    const string OldVersionArrayZip = """
        UEsDBBQAAAAAAAQHtFIAAAAAAAAAAAAAAABDAAAAX18xNjIxNDYxMzY3OTcyXzE2MjE0NjEzNjc5
        NzJfMWJlOWNlMDM3NDI1NGMyNGI2NjBlZDE3OGU3OWQ1ZTdfNS5va1BLAwQUAAAACAAEB7RSkZnF
        iBEAAAAsAAAARQAAAF9fMTYyMTQ2MTM2Nzk3Ml8xNjIxNDYxMzY3OTcyXzFiZTljZTAzNzQyNTRj
        MjRiNjYwZWQxNzhlNzlkNWU3XzUvLnRkYmNkgAAJKAYBRijNBKVZoTQAUEsDBBQAAAAIAAQHtFIu
        OY4+jQAAAI0EAABYAAAAX18xNjIxNDYxMzY3OTcyXzE2MjE0NjEzNjc5NzJfMWJlOWNlMDM3NDI1
        NGMyNGI2NjBlZDE3OGU3OWQ1ZTdfNS9fX2ZyYWdtZW50X21ldGFkYXRhLnRkYmNlYGCwYIAABSjN
        wghlMAiBSUYGkAAjK4gAs5DUiwCxAJTPiCRWwfg4KSEhoSVJgZXdJ0CMgXEJAz/IAH2oWpgeYu0C
        qedmQLULJlbBmJziwAqym2nUCphxMH2jVoxaAbECAZiQFIMAM5TWYUAFDgzEgRwofR5KG0ENngql
        f0DpaKjF+6C0ItTiFqgDAFBLAwQUAAAACAAEB7RSiHDSEyYAAABAAAAARgAAAF9fMTYyMTQ2MTM2
        Nzk3Ml8xNjIxNDYxMzY3OTcyXzFiZTljZTAzNzQyNTRjMjRiNjYwZWQxNzhlNzlkNWU3XzUvZC50
        ZGJjZIAACSCWAWIBKJ8RSUxjq/5fBYm5DAwRQHEmqAImBu1IHgZuAFBLAwQUAAAAAACkCbRSAAAA
        AAAAAAAAAAAAQwAAAF9fMTYyMTQ2MjM4Nzg5N18xNjIxNDYyMzg3ODk3Xzk5ZWQxNDgxMzc5ZTQ0
        ZTk4N2U0NTM1YWIyYWExYjBjXzUub2tQSwMEFAAAAAgApAm0UpGZxYgRAAAALAAAAEUAAABfXzE2
        MjE0NjIzODc4OTdfMTYyMTQ2MjM4Nzg5N185OWVkMTQ4MTM3OWU0NGU5ODdlNDUzNWFiMmFhMWIw
        Y181Ly50ZGJjZIAACSgGAUYozQSlWaE0AFBLAwQUAAAACACkCbRSLjmOPo0AAACNBAAAWAAAAF9f
        MTYyMTQ2MjM4Nzg5N18xNjIxNDYyMzg3ODk3Xzk5ZWQxNDgxMzc5ZTQ0ZTk4N2U0NTM1YWIyYWEx
        YjBjXzUvX19mcmFnbWVudF9tZXRhZGF0YS50ZGJjZWBgsGCAAAUozcIIZTAIgUlGBpAAIyuIALOQ
        1IsAsQCUz4gkVsH4OCkhIaElSYGV3SdAjIFxCQM/yAB9qFqYHmLtAqnnZkC1CyZWwZic4sAKsptp
        1AqYcTB9o1aMWgGxAgGYkBSDADOU1mFABQ4MxIEcKH0eShtBDZ4KpX9A6WioxfugtCLU4haoAwBQ
        SwMEFAAAAAgApAm0Uohw0hMmAAAAQAAAAEYAAABfXzE2MjE0NjIzODc4OTdfMTYyMTQ2MjM4Nzg5
        N185OWVkMTQ4MTM3OWU0NGU5ODdlNDUzNWFiMmFhMWIwY181L2QudGRiY2SAAAkglgFiASifEUlM
        Y6v+XwWJuQwMEUBxJqgCJgbtSB4GbgBQSwMEFAAAAAgAAwe0UsP6HbxgAAAAkAAAABIAAABfX2Fy
        cmF5X3NjaGVtYS50ZGJjZWBgiGGAgAYozcIIZTAIgUlGBpAAIyuIALOQ1FsAsQCUz4gkVsGYnMrA
        5MbGxlgqwMwi4RjDphYm8FGew8DV4ZPCnEOsSoo5CzgTzJoSmgwTeTqSFhSlxDIwsnNcAwBQSwME
        FAAAAAAAAwe0UgAAAAAAAAAAAAAAAAoAAABfX2xvY2sudGRiUEsBAhQAFAAAAAAABAe0UgAAAAAA
        AAAAAAAAAEMAAAAAAAAAAAAgAAAAAAAAAF9fMTYyMTQ2MTM2Nzk3Ml8xNjIxNDYxMzY3OTcyXzFi
        ZTljZTAzNzQyNTRjMjRiNjYwZWQxNzhlNzlkNWU3XzUub2tQSwECFAAUAAAACAAEB7RSkZnFiBEA
        AAAsAAAARQAAAAAAAAAAACAAAABhAAAAX18xNjIxNDYxMzY3OTcyXzE2MjE0NjEzNjc5NzJfMWJl
        OWNlMDM3NDI1NGMyNGI2NjBlZDE3OGU3OWQ1ZTdfNS8udGRiUEsBAhQAFAAAAAgABAe0Ui45jj6N
        AAAAjQQAAFgAAAAAAAAAAAAgAAAA1QAAAF9fMTYyMTQ2MTM2Nzk3Ml8xNjIxNDYxMzY3OTcyXzFi
        ZTljZTAzNzQyNTRjMjRiNjYwZWQxNzhlNzlkNWU3XzUvX19mcmFnbWVudF9tZXRhZGF0YS50ZGJQ
        SwECFAAUAAAACAAEB7RSiHDSEyYAAABAAAAARgAAAAAAAAAAACAAAADYAQAAX18xNjIxNDYxMzY3
        OTcyXzE2MjE0NjEzNjc5NzJfMWJlOWNlMDM3NDI1NGMyNGI2NjBlZDE3OGU3OWQ1ZTdfNS9kLnRk
        YlBLAQIUABQAAAAAAKQJtFIAAAAAAAAAAAAAAABDAAAAAAAAAAAAIAAAAGICAABfXzE2MjE0NjIz
        ODc4OTdfMTYyMTQ2MjM4Nzg5N185OWVkMTQ4MTM3OWU0NGU5ODdlNDUzNWFiMmFhMWIwY181Lm9r
        UEsBAhQAFAAAAAgApAm0UpGZxYgRAAAALAAAAEUAAAAAAAAAAAAgAAAAwwIAAF9fMTYyMTQ2MjM4
        Nzg5N18xNjIxNDYyMzg3ODk3Xzk5ZWQxNDgxMzc5ZTQ0ZTk4N2U0NTM1YWIyYWExYjBjXzUvLnRk
        YlBLAQIUABQAAAAIAKQJtFIuOY4+jQAAAI0EAABYAAAAAAAAAAAAIAAAADcDAABfXzE2MjE0NjIz
        ODc4OTdfMTYyMTQ2MjM4Nzg5N185OWVkMTQ4MTM3OWU0NGU5ODdlNDUzNWFiMmFhMWIwY181L19f
        ZnJhZ21lbnRfbWV0YWRhdGEudGRiUEsBAhQAFAAAAAgApAm0Uohw0hMmAAAAQAAAAEYAAAAAAAAA
        AAAgAAAAOgQAAF9fMTYyMTQ2MjM4Nzg5N18xNjIxNDYyMzg3ODk3Xzk5ZWQxNDgxMzc5ZTQ0ZTk4
        N2U0NTM1YWIyYWExYjBjXzUvZC50ZGJQSwECFAAUAAAACAADB7RSw/odvGAAAACQAAAAEgAAAAAA
        AAAAACAAAADEBAAAX19hcnJheV9zY2hlbWEudGRiUEsBAhQAFAAAAAAAAwe0UgAAAAAAAAAAAAAA
        AAoAAAAAAAAAAAAgAAAAVAUAAF9fbG9jay50ZGJQSwUGAAAAAAoACgA0BAAAfAUAAAAA
        """;

    [TestMethod]
    public void TestUpgradeVersion()
    {
        var context = Context.GetDefault();

        using var uri = new TemporaryDirectory("array_upgrade_version");

        TestUtil.UnzipBase64String(uri, OldVersionArrayZip);

        using (var schema = Array.LoadArraySchema(context, uri))
        {
            Assert.AreEqual(5u, schema.FormatVersion());
        }

        Array.UpgradeVersion(context, uri);

        using (var schema = Array.LoadArraySchema(context, uri))
        {
            Assert.IsTrue(schema.FormatVersion() >= 15u, "Array was not upgraded.");
        }
    }

    private static ArraySchema BuildDenseArraySchema(Context context)
    {
        var dimension = Dimension.Create<short>(context, "dim1", 1, 10, 5);
        Assert.IsNotNull(dimension);
        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimension(dimension);

        var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);

        var a1 = new Attribute(context, "a1", DataType.Int32);
        Assert.IsNotNull(a1);

        var a2 = new Attribute(context, "a2", DataType.StringAscii);
        Assert.IsNotNull(a2);

        a2.SetCellValNum(Attribute.VariableSized);

        array_schema.AddAttributes(a1, a2);

        array_schema.SetDomain(domain);

        array_schema.Check();

        return array_schema;
    }

    private static ArraySchema BuildSparseArraySchema(Context context)
    {
        var dim1 = Dimension.Create<short>(context, "dim1", 1, 10, 5);
        Assert.IsNotNull(dim1);

        var dim2 = Dimension.Create(context, "dim2", 1.0f, 4.0f, 0.5f);
        Assert.IsNotNull(dim2);

        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimensions(dim1, dim2);

        var array_schema = new ArraySchema(context, ArrayType.Sparse);
        Assert.IsNotNull(array_schema);

        var a1 = new Attribute(context, "a1", DataType.Int32);
        Assert.IsNotNull(a1);

        var a2 = new Attribute(context, "a2", DataType.StringAscii);
        Assert.IsNotNull(a2);

        a2.SetCellValNum(Attribute.VariableSized);

        array_schema.AddAttributes(a1, a2);

        array_schema.SetDomain(domain);

        array_schema.Check();

        return array_schema;
    }
}
