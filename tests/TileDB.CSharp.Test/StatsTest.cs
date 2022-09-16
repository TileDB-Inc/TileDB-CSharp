using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class StatsTest
    {
        private static readonly string ArrayUri = CoreUtil.MakeTestPath("test-stats");

        [TestMethod]
        public void TestStats()
        {
            Stats.Enable();

            var ctx = Context.GetDefault();
            var row = Dimension.Create(ctx, "rows", 1, 4, 2);
            var col = Dimension.Create(ctx, "cols", 1, 4, 2);
            var domain = new Domain(ctx);
            domain.AddDimensions(row, col);
            var schema = new ArraySchema(ctx, ArrayType.TILEDB_DENSE);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.Check();
            TestUtil.CreateArray(ctx, ArrayUri, schema);

            var array = new Array(ctx, ArrayUri);
            array.Open(QueryType.TILEDB_WRITE);
            var query = new Query(ctx, array);
            query.SetLayout(LayoutType.TILEDB_ROW_MAJOR);
            query.SetSubarray(new[] { 1, 4, 1, 4 });
            query.SetDataBuffer("a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            query.Submit();
            Assert.AreEqual(query.Status(), QueryStatus.TILEDB_COMPLETED);

            string? stats = null;
            stats = Stats.Get();
            Assert.IsNotNull(stats);
            var filePath = CoreUtil.MakeTestPath("test-stats-dump");
            Stats.Dump(filePath);
            Assert.IsTrue(System.IO.File.Exists(filePath));

            Stats.Dump();
        }

    }
}