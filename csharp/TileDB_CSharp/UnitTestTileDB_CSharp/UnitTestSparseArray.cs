using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestTileDB_CSharp
{
    /// <summary>
    /// Summary description for UnitTestSparseArray
    /// </summary>
    [TestClass]
    public class UnitTestSparseArray
    {
        public UnitTestSparseArray()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private String array_uri_ = "bench_array";

        private void CreateArray()
        {
            tiledb.Context ctx = new tiledb.Context();
            tiledb.Domain dom = new tiledb.Domain(ctx);
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            tiledb.ArraySchema schema = new tiledb.ArraySchema(ctx, tiledb.tiledb_array_type_t.TILEDB_SPARSE);
            schema.set_domain(dom);
            tiledb.Attribute attr1 = tiledb.Attribute.create_attribute(ctx, "a", tiledb.tiledb_datatype_t.TILEDB_INT32);
            schema.add_attribute(attr1);

            //delete array if it already exists
            tiledb.VFS vfs = new tiledb.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            tiledb.Array.create(array_uri_, schema);

        }//private void CreateArray()

        private tiledb.Query.Status WriteArray()
        {
            tiledb.Context ctx = new tiledb.Context();

            tiledb.VectorInt32 coords_rows = new tiledb.VectorInt32();
            coords_rows.Add(1);
            coords_rows.Add(2);
            coords_rows.Add(2);

            tiledb.VectorInt32 coords_cols = new tiledb.VectorInt32();
            coords_cols.Add(1);
            coords_cols.Add(4);
            coords_cols.Add(3);

            tiledb.VectorInt32 data = new tiledb.VectorInt32();
            for (int i = 1; i <= 3; ++i)
            {
                data.Add(i);
            }


            //open array for write
            tiledb.Array array = new tiledb.Array(ctx, array_uri_, tiledb.tiledb_query_type_t.TILEDB_WRITE);
            tiledb.Query query = new tiledb.Query(ctx, array, tiledb.tiledb_query_type_t.TILEDB_WRITE);
            query.set_layout(tiledb.tiledb_layout_t.TILEDB_UNORDERED);
            query.set_int32_vector_buffer("a", data);
            query.set_int32_vector_buffer("rows", coords_rows);
            query.set_int32_vector_buffer("cols", coords_cols);

            tiledb.Query.Status status = query.submit();
            array.close();

            return status;

        }//private void WriteArray()

        private tiledb.Query.Status ReadArray()
        {
            tiledb.Context ctx = new tiledb.Context();


            tiledb.VectorInt32 coords_rows = new tiledb.VectorInt32(3);
            tiledb.VectorInt32 coords_cols = new tiledb.VectorInt32(3);

            tiledb.VectorInt32 data = new tiledb.VectorInt32(3); //hold 3 elements

            tiledb.VectorInt32 subarray = new tiledb.VectorInt32();
            subarray.Add(1);
            subarray.Add(2);
            subarray.Add(2);
            subarray.Add(4);

            //open array for read
            tiledb.Array array = new tiledb.Array(ctx, array_uri_, tiledb.tiledb_query_type_t.TILEDB_READ);

            //query
            tiledb.Query query = new tiledb.Query(ctx, array, tiledb.tiledb_query_type_t.TILEDB_READ);
            query.set_layout(tiledb.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data);
            query.set_int32_vector_buffer("rows", coords_rows);
            query.set_int32_vector_buffer("cols", coords_cols);

            tiledb.Query.Status status = query.submit();
            array.close();

            return status;
        }//private tiledb.Query.Status ReadArray()


        [TestMethod]
        public void TestMethod1()
        {
            CreateArray();
            tiledb.Query.Status status_write = WriteArray();
            if (status_write == tiledb.Query.Status.FAILED)
            {
                Assert.Fail();
            }
            tiledb.Query.Status status_read = ReadArray();
            if (status_read == tiledb.Query.Status.FAILED)
            {
                Assert.Fail();
            }
        }
    }
}
