using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class DimensionTest
{
    [TestMethod]
    public void TestDimension() {
        var context = Context.GetDefault();

        int bound_lower=1, bound_upper=10, extent = 5;
        var dimension = Dimension.Create(context, "test", bound_lower, bound_upper, extent);

        Assert.AreEqual("test", dimension.Name());
        Assert.AreEqual(DataType.Int32, dimension.Type());
        Assert.AreEqual(extent, dimension.TileExtent<int>());
        var dim_domain = dimension.GetDomain<int>();
        Assert.AreEqual((1, 10), dim_domain);
    }

    [TestMethod]
    public void TestStringDimension()
    {
        var context = Context.GetDefault();

        var dimension = Dimension.CreateString(context, "strdim");
        Assert.AreEqual("strdim", dimension.Name());
        Assert.AreEqual(DataType.StringAscii, dimension.Type());
    }
}
