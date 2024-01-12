using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TileDB.CSharp.Test;

[TestClass]
public class QueryConditionTest
{
    [TestMethod]
    public void TestSimpleQueryCondition()
    {
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        using var dimension1 = Dimension.Create(context, "rows", 1, 4, 2);
        Assert.IsNotNull(dimension1);

        using var dimension2 = Dimension.Create(context, "cols", 1, 4, 2);
        Assert.IsNotNull(dimension2);

        using var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimension(dimension1);
        domain.AddDimension(dimension2);

        using var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);


        using var attr1 = new Attribute(context, "a1", DataType.Int32);
        Assert.IsNotNull(attr1);

        array_schema.AddAttribute(attr1);

        array_schema.SetDomain(domain);

        array_schema.Check();

        var tmpArrayPath = TestUtil.MakeTestPath("tiledb_test_sparse_array");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        Array.Create(context, tmpArrayPath, array_schema);

        //Write array
        using var array_write = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_write);

        array_write.Open(QueryType.Write);

        using var query_write = new Query(array_write);

        var attr1_data_buffer = new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        query_write.SetDataBuffer("a1", attr1_data_buffer);
        query_write.Submit();

        var status = query_write.Status();

        Assert.AreEqual(QueryStatus.Completed, status);

        array_write.Close();

        using var array_read = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_read);

        array_read.Open(QueryType.Read);

        using var query_read = new Query(array_read);

        query_read.SetLayout(LayoutType.RowMajor);

        using var subarray = new Subarray(array_read);
        subarray.SetSubarray(1, 4, 1, 4);
        query_read.SetSubarray(subarray);

        using QueryCondition qc1 = QueryCondition.Create(context, "a1", 3, QueryConditionOperatorType.GreaterThan);
        using QueryCondition qc2 = QueryCondition.Create(context, "a1", 7, QueryConditionOperatorType.LessThan);
        using QueryCondition qc = qc1 & qc2;

        query_read.SetCondition(qc);

        var attr_data_buffer_read = new int[16];
        query_read.SetDataBuffer("a1", attr_data_buffer_read);

        query_read.Submit();
        var status_read = query_read.Status();

        Assert.AreEqual(QueryStatus.Completed, status_read);

        array_read.Close();

        // the result query buffer for 3<x<7 should be 4,5,6
        // Assert.AreEqual<int>(4, attr_data_buffer_read[0]);
        // Assert.AreEqual<int>(5, attr_data_buffer_read[1]);
        // Assert.AreEqual<int>(6, attr_data_buffer_read[2]);
    }

    [TestMethod]
    public void TestNegateQueryCondition()
    {
        var context = Context.GetDefault();
        Assert.IsNotNull(context);

        using var dimension1 = Dimension.Create(context, "rows", 1, 4, 2);
        Assert.IsNotNull(dimension1);

        using var dimension2 = Dimension.Create(context, "cols", 1, 4, 2);
        Assert.IsNotNull(dimension2);

        using var domain = new Domain(context);
        Assert.IsNotNull(domain);

        domain.AddDimension(dimension1);
        domain.AddDimension(dimension2);

        using var array_schema = new ArraySchema(context, ArrayType.Dense);
        Assert.IsNotNull(array_schema);


        using var attr1 = new Attribute(context, "a1", DataType.Int32);
        Assert.IsNotNull(attr1);

        array_schema.AddAttribute(attr1);

        array_schema.SetDomain(domain);

        array_schema.Check();

        var tmpArrayPath = TestUtil.MakeTestPath("tiledb_test_sparse_array");

        if (Directory.Exists(tmpArrayPath))
        {
            Directory.Delete(tmpArrayPath, true);
        }

        Array.Create(context, tmpArrayPath, array_schema);

        //Write array
        using var array_write = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_write);

        array_write.Open(QueryType.Write);

        using var query_write = new Query(array_write);

        var attr1_data_buffer = new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        query_write.SetDataBuffer("a1", attr1_data_buffer);
        query_write.Submit();

        var status = query_write.Status();

        Assert.AreEqual(QueryStatus.Completed, status);

        array_write.Close();

        using var array_read = new Array(context, tmpArrayPath);
        Assert.IsNotNull(array_read);

        array_read.Open(QueryType.Read);

        using var query_read = new Query(array_read);

        query_read.SetLayout(LayoutType.RowMajor);

        using var subarray = new Subarray(array_read);
        subarray.SetSubarray(1, 4, 1, 4);
        query_read.SetSubarray(subarray);

        using QueryCondition qc1 = QueryCondition.Create(context, "a1", 3, QueryConditionOperatorType.GreaterThan);
        using QueryCondition qc2 = QueryCondition.Create(context, "a1", 7, QueryConditionOperatorType.LessThan);
        using QueryCondition qc3 = qc1 & qc2;
        using QueryCondition qc = !qc3;

        query_read.SetCondition(qc);

        var attr_data_buffer_read = new int[16];
        query_read.SetDataBuffer("a1", attr_data_buffer_read);

        query_read.Submit();
        var status_read = query_read.Status();

        Assert.AreEqual(QueryStatus.Completed, status_read);

        array_read.Close();

        // the result query buffer for !(3<x<7) should be 1,2,3,<FILL>,<FILL>,<FILL>7,8,9,10,11,12,13,14,15,16
        Assert.AreEqual(int.MinValue, attr_data_buffer_read[3]);
        Assert.AreEqual(int.MinValue, attr_data_buffer_read[4]);
        Assert.AreEqual(int.MinValue, attr_data_buffer_read[5]);
    }

    [TestMethod]
    public void TestIsNotNullCondition()
    {
        var context = Context.GetDefault();

        using var dimension = Dimension.Create(context, "dim", 1, 5, 5);

        using var domain = new Domain(context);

        domain.AddDimension(dimension);

        using var array_schema = new ArraySchema(context, ArrayType.Sparse);

        using var attr = new Attribute(context, "a1", DataType.Int32);
        attr.SetNullable(true);

        array_schema.AddAttribute(attr);

        array_schema.SetDomain(domain);

        array_schema.Check();

        using var tmpArrayPath = new TemporaryDirectory("query_condition_is_not_null");

        Array.Create(context, tmpArrayPath, array_schema);

        //Write array
        using var array_write = new Array(context, tmpArrayPath);

        array_write.Open(QueryType.Write);

        using var query_write = new Query(array_write);

        var attr_data_buffer = new int[] { 1, 2, 3, 4, 5 };
        var attr_validity_buffer = new byte[] { 1, 1, 0, 1, 1 };
        query_write.SetDataBuffer("dim", attr_data_buffer);
        query_write.SetDataBuffer("a1", attr_data_buffer);
        query_write.SetValidityBuffer("a1", attr_validity_buffer);
        query_write.Submit();

        var status = query_write.Status();

        Assert.AreEqual(QueryStatus.Completed, status);

        array_write.Close();

        using var array_read = new Array(context, tmpArrayPath);

        array_read.Open(QueryType.Read);

        using var query_read = new Query(array_read);

        query_read.SetLayout(LayoutType.RowMajor);

        using QueryCondition qc = QueryCondition.CreateIsNotNull(context, "a1");

        query_read.SetCondition(qc);

        var attr_data_buffer_read = new int[5];
        query_read.SetDataBuffer("dim", attr_data_buffer);
        query_read.SetDataBuffer("a1", attr_data_buffer_read);
        query_read.SetValidityBuffer("a1", new byte[5]);

        query_read.Submit();
        var status_read = query_read.Status();

        Assert.AreEqual(QueryStatus.Completed, status_read);

        Assert.AreEqual(4UL, query_read.GetResultDataElements("a1"));
        CollectionAssert.AreEqual(new int[] { 1, 2, 4, 5, 0 }, attr_data_buffer_read);

        array_read.Close();
    }

    [TestMethod]
    public void TestCombineDifferentContexts()
    {
        using var context1 = new Context();
        using var context2 = new Context();

        using var qc1 = QueryCondition.Create(context1, "a1", 5, QueryConditionOperatorType.GreaterThan);
        using var qc2 = QueryCondition.Create(context2, "a1", 8, QueryConditionOperatorType.LessThan);

        Assert.ThrowsException<InvalidOperationException>(() => qc1 | qc2);
    }
}
