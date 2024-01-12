using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class ArrayMetadataTest
{
    [TestMethod]
    public void TestArrayMetadata()
    {
        var tmpArrayPath = TestUtil.MakeTestPath("metadata_array");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }
        createArrayMetadataArray(tmpArrayPath);
        writeArrayMetadata(tmpArrayPath);
        readArrayMetadata(tmpArrayPath);
        clearArrayMetadata(tmpArrayPath);
    }

    private void createArrayMetadataArray(string tmpArrayPath)
    {
        var context = Context.GetDefault();

        var rowDim = Dimension.Create(context, "rows", 1, 4, 4);
        Assert.IsNotNull(rowDim);

        var colDim = Dimension.Create(context, "cols", 1, 4, 4);
        Assert.IsNotNull(rowDim);

        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimensions(rowDim, colDim);

        var array_schema = new ArraySchema(context, ArrayType.Sparse);
        Assert.IsNotNull(array_schema);
        var a1 = new Attribute(context, "a1", DataType.UInt32);
        Assert.IsNotNull(a1);

        array_schema.SetCellOrder(LayoutType.RowMajor);
        array_schema.SetTileOrder(LayoutType.RowMajor);

        array_schema.AddAttribute(a1);

        array_schema.SetDomain(domain);

        array_schema.Check();

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        array.Create(array_schema);
    }

    private void writeArrayMetadata(string tmpArrayPath)
    {
        var context = Context.GetDefault();

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        array.Open(QueryType.Write);

        array.PutMetadata<int>("key1", [25]);

        array.PutMetadata<int>("key2", [25, 26, 27, 28]);

        array.PutMetadata<float>("key3", [25.1f]);

        array.PutMetadata<float>("key4", [25.1f, 26.2f, 27.3f, 28.4f]);

        array.PutMetadata("key5", "This is TileDB array metadata, that supports Unicode characters! 🥳");

        array.Close();
    }

    private void readArrayMetadata(string tmpArrayPath)
    {
        var context = Context.GetDefault();

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        array.Open(QueryType.Read);

        var v1 = array.GetMetadata<int>("key1");
        CollectionAssert.AreEqual(new int[]{ 25 }, v1);

        var v2 = array.GetMetadata<int>("key2");
        CollectionAssert.AreEqual(new int[]{ 25, 26, 27, 28 }, v2);

        var v3 = array.GetMetadata<float>("key3");
        CollectionAssert.AreEqual(new float[]{ 25.1f }, v3);

        var v4 = array.GetMetadata<float>("key4");
        CollectionAssert.AreEqual(new float[]{ 25.1f, 26.2f, 27.3f, 28.4f }, v4);

        var s = array.GetMetadata("key5");
        Assert.AreEqual("This is TileDB array metadata, that supports Unicode characters! 🥳", s);

        var num = array.MetadataNum();
        Assert.AreEqual((ulong)5,  num);

        var keys = array.MetadataKeys();
        CollectionAssert.AreEqual(new string[]{"key1", "key2", "key3", "key4", "key5"}, keys);

        var arrayMetadata = array.GetMetadataFromIndex<float>(3);
        Assert.AreEqual("key4",  arrayMetadata.key);
        Assert.AreEqual(4,  arrayMetadata.key.Length);

        Assert.AreEqual(4,  arrayMetadata.data.Length);

        array.Close();
    }

    private void clearArrayMetadata(string tmpArrayPath)
    {
        var context = Context.GetDefault();

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        array.Open(QueryType.Write);

        array.DeleteMetadata("key1");
        array.DeleteMetadata("key2");
        array.DeleteMetadata("key3");
        array.DeleteMetadata("key4");
        array.DeleteMetadata("key5");
        array.Close();
    }
}
