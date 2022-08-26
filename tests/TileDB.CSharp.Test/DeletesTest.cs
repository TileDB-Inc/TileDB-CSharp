using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO: Assertions
// TODO: Implement remaining tests:
// + Multiple fragments
// + Delete after schema evolution
// + Write fragment and delete at same timestamp
// + Consolidation
// + Purging
namespace TileDB.CSharp.Test
{
    [TestClass]
    public class DeletesTest
    {
        [TestInitialize]
        public void InitTests() { }

        [DataRow(LayoutType.TILEDB_UNORDERED)]    // Pass
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER)] // Pass
        // [DataRow(LayoutType.TILEDB_ROW_MAJOR)] // Assertion `array_schema_.dense()' failed
        [DataTestMethod]
        public void MultipleCells(LayoutType layout)
        {
            var ctx = Context.GetDefault();
            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);

            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);

            // Deletes are only supported for sparse arrays
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.Check();

            string arrayName = "deletes-multiple-cells";
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            TestUtil.ReadTestArray(schema, layout, arrayName);

            // Delete multiple cells at once
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var queryCondition = new QueryCondition(ctx);
            queryCondition.Init("cols", 2, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(queryCondition);
            deleteQuery.Submit();
            Console.WriteLine($"Delete multiple cells status: {deleteQuery.Status()}");
            array.Close();

            TestUtil.ReadTestArray(schema, layout, arrayName);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED)]    // Pass
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER)] // Pass
        // [DataRow(LayoutType.TILEDB_ROW_MAJOR)] // Assertion `array_schema_.dense()' failed
        [DataTestMethod]
        public void SingleCell(LayoutType layout)
        {
            // TODO: Separate this into initializer?
            var ctx = Context.GetDefault();
            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);

            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);

            // Deletes are only supported for sparse arrays
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.Check();

            string arrayName = "deletes-single-cell";
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            TestUtil.ReadTestArray(schema, layout, arrayName);

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
            Console.WriteLine($"Delete single cells status: {deleteQuery.Status()}");
            TestUtil.ReadTestArray(schema, layout, arrayName);

            TestUtil.ReadTestArray(schema, layout, arrayName);
        }

        [DataRow(LayoutType.TILEDB_UNORDERED)]    // Pass
        [DataRow(LayoutType.TILEDB_GLOBAL_ORDER)] // Pass
        // [DataRow(LayoutType.TILEDB_ROW_MAJOR)] // Assertion `array_schema_.dense()' failed
        [DataTestMethod]
        public void VaryingCells(LayoutType layout)
        {
            var ctx = Context.GetDefault();
            var rows = Dimension.Create(ctx, "rows", new[] { 1, 4 }, 4);
            var cols = Dimension.Create(ctx, "cols", new[] { 1, 4 }, 4);

            var domain = new Domain(ctx);
            domain.AddDimensions(rows, cols);

            // Deletes are only supported for sparse arrays
            var arrayType = ArrayType.TILEDB_SPARSE;
            var schema = new ArraySchema(ctx, arrayType);
            schema.SetDomain(domain);
            schema.AddAttribute(new Attribute(ctx, "a1", DataType.TILEDB_INT32));
            schema.Check();

            string arrayName = "deletes-varying-cells";
            var array = new Array(ctx, arrayName);
            TestUtil.CreateTestArray(schema, layout, arrayName);
            TestUtil.ReadTestArray(schema, layout, arrayName);

            // Delete 4 cells at once
            array.Open(QueryType.TILEDB_DELETE);
            var deleteQuery = new Query(ctx, array, QueryType.TILEDB_DELETE);
            var colsCondition = new QueryCondition(ctx);
            colsCondition.Init("cols", 2, QueryConditionOperatorType.TILEDB_EQ);
            deleteQuery.SetCondition(colsCondition);
            deleteQuery.Submit();
            Console.WriteLine($"Delete 4 cells status: {deleteQuery.Status()}");
            array.Close();
            TestUtil.ReadTestArray(schema, layout, arrayName);

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
            Console.WriteLine($"Delete 1 cells status: {deleteQuery.Status()}");
            TestUtil.ReadTestArray(schema, layout, arrayName);

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
            Console.WriteLine($"Delete 8 cells status: {deleteQuery.Status()}");
            TestUtil.ReadTestArray(schema, layout, arrayName);
        }
    }
}