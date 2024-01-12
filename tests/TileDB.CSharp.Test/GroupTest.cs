using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class GroupTest
{
    [TestMethod]
    public void TestGroupMetadata()
    {
        var ctx = Context.GetDefault();
        using var temp_dir = new TemporaryDirectory("group_metadata");
        string group1_uri = Path.Combine(temp_dir, "group1");
        Group.Create(ctx, group1_uri);
        using var group = new Group(ctx, group1_uri);
        group.Open(QueryType.Write);
        group.Close();

        // Put metadata on a group that is not opened
        int v = 5;
        Assert.ThrowsException<TileDBException>(() => group.PutMetadata("key", v));

        // Write metadata on a group opened in READ mode
        group.Open(QueryType.Read);
        Assert.ThrowsException<TileDBException>(() => group.PutMetadata("key", v));

        // Close and reopen in WRITE mode
        group.Close();
        group.Open(QueryType.Write);

        group.PutMetadata("key", v);
        group.Close();

        group.Open(QueryType.Read);
        var data = group.GetMetadata<int>("key");

        Assert.AreEqual(5, data[0]);

        group.Close();
    }

    [TestMethod]
    public void TestGroupMember()
    {
        var ctx = Context.GetDefault();
        using var temp_dir = new TemporaryDirectory("group_member");

        string array1_uri = Path.Combine(temp_dir, "array1");
        CreateArray(array1_uri);
        string array2_uri = Path.Combine(temp_dir, "array2");
        CreateArray(array2_uri);

        string group1_uri = Path.Combine(temp_dir, "group1");
        Group.Create(ctx, group1_uri);

        string group2_uri = Path.Combine(temp_dir, "group2");
        Group.Create(ctx, group2_uri);

        using var group1 = new Group(ctx, group1_uri);
        group1.Open(QueryType.Write);

        group1.AddMember(array1_uri, false, "array1");
        group1.AddMember(array2_uri, false, "array2");
        group1.Close();

        using var group2 = new Group(ctx, group2_uri);
        group2.Open(QueryType.Write);
        group2.AddMember(array2_uri, false, "array2");
        group2.Close();

        //Reopen in read mode
        group1.Open(QueryType.Read);
        Assert.AreEqual<ulong>(2, group1.MemberCount());
        group1.Close();

        //Reopen in write mode
        group1.Open(QueryType.Write);
        group1.RemoveMember("array1");
        group1.Close();

        //Reopen in read mode
        group1.Open(QueryType.Read);
        Assert.AreEqual<ulong>(1, group1.MemberCount());
        group1.Close();
    }

    [TestMethod]
    public void TestIsUriRelative()
    {
        var ctx = Context.GetDefault();
        using var uri = new TemporaryDirectory("group_is_uri_relative");
        string group1Uri = Path.Combine(uri, "group1");
        Group.Create(ctx, group1Uri);

        var array1Uri = Path.Combine(uri, "array1");
        CreateArray(array1Uri);
        var array2Uri = Path.Combine(group1Uri, "array2");
        CreateArray(array2Uri);

        using (var group = new Group(ctx, group1Uri))
        {
            group.Open(QueryType.Write);

            group.AddMember(array1Uri, false, "array1");
            group.AddMember("array2", true, "array2");
            group.Close();
        }

        using (var group = new Group(ctx, group1Uri))
        {
            group.Open(QueryType.Read);
            Assert.IsFalse(group.IsUriRelative("array1"));
            Assert.IsTrue(group.IsUriRelative("array2"));
        }
    }

    private static void CreateArray(string uri)
    {
        string tmpArrayPath = uri;
        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        var context = Context.GetDefault();
        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        var dim_1 = Dimension.Create(context, "d1", 1, 1, 1);
        Assert.IsNotNull(dim_1);
        domain.AddDimension(dim_1);

        var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);

        var attr1 = new Attribute(context, "a1", DataType.Float32);
        Assert.IsNotNull(attr1);
        array_schema.AddAttribute(attr1);

        array_schema.SetDomain(domain);
        array_schema.SetCellOrder(LayoutType.RowMajor);
        array_schema.SetTileOrder(LayoutType.RowMajor);

        array_schema.Check();

        Array.Create(context, tmpArrayPath, array_schema);
    }
}
