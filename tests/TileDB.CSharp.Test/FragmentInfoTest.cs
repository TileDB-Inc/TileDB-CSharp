using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace TileDB.CSharp.Test;

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

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestFragmentCount(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_fragment_num");
        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
        }

        using FragmentInfo info = new FragmentInfo(_ctx, uri);
        info.Load();

        Assert.AreEqual(FragmentCount, info.FragmentCount);
    }

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestArraySchemaName(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_array_schema_name");
        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
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

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestGetFragmentSize(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_fragment_size");

        CreateArray(uri, isDense);

        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
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

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestIsDenseSparse(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_is_dense_sparse");

        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
        }

        using FragmentInfo info = new FragmentInfo(_ctx, uri);
        info.Load();

        uint fragmentCount = info.FragmentCount;

        for (uint i = 0; i < fragmentCount; i++)
        {
            Assert.AreEqual(isDense, info.IsDense(i));
            Assert.AreEqual(!isDense, info.IsSparse(i));
        }
    }

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestGetTimestampRange(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_timestamp_range");

        CreateArray(uri, isDense);

        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
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

        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<int>(0, 0, 0));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<long>(0, 0, 0));
        Assert.ThrowsException<NotSupportedException>(() => info.GetMinimumBoundedRectangle<Guid>(0, 0, 0));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<int>(0, 1, "d"));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<long>(0, 1, "d"));
        Assert.ThrowsException<NotSupportedException>(() => info.GetMinimumBoundedRectangle<Guid>(0, 1, "d"));
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

        Assert.AreEqual((1, 2), info.GetMinimumBoundedRectangle<long>(0, 0, 0));
        Assert.AreEqual((7, 8), info.GetMinimumBoundedRectangle<long>(1, 1, "d1"));

        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<int>(0, 0, 0));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<string>(0, 0, 0));
        Assert.ThrowsException<NotSupportedException>(() => info.GetMinimumBoundedRectangle<Guid>(0, 0, 0));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<int>(1, 1, "d1"));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<string>(1, 1, "d1"));
        Assert.ThrowsException<NotSupportedException>(() => info.GetMinimumBoundedRectangle<Guid>(1, 1, "d1"));
    }

    [TestMethod]
    public void TestMinimumBoundedRectanglesInt32()
    {
        using var uri = new TemporaryDirectory("fragment_info_mbr_int32");
        CreateSparseArrayNoVarDimInt32(uri);
        WriteSparseArrayNoVarDim3FragsInt32(uri);

        using FragmentInfo info = new FragmentInfo(_ctx, uri);
        info.Load();

        Assert.AreEqual(3u, info.FragmentCount);

        Assert.AreEqual(1ul, info.GetMinimumBoundedRectangleCount(0));
        Assert.AreEqual(2ul, info.GetMinimumBoundedRectangleCount(1));
        Assert.AreEqual(2ul, info.GetMinimumBoundedRectangleCount(2));

        Assert.AreEqual((1, 2), info.GetMinimumBoundedRectangle<int>(0, 0, 0));
        Assert.AreEqual((7, 8), info.GetMinimumBoundedRectangle<int>(1, 1, "d1"));

        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<long>(0, 0, 0));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<string>(0, 0, 0));
        Assert.ThrowsException<NotSupportedException>(() => info.GetMinimumBoundedRectangle<Guid>(0, 0, 0));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<long>(1, 1, "d1"));
        Assert.ThrowsException<ArgumentException>(() => info.GetMinimumBoundedRectangle<string>(1, 1, "d1"));
        Assert.ThrowsException<NotSupportedException>(() => info.GetMinimumBoundedRectangle<Guid>(1, 1, "d1"));
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
            arr.Open(QueryType.Read);
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
                Assert.ThrowsException<ArgumentException>(() => info.GetNonEmptyDomain<long>(i, dim));
                Assert.ThrowsException<ArgumentException>(() => info.GetNonEmptyDomain<string>(i, dim));
                Assert.ThrowsException<NotSupportedException>(() => info.GetNonEmptyDomain<Guid>(i, dim));

                string name = d.Name();
                expectedNonEmptyDomain = info.GetNonEmptyDomain<int>(i, name);
                actualNonEmptyDomain = arr.NonEmptyDomain<int>(name) switch
                {
                    (int Start, int End, _) => (Start, End)
                };
                Assert.AreEqual(expectedNonEmptyDomain, actualNonEmptyDomain);
                Assert.ThrowsException<ArgumentException>(() => info.GetNonEmptyDomain<long>(i, name));
                Assert.ThrowsException<ArgumentException>(() => info.GetNonEmptyDomain<string>(i, name));
                Assert.ThrowsException<NotSupportedException>(() => info.GetNonEmptyDomain<Guid>(i, name));
            }
        }
    }

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestGetCellCount(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_cell_num");
        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
        }

        using FragmentInfo info = new FragmentInfo(_ctx, uri);
        info.Load();

        uint fragmentCount = info.FragmentCount;

        for (uint i = 0; i < fragmentCount; i++)
        {
            Assert.AreEqual(isDense ? 8u : 5u, info.GetCellsWritten(i));
        }
    }

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestGetFormatVersion(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_version");
        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
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

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestHasConsolidatedMetadata(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_has_consolidated_metadata");
        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
        }

        using FragmentInfo info = new FragmentInfo(_ctx, uri);
        info.Load();

        uint fragmentCount = info.FragmentCount;

        for (uint i = 0; i < fragmentCount; i++)
        {
            Assert.IsFalse(info.HasConsolidatedMetadata(i));
        }
    }

    [DataTestMethod]
    [DataRow(true, DisplayName = "Dense")]
    [DataRow(false, DisplayName = "Sparse")]
    public void TestHasUnconsolidatedMetadata(bool isDense)
    {
        using var uri = new TemporaryDirectory("fragment_info_has_unconsolidated_metadata");
        CreateArray(uri, isDense);
        for (uint i = 0; i < FragmentCount; i++)
        {
            WriteArray(uri, isDense);
        }

        using FragmentInfo info = new FragmentInfo(_ctx, uri);
        info.Load();

        Assert.AreEqual(FragmentCount, info.FragmentWithUnconsolidatedMetadataCount);
    }

    private void CreateArray(string arrayUri, bool isDense)
    {
        if (isDense)
        {
            CreateDenseArray(arrayUri);
        }
        else
        {
            CreateSparseVarDimArray(arrayUri);
        }
    }

    private void WriteArray(string arrayUri, bool isDense)
    {
        if (isDense)
        {
            WriteDenseArray(arrayUri);
        }
        else
        {
            WriteSparseVarDimArray(arrayUri);
        }
    }

    private void CreateDenseArray(string arrayUri)
    {
        using Dimension rows = Dimension.Create(_ctx, nameof(rows), 1, 4, 2);
        using Dimension columns = Dimension.Create(_ctx, nameof(columns), 1, 4, 2);

        using Domain domain = new Domain(_ctx);
        domain.AddDimension(rows);
        domain.AddDimension(columns);

        using Attribute a = new Attribute(_ctx, nameof(a), DataType.Int32);

        using ArraySchema schema = new ArraySchema(_ctx, ArrayType.Dense);
        schema.SetTileOrder(LayoutType.RowMajor);
        schema.SetCellOrder(LayoutType.RowMajor);
        schema.SetDomain(domain);
        schema.AddAttribute(a);

        Array.Create(_ctx, arrayUri, schema);
    }

    private void WriteDenseArray(string arrayUri)
    {
        int[] data = [1, 2, 3, 4, 5, 6, 7, 8];

        using Array array = new Array(_ctx, arrayUri);
        array.Open(QueryType.Write);
        using Query query = new Query(array);
        query.SetDataBuffer("a", data);
        using Subarray subarray = new Subarray(array);
        subarray.SetSubarray(1, 2, 1, 4);
        query.SetSubarray(subarray);

        query.Submit();
    }

    private void CreateSparseVarDimArray(string arrayUri)
    {
        using Dimension d1 = Dimension.CreateString(_ctx, nameof(d1));

        using Domain domain = new Domain(_ctx);
        domain.AddDimension(d1);

        using Attribute a1 = new Attribute(_ctx, nameof(a1), DataType.Int32);

        using ArraySchema schema = new ArraySchema(_ctx, ArrayType.Sparse);
        schema.SetTileOrder(LayoutType.RowMajor);
        schema.SetCellOrder(LayoutType.RowMajor);
        schema.SetDomain(domain);
        schema.AddAttribute(a1);

        Array.Create(_ctx, arrayUri, schema);
    }

    private void WriteSparseVarDimArray(string arrayUri)
    {
        byte[] dData = Encoding.ASCII.GetBytes("abbccddee");
        ulong[] dOffsets = [0, 2, 4, 6, 8];

        int[] a1 = [1, 2, 3, 4, 5];

        using Array array = new Array(_ctx, arrayUri);
        array.Open(QueryType.Write);
        using Query query = new Query(array);
        query.SetLayout(LayoutType.GlobalOrder);

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

        using Attribute a1 = new Attribute(_ctx, nameof(a1), DataType.Int32);

        using ArraySchema schema = new ArraySchema(_ctx, ArrayType.Sparse);
        schema.SetTileOrder(LayoutType.RowMajor);
        schema.SetCellOrder(LayoutType.RowMajor);
        schema.SetCapacity(2);
        schema.SetDomain(domain);
        schema.AddAttribute(a1);

        schema.Check();
        Array.Create(_ctx, arrayUri, schema);
    }

    private void CreateSparseArrayNoVarDimInt32(string arrayUri)
    {
        using Dimension d1 = Dimension.Create<int>(_ctx, nameof(d1), 1, 10, 5);
        using Dimension d2 = Dimension.Create<int>(_ctx, nameof(d2), 1, 10, 5);

        using Domain domain = new Domain(_ctx);
        domain.AddDimension(d1);
        domain.AddDimension(d2);

        using Attribute a1 = new Attribute(_ctx, nameof(a1), DataType.Int32);

        using ArraySchema schema = new ArraySchema(_ctx, ArrayType.Sparse);
        schema.SetTileOrder(LayoutType.RowMajor);
        schema.SetCellOrder(LayoutType.RowMajor);
        schema.SetCapacity(2);
        schema.SetDomain(domain);
        schema.AddAttribute(a1);

        schema.Check();
        Array.Create(_ctx, arrayUri, schema);
    }

    private void WriteSparseArrayNoVarDim3Frags(string arrayUri)
    {
        using Array a = new Array(_ctx, arrayUri);
        a.Open(QueryType.Write);

        WriteImpl([1, 2], [1, 2], [1, 2]);
        WriteImpl([1, 2, 7, 8], [1, 2, 7, 8], [9, 10, 11, 12]);
        WriteImpl([1, 2, 7, 1], [1, 2, 7, 8], [5, 6, 7, 8]);

        void WriteImpl(long[] d1, long[] d2, int[] a1)
        {
            using Query query = new Query(a);
            query.SetLayout(LayoutType.Unordered);
            query.SetDataBuffer("d1", d1);
            query.SetDataBuffer("d2", d2);
            query.SetDataBuffer("a1", a1);

            query.Submit();
            query.FinalizeQuery();
        }
    }

    private void WriteSparseArrayNoVarDim3FragsInt32(string arrayUri)
    {
        using Array a = new Array(_ctx, arrayUri);
        a.Open(QueryType.Write);

        WriteImpl([1, 2], [1, 2], [1, 2]);
        WriteImpl([1, 2, 7, 8], [1, 2, 7, 8], [9, 10, 11, 12]);
        WriteImpl([1, 2, 7, 1], [1, 2, 7, 8], [5, 6, 7, 8]);

        void WriteImpl(int[] d1, int[] d2, int[] a1)
        {
            using Query query = new Query(a);
            query.SetLayout(LayoutType.Unordered);
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

        using Attribute a = new Attribute(_ctx, nameof(a), DataType.Int32);
        using ArraySchema schema = new ArraySchema(_ctx, ArrayType.Sparse);
        schema.SetTileOrder(LayoutType.RowMajor);
        schema.SetCellOrder(LayoutType.RowMajor);
        schema.SetDomain(domain);
        schema.SetCapacity(2);
        schema.AddAttribute(a);

        Array.Create(_ctx, arrayUri, schema);
    }

    private void WriteSparseVarDimArrayForMbrTesting(string arrayUri)
    {
        byte[] dData = Encoding.ASCII.GetBytes("abbcddd");
        ulong[] dOffsets = [0, 1, 3, 4];

        int[] a = [11, 12, 13, 14];

        using Array array = new Array(_ctx, arrayUri);
        array.Open(QueryType.Write);
        using Query query = new Query(array);
        query.SetLayout(LayoutType.Unordered);

        query.SetDataBuffer("d", dData);
        query.SetOffsetsBuffer("d", dOffsets);
        query.SetDataBuffer("a", a);

        query.Submit();

        query.FinalizeQuery();
    }
}
