using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class FragmentInfoTest
    {
        private const uint FragmentCount = 10;

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
        public void TestFragmentCount()
        {
            using var uri = new TemporaryDirectory("fragment_info_fragment_num");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            Assert.AreEqual(FragmentCount, info.FragmentCount);
        }

        [TestMethod]
        public void TestArraySchemaName()
        {
            using var uri = new TemporaryDirectory("fragment_info_array_schema_name");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Assert.IsNotNull(info.GetSchemaName(i));
            }

            Assert.AreEqual(FragmentCount, fragmentCount);
        }

        [TestMethod]
        public void TestGetFragmentSize()
        {
            using var uri = new TemporaryDirectory("fragment_info_fragment_size");

            CreateDenseArray(uri);

            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Uri fragmentUri = new(info.GetFragmentUri(i));

                var dirLength = (ulong)TestUtil.GetDirectorySize(fragmentUri.LocalPath);

                Assert.AreEqual(dirLength, info.GetFragmentSize(i));
            }
        }

        [TestMethod]
        public void TestIsDenseSparse()
        {
            using var uri = new TemporaryDirectory("fragment_info_is_dense_sparse");

            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Assert.IsTrue(info.IsDense(i));
                Assert.IsFalse(info.IsSparse(i));
            }
        }

        [TestMethod]
        public void TestGetTimestampRange()
        {
            using var uri = new TemporaryDirectory("fragment_info_timestamp_range");

            CreateDenseArray(uri);

            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Uri fragmentUri = new(info.GetFragmentUri(i));

                (ulong, ulong) expectedTimestampRange = info.GetTimestampRange(i);

                string[] dirNameSplit = Path.GetFileName(fragmentUri.LocalPath)!.Split('_');

                ulong start = ulong.Parse(dirNameSplit[2]);
                ulong end = ulong.Parse(dirNameSplit[3]);

                Assert.AreEqual(expectedTimestampRange, (start, end));
            }
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

        [TestMethod]
        public void TestNonEmptyDomain()
        {
            using var uri = new TemporaryDirectory("fragment_info_non_empty_domain");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                using Array arr = new Array(_ctx, uri);
                arr.Open(QueryType.TILEDB_READ);
                using ArraySchema schema = arr.Schema();
                using Domain domain = schema.Domain();
                uint ndim = domain.NDim();

                for (uint dim = 0; dim < ndim; dim++)
                {
                    using Dimension d = domain.Dimension(dim);

                    var expectedNonEmptyDomain = info.GetNonEmptyDomain<int>(i, dim);
                    var actualNonEmptyDomain = arr.NonEmptyDomain<int>(dim) switch
                    {
                        (int Start, int End, _) => (Start, End)
                    };
                    Assert.AreEqual(expectedNonEmptyDomain, actualNonEmptyDomain);

                    string name = d.Name();
                    expectedNonEmptyDomain = info.GetNonEmptyDomain<int>(i, name);
                    actualNonEmptyDomain = arr.NonEmptyDomain<int>(name) switch
                    {
                        (int Start, int End, _) => (Start, End)
                    };
                    Assert.AreEqual(expectedNonEmptyDomain, actualNonEmptyDomain);
                }
            }
        }

        [TestMethod]
        public void TestGetCellCount()
        {
            using var uri = new TemporaryDirectory("fragment_info_cell_num");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Assert.AreEqual(8u, info.GetCellsWritten(i));
            }
        }

        [TestMethod]
        public void TestGetFormatVersion()
        {
            using var uri = new TemporaryDirectory("fragment_info_version");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Uri fragmentUri = new(info.GetFragmentUri(i));

                ulong formatVersionFromPath = ulong.Parse(fragmentUri.LocalPath.Split('_')[^1]);

                Assert.AreEqual(formatVersionFromPath, info.GetFormatVersion(i));
            }
        }

        [TestMethod]
        public void TestHasConsolidatedMetadata()
        {
            using var uri = new TemporaryDirectory("fragment_info_has_consolidated_metadata");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            uint fragmentCount = info.FragmentCount;

            for (uint i = 0; i < fragmentCount; i++)
            {
                Assert.IsFalse(info.HasConsolidatedMetadata(i));
            }
        }

        [TestMethod]
        public void TestHasUnconsolidatedMetadata()
        {
            using var uri = new TemporaryDirectory("fragment_info_has_unconsolidated_metadata");
            CreateDenseArray(uri);
            for (uint i = 0; i < FragmentCount; i++)
            {
                WriteDenseArray(uri);
            }

            using FragmentInfo info = new FragmentInfo(_ctx, uri);
            info.Load();

            Assert.AreEqual(FragmentCount, info.FragmentWithUnconsolidatedMetadataCount);
        }

        private void CreateDenseArray(string arrayUri)
        {
            using Dimension rows = Dimension.Create(_ctx, nameof(rows), 1, 4, 2);
            using Dimension columns = Dimension.Create(_ctx, nameof(columns), 1, 4, 2);

            using Domain domain = new Domain(_ctx);
            domain.AddDimension(rows);
            domain.AddDimension(columns);

            using Attribute a = new Attribute(_ctx, nameof(a), DataType.TILEDB_INT32);

            using ArraySchema schema = new ArraySchema(_ctx, ArrayType.TILEDB_DENSE);
            schema.SetTileOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetCellOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetDomain(domain);
            schema.AddAttribute(a);

            Array.Create(_ctx, arrayUri, schema);
        }

        private void WriteDenseArray(string arrayUri)
        {
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] subarray = { 1, 2, 1, 4 };

            using Array array = new Array(_ctx, arrayUri);
            array.Open(QueryType.TILEDB_WRITE);
            using Query query = new Query(_ctx, array);
            query.SetDataBuffer("a", data);
            query.SetSubarray(subarray);

            query.Submit();
        }

        private void CreateSparseVarDimArray(string arrayUri)
        {
            using Dimension d1 = Dimension.CreateString(_ctx, nameof(d1));

            using Domain domain = new Domain(_ctx);
            domain.AddDimension(d1);

            using Attribute a1 = new Attribute(_ctx, nameof(a1), DataType.TILEDB_INT32);

            using ArraySchema schema = new ArraySchema(_ctx, ArrayType.TILEDB_SPARSE);
            schema.SetTileOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetCellOrder(LayoutType.TILEDB_ROW_MAJOR);
            schema.SetDomain(domain);
            schema.AddAttribute(a1);

            Array.Create(_ctx, arrayUri, schema);
        }

        private void WriteSparseVarDimArray(string arrayUri)
        {
            byte[] dData = Encoding.ASCII.GetBytes("abbccddee");
            ulong[] dOffsets = { 0, 2, 4, 6, 8 };

            int[] a1 = { 1, 2, 3, 4, 5 };

            using Array array = new Array(_ctx, arrayUri);
            array.Open(QueryType.TILEDB_WRITE);
            using Query query = new Query(_ctx, array);
            query.SetLayout(LayoutType.TILEDB_GLOBAL_ORDER);

            query.SetDataBuffer("d1", dData);
            query.SetOffsetsBuffer("d1", dOffsets);
            query.SetDataBuffer("a1", a1);

            query.Submit();
            query.FinalizeQuery();
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

            WriteImpl(new long[] { 1, 2 }, new long[] { 1, 2 }, new int[] { 1, 2 });
            WriteImpl(new long[] { 1, 2, 7, 8 }, new long[] { 1, 2, 7, 8 }, new int[] { 9, 10, 11, 12 });
            WriteImpl(new long[] { 1, 2, 7, 1 }, new long[] { 1, 2, 7, 8 }, new int[] { 5, 6, 7, 8 });

            void WriteImpl(long[] d1, long[] d2, int[] a1)
            {
                using Query query = new Query(_ctx, a);
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
            ulong[] dOffsets = { 0, 1, 3, 4 };

            int[] a = { 11, 12, 13, 14 };

            using Array array = new Array(_ctx, arrayUri);
            array.Open(QueryType.TILEDB_WRITE);
            using Query query = new Query(_ctx, array);
            query.SetLayout(LayoutType.TILEDB_UNORDERED);

            query.SetDataBuffer("d", dData);
            query.SetOffsetsBuffer("d", dOffsets);
            query.SetDataBuffer("a", a);

            query.Submit();

            query.FinalizeQuery();
        }
    }
}
