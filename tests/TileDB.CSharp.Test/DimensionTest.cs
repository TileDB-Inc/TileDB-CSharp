using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class DimensionTest
    {
        [TestMethod]
        public void TestDimension() {
            
            var context = Context.GetDefault();

            int bound_lower=1, bound_upper=10, extent = 5;
            var dimension = Dimension.Create(context, "test", bound_lower, bound_upper, extent);

            Assert.AreEqual("test", dimension.Name());
            Assert.AreEqual(DataType.TILEDB_INT32, dimension.Type());
            Assert.AreEqual(extent, dimension.TileExtent<int>());
            var dim_domain = dimension.Domain<int>();
            Assert.AreEqual(1, dim_domain[0]);
            Assert.AreEqual(10, dim_domain[1]);
        }

        [TestMethod]
        public void TestStringDimension()
        {
            
            var context = Context.GetDefault();

            var dimension = Dimension.Create<string>(context, "strdim", "", "", "");
            Assert.AreEqual<string>("strdim", dimension.Name());
            Assert.AreEqual(DataType.TILEDB_STRING_ASCII, dimension.Type());
        }

    }

}//namespace