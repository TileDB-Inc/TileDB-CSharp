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

            domain.add_dimensions(rowDim, colDim);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_SPARSE);
            Assert.IsNotNull(array_schema);
            var a1 = new Attribute(context, "a1", DataType.TILEDB_UINT32);
            Assert.IsNotNull(a1);

            array_schema.set_cell_order(LayoutType.TILEDB_ROW_MAJOR);
            array_schema.set_tile_order(LayoutType.TILEDB_ROW_MAJOR);

            array_schema.add_attribute(a1);

            array_schema.set_domain(domain);

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

            array.put_metadata<int>("key1", new int[] { 25 });

            array.put_metadata<int>("key2", new int[] { 25, 26, 27, 28 });

            array.put_metadata<float>("key3", new float[] { 25.1f });

            array.put_metadata<float>("key4", new float[] { 25.1f, 26.2f, 27.3f, 28.4f });

            array.put_metadata("key5", "This is TileDB array metadata");

            array.Close();
        }

        private void readArrayMetadata(string tmpArrayPath)
        {
            var context = Context.GetDefault();

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            array.Open(QueryType.TILEDB_READ);

            var (_, _, v1) = array.Metadata<int>("key1");
            CollectionAssert.AreEqual(new int[]{ 25 }, v1);

            var (_, _, v2) = array.Metadata<int>("key2");
            CollectionAssert.AreEqual(new int[]{ 25, 26, 27, 28 }, v2);

            var (_, _, v3) = array.Metadata<float>("key3");
            CollectionAssert.AreEqual(new float[]{ 25.1f }, v3);

            var (_, _, v4) = array.Metadata<float>("key4");
            CollectionAssert.AreEqual(new float[]{ 25.1f, 26.2f, 27.3f, 28.4f }, v4);

            var s = array.Metadata("key5");
            Assert.AreEqual("This is TileDB array metadata", s);

            var num = array.metadata_num();
            Assert.AreEqual((ulong)5,  num);

            var keys = array.metadata_keys();
            CollectionAssert.AreEqual(new string[]{"key1", "key2", "key3", "key4", "key5"}, keys);

            var arrayMetadata = array.metadata_from_index<float>(3);
            Assert.AreEqual("key4",  arrayMetadata.Key);
            Assert.AreEqual((uint)4,  arrayMetadata.KeyLen);
            Assert.AreEqual(Interop.tiledb_datatype_t.TILEDB_FLOAT32,  arrayMetadata.Datatype);
            Assert.AreEqual((uint)4,  arrayMetadata.ValueNum);
            Assert.AreEqual((int)4,  arrayMetadata.Value.Length);

            array.Close();
        }

        private void clearArrayMetadata(string tmpArrayPath)
        {
            var context = Context.GetDefault();

            var array = new Array(context, tmpArrayPath);
            Assert.IsNotNull(array);

            array.Open(QueryType.TILEDB_WRITE);

            array.delete_metadata("key1");
            array.delete_metadata("key2");
            array.delete_metadata("key3");
            array.delete_metadata("key4");
            array.delete_metadata("key5");
            array.Close();
        }
    }
}