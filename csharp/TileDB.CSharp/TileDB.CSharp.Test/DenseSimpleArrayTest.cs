using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    /// <summary>
    /// Summary description for DenseSimpleArrayTest
    /// </summary>
    [TestClass]
    public class DenseSimpleArrayTest
    {
        public DenseSimpleArrayTest()
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

        private String array_uri_ = "test_dense_array";

        #region Simple Dense Array
        private void CreateDenseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();
            TileDB.Domain dom = new TileDB.Domain(ctx);
            dom.add_int32_dimension("rows", 1, 4, 4);
            dom.add_int32_dimension("cols", 1, 4, 4);

            TileDB.ArraySchema schema = new TileDB.ArraySchema(ctx, TileDB.ArrayType.TILEDB_DENSE);
            schema.set_domain(dom);
            TileDB.Attribute attr1 = TileDB.Attribute.create_attribute(ctx, "a", TileDB.DataType.TILEDB_INT32);
            schema.add_attribute(attr1);

            //delete array if it already exists
            TileDB.VFS vfs = new TileDB.VFS(ctx);
            if (vfs.is_dir(array_uri_))
            {
                vfs.remove_dir(array_uri_);
            }

            //create array
            TileDB.Array.create(array_uri_, schema);

        }//private void CreateArray()

        private TileDB.QueryStatus WriteDenseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 data = new TileDB.VectorInt32();
            for (int i = 1; i <= 16; ++i)
            {
                data.Add(i);
            }


            //open array for write
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_WRITE);
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_WRITE);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int32_vector_buffer("a", data);

            TileDB.QueryStatus status = query.submit();
            array.close();

            return status;

        }//private void WriteArray()

        private TileDB.QueryStatus ReadDenseSimpleArray()
        {
            TileDB.Context ctx = new TileDB.Context();

            TileDB.VectorInt32 subarray = new TileDB.VectorInt32();
            subarray.Add(1);
            subarray.Add(2); //rows 1,2
            subarray.Add(2);
            subarray.Add(4); //cols 2,3,4

            TileDB.VectorInt32 data = new TileDB.VectorInt32(6); //hold 6 elements

            //open array for read
            TileDB.Array array = new TileDB.Array(ctx, array_uri_, TileDB.QueryType.TILEDB_READ);

            //query
            TileDB.Query query = new TileDB.Query(ctx, array, TileDB.QueryType.TILEDB_READ);
            query.set_layout(TileDB.LayoutType.TILEDB_ROW_MAJOR);
            query.set_int32_subarray(subarray);
            query.set_int32_vector_buffer("a", data);

            TileDB.QueryStatus status = query.submit();
            array.close();

            return status;
        }//private TileDB.Query.Status ReadArray()
        #endregion



        [TestMethod]
        public void TestDenseSimpleArray()
        {
            CreateDenseSimpleArray();
            TileDB.QueryStatus status_write = WriteDenseSimpleArray();
            if (status_write == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Assert.Fail();
            }
            TileDB.QueryStatus status_read = ReadDenseSimpleArray();
            if (status_read == TileDB.QueryStatus.TILEDB_FAILED)
            {
                Assert.Fail();
            }
        }
    }
}
