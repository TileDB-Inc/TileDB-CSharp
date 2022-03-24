using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ArrayMetadataTest
    {
        [TestMethod]
        public void TestArrayMetadata()
        {
            var tmpArrayPath = Path.Join( Path.GetTempPath(), "metadata_array");

            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }
            createArrayMetadataArray(tmpArrayPath);
            writeArrayMetadata(tmpArrayPath);
            readArrayMetadata(tmpArrayPath);
            clearArrayMetadata(tmpArrayPath);
        }

        private void createArrayMetadataArray(string tmpArrayPath)
        {
            var context = Context.GetDefault();

            var bound = new short[] { 1, 4 };
            const short extent = 4;
            var rowDim = Dimension.Create(context, "rows", bound, extent);
            Assert.IsNotNull(rowDim);

            var colDim = Dimension.Create(context, "cols", bound, extent);
            Assert.IsNotNull(rowDim);

            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            domain.AddDimensions(rowDim, colDim);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
            Assert.IsNotNull(array_schema);
            var a1 = new Attribute(context, "a1", DataType.TILEDB_UINT32);
            Assert.IsNotNull(a1);

            array_schema.SetCellOrder(LayoutType.TILEDB_ROW_MAJOR);
            array_schema.SetTileOrder(LayoutType.TILEDB_ROW_MAJOR);

            array_schema.AddAttribute(a1);

            array_schema.SetDomain(domain);

            array_schema.Check();

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            array.Create(array_schema);
        }

        private void writeArrayMetadata(string tmpArrayPath)
        {
            var context = Context.GetDefault();

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            array.Open(QueryType.TILEDB_WRITE);

            array.PutMetadata<int>("key1", new int[] { 25 });

            array.PutMetadata<int>("key2", new int[] { 25, 26, 27, 28 });

            array.PutMetadata<float>("key3", new float[] { 25.1f });

            array.PutMetadata<float>("key4", new float[] { 25.1f, 26.2f, 27.3f, 28.4f });

            array.PutMetadata("key5", "This is TileDB array metadata");

            array.Close();
        }

        private void readArrayMetadata(string tmpArrayPath)
        {
            var context = Context.GetDefault();

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            array.Open(QueryType.TILEDB_READ);

            var (_, v1, _, _) = array.Metadata<int>("key1");
            CollectionAssert.AreEqual(new int[]{ 25 }, v1);

            var (_, v2, _, _) = array.Metadata<int>("key2");
            CollectionAssert.AreEqual(new int[]{ 25, 26, 27, 28 }, v2);

            var (_, v3,  _, _) = array.Metadata<float>("key3");
            CollectionAssert.AreEqual(new float[]{ 25.1f }, v3);

            var (_, v4, _, _) = array.Metadata<float>("key4");
            CollectionAssert.AreEqual(new float[]{ 25.1f, 26.2f, 27.3f, 28.4f }, v4);

            var s = array.Metadata("key5");
            Assert.AreEqual("This is TileDB array metadata", s);

            var num = array.MetadataNum();
            Assert.AreEqual((ulong)5,  num);

            var keys = array.MetadataKeys();
            CollectionAssert.AreEqual(new string[]{"key1", "key2", "key3", "key4", "key5"}, keys);

            var arrayMetadata = array.MetadataFromIndex<float>(3);
            Assert.AreEqual("key4",  arrayMetadata.key);
            Assert.AreEqual((int)4,  arrayMetadata.key.Length);
            Assert.AreEqual(DataType.TILEDB_FLOAT32,  arrayMetadata.datatype);
            Assert.AreEqual((uint)4,  arrayMetadata.value_num);
            Assert.AreEqual((int)4,  arrayMetadata.data.Length);

            array.Close();
        }

        private void clearArrayMetadata(string tmpArrayPath)
        {
            var context = Context.GetDefault();

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            array.Open(QueryType.TILEDB_WRITE);

            array.DeleteMetadata("key1");
            array.DeleteMetadata("key2");
            array.DeleteMetadata("key3");
            array.DeleteMetadata("key4");
            array.DeleteMetadata("key5");
            array.Close();
        }
    }
}