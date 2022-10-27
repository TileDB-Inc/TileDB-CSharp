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
        public void TestMinimumBoundedRectanglesString()
        {
            using var uri = new TemporaryDirectory("fragment_info_mbr_var");
            CreateSparseVarDimArrayForMbrTesting(uri);
            WriteSparseVarDimArrayForMbrTesting(uri);

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            Assert.AreEqual(1u, info.FragmentCount);

            Assert.AreEqual(2ul, info.GetMinimumBoundedRectangleCount(0));

            Assert.AreEqual(("a", "bb"), info.GetMinimumBoundedRectangle<string>(0, 0, 0));
            Assert.AreEqual(("c", "ddd"), info.GetMinimumBoundedRectangle<string>(0, 1, "d"));
        }

        [TestMethod]
        public void TestMinimumBoundedRectangles()
        {
            using var uri = new TemporaryDirectory("fragment_info_mbr");
            CreateSparseArrayNoVarDim(uri);
            WriteSparseArrayNoVarDim3Frags(uri);

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            Assert.AreEqual(3u, info.FragmentCount);

            Assert.AreEqual(1ul, info.GetMinimumBoundedRectangleCount(0));
            Assert.AreEqual(2ul, info.GetMinimumBoundedRectangleCount(1));
            Assert.AreEqual(2ul, info.GetMinimumBoundedRectangleCount(2));

            Assert.AreEqual((1, 2), info.GetMinimumBoundedRectangle<int>(0, 0, 0));
            Assert.AreEqual((7, 8), info.GetMinimumBoundedRectangle<int>(1, 1, "d1"));
        }

        private void CreateSparseArrayNoVarDim(string arrayUri)
        {
            using Dimension d1 = Dimension.Create<long>(_ctx, nameof(d1), 1, 10, 5);
            using Dimension d2 = Dimension.Create<long>(_ctx, nameof(d2), 1, 10, 5);

            using Domain domain = new Domain(_ctx);
            domain.AddDimension(d1);
            domain.AddDimension(d2);

            using Attribute a1 = new Attribute(_ctx, nameof(a1), DataType.TILEDB_INT32);

            using ArraySchema schema = new ArraySchema(_ctx, ArrayType.TILEDB_SPARSE);
            schema.SetTileOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetCellOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetCapacity(2);
            schema.SetDomain(domain);
            schema.AddAttribute(a1);

            schema.Check();
            Array.Create(_ctx, arrayUri, schema);
        }

        private void WriteSparseArrayNoVarDim3Frags(string arrayUri)
        {
            using Array a = new Array(_ctx, arrayUri);
            a.Open(QueryType.TILEDB_WRITE);

            WriteImpl(new long[] {1, 2}, new long[] {1, 2}, new int[] {1, 2});
            WriteImpl(new long[] {1, 2, 7, 8}, new long[] {1, 2, 7, 8}, new int[] {9, 10, 11, 12});
            WriteImpl(new long[] {1, 2, 7, 1}, new long[] {1, 2, 7, 8}, new int[] {5, 6, 7, 8});

            void WriteImpl(long[] d1, long[] d2, int[] a1)
            {
                using Query query = new Query(_ctx, a, QueryType.TILEDB_WRITE);
                query.SetLayout(LayoutType.TILEDB_UNORDERED);
                query.SetDataBuffer("d1", d1);
                query.SetDataBuffer("d2", d2);
                query.SetDataBuffer("a1", a1);

                query.Submit();
                query.FinalizeQuery();
            }
        }

        private void CreateSparseVarDimArrayForMbrTesting(string arrayUri)
        {
            using Dimension d = Dimension.CreateString(_ctx, nameof(d));
            using Domain domain = new Domain(_ctx);
            domain.AddDimension(d);

            using Attribute a = new Attribute(_ctx, nameof(a), DataType.TILEDB_INT32);
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
