using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class DomainTest
    {
        [TestMethod]
        public void TestDomain() {
            // Test context with config
            var context = Context.GetDefault();

            // Test creating domain
            var domain = new Domain(context);

            // Test create dimension
            uint bound_lower=1, bound_upper=10, extent = 5;
            var dimension = Dimension.Create(context, "testuint", bound_lower, bound_upper, extent);

            Assert.AreEqual(false, domain.HasDimension("testuint"));
            domain.AddDimension(dimension);
            Assert.AreEqual(true, domain.HasDimension("testuint"));
            Assert.AreEqual(DataType.TILEDB_UINT32, domain.Type());
            Assert.AreEqual<uint>(1, domain.NDim());

            // Test create another dimension
            int bound_lower2=1, bound_upper2=10, extent2 = 5;
            var dimension2 = Dimension.Create(context, "testint", bound_lower2, bound_upper2, extent2);
            domain.AddDimension(dimension2);
            Assert.AreEqual(true, domain.HasDimension("testint"));
            Assert.ThrowsException<Exception>(() => domain.Type()); 
            //Assert.AreEqual(DataType.TILEDB_ANY, domain.type());
            Assert.AreEqual<uint>(2, domain.NDim());

            var dim1 = domain.Dimension(0);
            Assert.AreEqual(DataType.TILEDB_UINT32, dim1.Type());
            var dim2 = domain.Dimension(1);
            Assert.AreEqual(DataType.TILEDB_INT32, dim2.Type());

            dim1 = domain.Dimension("testuint");
            Assert.AreEqual(DataType.TILEDB_UINT32, dim1.Type());
            Assert.AreEqual<string>("testuint", dim1.Name());

            dim2 = domain.Dimension("testint");
            Assert.AreEqual(DataType.TILEDB_INT32, dim2.Type());
            Assert.AreEqual<string>("testint", dim2.Name());
        }
    }

}//namespace