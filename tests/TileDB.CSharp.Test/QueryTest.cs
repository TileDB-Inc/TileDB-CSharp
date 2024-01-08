using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TileDB.CSharp.Test;

[TestClass]
public class QueryTest
{

    [DataRow(LayoutType.GlobalOrder, ArrayType.Sparse)]
    [DataRow(LayoutType.GlobalOrder, ArrayType.Dense)]
    [DataTestMethod]
    public void TestGlobalQuery(LayoutType layoutType, ArrayType arrayType)
    {
        string arrayPath = TestUtil.MakeTestPath(
            $"array-query-{EnumUtil.ArrayTypeToStr(arrayType)}-{EnumUtil.LayoutTypeToStr(layoutType)}");
        Console.WriteLine($"Creating temp test array: {arrayPath}");
        if (Directory.Exists(arrayPath))
        {
            Directory.Delete(arrayPath, true);
        }

        var ctx = Context.GetDefault();
        Assert.IsNotNull(ctx);

        // Create array
        var rows = Dimension.Create(ctx, "rows", 1, 4, 2);
        Assert.IsNotNull(rows);
        var cols = Dimension.Create(ctx, "cols", 1, 4, 2);
        Assert.IsNotNull(cols);
        using var domain = new Domain(ctx);
        Assert.IsNotNull(domain);
        domain.AddDimensions(rows, cols);
        Assert.IsTrue(domain.HasDimension("rows"));
        Assert.IsTrue(domain.HasDimension("cols"));
        using var schema = new ArraySchema(ctx, arrayType);
        schema.SetDomain(domain);
        schema.AddAttribute(Attribute.Create<int>(ctx, "a1"));
        Array.Create(ctx, arrayPath, schema);
        var array = new Array(ctx, arrayPath);

        // Write array
        array.Open(QueryType.Write);
        Assert.IsTrue(array.IsOpen());
        using var queryWrite = new Query(array);
        queryWrite.SetLayout(layoutType);
        if (arrayType == ArrayType.Dense)
        {
            using var subarray = new Subarray(array);
            subarray.AddRange("rows", 1, 4);
            Assert.ThrowsException<ArgumentException>(() => subarray.AddRange<long>("rows", 1, 4));
            Assert.AreEqual((1, 4), subarray.GetRange<int>("rows", 0));
            Assert.ThrowsException<ArgumentException>(() => subarray.GetRange<long>("rows", 0));
            subarray.AddRange(1, 1, 2); // cols
            Assert.ThrowsException<ArgumentException>(() => subarray.AddRange<long>(1, 1, 2));
            Assert.AreEqual((1, 2), subarray.GetRange<int>(1, 0));
            Assert.ThrowsException<ArgumentException>(() => subarray.GetRange<long>(1, 0));
            queryWrite.SetSubarray(subarray);
            queryWrite.SetDataReadOnlyBuffer<int>("a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8 }.AsMemory());
        }
        else // Sparse
        {
            queryWrite.SetDataReadOnlyBuffer<int>("rows", new[] { 1, 2, 2, 3, 4, 4 }.AsMemory());
            queryWrite.SetDataReadOnlyBuffer<int>("cols", new[] { 1, 1, 4, 1, 1, 4 }.AsMemory());
            queryWrite.SetDataReadOnlyBuffer<int>("a1", new[] { 1, 2, 3, 4, 5, 6 }.AsMemory());
        }
        queryWrite.Submit();
        Assert.AreEqual(QueryStatus.Completed, queryWrite.Status());
        queryWrite.FinalizeQuery();
        array.Close();

        // Read array
        array.Open(QueryType.Read);
        Assert.IsTrue(array.IsOpen());
        using var queryRead = new Query(array);
        // Initially allocate buffers for dense read
        var a1Read = new int[16];
        var rowsRead = new int[16];
        var colsRead = new int[16];
        if (arrayType == ArrayType.Dense)
        {
            using var subarray = new Subarray(array);
            subarray.SetSubarray(1, 4, 1, 4);
            queryRead.SetSubarray(subarray);
        }
        else // Sparse
        {
            // Reallocate buffers for sparse read
            a1Read = new int[6];
            rowsRead = new int[6];
            colsRead = new int[6];
        }

        // Get coords and attributes for dense and sparse reads
        queryRead.SetDataBuffer("rows", rowsRead);
        queryRead.SetDataBuffer("cols", colsRead);
        queryRead.SetDataBuffer("a1", a1Read);
        queryRead.Submit();
        Assert.AreEqual(QueryStatus.Completed, queryRead.Status());
        array.Close();

        // Check expected values
        int[] a1Expected;
        int[] rowsExpected;
        int[] colsExpected;
        if (arrayType == ArrayType.Dense)
        {
            a1Expected =
            [
                1, 2, int.MinValue, int.MinValue,
                3, 4, int.MinValue, int.MinValue,
                5, 6, int.MinValue, int.MinValue,
                7, 8, int.MinValue, int.MinValue
            ];
            rowsExpected = [1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4];
            colsExpected = [1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4];
        }
        else // Sparse
        {
            a1Expected = [1, 2, 3, 4, 5, 6];
            rowsExpected = [1, 2, 2, 3, 4, 4];
            colsExpected = [1, 1, 4, 1, 1, 4];
        }
        CollectionAssert.AreEqual(a1Expected, a1Read);
        CollectionAssert.AreEqual(colsExpected, colsRead);
        CollectionAssert.AreEqual(rowsExpected, rowsRead);
    }

    [TestMethod]
    public void TestDenseQuery()
    {
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        const sbyte boundLower = 0;
        const sbyte boundUpper = 9;
        const sbyte extent = 2;
        var dimension = Dimension.Create(context, "dim1", boundLower, boundUpper, extent);
        Assert.IsNotNull(dimension);

        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimension(dimension);

        var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);

        var a1 = new Attribute(context, "a1", DataType.Int32);
        Assert.IsNotNull(a1);

        var a2 = new Attribute(context, "a2", DataType.Float32);
        Assert.IsNotNull(a2);

        a2.SetCellValNum(Attribute.VariableSized);

        array_schema.AddAttributes(a1, a2);

        array_schema.SetDomain(domain);

        array_schema.Check();

        var tmpArrayPath = TestUtil.MakeTestPath("tiledb_test_array");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        var array = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array);

