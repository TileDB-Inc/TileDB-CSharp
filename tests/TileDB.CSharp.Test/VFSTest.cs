using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class VFSTest
    {
        [TestMethod]
        public void TestDir()
        {
            using var vfs = new VFS();

            string dirname = "tempdir";
            if (vfs.IsDir(dirname))
            {
                vfs.RemoveDir(dirname);
            }

            vfs.CreateDir(dirname);
            Assert.IsTrue(vfs.IsDir(dirname));

            vfs.RemoveDir(dirname);
        }

        [TestMethod]
        public void TestReadWrite()
        {
            const int DataSize = 4096;

            using var dir = new TemporaryDirectory("vfs-read-write");
            var fileUri = Path.Combine(dir, "file");

            using var vfs = new VFS();

            byte[] dataWrite = new byte[DataSize];
            RandomNumberGenerator.Fill(dataWrite);

            using (var f = vfs.Open(fileUri, VfsMode.Write))
            {
                f.Write(dataWrite);
            }

            using (var f = vfs.Open(fileUri, VfsMode.Read))
            {
                byte[] dataRead = new byte[DataSize];
                f.ReadExactly(0, dataRead);
                CollectionAssert.AreEqual(dataWrite, dataRead);
            }
        }

        [TestMethod]
        public void TestVisit()
        {
            using var dir = new TemporaryDirectory("vfs-visit");

            using var vfs = new VFS();
            vfs.Touch(Path.Combine(dir, "file1"));
            vfs.Touch(Path.Combine(dir, "file2"));
            vfs.Touch(Path.Combine(dir, "file3"));

            int i = 0;

            vfs.VisitChildren(dir, (uri, _) =>
            {
                string path = new Uri(uri).LocalPath;
                Assert.IsTrue(System.IO.File.Exists(path));
                Assert.AreEqual((string)dir, Path.GetDirectoryName(path));

                i++;
                return i != 2;
            }, null);

            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void TestVisitPropagatesExceptions()
        {
            using var dir = new TemporaryDirectory("vfs-visit-exceptions");

            const string ExceptionKey = "foo";

            using var vfs = new VFS();
            vfs.Touch(Path.Combine(dir, "file1"));
            vfs.Touch(Path.Combine(dir, "file2"));
            vfs.Touch(Path.Combine(dir, "file3"));

            int i = 0;

            try
            {
                vfs.VisitChildren(dir, (_, _) =>
                {
                    i++;
                    throw new Exception(ExceptionKey);
                }, null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(1, i);
                Assert.AreEqual(typeof(Exception), e.GetType());
                Assert.AreEqual(ExceptionKey, e.Message);
                return;
            }

            Assert.Fail("Exception was not propagated.");
        }
    }
}
