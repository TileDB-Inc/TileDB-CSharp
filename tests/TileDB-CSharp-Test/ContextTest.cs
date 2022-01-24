using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB_CSharp;

namespace TileDB_CSharp_Test
{
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void NewContextIsValid()
        {
            using (var ctx = new Context())
            {
                Assert.AreNotEqual(null, ctx);
            }
        }
    }
}