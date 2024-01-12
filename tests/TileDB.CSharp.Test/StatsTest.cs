using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class StatsTest
{
    private static readonly string ArrayUri = TestUtil.MakeTestPath("test-stats");

    [TestMethod]
    public void TestStats()
    {
        Stats.Enable();

        var ctx = Context.GetDefault();
        var row = Dimension.Create(ctx, "rows", 1, 4, 2);
        var col = Dimension.Create(ctx, "cols", 1, 4, 2);
        var domain = new Domain(ctx);
        domain.AddDimensions(row, col);
        var schema = new ArraySchema(ctx, ArrayType.Dense);
        schema.SetDomain(domain);
        schema.AddAttribute(new Attribute(ctx, "a1", DataType.Int32));
        schema.Check();
        TestUtil.CreateArray(ctx, ArrayUri, schema);

        var array = new Array(ctx, ArrayUri);
        array.Open(QueryType.Write);
        var query = new Query(array);
        query.SetLayout(LayoutType.RowMajor);
        var subarray = new Subarray(array);
        subarray.SetSubarray(1, 4, 1, 4);
        query.SetSubarray(subarray);
        query.SetDataBuffer("a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
        query.Submit();
        Assert.AreEqual(query.Status(), QueryStatus.Completed);

        string? stats = null;
        stats = Stats.Get();
        Assert.IsNotNull(stats);
        var filePath = TestUtil.MakeTestPath("test-stats-dump");
        Stats.Dump(filePath);
        Assert.IsTrue(System.IO.File.Exists(filePath));

        Stats.Dump();
    }
}
