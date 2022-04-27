using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        void TestGroupMetadata()
        {
            var ctx = Context.GetDefault();
            string temp_dir = GetTempDir();
            RemoveTempDir(temp_dir);
            CreateTempDir(temp_dir);
            string group1_uri = System.IO.Path.Combine(temp_dir, "group1");
            Group.Create(ctx, group1_uri);
            var group = new Group(ctx, group1_uri);
            group.Open(QueryType.TILEDB_WRITE);
            group.Close();

            // Put metadata on a group that is not opened
            int v = 5;
            Assert.ThrowsException<Exception>(() => group.PutMetadata<int>("key",v));

            // Write metadata on a group opened in READ mode
            group.Open(QueryType.TILEDB_READ);
            Assert.ThrowsException<Exception>(() => group.PutMetadata<int>("key", v));

            // Close and reopen in WRITE mode
            group.Close();
            group.Open(QueryType.TILEDB_WRITE);

            group.PutMetadata<int>("key", v);
            group.Close();

            group.Open(QueryType.TILEDB_READ);
            var data = group.GetMetadata<int>("key");

            Assert.AreEqual<int>(5, data[0]);

            group.Close();
        }

        [TestMethod]
        public void TestGroupMember()
        {
            var ctx = Context.GetDefault();
            string temp_dir = GetTempDir();
            RemoveTempDir(temp_dir);
            CreateTempDir(temp_dir);

            string array1_uri = System.IO.Path.Combine(temp_dir, "array1");
            CreateArray(array1_uri);
            string array2_uri = System.IO.Path.Combine(temp_dir, "array2");
            CreateArray(array2_uri);

            string group1_uri = System.IO.Path.Combine(temp_dir, "group1");
            Group.Create(ctx, group1_uri);

            string group2_uri = System.IO.Path.Combine(temp_dir, "group2");
            Group.Create(ctx, group2_uri);

            var group1 = new Group(ctx, group1_uri);
            group1.Open(QueryType.TILEDB_WRITE);

            group1.AddMember(array1_uri, false);
            group1.Close();

            var group2 = new Group(ctx, group2_uri);
            group2.Open(QueryType.TILEDB_WRITE);
            group2.AddMember(array2_uri, false);
            group2.Close();

            //Reopen in read mode
            group1.Open(QueryType.TILEDB_READ);
            Assert.AreEqual<ulong>(1, group1.MemberCount());
            group1.Close();

            //Reopen in write mode
            group1.Open(QueryType.TILEDB_WRITE);
            group1.RemoveMember(array1_uri);
            group1.Close();

            //Reopen in read mode
            group1.Open(QueryType.TILEDB_READ);
            Assert.AreEqual<ulong>(0, group1.MemberCount());
            group1.Close();
        }

        private string GetTempDir()
        {
            return System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "tiledb_test");
        }
        private void RemoveTempDir(string path)
        {
            var ctx = Context.GetDefault();
            using (var vfs = new VFS(ctx))
            {
                if (vfs.IsDir(path))
                {
                    vfs.RemoveDir(path);
                }
            }
        }
        private void CreateTempDir(string path)
        {
            var ctx = Context.GetDefault();
            using(var vfs = new VFS(ctx))
            {
                if (vfs.IsDir(path))
                {
                    vfs.RemoveDir(path);
                }
                vfs.CreateDir(path);
                Assert.IsTrue(vfs.IsDir(path));
            }

        }

        private void CreateArray(string uri)
        {
            string tmpArrayPath = uri;
            if (Directory.Exists(tmpArrayPath))
            {
                Directory.Delete(tmpArrayPath, true);
            }

            var context = Context.GetDefault();
            var domain = new Domain(context);
            Assert.IsNotNull(domain);

            var dim_1 = Dimension.Create(context, "d1", new[] { 1, 1 }, 1);
            Assert.IsNotNull(dim_1);
            domain.AddDimension(dim_1);

            var array_schema = new ArraySchema(context, ArrayType.TILEDB_DENSE);
            Assert.IsNotNull(array_schema);

            var attr1 = new Attribute(context, "a1", DataType.TILEDB_FLOAT32);
            Assert.IsNotNull(attr1);
            array_schema.AddAttribute(attr1);

            array_schema.SetDomain(domain);
            array_schema.SetCellOrder(LayoutType.TILEDB_ROW_MAJOR);
            array_schema.SetTileOrder(LayoutType.TILEDB_ROW_MAJOR);

            array_schema.Check();

            Array.Create(context, tmpArrayPath, array_schema);
        }

    }//class

}//namespace