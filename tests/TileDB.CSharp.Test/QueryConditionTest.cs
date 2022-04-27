using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB.CSharp;
using System.Runtime.InteropServices;
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

            var bound1 = new int[] { 1, 4 };
            const int extent1 = 2;
            var dimension1 = Dimension.Create<int>(context, "rows", bound1, extent1);
            Assert.IsNotNull(dimension1);

            var bound2 = new int[] { 1, 4 };
            const int extent2 = 2;
            var dimension2 = Dimension.Create<int>(context, "cols", bound2, extent2);
            Assert.IsNotNull(dimension2);

            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            domain.AddDimension(dimension1);
            domain.AddDimension(dimension2);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
            Assert.IsNotNull(array_schema);


            var attr1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
            Assert.IsNotNull(attr1);

            array_schema.AddAttribute(attr1);

            array_schema.SetDomain(domain);

            array_schema.Check();

            var tmpArrayPath = Path.Join(Path.GetTempPath(), "tiledb_test_sparse_array");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            Array.Create(context, tmpArrayPath, array_schema);

            //Write array
            var array_write = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_write);

            array_write.Open(QueryType.TILEDB_WRITE);

            var query_write = new Query(context, array_write);

            var attr1_data_buffer = new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            query_write.SetDataBuffer<int>("a1", attr1_data_buffer);
            query_write.Submit();

            var status = query_write.Status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status);

            array_write.Close();

            var array_read = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array_read);

            array_read.Open(QueryType.TILEDB_READ);

            var query_read = new Query(context, array_read);

            query_read.SetLayout(LayoutType.TILEDB_ROW_MAJOR);

            int[] subarray = new int[4] { 1, 4, 1, 4 };
            query_read.SetSubarray<int>(subarray);

            QueryCondition qc1 = QueryCondition.Create<int>(context, "a1", 3, QueryConditionOperatorType.TILEDB_GT);
            QueryCondition qc2 = QueryCondition.Create<int>(context, "a1", 7, QueryConditionOperatorType.TILEDB_LT);
            var qc = qc1.Combine(qc2, QueryConditionCombinationOperatorType.TILEDB_AND);

            query_read.SetCondition(qc);

            var attr_data_buffer_read = new int[16];
            query_read.SetDataBuffer<int>("a1", attr_data_buffer_read);

            query_read.Submit();
            var status_read = query_read.Status();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, status_read);

            array_read.Close();

            // the result query buffer for 3<x<7 should be 4,5,6
            // Assert.AreEqual<int>(4, attr_data_buffer_read[0]);
            // Assert.AreEqual<int>(5, attr_data_buffer_read[1]);
            // Assert.AreEqual<int>(6, attr_data_buffer_read[2]);


        }//public void TestSimpleQueryCondition()

    }//class

}//namespace