using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TileDB.CSharp.Test;

[TestClass]
public class ContextTest
{
    [TestMethod]
    public void NewContextIsValid()
    {
        var config = new Config();

        // Set values
        config.Set("sm.memory_budget", "512000000");
        config.Set("vfs.s3.connect_timeout_ms", "5000");
        config.Set("vfs.s3.endpoint_override", "localhost:8888");
        using (var ctx = new Context(config))
        {
            // Get values
            Assert.AreEqual("512000000", ctx.Config().Get("sm.memory_budget"));
            Assert.AreEqual("5000", ctx.Config().Get("vfs.s3.connect_timeout_ms"));
            Assert.AreEqual("localhost:8888", ctx.Config().Get("vfs.s3.endpoint_override"));
        }
    }

    [TestMethod]
    public void ConfigReadWrite()
    {
        var config = new Config();

        // Set values
        config.Set("sm.memory_budget", "512000000");
        config.Set("vfs.s3.connect_timeout_ms", "5000");
        config.Set("vfs.s3.endpoint_override", "localhost:8888");

        // Get values
        Assert.AreEqual("512000000", config.Get("sm.memory_budget"));
        Assert.AreEqual("5000", config.Get("vfs.s3.connect_timeout_ms"));
        Assert.AreEqual("localhost:8888", config.Get("vfs.s3.endpoint_override"));

        var tempConfigPath = TestUtil.MakeTestPath("temp.cfg");
        config.SaveToFile(tempConfigPath);

        var config2 = new Config();
        config2.LoadFromFile(tempConfigPath);

        // Get values from config2
        Assert.AreEqual("512000000", config2.Get("sm.memory_budget"));
        Assert.AreEqual("5000", config2.Get("vfs.s3.connect_timeout_ms"));
        Assert.AreEqual("localhost:8888", config2.Get("vfs.s3.endpoint_override"));
    }

    [TestMethod]
    public void TestIsFileSystemSupported()
    {
        using var ctx = new Context();
        Assert.IsTrue(ctx.IsFileSystemSupported(FileSystemType.InMemory));
        // While the release binaries support all other filesystems (except for
        // HDFS), binaries from nightly builds and other custom builds may not.
        // MemFS is the only filesystem that is known to be always supported.
    }

    [TestMethod]
    public void TestObjectMethods()
    {
        using var arrayUri = new TemporaryDirectory("ctx-object-methods");
        var arrayUri2 = Path.Combine(arrayUri, "../ctx-object-methods2");
        TemporaryDirectory.DeleteDirectory(arrayUri2);

        using var ctx = new Context();
        Assert.AreEqual(ObjectType.Invalid, ctx.GetObjectType(arrayUri));

        CreateArray(ctx, arrayUri);
        Assert.AreEqual(ObjectType.Array, ctx.GetObjectType(arrayUri));

        ctx.MoveObject(arrayUri, arrayUri2);
        Assert.AreEqual(ObjectType.Invalid, ctx.GetObjectType(arrayUri));
        Assert.AreEqual(ObjectType.Array, ctx.GetObjectType(arrayUri2));

        ctx.RemoveObject(arrayUri2);
        Assert.AreEqual(ObjectType.Invalid, ctx.GetObjectType(arrayUri2));
    }

    [TestMethod]
    public void TestGetChildObjects()
    {
        using var path = new TemporaryDirectory("ctx-child-objects");
        var arrayUri = Path.Combine(path, "array1");
        var groupUri = Path.Combine(path, "group1");
        var arrayUri2 = Path.Combine(groupUri, "array2");

        using var ctx = new Context();
        CreateArray(ctx, arrayUri);
        Group.Create(ctx, groupUri);
        CreateArray(ctx, arrayUri2);
        
        Assert.AreEqual(2, ctx.GetChildObjects(path, null).Count);
        Assert.AreEqual(3, ctx.GetChildObjects(path, WalkOrderType.PreOrder).Count);
        Assert.AreEqual(3, ctx.GetChildObjects(path, WalkOrderType.PostOrder).Count);
    }

    private static void CreateArray(Context ctx, string arrayUri)
    {
        using Dimension d1 = Dimension.Create(ctx, nameof(d1), 1, 10, 5);
        using Dimension d2 = Dimension.Create(ctx, nameof(d2), 1, 10, 5);

        using Domain domain = new Domain(ctx);
        domain.AddDimension(d1);
        domain.AddDimension(d2);

        using Attribute a1 = new Attribute(ctx, nameof(a1), DataType.Int32);

        using ArraySchema schema = new ArraySchema(ctx, ArrayType.Sparse);
        schema.SetTileOrder(LayoutType.RowMajor);
        schema.SetCellOrder(LayoutType.RowMajor);
        schema.SetCapacity(2);
        schema.SetDomain(domain);
        schema.AddAttribute(a1);

        Array.Create(ctx, arrayUri, schema);
    }
}
