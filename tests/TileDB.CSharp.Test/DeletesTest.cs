using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO: Normalize paths
// + Consolidation (after every test)
// + Purging
namespace TileDB.CSharp.Test
{
    [TestClass]
    public class DeletesTest
    {
        [TestInitialize]
        public void InitTests()
        {
            // Console.WriteLine(Assembly.GetAssembly(typeof(TileDB.CSharp.Array))!.FullName);
            // Console.WriteLine($"TileDB Core version: {TileDB.CSharp.CoreUtil.GetCoreLibVersion()}");
        }

        // Params for all tests are (layout, legacy, duplicates)
        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void MultipleCells(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            string arrayName = "deletes-multiple-cells";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);
            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);
            // Deletes are only supported for sparse arrays
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.SetAllowsDups(duplicates);
            schema.Check();
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            var readBuffers = new Dictionary<string, dynamic>
                { {"rows", new int[16]}, {"cols", new int[16]}, {"a1", new int[16]} };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a1", Enumerable.Range(1, 16).ToArray() },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete multiple cells at once
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var queryCondition = new QueryCondition(ctx);
            queryCondition.Init("cols", 2, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(queryCondition);
            deleteQuery.Submit();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete multiple cells status: {deleteQuery.Status()}");
            array.Close();

            // Check for expected values
            readBuffers = new()
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            expectedData["a1"] = new[] { 1, 3, 4, 5, 7, 8, 9, 11, 12, 13, 15, 16, 0, 0, 0, 0};
            expectedData["rows"] = new[] { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 0, 0, 0, 0 };
            expectedData["cols"] = new[] { 1, 3, 4, 1, 3, 4, 1, 3, 4, 1, 3, 4, 0, 0, 0, 0 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void SingleCell(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            string arrayName = "deletes-single-cell";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);
            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.SetAllowsDups(duplicates);
            schema.Check();
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            var readBuffers = new Dictionary<string, dynamic>
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a1", Enumerable.Range(1, 16).ToArray() },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);


            // Delete one cell
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 1, QueryConditionOperatorType.TILEDB_EQ);
            var rowsCondition = new QueryCondition(ctx);
            rowsCondition.Init("rows", 1, QueryConditionOperatorType.TILEDB_EQ);
            var andCondition = colsCondition.Combine(rowsCondition, QueryConditionCombinationOperatorType.TILEDB_AND);
            deleteQuery.SetCondition(andCondition);
            deleteQuery.Submit();
            array.Close();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete single cells status: {deleteQuery.Status()}");

            // Check for expected values
            readBuffers = new()
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

            expectedData["a1"] = new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 };
            expectedData["rows"] = new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 };
            expectedData["cols"] = new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void VaryingCells(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            string arrayName = "deletes-varying-cells";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);
            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.SetAllowsDups(duplicates);
            schema.Check();
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            var readBuffers = new Dictionary<string, dynamic>
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a1", Enumerable.Range(1, 16).ToArray() },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete 4 cells at once
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 2, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(colsCondition);
            deleteQuery.Submit();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete 4 cells status: {deleteQuery.Status()}");
            array.Close();
            readBuffers = new()
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

            expectedData["a1"] = new[] { 1, 3, 4, 5, 7, 8, 9, 11, 12, 13, 15, 16, 0, 0, 0, 0 };
            expectedData["cols"] = new[] { 1, 3, 4, 1, 3, 4, 1, 3, 4, 1, 3, 4, 0, 0, 0, 0 };
            expectedData["rows"] = new[] { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 0, 0, 0, 0 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete 1 cell
            array.Open(QueryType.TILEDB_DELETE);
            deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 1, QueryConditionOperatorType.TILEDB_EQ);
            var rowsCondition = new QueryCondition(ctx);
            rowsCondition.Init("rows", 1, QueryConditionOperatorType.TILEDB_EQ);
            var andCondition = colsCondition.Combine(rowsCondition, QueryConditionCombinationOperatorType.TILEDB_AND);
            deleteQuery.SetCondition(andCondition);
            deleteQuery.Submit();
            array.Close();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete 1 cells status: {deleteQuery.Status()}");
            readBuffers = new()
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            expectedData["a1"] = new[] { 3, 4, 5, 7, 8, 9, 11, 12, 13, 15, 16, 0, 0, 0, 0, 0 };
            expectedData["cols"] = new[] { 3, 4, 1, 3, 4, 1, 3, 4, 1, 3, 4, 0, 0, 0, 0, 0 };
            expectedData["rows"] = new[] { 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 0, 0, 0, 0, 0 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete 6 cells
            // + The OR condition overlaps on (3, 4)
            // + (3, 2) was deleted by a previous delete query
            array.Open(QueryType.TILEDB_DELETE);
            deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 4, QueryConditionOperatorType.TILEDB_EQ);
            rowsCondition = new QueryCondition(ctx);
            rowsCondition.Init("rows", 3, QueryConditionOperatorType.TILEDB_EQ);
            var orCondition = colsCondition.Combine(rowsCondition, QueryConditionCombinationOperatorType.TILEDB_OR);
            deleteQuery.SetCondition(orCondition);
            deleteQuery.Submit();
            array.Close();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete 8 cells status: {deleteQuery.Status()}");

            // Check for expected values
            readBuffers = new()
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            expectedData["a1"] = new[] { 3, 5, 7, 13, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            expectedData["rows"] = new[] { 1, 2, 2, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            expectedData["cols"] = new[] { 3, 1, 3, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void MultipleFragments(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            string arrayName = "deletes-multiple-fragments";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);
            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.SetAllowsDups(duplicates);
            schema.Check();
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            var readBuffers = new Dictionary<string, dynamic>
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a1", Enumerable.Range(1, 16).ToArray() },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete single cell
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var rowsCondition = new QueryCondition(ctx);
            rowsCondition.Init("rows", 1, QueryConditionOperatorType.TILEDB_EQ);
            var colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 1, QueryConditionOperatorType.TILEDB_EQ);
            var andCondition = rowsCondition.Combine(colsCondition, QueryConditionCombinationOperatorType.TILEDB_AND);
            deleteQuery.SetCondition(andCondition);
            deleteQuery.Submit();
            array.Close();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);

            readBuffers = new()
                { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
            var readQuery = TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            array.Open(QueryType.TILEDB_READ);
            Assert.AreEqual(15UL, readQuery.ResultBufferElements()["a1"].Item1);
            array.Close();
            expectedData["a1"] = new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 };
            expectedData["rows"] = new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 };
            expectedData["cols"] = new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Re-add; If duplicates are enabled result buffer size will grow
            var writeQuery = TestUtil.WriteArray(array, layout, new()
                { {"rows", new[] {1}}, {"cols", new[] {1}}, {"a1", new[] {17}} }, ctx: ctx);
            Assert.AreEqual(writeQuery.Status(), QueryStatus.TILEDB_COMPLETED);

            // Scale buffers to fit extra value if duplicates enabled
            if (duplicates)
            {
                readBuffers = new()
                    { { "rows", new int[17] }, { "cols", new int[17] }, { "a1", new int[17] } };

                if (layout == LayoutType.TILEDB_UNORDERED)
                {
                    expectedData["a1"] = new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 0 };
                    expectedData["rows"] = new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 1, 0 };
                    expectedData["cols"] = new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 0 };
                }
                else
                {
                    expectedData["a1"] = new[] { 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 };
                    expectedData["rows"] = new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 };
                    expectedData["cols"] = new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 };
                }
            }
            else
            {
                readBuffers = new()
                    { { "rows", new int[16] }, { "cols", new int[16] }, { "a1", new int[16] } };
                expectedData["a1"] = new[] { 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
                expectedData["rows"] = new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
                expectedData["cols"] = new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 };
            }
            readQuery = TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

            array.Open(QueryType.TILEDB_READ);
            // For duplicates we allocate for 17 values including deleted cell, but we only retrieve 16 valid results
            Assert.AreEqual(16UL, readQuery.ResultBufferElements()["a1"].Item1);
            array.Close();

            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void SchemaEvolution(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            string arrayName = "deletes-schema-evolution";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            // Create array
            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);
            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.SetAllowsDups(duplicates);
            schema.Check();
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);

            // We will increment this value according to writes if duplicates are enabled
            int bufferSize = 16;
            var readBuffers = new Dictionary<string, dynamic>()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Evolve schema; Write to evolved array; Read and output array attributes
            var schemaEvolution = new ArraySchemaEvolution(ctx);
            var a2 = new Attribute(ctx, "a2", DataType.TILEDB_FLOAT64);
            a2.SetFillValue(-1.0);
            schemaEvolution.AddAttribute(a2);
            array.Evolve(ctx, schemaEvolution);
            Console.WriteLine($"\nEvolved array schema: ");
            TestUtil.PrintLocalSchema(arrayName);
            array.Open(QueryType.TILEDB_READ);
            Assert.IsTrue(array.Schema().HasAttribute("a2"));
            array.Close();

            // Write to evolved array
            var writeData = new Dictionary<string, dynamic>()
            {
                { "a1", new[] { 1 } },
                { "a2", new[] { 1.0 } },
                { "rows", new[] { 1 } },
                { "cols", new[] { 1 } },
            };
            var writeQuery = TestUtil.WriteArray(array, layout, writeData, ctx: ctx);
            Assert.AreEqual(writeQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Write query status: {writeQuery.Status()}");
            bufferSize = duplicates ? ++bufferSize : bufferSize;
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
                    { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };

            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
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
                expectedData = new Dictionary<string, dynamic>()
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
            writeData = new Dictionary<string, dynamic>()
            {
                { "a1", new[] { 1 } },
                { "a2", new[] { 1.0 } },
                { "rows", new[] { 1 } },
                { "cols", new[] { 1 } },
            };
            bufferSize = duplicates ? ++bufferSize : bufferSize;
            writeQuery = TestUtil.WriteArray(array, layout, writeData, ctx: ctx);
            Assert.AreEqual(writeQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Write query status: {writeQuery.Status()}");
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
                    { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };

            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
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
                expectedData = new Dictionary<string, dynamic>()
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
            var delArray = new Array(ctx, arrayName);
            delArray.SetOpenTimestampStart(deleteTime);
            delArray.SetOpenTimestampEnd(deleteTime);
            delArray.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, delArray, QueryType.TILEDB_DELETE);
            var rowsCondition = new QueryCondition(ctx);
            rowsCondition.Init("rows", 1, QueryConditionOperatorType.TILEDB_EQ);
            var colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 1, QueryConditionOperatorType.TILEDB_EQ);
            var andCondition = rowsCondition.Combine(colsCondition, QueryConditionCombinationOperatorType.TILEDB_AND);
            deleteQuery.SetCondition(andCondition);
            deleteQuery.Submit();
            Console.WriteLine($"Delete timestamp: {deleteTime}");
            delArray.Close();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete query status: {deleteQuery.Status()}");

            // Read before the timestamp of the delete
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
                    { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime-1), ctx: ctx);
            Console.WriteLine("\nArray set to open before delete");
            // We opened pre-delete so expectedData should not change
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Read after the timestamp of the delete
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] },
                    { "a1", new int[bufferSize] }, { "a2", new double[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime), ctx: ctx);

            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
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
                expectedData = new Dictionary<string, dynamic>()
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
            schemaEvolution = new ArraySchemaEvolution(ctx);
            schemaEvolution.DropAttribute("a2");
            array.Evolve(ctx, schemaEvolution);
            Console.WriteLine($"\nSchema after removing attribute (a2)");
            TestUtil.PrintLocalSchema(array.Uri());
            array.Open(QueryType.TILEDB_READ);
            Assert.IsFalse(array.Schema().HasAttribute("a2"));
            array.Close();

            // Read remaining attribute before delete and evolution
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime-1), ctx: ctx);
            Assert.IsTrue(expectedData.Remove("a2"));

            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 1, 1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                    { "rows", new[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                    { "cols", new[] { 1, 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
                };
            }
            else
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                    { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                    { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
                };
            }
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, timestampRange: (0, deleteTime), ctx: ctx);

            // Read remaining attribute after delete
            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0 } },
                    { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0 } },
                    { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0 } },
                };
            }
            else
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                    { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0} },
                    { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
                };
            }
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Write some cells
            writeQuery = TestUtil.WriteArray(array, layout, new()
                { {"rows", new[] {2}}, {"cols", new[] {1}}, {"a1", new[] {18}} }, ctx: ctx);
            bufferSize = duplicates ? ++bufferSize : bufferSize;
            Console.WriteLine($"Write query status: {writeQuery.Status()}");
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 2, 3, 4, 5, 18, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0 } },
                    { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0 } },
                    { "cols", new[] { 2, 3, 4, 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0 } },
                };
            }
            else
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 2, 3, 4, 18, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                    { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0} },
                    { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
                };
            }
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Make a delete condition on an attribute
            array.Open(QueryType.TILEDB_DELETE);
            deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var attrCondition = new QueryCondition(ctx);
            attrCondition.Init("a1", 10, QueryConditionOperatorType.TILEDB_LT);
            deleteQuery.SetCondition(attrCondition);
            deleteQuery.Submit();
            array.Close();
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            Console.WriteLine($"Delete query status: {deleteQuery.Status()}");

            // Check for expected values
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);

            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 18, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
                    { "rows", new[] {  2, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
                    { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
                };
            }
            else
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 18, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0, 0, 0, 0, 0, 0 } },
                    { "rows", new[] {  2, 3, 3, 3, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0 } },
                    { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0 } },
                };
            }
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void SameFragment(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            string arrayName = "deletes-same-fragment";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);
            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.SetAllowsDups(duplicates);
            schema.Check();
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);

            // For tests with duplicates we will increment bufferSize with write queries
            int bufferSize = 16;
            var readBuffers = new Dictionary<string, dynamic>()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Write a value to the array, creating a second fragment; Get fragment timestamp from writeQuery
            bufferSize = duplicates ? ++bufferSize : bufferSize;
            var writeQuery = TestUtil.WriteArray(array, layout, new()
                { {"rows", new[] { 1 }}, {"cols", new[] { 1 }}, {"a1", new[] { 17 }}, }, ctx: ctx);
            Assert.AreEqual(1U, writeQuery.FragmentNum());
            var writeTime = writeQuery.FragmentTimestampRange(0);
            Console.WriteLine($"Second fragment timestamp range: {writeTime}");
            // Apply same write operation to local array used to check expected attribute values
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            expectedData["a1"][0] = 17; // If dups are disabled, we can just overwrite the first attr value
            if (duplicates)
            {
                if (layout == LayoutType.TILEDB_UNORDERED)
                {
                    expectedData = new Dictionary<string, dynamic>()
                    {
                        { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 } },
                        { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 1 } },
                        { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1 } },
                    };
                }
                else
                {
                    expectedData = new Dictionary<string, dynamic>()
                    {
                        { "a1", new[] { 1, 17, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 } },
                        { "rows", new[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 } },
                        { "cols", new[] { 1, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 } },
                    };
                }

            }
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete the new value from the array, opening at previous write fragment timestamp
            array.SetOpenTimestampStart(ulong.MinValue);
            array.SetOpenTimestampEnd(writeTime.Item1);
            array.Open(QueryType.TILEDB_DELETE);
            Console.WriteLine("Array opened at (start / end): " +
                              $"{array.OpenTimestampStart()} / {array.OpenTimestampEnd()}");
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var attrCondition = new QueryCondition(ctx);
            attrCondition.Init("a1", 17, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(attrCondition);
            deleteQuery.Submit();
            array.Close();
            Console.WriteLine($"Delete query status: {deleteQuery.Status()}");
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);

            // Check for expected values
            readBuffers = new()
                { { "rows", new int[bufferSize] }, { "cols", new int[bufferSize] }, { "a1", new int[bufferSize] } };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            if (duplicates)
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                    { "rows", new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 } },
                    { "cols", new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
                };
            }
            else
            {
                expectedData = new Dictionary<string, dynamic>()
                {
                    { "a1", new[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0 } },
                    { "rows", new[] { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 0 } },
                    { "cols", new[] { 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 0 } },
                };
            }

            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
        }

        // Intermittent failure across all tests below
        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void SequenceTest(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            var arrayName = "deletes-sequence-test";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var cols = Dimension.Create(ctx, "cols", 1, 6, 2);
            var a0 = Attribute.Create<int>(ctx, "a0");
            var domain = new Domain(ctx);
            domain.AddDimension(cols);
            var schema = new ArraySchema(ctx, ArrayType.TILEDB_SPARSE);
            schema.SetDomain(domain);
            schema.AddAttribute(a0);
            schema.SetAllowsDups(duplicates);
            schema.Check();
            if (Directory.Exists(arrayName))
            {
                Directory.Delete(arrayName, true);
            }

            Array.Create(ctx, arrayName, schema);
            var array = new Array(ctx, arrayName);

            // Write { 1, 2, 3 }
            var writeQuery = TestUtil.WriteArray(array, layout, new()
                { {"cols", new[] { 1, 2, 3 }}, {"a0", new[] { 1, 2, 3 }}}, ctx: ctx);
            Console.WriteLine($"Write query status: {writeQuery.Status()}");
            Assert.AreEqual(writeQuery.Status(), QueryStatus.TILEDB_COMPLETED);
            var readBuffers = new Dictionary<string, dynamic>
                { {"cols", new int[6]}, {"a0", new int[6]} };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            var expectedData = new Dictionary<string, dynamic>()
            {
                { "a0", new[] { 1, 2, 3, 0, 0, 0 } },
                { "cols", new[] { 1, 2, 3, 0, 0, 0 } },
            };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);

            // Delete[4]
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var attrCondition = new QueryCondition(ctx);
            attrCondition.Init("a0", 4, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(attrCondition);
            deleteQuery.Submit();
            array.Close();
            Console.WriteLine($"Delete query status: {deleteQuery.Status()}");
            Assert.AreEqual(deleteQuery.Status(), QueryStatus.TILEDB_COMPLETED);

            // Write { 4, 5, 6 }
            writeQuery = TestUtil.WriteArray(array, layout, new()
                { {"cols", new[] { 4, 5, 6 }}, {"a0", new[] { 4, 5, 6 }} }, ctx: ctx);
            Console.WriteLine($"Write query status: {writeQuery.Status()}");
            Assert.AreEqual(writeQuery.Status(), QueryStatus.TILEDB_COMPLETED);

            // Check for expected values
            readBuffers = new() { {"cols", new int[6]}, {"a0", new int[6]} };
            TestUtil.ReadArray(array, layout, readBuffers, ctx: ctx);
            expectedData["a0"] = new[] { 1, 2, 3, 4, 5, 6 };
            expectedData["cols"] = new[] { 1, 2, 3, 4, 5, 6 };
            TestUtil.CompareBuffers(expectedData, readBuffers, duplicates);
            Thread.Sleep(700);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED, true, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, true, false)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, true)]
        [DataRow(LayoutType.TILEDB_UNORDERED, false, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, true, false)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, true)]
        [DataRow(LayoutType.TILEDB_ROW_MAJOR, false, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, true, false)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, true)]
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER, false, false)]
        [DataTestMethod]
        public void TestStrings(LayoutType layout, bool legacy, bool duplicates)
        {
            if (!TestUtil.VersionMinimum(2, 12))
            {
                return;
            }
            var arrayName = "deletes-strings-test";

            var ctx = Context.GetDefault();
            ctx.Config().Set("sm.query.sparse_global_order.reader", legacy ? "legacy" : "refactored");
            ctx.Config().Set("sm.query.sparse_unordered_with_dups.reader", legacy ? "legacy" : "refactored");

            var cols = Dimension.Create(ctx, "cols", 1, 6, 2);
            var a1 = Attribute.Create<string>(ctx, "a1");
            var domain = new Domain(ctx);
            domain.AddDimension(cols);
            var schema = new ArraySchema(ctx, ArrayType.TILEDB_SPARSE);
            schema.SetDomain(domain);
            schema.AddAttribute(a1);
            schema.SetAllowsDups(duplicates);
            schema.Check();
            if (Directory.Exists(arrayName))
            {
                Directory.Delete(arrayName, true);
            }

            Array.Create(ctx, arrayName, schema);
            var array = new Array(ctx, arrayName);

            var writeQuery = TestUtil.WriteArray(array, layout,
                buffers: new()
                    { {"cols", new[] {1, 2, 3}}, {"a1", Encoding.UTF8.GetBytes("onetwothree")} },
                offsets: new Dictionary<string, ulong[]>() { {"a1", new ulong[] { 0, 3, 6 }} });

            var readBuffers = new Dictionary<string, dynamic>()
                { {"cols", new int[6]}, { "a1", new byte[11] }, { "a1Offsets", new ulong[3]}};
            var readQuery = TestUtil.ReadArray(array, layout, readBuffers);

            array.Open(QueryType.TILEDB_READ);
            var bufferElements = readQuery.ResultBufferElements();
            array.Close();

            var elementCount = (int)bufferElements["a1"].Item1;
            var offsetCount = (int)bufferElements["a1"].Item2!;
            var a1Data = new List<string>();
            for (int i = 0; i < offsetCount; i++)
            {
                int cellSize = i == offsetCount - 1
                    ? elementCount - (int)readBuffers["a1Offsets"][i]
                    : (int)readBuffers["a1Offsets"][i + 1] - (int)readBuffers["a1Offsets"][i];
                a1Data.Add(new (Encoding.ASCII.GetChars(readBuffers["a1"], (int)readBuffers["a1Offsets"][i],
                    cellSize)));
            }
            CollectionAssert.AreEqual(new[] {"one", "two", "three"}, a1Data);

            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 1, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(colsCondition);
            deleteQuery.Submit();
            array.Close();
            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, deleteQuery.Status());

            readBuffers = new Dictionary<string, dynamic>()
                { {"cols", new int[6]}, { "a1", new byte[11] }, { "a1Offsets", new ulong[3]}};
            readQuery = TestUtil.ReadArray(array, layout, readBuffers);

            array.Open(QueryType.TILEDB_READ);
            bufferElements = readQuery.ResultBufferElements();
            array.Close();

            elementCount = (int)bufferElements["a1"].Item1;
            offsetCount = (int)bufferElements["a1"].Item2!;
            a1Data = new List<string>();
            for (int i = 0; i < offsetCount; i++)
            {
                int cellSize = i == offsetCount - 1
                    ? elementCount - (int)readBuffers["a1Offsets"][i]
                    : (int)readBuffers["a1Offsets"][i + 1] - (int)readBuffers["a1Offsets"][i];
                a1Data.Add(new (Encoding.ASCII.GetChars(readBuffers["a1"], (int)readBuffers["a1Offsets"][i],
                    cellSize)));
            }

            Console.WriteLine(string.Join(", ", a1Data));
            CollectionAssert.AreEqual(new[] {"two", "three"}, a1Data);
        }
    }
}
