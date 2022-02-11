using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class DimensionTest
    {
        [TestMethod]
        public void TestDimension() {
            
            var context = TileDB.Context.GetDefault();

            int bound_lower=1, bound_upper=10, extent = 5;
            var dimension = Dimension.create(context, "test", bound_lower, bound_upper, extent);

            Assert.AreEqual("test", dimension.name());
            Assert.AreEqual(DataType.TILEDB_INT32, dimension.type());
            Assert.AreEqual(extent, dimension.tile_extent<int>());
            var dim_domain = dimension.domain<int>();
            Assert.AreEqual<int>(1, dim_domain[0]);
            Assert.AreEqual<int>(10, dim_domain[1]);
        }

        [TestMethod]
        public void TestStringDimension()
        {
            
            var context = TileDB.Context.GetDefault();

            var dimension = Dimension.create<string>(context, "strdim", "", "", "");
            Assert.AreEqual<string>("strdim", dimension.name());
            Assert.AreEqual(DataType.TILEDB_STRING_ASCII, dimension.type());
        }

    }

}//namespace