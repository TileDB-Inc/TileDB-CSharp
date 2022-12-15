using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TileDB.CSharp.Test
{
    [TestClass]
    public class QueryConditionTest
    {
        [TestMethod]
        public void TestSimpleQueryCondition()
        {
            var context = Context.GetDefault();
            Assert.IsNotNull(context);

            var dimension1 = Dimension.Create(context, "rows", 1, 4, 2);
            Assert.IsNotNull(dimension1);

            var dimension2 = Dimension.Create(context, "cols", 1, 4, 2);
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

            using var query_write = new Query(context, array_write);

            var attr1_data_buffer = new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            query_write.SetDataBuffer("a1", attr1_data_buffer);
            query_write.Submit();

            var status = query_write.Status();

            Assert.AreEqual(QueryStatus.Completed, status);

            array_write.Close();

            using var array_read = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.Read);

            using var query_read = new Query(context, array_read);

            query_read.SetLayout(LayoutType.RowMajor);

            int[] subarray = new int[4] { 1, 4, 1, 4 };
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
    }
}