        array.Create(array_schema);

        array.Open(QueryType.Write);

        var query = new Query(array);

        using (var subarray = new Subarray(array))
        {
            subarray.SetSubarray<sbyte>(0, 1);
            Assert.ThrowsException<ArgumentException>(() => subarray.SetSubarray(0, 1));
            Assert.ThrowsException<ArgumentException>(() => subarray.SetSubarray(0));
            query.SetSubarray(subarray);
        }

        query.SetLayout(LayoutType.RowMajor);

        var a1_data_buffer = new int[2] { 1, 2 };

        query.SetDataBuffer("a1", a1_data_buffer);

        var a2_data_buffer = new float[5] { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

        query.SetDataBuffer("a2", a2_data_buffer);

        var a2_offset_buffer = new ulong[2] { 0, 3 };

        query.SetOffsetsBuffer("a2", a2_offset_buffer);

        query.Submit();

        var status = query.Status();

        Assert.AreEqual(QueryStatus.Completed, status);

        query.FinalizeQuery();

        array.Close();

        //Start to read
        var array_read = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_read);

        array_read.Open(QueryType.Read);

        var query_read = new Query(array_read);

        using (var subarray = new Subarray(array_read))
        {
            subarray.SetSubarray<sbyte>(0, 1);

            query_read.SetSubarray(subarray);
        }

        query_read.SetLayout(LayoutType.RowMajor);

        var a1_data_buffer_read = new int[2];

        query_read.SetDataBuffer("a1", a1_data_buffer_read);

        var a2_data_buffer_read = new float[5];

        query_read.SetDataBuffer("a2", a2_data_buffer_read);

        var a2_offset_buffer_read = new ulong[2];

        query_read.SetOffsetsBuffer("a2", a2_offset_buffer_read);

        query_read.Submit();
        var status_read = query_read.Status();

