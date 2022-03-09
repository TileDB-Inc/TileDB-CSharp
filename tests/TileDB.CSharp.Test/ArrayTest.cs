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

            var tmpArrayPath = Path.Join( Path.GetTempPath(), "dense_array_test");

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

            array.Open(QueryType.TILEDB_READ);

            array.Reopen();

            array.Close();

            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            array.set_open_timestamp_start((ulong)(unixTimestamp / 1000000));

            array.Open(QueryType.TILEDB_READ);

            array_schema = array.Schema();
            Assert.IsNotNull(array_schema);

            Assert.AreEqual(LayoutType.TILEDB_ROW_MAJOR, array_schema.tile_order());

            Assert.AreEqual(QueryType.TILEDB_READ, array.query_type());

            (_, _, bool isEmpty) = array.non_empty_domain<short>("dim1");
            Assert.IsTrue(isEmpty);

            (_, _, isEmpty) = array.non_empty_domain<short>(0);
            Assert.IsTrue(isEmpty);

            array.Close();

            var array_schema_loaded = Array.load_array_schema(context, tmpArrayPath);
            Assert.IsNotNull(array_schema_loaded);
        }

        [TestMethod]
        public void TestSparseArray()
        {
            var context = Context.GetDefault();

            var tmpArrayPath = Path.Join( Path.GetTempPath(), "sparse_array_test");

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

            array.Open(QueryType.TILEDB_READ);

            array.Reopen();

            array.Close();

            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            array.set_open_timestamp_start((ulong)(unixTimestamp / 1000000));

            array.Open(QueryType.TILEDB_READ);

            array_schema = array.Schema();
            Assert.IsNotNull(array_schema);

            Assert.AreEqual(LayoutType.TILEDB_ROW_MAJOR, array_schema.tile_order());

            Assert.AreEqual(QueryType.TILEDB_READ, array.query_type());

            (_, _, bool isEmpty) = array.non_empty_domain<short>("dim1");
            Assert.IsTrue(isEmpty);

            (_, _, isEmpty) = array.non_empty_domain<short>(0);
            Assert.IsTrue(isEmpty);

            Assert.ThrowsException<ArgumentException>(()=>array.non_empty_domain<short>("dim2"));
            Assert.ThrowsException<ArgumentException>(()=>array.non_empty_domain<short>(1));
            
            (_, _, isEmpty) = array.non_empty_domain<float>("dim2");
            Assert.IsTrue(isEmpty);

            (_, _, isEmpty) = array.non_empty_domain<float>(1);
            Assert.IsTrue(isEmpty);

            (_, isEmpty) = array.non_empty_domain();
            Assert.IsTrue(isEmpty);
            
            array.Close();

            var array_schema_loaded = Array.load_array_schema(context, tmpArrayPath);
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

            domain.add_dimension(dimension);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
            Assert.IsNotNull(array_schema);

            var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
            Assert.IsNotNull(a1);

            var a2 = new Attribute(context, "a2", DataType.TILEDB_STRING_ASCII);
            Assert.IsNotNull(a2);

            a2.set_cell_val_num((uint)Constants.TILEDB_VAR_NUM);

            array_schema.add_attributes(a1, a2);

            array_schema.set_domain(domain);

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

            domain.add_dimensions(dim1, dim2);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
            Assert.IsNotNull(array_schema);

            var a1 = new Attribute(context, "a1", DataType.TILEDB_INT32);
            Assert.IsNotNull(a1);

            var a2 = new Attribute(context, "a2", DataType.TILEDB_STRING_ASCII);
            Assert.IsNotNull(a2);

            a2.set_cell_val_num((uint)Constants.TILEDB_VAR_NUM);

            array_schema.add_attributes(a1, a2);

            array_schema.set_domain(domain);

            array_schema.Check();

            return array_schema;
        }
    }
}//namespace