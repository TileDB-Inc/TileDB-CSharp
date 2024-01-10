using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace TileDB.CSharp.Test;

[TestClass]
public class DeletesTest
{
    private static void InitTest(Context ctx, string arrayName, LayoutType layout, bool duplicates)
    {
        using var rows = Dimension.Create(ctx, "rows", 1, 4, 4);
        using var cols = Dimension.Create(ctx, "cols", 1, 4, 4);
        using var domain = new Domain(ctx);
        domain.AddDimensions(rows, cols);
        // Deletes are only supported for sparse arrays
        var arrayType = ArrayType.Sparse;
        using var schema = new ArraySchema(ctx, arrayType);
        schema.SetDomain(domain);
        schema.AddAttribute(new Attribute(ctx, "a1", DataType.Int32));
        schema.SetAllowsDups(duplicates);
        schema.Check();
        using var array = new Array(ctx, arrayName);
        TestUtil.CreateArray(ctx, arrayName, schema);
        var initialData = new Dictionary<string, System.Array>()
        {
            { "a1", Enumerable.Range(1, 16).ToArray() },
            {"rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
            { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } }
        };
        TestUtil.WriteArray(array, layout, initialData, keepOpen: false);

        var readBuffers = new Dictionary<string, System.Array>
            { {"rows", new int[16]}, {"cols", new int[16]}, {"a1", new int[16]} };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a1", Enumerable.Range(1, 16).ToArray() },
            { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
            { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
        };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    private static void DeleteFour(Context ctx, Array array, LayoutType layout, bool duplicates)
    {
        // Delete multiple cells at once
        array.Open(QueryType.Delete);
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var notCondition = QueryCondition.Create(ctx, "cols", 3, QueryConditionOperatorType.NotEqual);
        using var andCondition = QueryCondition.Create(ctx, "cols", 2, QueryConditionOperatorType.GreaterThan);
        using var queryCondition = notCondition & andCondition;
        deleteQuery.SetCondition(queryCondition);
        deleteQuery.Submit();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());
        array.Close();

        // Check for expected values
        var readBuffers = new Dictionary<string, System.Array>()
            { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
        using var readQuery = TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 1, 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15, 0, 0, 0, 0 } },
            { "rows", new[] { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 0, 0, 0, 0 } },
            { "cols", new[] { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 0, 0, 0, 0 } },
        };

        array.Open(QueryType.Read);
        Assert.AreEqual(12UL, readQuery.ResultBufferElements()["a1"].Item1);
        Assert.AreEqual(12UL, readQuery.GetResultDataElements("a1"));
        array.Close();
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    // Params for all tests are (layout, legacy, duplicates)
    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void MultipleCells(LayoutType layout, string queryReader, bool duplicates)
    {
        string arrayName = "deletes-multiple-cells";
        var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);
        InitTest(ctx, arrayName, layout, duplicates);
        using var array = new Array(ctx, arrayName);

        DeleteFour(ctx, array, layout, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void SingleCell(LayoutType layout, string queryReader, bool duplicates)
    {
        string arrayName = "deletes-single-cell";
        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);
        InitTest(ctx, arrayName, layout, duplicates);
        using var array = new Array(ctx, arrayName);

        // Delete one cell
        array.Open(QueryType.Delete);
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var colsCondition = QueryCondition.Create(ctx, "cols", 1, QueryConditionOperatorType.Equal);
        using var rowsCondition = QueryCondition.Create(ctx, "rows", 1, QueryConditionOperatorType.Equal);
        using var andCondition = colsCondition & rowsCondition;
        deleteQuery.SetCondition(andCondition);
        deleteQuery.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());

        // Check for expected values
        var readBuffers = new Dictionary<string, System.Array>()
            { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
        using var readQuery = TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

        array.Open(QueryType.Read);
        Assert.AreEqual(15UL, readQuery.ResultBufferElements()["a1"].Item1);
        Assert.AreEqual(15UL, readQuery.GetResultDataElements("a1"));
        array.Close();
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
            { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 } },
            { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
        };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void VaryingCells(LayoutType layout, string queryReader, bool duplicates)
    {
        string arrayName = "deletes-varying-cells";
        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);
        InitTest(ctx, arrayName, layout, duplicates);
        using var array = new Array(ctx, arrayName);

