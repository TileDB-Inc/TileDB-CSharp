using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class DomainTest
    {
        [TestMethod]
        public void TestDomain() {
            // Test context with config
            var context = TileDB.Context.GetDefault();

            // Test creating domain
            var domain = new Domain(context);

            // Test create dimension
            uint bound_lower=1, bound_upper=10, extent = 5;
            var dimension = Dimension.create(context, "testuint", bound_lower, bound_upper, extent);

            Assert.AreEqual(false, domain.has_dimension("testuint"));
            domain.add_dimension(dimension);
            Assert.AreEqual(true, domain.has_dimension("testuint"));
            Assert.AreEqual(DataType.TILEDB_UINT32, domain.type());
            Assert.AreEqual<uint>(1, domain.ndim());

            // Test create another dimension
            int bound_lower2=1, bound_upper2=10, extent2 = 5;
            var dimension2 = Dimension.create(context, "testint", bound_lower2, bound_upper2, extent2);
            domain.add_dimension(dimension2);
            Assert.AreEqual(true, domain.has_dimension("testint"));
            Assert.ThrowsException<System.Exception>(() => domain.type()); 
            //Assert.AreEqual(DataType.TILEDB_ANY, domain.type());
            Assert.AreEqual<uint>(2, domain.ndim());

            var dim1 = domain.dimension(0);
            Assert.AreEqual(DataType.TILEDB_UINT32, dim1.type());
            var dim2 = domain.dimension(1);
            Assert.AreEqual(DataType.TILEDB_INT32, dim2.type());

            dim1 = domain.dimension("testuint");
            Assert.AreEqual(DataType.TILEDB_UINT32, dim1.type());
            Assert.AreEqual<string>("testuint", dim1.name());

            dim2 = domain.dimension("testint");
            Assert.AreEqual(DataType.TILEDB_INT32, dim2.type());
            Assert.AreEqual<string>("testint", dim2.name());
        }
    }

}//namespace