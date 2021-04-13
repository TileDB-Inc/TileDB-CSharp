using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    /// <summary>
    /// Summary description for SparseSimpleArrayTest
    /// </summary>
    [TestClass]
    public class SparseSimpleArrayTest
    {
        public SparseSimpleArrayTest()
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


        private String array_uri_ = "test_sparse_array";

        #region Simple Sparse Array
        private void CreateSparseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.tiledb_array_type_t.TILEDB_SPARSE);
            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.tiledb_datatype_t.TILEDB_INT32);
            schema.add_attribute(attr1);

            //delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            TileDB.Array.create(array_uri_, schema);

        }//private void CreateSparseSimpleArray()

        private TileDB.Query.Status WriteSparseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 coords_rows = new TileDB.VectorInt32();
            coords_rows.Add(1);
            coords_rows.Add(2);
            coords_rows.Add(2);

            TileDB.VectorInt32 coords_cols = new TileDB.VectorInt32();
            coords_cols.Add(1);
            coords_cols.Add(4);
            coords_cols.Add(3);

            TileDB.VectorInt32 data = new TileDB.VectorInt32();
            for (int i = 1; i <= 3; ++i)
            {
                data.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_WRITE);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_UNORDERED);
            query.set_int32_vector_buffer("a", data);
            query.set_int32_vector_buffer("rows", coords_rows);
            query.set_int32_vector_buffer("cols", coords_cols);

            TileDB.Query.Status status = query.submit();
            array.close();

            return status;

        }//private void WriteSparseSimpleArray()

        private TileDB.Query.Status ReadSparseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();


            TileDB.VectorInt32 coords_rows = new TileDB.VectorInt32(3);
            TileDB.VectorInt32 coords_cols = new TileDB.VectorInt32(3);

            TileDB.VectorInt32 data = new TileDB.VectorInt32(3); //hold 3 elements

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(2);
            subarray.Add(2);
            subarray.Add(4);

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.tiledb_query_type_t.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.tiledb_query_type_t.TILEDB_READ);
            query.set_layout(TileDB.tiledb_layout_t.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data);
            query.set_int32_vector_buffer("rows", coords_rows);
            query.set_int32_vector_buffer("cols", coords_cols);

            TileDB.Query.Status status = query.submit();
            array.close();

            return status;
        }//private TileDB.Query.Status ReadSimpleSparseArray()

        #endregion


        [TestMethod]
        public void TestSparseSimpleArray()
        {
            CreateSparseSimpleArray();
            TileDB.Query.Status status_write = WriteSparseSimpleArray();
            if (status_write == TileDB.Query.Status.FAILED)
            {
                Assert.Fail();
            }
            TileDB.Query.Status status_read = ReadSparseSimpleArray();
            if (status_read == TileDB.Query.Status.FAILED)
            {
                Assert.Fail();
            }
        }
    }
}