        DeleteFour(ctx, array, layout, duplicates);

        // Delete 1 cell
        array.Open(QueryType.Delete);
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var colsCondition = QueryCondition.Create(ctx, "cols", 1, QueryConditionOperatorType.Equal);
        using var rowsCondition = QueryCondition.Create(ctx, "rows", 1, QueryConditionOperatorType.Equal);
        using var andCondition = colsCondition & rowsCondition;
        deleteQuery.SetCondition(andCondition);
        deleteQuery.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());
        var readBuffers = new Dictionary<string, System.Array>
            { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15, 0, 0, 0, 0, 0 } },
            { "rows", new[] { 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 0, 0, 0, 0, 0 } },
            { "cols", new[] { 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 0, 0, 0, 0, 0 } },
        };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Delete 6 cells
        // + The OR condition overlaps on (3, 4)
        // + (3, 2) was deleted by a previous delete query
        array.Open(QueryType.Delete);
        using var deleteQuery2 = new Query(array, QueryType.Delete);
        using var colsCondition2 = QueryCondition.Create(ctx, "cols", 4, QueryConditionOperatorType.Equal);
        using var rowsCondition2 = QueryCondition.Create(ctx, "rows", 3, QueryConditionOperatorType.Equal);
        using var orCondition = colsCondition2 | rowsCondition2;
        deleteQuery2.SetCondition(orCondition);
        deleteQuery2.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery2.Status());

        // Check for expected values
        readBuffers = new()
            { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        expectedData["a1"] = new[] { 2, 3, 5, 6, 7, 13, 14, 15, 0, 0, 0, 0, 0, 0, 0, 0 };
        expectedData["rows"] = new[] { 1, 1, 2, 2, 2, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0 };
        expectedData["cols"] = new[] { 2, 3, 1, 2, 3, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0 };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void MultipleFragments(LayoutType layout, string queryReader, bool duplicates)
    {
        string arrayName = "deletes-multiple-fragments";
        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);
        InitTest(ctx, arrayName, layout, duplicates);
        using var array = new Array(ctx, arrayName);

        // Delete single cell
        array.Open(QueryType.Delete);
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var rowsCondition = QueryCondition.Create(ctx, "rows", 1, QueryConditionOperatorType.Equal);
        using var colsCondition = QueryCondition.Create(ctx, "cols", 1, QueryConditionOperatorType.Equal);
        using var andCondition = rowsCondition & colsCondition;
        deleteQuery.SetCondition(andCondition);
        deleteQuery.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());

        int bufferSize = 16;
        var readBuffers = new Dictionary<string, System.Array>
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        using (var readQuery = TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx)) {
            array.Open(QueryType.Read);
            Assert.AreEqual(15UL, readQuery.ResultBufferElements()["a1"].Item1);
            Assert.AreEqual(15UL, readQuery.GetResultDataElements("a1"));
            array.Close();
        }
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
            { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 } },
            { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
        };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Re-add; If duplicates are enabled result buffer size will grow
        bufferSize = duplicates ? ++bufferSize : bufferSize;
        using var writeQuery = TestUtil.WriteArray(array, layout, new()
            { {"rows", new[] {1}}, {"cols", new[] {1}}, {"a1", new[] {17}} }, ctx: ctx);
        Assert.AreEqual(QueryStatus.Completed, writeQuery.Status());
        readBuffers = new()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };

        // Scale buffers to fit extra value if duplicates enabled
        if (duplicates)
        {
            expectedData["a1"] = new[] { 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 };
            expectedData["rows"] = new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 };
            expectedData["cols"] = new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 };
        }
        else
        {
            expectedData["a1"] = new[] { 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            expectedData["rows"] = new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
            expectedData["cols"] = new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 };
        }

        using (var readQuery = TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx))
        {
            array.Open(QueryType.Read);
            // For duplicates we allocate for 17 values including deleted cell, but we only retrieve 16 valid results
            Assert.AreEqual(16UL, readQuery.ResultBufferElements()["a1"].Item1);
            Assert.AreEqual(16UL, readQuery.GetResultDataElements("a1"));
            array.Close();
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void SchemaEvolution(LayoutType layout, string queryReader, bool duplicates)
    {
        string arrayName = "deletes-schema-evolution";
        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);
        InitTest(ctx, arrayName, layout, duplicates);
        using var array = new Array(ctx, arrayName);

        // We will increment this value according to writes if duplicates are enabled
        int bufferSize = 16;
        var readBuffers = new Dictionary<string, System.Array>()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
            { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
            { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
        };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Evolve schema; Write to evolved array; Read and output array attributes
        using var schemaEvolution = new ArraySchemaEvolution(ctx);
        using var a2 = new Attribute(ctx, "a2", DataType.Float64);
        a2.SetFillValue(-1.0);
        schemaEvolution.AddAttribute(a2);
        array.Evolve(schemaEvolution);
        Logger.LogMessage("Initial array schema: ");
        TestUtil.PrintLocalSchema(arrayName);
        array.Open(QueryType.Read);
        Assert.IsTrue(array.Schema().HasAttribute("a2"));
        array.Close();

        // Write to evolved array
        var writeData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 1 } },
            { "a2", new[] { 1.0 } },
            { "rows", new[] { 1 } },
            { "cols", new[] { 1 } },
        };
        using var writeQuery = TestUtil.WriteArray(array, layout, writeData, ctx: ctx);
        Assert.AreEqual(QueryStatus.Completed, writeQuery.Status());
        bufferSize = duplicates ? ++bufferSize : bufferSize;
        readBuffers = new()
        { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
            { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };

        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "a2", new[] { -1.0, 1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0,
                    -1.0, -1.0, -1.0, } },
                { "rows", new[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "a2", new[] { 1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0,
                    -1.0, -1.0, } },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Repeat write to produce another fragment
        writeData = new Dictionary<string, System.Array>()
        {
            { "a1", new[] { 1 } },
            { "a2", new[] { 1.0 } },
            { "rows", new[] { 1 } },
            { "cols", new[] { 1 } },
        };
        bufferSize = duplicates ? ++bufferSize : bufferSize;
        TestUtil.WriteArray(array, layout, writeData, ctx: ctx, keepOpen: false);
        readBuffers = new()
        { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
            { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };

        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "a2", new[] { 1.0, 1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0,
                    -1.0, -1.0, -1.0, -1.0, } },
                { "rows", new[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "a2", new[] { 1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0,
                    -1.0, -1.0, } },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Issue a delete
        var deleteTime = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds();
        using var delArray = new Array(ctx, arrayName);
        delArray.SetOpenTimestampStart(deleteTime);
        delArray.SetOpenTimestampEnd(deleteTime);
        delArray.Open(QueryType.Delete);
        using var deleteQuery = new Query(delArray, QueryType.Delete);
        using var rowsCondition = QueryCondition.Create(ctx, "rows", 1, QueryConditionOperatorType.Equal);
        using var colsCondition = QueryCondition.Create(ctx, "cols", 1, QueryConditionOperatorType.Equal);
        using var andCondition = rowsCondition & colsCondition;
        deleteQuery.SetCondition(andCondition);
        deleteQuery.Submit();
        Logger.LogMessage($"Delete timestamp: {deleteTime}");
        delArray.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());

        // Read before the timestamp of the delete
        readBuffers = new()
        { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
            { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime-1), ctx: ctx, keepOpen: false);
        Logger.LogMessage("\nArray set to open before delete");
        // We opened pre-delete so expectedData should not change
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Read after the timestamp of the delete
        readBuffers = new()
        { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
            { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime), ctx: ctx, keepOpen: false);

        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0 } },
                { "a2", new[] { -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0,
                    -1.0, 0, 0, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0 } },
                { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                { "a2", new[] { -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0,
                    -1.0, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0} },
                { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Remove attribute (a2)
        using var schemaEvolution2 = new ArraySchemaEvolution(ctx);
        schemaEvolution2.DropAttribute("a2");
        array.Evolve(schemaEvolution2);
        Logger.LogMessage($"Schema after removing attribute (a2)");
        TestUtil.PrintLocalSchema(array.Uri());
        array.SetOpenTimestampStart(0);
        array.SetOpenTimestampEnd(ulong.MaxValue);
        array.Open(QueryType.Read);
        Assert.IsFalse(array.Schema().HasAttribute("a2"));
        array.Close();

        // Read remaining attribute before delete and evolution
        readBuffers = new()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime-1), ctx: ctx, keepOpen: false);
        Assert.IsTrue(expectedData.Remove("a2"));

        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "rows", new[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        readBuffers = new()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime), ctx: ctx, keepOpen: false);

        // Read remaining attribute after delete
        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0 } },
                { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0} },
                { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Write some cells
        TestUtil.WriteArray(array, layout, new()
            { {"rows", new[] {2}}, {"cols", new[] {1}}, {"a1", new[] {18}} }, ctx: ctx, keepOpen: false);
        bufferSize = duplicates ? ++bufferSize : bufferSize;
        readBuffers = new()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);

        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 5, 18, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0 } },
                { "cols", new[] { 2, 3, 4, 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 18, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0} },
                { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Make a delete condition on an attribute
        array.Open(QueryType.Delete);
        using var deleteQuery2 = new Query(array, QueryType.Delete);
        using var attrCondition = QueryCondition.Create(ctx, "a1", 10, QueryConditionOperatorType.LessThan);
        deleteQuery2.SetCondition(attrCondition);
        deleteQuery2.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery2.Status());

        // Check for expected values
        readBuffers = new()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);

        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 18, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
                { "rows", new[] {  2, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 18, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0, 0, 0, 0, 0, 0 } },
                { "rows", new[] {  2, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void SameFragment(LayoutType layout, string queryReader, bool duplicates)
    {
        string arrayName = "deletes-same-fragment";
        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);
        using var array = new Array(ctx, arrayName);
        InitTest(ctx, arrayName, layout, duplicates);
        // For tests with duplicates we will increment bufferSize with write queries
        int bufferSize = 16;

        // Write a value to the array, creating a second fragment; Get fragment timestamp from writeQuery
        bufferSize = duplicates ? ++bufferSize : bufferSize;
        using var writeQuery = TestUtil.WriteArray(array, layout, new()
            { {"rows", new[] { 1 }}, {"cols", new[] { 1 }}, {"a1", new[] { 17 }}, }, ctx: ctx);
        Assert.AreEqual(1U, writeQuery.FragmentNum());
        var writeTime = writeQuery.FragmentTimestampRange(0);
        Logger.LogMessage($"Second fragment timestamp range: {writeTime}");
        // Apply same write operation to local array used to check expected attribute values
        var readBuffers = new Dictionary<string, System.Array>()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);

        var expectedData = new Dictionary<string, System.Array>();
        if (duplicates)
        {
            expectedData = new()
            {
                { "a1", new[] { 1, 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "rows", new[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        else
        {
            expectedData = new()
            {
                { "a1", new[] { 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16} },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] {1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
        }
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

        // Delete the new value from the array, opening at previous write fragment timestamp
        array.SetOpenTimestampStart(ulong.MinValue);
        array.SetOpenTimestampEnd(writeTime.Item1);
        array.Open(QueryType.Delete);
        Logger.LogMessage($"Array opened at between {array.OpenTimestampStart()} and {array.OpenTimestampEnd()}");
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var attrCondition = QueryCondition.Create(ctx, "a1", 17, QueryConditionOperatorType.Equal);
        deleteQuery.SetCondition(attrCondition);
        deleteQuery.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());

        // Check for expected values
        readBuffers = new()
            { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        if (duplicates)
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
            };
        }
        else
        {
            expectedData = new Dictionary<string, System.Array>()
            {
                { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 } },
                { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
            };
        }

        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void SequenceTest(LayoutType layout, string queryReader, bool duplicates)
    {
        var arrayName = "deletes-sequence-test";

        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);

        using var cols = Dimension.Create(ctx, "cols", 1, 6, 2);
        using var a0 = Attribute.Create<int>(ctx, "a0");
        using var domain = new Domain(ctx);
        domain.AddDimension(cols);
        using var schema = new ArraySchema(ctx, ArrayType.Sparse);
        schema.SetDomain(domain);
        schema.AddAttribute(a0);
        schema.SetAllowsDups(duplicates);
        schema.Check();
        if (Directory.Exists(arrayName))
        {
            Directory.Delete(arrayName, true);
        }

        Array.Create(ctx, arrayName, schema);
        using var array = new Array(ctx, arrayName);

        // Write { 1, 2, 3 }
        ulong nextOpen = 0;
        var readBuffers = new Dictionary<string, System.Array>
            { {"cols", new int[6]}, {"a0", new int[6]} };
        var expectedData = new Dictionary<string, System.Array>()
        {
            { "a0", new[] { 1, 2, 3, 0, 0, 0 } },
            { "cols", new[] { 1, 2, 3, 0, 0, 0 } },
        };
        using (var writeQuery = TestUtil.WriteArray(array, layout, new()
                   { { "cols", new[] { 1, 2, 3 } }, { "a0", new[] { 1, 2, 3 } } }, ctx: ctx))
        {
            Assert.AreEqual(QueryStatus.Completed, writeQuery.Status());
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
            var writeTime = writeQuery.FragmentTimestampRange(0);
            nextOpen = writeTime.Item2 + 20;
            // Delete[4]
            array.SetOpenTimestampEnd(writeTime.Item2+10);
        }

        array.Open(QueryType.Delete);
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var attrCondition = QueryCondition.Create(ctx, "a0", 4, QueryConditionOperatorType.Equal);
        deleteQuery.SetCondition(attrCondition);
        deleteQuery.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());

        // Write { 4, 5, 6 }
        // Ensure that the write is performed with a unique timestamp
        // + If the previous DELETE happens at the same timestamp of this WRITE the DELETE will win
        array.SetOpenTimestampEnd(nextOpen);
        using (var writeQuery = TestUtil.WriteArray(array, layout, new()
                   { { "cols", new[] { 4, 5, 6 } }, { "a0", new[] { 4, 5, 6 } } }, ctx: ctx))
        {
            Assert.AreEqual(QueryStatus.Completed, writeQuery.Status());

        }

        // Check for expected values
        readBuffers = new() { {"cols", new int[6]}, {"a0", new int[6]} };
        TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx, keepOpen: false);
        expectedData["a0"] = new[] { 1, 2, 3, 4, 5, 6 };
        expectedData["cols"] = new[] { 1, 2, 3, 4, 5, 6 };
        TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
    }

    [DataRow(LayoutType.Unordered, "legacy", true)]
    [DataRow(LayoutType.Unordered, "legacy", false)]
    [DataRow(LayoutType.Unordered, "refactored", true)]
    [DataRow(LayoutType.Unordered, "refactored", false)]
    [DataRow(LayoutType.RowMajor, "legacy", true)]
    [DataRow(LayoutType.RowMajor, "legacy", false)]
    [DataRow(LayoutType.RowMajor, "refactored", true)]
    [DataRow(LayoutType.RowMajor, "refactored", false)]
    [DataRow(LayoutType.GlobalOrder, "legacy", true)]
    [DataRow(LayoutType.GlobalOrder, "legacy", false)]
    [DataRow(LayoutType.GlobalOrder, "refactored", true)]
    [DataRow(LayoutType.GlobalOrder, "refactored", false)]
    [DataTestMethod]
    public void TestStrings(LayoutType layout, string queryReader, bool duplicates)
    {
        var arrayName = "deletes-strings-test";

        using var ctx = Context.GetDefault();
        ctx.Config().Set("sm.query.sparse_global_order.reader", queryReader);
        ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", queryReader);

        using var cols = Dimension.Create(ctx, "cols", 1, 6, 2);
        using var a1 = Attribute.Create<string>(ctx, "a1");
        using var domain = new Domain(ctx);
        domain.AddDimension(cols);
        using var schema = new ArraySchema(ctx, ArrayType.Sparse);
        schema.SetDomain(domain);
        schema.AddAttribute(a1);
        schema.SetAllowsDups(duplicates);
        schema.Check();
        if (Directory.Exists(arrayName))
        {
            Directory.Delete(arrayName, true);
        }

        Array.Create(ctx, arrayName, schema);
        using var array = new Array(ctx, arrayName);

        using var writeQuery = TestUtil.WriteArray(array, layout,
            buffers: new()
                { {"cols", new[] {1, 2, 3}}, {"a1", Encoding.UTF8.GetBytes("onetwothree")} },
            offsets: new Dictionary<string, ulong[]>() { {"a1", new ulong[] { 0, 3, 6 }} });

        var readBuffers = new Dictionary<string, System.Array>() { {"cols", new int[6]}, { "a1", new byte[11] } };
        var readOffsetBuffers = new Dictionary<string, ulong[]>() { { "a1", new ulong[3] } };
        using (var readQuery = TestUtil.ReadArray(array, layout, readBuffers, offsets: readOffsetBuffers))
        {
            array.Open(QueryType.Read);
            var bufferElements = readQuery.ResultBufferElements();
            array.Close();
            var elementCount = (int)bufferElements["a1"].Item1;
            Assert.AreEqual((ulong)elementCount, readQuery.GetResultDataElements("a1"));
            var offsetCount = (int)bufferElements["a1"].Item2!;
            Assert.AreEqual((ulong)offsetCount, readQuery.GetResultOffsets("a1"));
            var a1Data = new List<string>();
            for (int i = 0; i < offsetCount; i++)
            {
                int cellSize = i == offsetCount - 1
                    ? elementCount - (int)readOffsetBuffers["a1"][i]
                    : (int)readOffsetBuffers["a1"][i + 1] - (int)readOffsetBuffers["a1"][i];
                a1Data.Add(new (Encoding.ASCII.GetChars((byte[])readBuffers["a1"], (int)readOffsetBuffers["a1"][i],
                    cellSize)));
            }
            CollectionAssert.AreEqual(new[] {"one", "two", "three"}, a1Data);
        }

        array.Open(QueryType.Delete);
        using var deleteQuery = new Query(array, QueryType.Delete);
        using var colsCondition = QueryCondition.Create(ctx, "cols", 1, QueryConditionOperatorType.Equal);
        deleteQuery.SetCondition(colsCondition);
        deleteQuery.Submit();
        array.Close();
        Assert.AreEqual(QueryStatus.Completed, deleteQuery.Status());

        readBuffers = new Dictionary<string, System.Array>()
            { {"cols", new int[6]}, { "a1", new byte[11] } };
        readOffsetBuffers = new Dictionary<string, ulong[]>() { { "a1", new ulong[17] } };
        using (var readQuery = TestUtil.ReadArray(array, layout, readBuffers, offsets: readOffsetBuffers))
        {
            array.Open(QueryType.Read);
            var bufferElements = readQuery.ResultBufferElements();
            array.Close();

            var elementCount = (int)bufferElements["a1"].Item1;
            Assert.AreEqual((ulong)elementCount, readQuery.GetResultDataElements("a1"));
            var offsetCount = (int)bufferElements["a1"].Item2!;
            Assert.AreEqual((ulong)offsetCount, readQuery.GetResultOffsets("a1"));
            var a1Data = new List<string>();
            for (int i = 0; i < offsetCount; i++)
            {
                int cellSize = i == offsetCount - 1
                    ? elementCount - (int)readOffsetBuffers["a1"][i]
                    : (int)readOffsetBuffers["a1"][i + 1] - (int)readOffsetBuffers["a1"][i];
                a1Data.Add(new (Encoding.ASCII.GetChars((byte[])readBuffers["a1"], (int)readOffsetBuffers["a1"][i],
                    cellSize)));
            }

            Logger.LogMessage(string.Join(", ", a1Data));
            CollectionAssert.AreEqual(new[] {"two", "three"}, a1Data);
        }
    }
}
