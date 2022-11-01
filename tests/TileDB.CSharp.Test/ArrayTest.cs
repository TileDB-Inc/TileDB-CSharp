using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ArrayTest
    {
        [TestMethod]
        public void TestDenseArray()
        {
            var context = Context.GetDefault();

            var tmpArrayPath = TestUtil.MakeTestPath("dense_array_test");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            var array_schema = BuildDenseArraySchema(context);
            Assert.IsNotNull(array_schema);

            array.Create(array_schema);

            Assert.AreEqual(("file://" + tmpArrayPath).Replace('\\', '/').Replace("///","//"), array.Uri().Replace("///","//"));

            array.Open(QueryType.Read);

            array.Reopen();

            array.Close();

            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            array.SetOpenTimestampStart((ulong)(unixTimestamp / 1000000));

            array.Open(QueryType.Read);

            array_schema = array.Schema();
            Assert.IsNotNull(array_schema);

            Assert.AreEqual(LayoutType.RowMajor, array_schema.TileOrder());

            Assert.AreEqual(QueryType.Read, array.QueryType());

            (_, _, bool isEmpty) = array.NonEmptyDomain<short>("dim1");
            Assert.IsTrue(isEmpty);

            (_, _, isEmpty) = array.NonEmptyDomain<short>(0);
            Assert.IsTrue(isEmpty);

            array.Close();

            var array_schema_loaded = Array.LoadArraySchema(context, tmpArrayPath);
            Assert.IsNotNull(array_schema_loaded);
        }

        [TestMethod]
        public void TestSparseArray()
        {
            var context = Context.GetDefault();

            var tmpArrayPath = TestUtil.MakeTestPath("sparse_array_test");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            var array_schema = BuildSparseArraySchema(context);
            Assert.IsNotNull(array_schema);

            array.Create(array_schema);

            Assert.AreEqual(("file://" + tmpArrayPath).Replace('\\','/').Replace("///","//"), array.Uri().Replace("///","//"));

            array.Open(QueryType.Read);

            array.Reopen();

            array.Close();

            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            array.SetOpenTimestampStart((ulong)(unixTimestamp / 1000000));

            array.Open(QueryType.Read);

            array_schema = array.Schema();
            Assert.IsNotNull(array_schema);

            Assert.AreEqual(LayoutType.RowMajor, array_schema.TileOrder());

            Assert.AreEqual(QueryType.Read, array.QueryType());

            (_, _, bool isEmpty) = array.NonEmptyDomain<short>("dim1");
            Assert.IsTrue(isEmpty);

            (_, _, isEmpty) = array.NonEmptyDomain<short>(0);
            Assert.IsTrue(isEmpty);

            Assert.ThrowsException<ArgumentException>(()=>array.NonEmptyDomain<short>("dim2"));
            Assert.ThrowsException<ArgumentException>(()=>array.NonEmptyDomain<short>(1));

            (_, _, isEmpty) = array.NonEmptyDomain<float>("dim2");
            Assert.IsTrue(isEmpty);

            (_, _, isEmpty) = array.NonEmptyDomain<float>(1);
            Assert.IsTrue(isEmpty);

            (_, isEmpty) = array.NonEmptyDomain();
            Assert.IsTrue(isEmpty);

            array.Close();

            var array_schema_loaded = Array.LoadArraySchema(context, tmpArrayPath);
            Assert.IsNotNull(array_schema_loaded);
        }

        private ArraySchema BuildDenseArraySchema(Context context)
        {
            var bound = new short[] { 1, 10 };
            const short extent = 5;
            var dimension = Dimension.Create(context, "dim1", bound, extent);
            Assert.IsNotNull(dimension);
            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            domain.AddDimension(dimension);

            var array_schema = new ArraySchema(context, ArrayType.Dense);
            Assert.IsNotNull(array_schema);

            var a1 = new Attribute(context, "a1", DataType.Int32);
            Assert.IsNotNull(a1);

            var a2 = new Attribute(context, "a2", DataType.StringAscii);
            Assert.IsNotNull(a2);

            a2.SetCellValNum((uint)Constants.TILEDB_VAR_NUM);

            array_schema.AddAttributes(a1, a2);

            array_schema.SetDomain(domain);

            array_schema.Check();

            return array_schema;
        }

        private ArraySchema BuildSparseArraySchema(Context context)
        {
            var boundDim1 = new short[] { 1, 10 };
            const short extentDim1 = 5;
            var dim1 = Dimension.Create(context, "dim1", boundDim1, extentDim1);
            Assert.IsNotNull(dim1);

            var boundDim2 = new float[] { 1.0f, 4.0f };
            const float extentDim2 = 0.5f;
            var dim2 = Dimension.Create(context, "dim2", boundDim2, extentDim2);
            Assert.IsNotNull(dim2);

            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            domain.AddDimensions(dim1, dim2);

            var array_schema = new ArraySchema(context, ArrayType.Sparse);
            Assert.IsNotNull(array_schema);

            var a1 = new Attribute(context, "a1", DataType.Int32);
            Assert.IsNotNull(a1);

            var a2 = new Attribute(context, "a2", DataType.StringAscii);
            Assert.IsNotNull(a2);

            a2.SetCellValNum((uint)Constants.TILEDB_VAR_NUM);

            array_schema.AddAttributes(a1, a2);

            array_schema.SetDomain(domain);

            array_schema.Check();

            return array_schema;
        }
    }
}