        Assert.AreEqual(QueryStatus.Completed, status_read);

        query_read.FinalizeQuery();

        array_read.Close();

        CollectionAssert.AreEqual(a1_data_buffer, a1_data_buffer_read);

        CollectionAssert.AreEqual(a2_data_buffer, a2_data_buffer_read);

        CollectionAssert.AreEqual(a2_offset_buffer, a2_offset_buffer_read);
    }

    [TestMethod]
    public void TestSimpleSparseArrayQuery()
    {
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        // Create array
        int dim1_bound_lower = 1;
        int dim1_bound_upper = 4;
        int dim1_extent = 4;
        var dim1 = Dimension.Create(context, "rows", dim1_bound_lower, dim1_bound_upper, dim1_extent);

        int dim2_bound_lower = 1;
        int dim2_bound_upper = 4;
        int dim2_extent = 4;
        var dim2 = Dimension.Create(context, "cols", dim2_bound_lower, dim2_bound_upper, dim2_extent);

        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimension(dim1);
        domain.AddDimension(dim2);

        var array_schema = new ArraySchema(context, ArrayType.Sparse);
        Assert.IsNotNull(array_schema);

        var attr = new Attribute(context, "a", DataType.Int32);
        Assert.IsNotNull(attr);

        array_schema.AddAttribute(attr);

        array_schema.SetDomain(domain);

        array_schema.Check();

        var tmpArrayPath = TestUtil.MakeTestPath("tiledb_test_sparse_array");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        Array.Create(context, tmpArrayPath, array_schema);

        //Write array
        var dim1_data_buffer = new int[3] { 1, 2, 3 };
        var dim2_data_buffer = new int[3] { 1, 3, 4 };
        var attr_data_buffer = new int[3] { 1, 2, 3 };

        using (var array_write = new Array(context, tmpArrayPath))
        {
            Assert.IsNotNull(array_write);

            array_write.Open(QueryType.Write);

            var query_write = new Query(array_write);

            query_write.SetLayout(LayoutType.Unordered);

            query_write.SetDataBuffer("rows", dim1_data_buffer);
            query_write.SetDataBuffer("cols", dim2_data_buffer);
            query_write.SetDataBuffer("a", attr_data_buffer);

            query_write.Submit();

            var status = query_write.Status();

            Assert.AreEqual(QueryStatus.Completed, status);

            array_write.Close();
        }//array_write


        //Read array
        var dim1_data_buffer_read = new int[3];
        var dim2_data_buffer_read = new int[3];
        var attr_data_buffer_read = new int[3];

        using (var array_read = new Array(context, tmpArrayPath))
        {
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.Read);

            var query_read = new Query(array_read);

            query_read.SetLayout(LayoutType.RowMajor);

            query_read.SetDataBuffer("rows", dim1_data_buffer_read);

            query_read.SetDataBuffer("cols", dim2_data_buffer_read);

            query_read.SetDataBuffer("a", attr_data_buffer_read);

            query_read.Submit();
            var status_read = query_read.Status();

            Assert.AreEqual(QueryStatus.Completed, status_read);

            array_read.Close();
        }


        CollectionAssert.AreEqual(dim1_data_buffer, dim1_data_buffer_read);

        CollectionAssert.AreEqual(dim2_data_buffer, dim2_data_buffer_read);

        CollectionAssert.AreEqual(attr_data_buffer, attr_data_buffer_read);
    }

    [TestMethod]
    public unsafe void TestNullableAttributeArrayQuery()
    {
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        // Create array
        int dim1_bound_lower = 1;
        int dim1_bound_upper = 2;
        int dim1_extent = 2;
        var dim1 = Dimension.Create(context, "rows", dim1_bound_lower, dim1_bound_upper, dim1_extent);

        int dim2_bound_lower = 1;
        int dim2_bound_upper = 2;
        int dim2_extent = 2;
        var dim2 = Dimension.Create(context, "cols", dim2_bound_lower, dim2_bound_upper, dim2_extent);

        var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimension(dim1);
        domain.AddDimension(dim2);

        var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);

        var attr1 = new Attribute(context, "a1", DataType.Int32);
        Assert.IsNotNull(attr1);
        attr1.SetNullable(true);
        array_schema.AddAttribute(attr1);

        var attr2 = new Attribute(context, "a2", DataType.Int32);
        Assert.IsNotNull(attr2);
        attr2.SetNullable(true);
        attr2.SetCellValNum(Attribute.VariableSized);
        array_schema.AddAttribute(attr2);

        var attr3 = new Attribute(context, "a3", DataType.StringAscii);
        Assert.IsNotNull(attr3);
        attr3.SetNullable(true);
        array_schema.AddAttribute(attr3);

        array_schema.SetDomain(domain);
        array_schema.SetTileOrder(LayoutType.RowMajor);
        array_schema.SetCellOrder(LayoutType.RowMajor);

        array_schema.Check();

        var tmpArrayPath = TestUtil.MakeTestPath("tiledb_test_nullable_array");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        Array.Create(context, tmpArrayPath, array_schema);

        // Write array
        int[] a1_data = [100, 200, 300, 400];
        int[] a2_data = [10, 10, 20, 30, 30, 30, 40, 40];
        ulong[] a2_el_off = [0, 2, 3, 6];
        ulong[] a2_off = new ulong[4];
        for (int i = 0; i < a2_el_off.Length; ++i)
        {
            a2_off[i] = a2_el_off[i] * EnumUtil.DataTypeSize(DataType.Int32);
        }

        byte[] a3_data = "abcdewxyz"u8.ToArray();
        ulong[] a3_el_off = [0, 3, 4, 5];
        ulong[] a3_off = new ulong[4];
        for (int i = 0; i < a3_el_off.Length; ++i)
        {
            a3_off[i] = a3_el_off[i] * EnumUtil.DataTypeSize(DataType.StringAscii);
        }

        byte[] a1_validity = [1, 0, 0, 1];
        byte[] a2_validity = [0, 1, 1, 0];
        byte[] a3_validity = [1, 0, 0, 1];

        fixed (int* a1_data_ptr = &a1_data[0])
        fixed (int* a2_data_ptr = &a2_data[0])
        fixed (ulong* a2_off_ptr = &a2_off[0])
        fixed (byte* a2_validity_ptr = &a2_validity[0])
        {
            using var array_write = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_write);

            array_write.Open(QueryType.Write);

            using var query_write = new Query(array_write);
            query_write.SetLayout(LayoutType.RowMajor);

            query_write.SetDataBuffer("a1", a1_data_ptr, (ulong)a1_data.Length);
            query_write.SetValidityBuffer("a1", a1_validity);
            Assert.ThrowsException<ArgumentException>(() => query_write.UnsafeSetDataReadOnlyBuffer("a1", ReadOnlyMemory<byte>.Empty));

            query_write.UnsafeSetDataBuffer("a2", (void*)a2_data_ptr, (ulong)a2_data.Length * sizeof(int));
            query_write.SetOffsetsBuffer("a2", a2_off_ptr, (ulong)a2_off.Length);
            query_write.SetValidityBuffer("a2", a2_validity_ptr, (ulong)a2_validity.Length);

            query_write.SetDataBuffer("a3", a3_data);
            query_write.SetOffsetsBuffer("a3", a3_off);
            query_write.SetValidityBuffer("a3", a3_validity);

            query_write.Submit();

            var status = query_write.Status();

            Assert.AreEqual(QueryStatus.Completed, status);

            array_write.Close();
        }

        // Read array
        int[] a1_data_read = new int[4];
        byte[] a1_validity_read = new byte[4];

        int[] a2_data_read = new int[8];
        ulong[] a2_off_read = new ulong[4];
        byte[] a2_validity_read = new byte[4];

        byte[] a3_data_read = new byte[9];
        ulong[] a3_off_read = new ulong[4];
        byte[] a3_validity_read = new byte[4];

        using (var array_read = new Array(context, tmpArrayPath))
        {
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.Read);

            using var query_read = new Query(array_read);

            query_read.SetLayout(LayoutType.RowMajor);
            using var subarray = new Subarray(array_read);
            subarray.SetSubarray(1, 2, 1, 2);
            query_read.SetSubarray(subarray);

            query_read.UnsafeSetDataBuffer("a1", a1_data_read.AsMemory().Pin(), (ulong)a1_data_read.Length * sizeof(int));
            query_read.SetValidityBuffer("a1", a1_validity_read);

            query_read.UnsafeSetDataBuffer("a2", a2_data_read.AsMemory().Pin(), (ulong)a2_data_read.Length * sizeof(int));
            query_read.SetOffsetsBuffer("a2", a2_off_read);
            query_read.SetValidityBuffer("a2", a2_validity_read);

            query_read.UnsafeSetDataBuffer("a3", a3_data_read.AsMemory());
            Assert.ThrowsException<ArgumentException>(() => query_read.UnsafeSetDataBuffer("a3", default(MemoryHandle), 0));
            query_read.SetOffsetsBuffer("a3", a3_off_read);
            query_read.SetValidityBuffer("a3", a3_validity_read);

            query_read.Submit();
            var status_read = query_read.Status();

            Assert.AreEqual(QueryStatus.Completed, status_read);
            Assert.AreEqual((ulong)a1_data_read.Length * sizeof(int), query_read.GetResultDataBytes("a1"));
            Assert.AreEqual((ulong)a2_data_read.Length * sizeof(int), query_read.GetResultDataBytes("a2"));
            Assert.AreEqual((ulong)a3_data_read.Length * sizeof(byte), query_read.GetResultDataBytes("a3"));
            Assert.ThrowsException<InvalidOperationException>(() => query_read.GetResultDataElements("a1"));
            Assert.ThrowsException<InvalidOperationException>(() => query_read.GetResultDataElements("a2"));
            Assert.ThrowsException<InvalidOperationException>(() => query_read.GetResultDataElements("a3"));

            Assert.AreEqual((ulong)a2_off_read.Length, query_read.GetResultOffsets("a2"));
            Assert.AreEqual((ulong)a3_off_read.Length, query_read.GetResultOffsets("a3"));

            Assert.AreEqual((ulong)a2_validity_read.Length, query_read.GetResultValidities("a2"));
            Assert.AreEqual((ulong)a3_validity_read.Length, query_read.GetResultValidities("a3"));

            array_read.Close();
        }

        CollectionAssert.AreEqual(a1_data, a1_data_read);
        CollectionAssert.AreEqual(a1_validity, a1_validity_read);

        CollectionAssert.AreEqual(a2_data, a2_data_read);
        CollectionAssert.AreEqual(a2_validity, a2_validity_read);
        CollectionAssert.AreEqual(a2_off, a2_off_read);

        CollectionAssert.AreEqual(a3_data, a3_data_read);
        CollectionAssert.AreEqual(a3_validity, a3_validity_read);
        CollectionAssert.AreEqual(a3_off, a3_off_read);
    }// public void TestNullableAttributeArrayQuery

    [TestMethod]
    public void TestBoolAttributeArrayQuery()
    {
        // Create array
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        var dim1 = Dimension.Create(context, "rows", 1, 2, 2);
        var dim2 = Dimension.Create(context, "cols", 1, 2, 2);
        var domain = new Domain(context);
        domain.AddDimensions(dim1, dim2);

        var a1 = new Attribute(context, "a1", DataType.Boolean);

        var array_schema = new ArraySchema(context, ArrayType.Dense);
        array_schema.AddAttribute(a1);
        array_schema.SetDomain(domain);
        array_schema.Check();

        var tmpArrayPath = TestUtil.MakeTestPath("tiledb_test_bool_array");
        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        // Write to array
        var array_write = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_write);
        array_write.Create(array_schema);
        array_write.Open(QueryType.Write);

        var query_write = new Query(array_write);
        using (var subarray = new Subarray(array_write))
        {
            subarray.SetSubarray(1, 2, 1, 2);
            query_write.SetSubarray(subarray);
        }
        query_write.SetLayout(LayoutType.RowMajor);

        var a1_data = new bool[] { false, true, true, false };
        query_write.SetDataBuffer("a1", a1_data);

        query_write.Submit();
        var status = query_write.Status();
        Assert.AreEqual(status, QueryStatus.Completed);
        query_write.FinalizeQuery();
        query_write.Dispose();
        array_write.Close();

        // Read from array into bool[]
        var array_read = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_read);
        array_read.Open(QueryType.Read);

        var query_read = new Query(array_read);
        using (var subarray = new Subarray(array_read))
        {
            subarray.SetSubarray(1, 2, 1, 2);
            query_read.SetSubarray(subarray);
        }
        query_read.SetLayout(LayoutType.RowMajor);

        var a1_data_read = new bool[4];
        query_read.SetDataBuffer("a1", a1_data_read);

        query_read.Submit();
        status = query_read.Status();
        Assert.AreEqual(status, QueryStatus.Completed);
        CollectionAssert.AreEqual(a1_data, a1_data_read);

        query_read.FinalizeQuery();
        query_read.Dispose();

        // Read from array into byte[]
        query_read = new Query(array_read);
        using (var subarray = new Subarray(array_read))
        {
            subarray.SetSubarray(1, 2, 1, 2);
            query_read.SetSubarray(subarray);
        }
        query_read.SetLayout(LayoutType.RowMajor);

        var a1_data_read_bytes = new byte[4];
        query_read.SetDataBuffer("a1", a1_data_read_bytes);

        query_read.Submit();
        status = query_read.Status();
        Assert.AreEqual(status, QueryStatus.Completed);
        CollectionAssert.AreEqual(a1_data, System.Array.ConvertAll(a1_data_read_bytes, b => b == 1));

        query_read.FinalizeQuery();
        query_read.Dispose();
        array_read.Close();
    }

    [TestMethod]
    public void TestMemoryHandleGetsUnpinned()
    {
        // Create array
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        using var dim1 = Dimension.Create(context, "rows", 1, 2, 2);
        using var dim2 = Dimension.Create(context, "cols", 1, 2, 2);
        using var domain = new Domain(context);
        domain.AddDimensions(dim1, dim2);

        using var a1 = new Attribute(context, "a1", DataType.Boolean);

        using var array_schema = new ArraySchema(context, ArrayType.Dense);
        array_schema.AddAttribute(a1);
        array_schema.SetDomain(domain);
        array_schema.Check();

        using var tmpArrayPath = new TemporaryDirectory("query_unpin");

        var disposalCanary = new DisposalCanary();

        using var array = new Array(context, tmpArrayPath);
        array.Create(array_schema);
        array.Open(QueryType.Read);

        // From the documentation, the memory will be unpinned if:

        // the query is disposed,
        using (var q = new Query(array))
        {
            q.UnsafeSetDataBuffer("rows", disposalCanary.Memory.Pin(), 1 * sizeof(int));
        }
        Assert.AreEqual(1, disposalCanary.UnpinCount);

        // the buffer is reassigned,
        using (var q = new Query(array))
        {
            q.UnsafeSetDataBuffer("rows", disposalCanary.Memory.Pin(), 1 * sizeof(int));
            q.SetDataBuffer("rows", new int[1]);
            Assert.AreEqual(2, disposalCanary.UnpinCount);
        }

        // or setting the buffer fails.
        using (var q = new Query(array))
        {
            Assert.ThrowsException<TileDBException>(() => q.UnsafeSetDataBuffer("foo", disposalCanary.Memory.Pin(), 1 * sizeof(int)));
            Assert.AreEqual(3, disposalCanary.UnpinCount);
        }
    }

    /// <summary>
    /// Holds an array and tracks how many times it was unpinned.
    /// </summary>
    private sealed unsafe class DisposalCanary : MemoryManager<int>
    {
        private readonly int[] _array = new int[1];

        public int UnpinCount { get; private set; }

        public override Span<int> GetSpan() => _array;

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            var gcHandle = GCHandle.Alloc(_array, GCHandleType.Pinned);
            var ptr = (void*)gcHandle.AddrOfPinnedObject();
            return new(ptr, gcHandle, this);
        }

        public override void Unpin()
        {
            UnpinCount++;
        }

        protected override void Dispose(bool disposing) { }
    }
}
