using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class FragmentInfoTest
    {
        private Context _ctx = null!;

        [TestInitialize]
        public void Setup()
        {
            _ctx = new Context();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _ctx.Dispose();
        }

        [TestMethod]
        public void TestMbrVarDim()
        {
            using var uri = new TemporaryDirectory("fragment_info_mbr");
            CreateSparseVarDimArrayForMbrTesting(uri);
            WriteSparseVarDimArrayForMbrTesting(uri);

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            Assert.AreEqual(1u, info.FragmentCount);

            Assert.AreEqual(2ul, info.GetMinimumBoundedRectangleCount(0));

            Assert.AreEqual(("a", "bb"), info.GetMinimumBoundedRectangle<string>(0, 0, 0));
            Assert.AreEqual(("c", "ddd"), info.GetMinimumBoundedRectangle<string>(0, 1, "d"));
        }

        private void CreateSparseVarDimArrayForMbrTesting(string arrayUri)
        {
            using Dimension d = Dimension.CreateString(_ctx, "d");
            using Domain domain = new Domain(_ctx);
            domain.AddDimension(d);

            using Attribute a = new Attribute(_ctx, "a", DataType.TILEDB_INT32);
            using ArraySchema schema = new ArraySchema(_ctx, ArrayType.TILEDB_SPARSE);
            schema.SetTileOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetCellOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetDomain(domain);
            schema.SetCapacity(2);
            schema.AddAttribute(a);

            Array.Create(_ctx, arrayUri, schema);
        }

        private void WriteSparseVarDimArrayForMbrTesting(string arrayUri)
        {
            byte[] dData = Encoding.ASCII.GetBytes("abbcddd");
            ulong[] dOffsets = {0, 1, 3, 4};

            int[] a = {11, 12, 13, 14};

            using Array array = new Array(_ctx, arrayUri);
            array.Open(QueryType.TILEDB_WRITE);
            using Query query = new Query(_ctx, array, QueryType.TILEDB_WRITE);
            query.SetLayout(LayoutType.TILEDB_UNORDERED);

            query.SetDataBuffer("d", dData);
            query.SetOffsetsBuffer("d", dOffsets);
            query.SetDataBuffer("a", a);

            query.Submit();

            query.FinalizeQuery();
        }
    }
}
