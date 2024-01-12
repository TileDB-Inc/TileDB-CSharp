using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;

namespace TileDB.CSharp.Test;

[TestClass]
public class VFSTest
{
    private static void WriteRandomData(VFS vfs, string fileUri, int byteCount, out byte[] data)
    {
        data = new byte[byteCount];
        RandomNumberGenerator.Fill(data);
        using var f = vfs.Open(fileUri, VfsMode.Write);
        f.Write(data);
    }

    [TestMethod]
    public void TestCreateWithConfig()
    {
        using var config = new Config();
        config.Set("test", "test");
        using var vfs = new VFS(config: config);
        using var retrievedConfig = vfs.Config();
        Assert.AreEqual("test", retrievedConfig.Get("test"));
    }

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
        Assert.IsTrue(Directory.Exists(dirname));

        vfs.RemoveDir(dirname);
    }

    [TestMethod]
    public void TestReadWrite()
    {
        const int DataSize = 4096;

        using var dir = new TemporaryDirectory("vfs-read-write");
        var fileUri = Path.Combine(dir, "file");

        using var vfs = new VFS();

        WriteRandomData(vfs, fileUri, DataSize, out var dataWrite);
        Assert.AreEqual((ulong)DataSize, vfs.FileSize(fileUri));

        using (var f = vfs.Open(fileUri, VfsMode.Read))
        {
            byte[] dataRead = new byte[DataSize];
            f.ReadExactly(0, dataRead);
            CollectionAssert.AreEqual(dataWrite, dataRead);
        }
    }

    [TestMethod]
    public void TestCopyMove()
    {
        const int DataSize = 4096;

        using var dir = new TemporaryDirectory("vfs-copy-move");
        var file1Uri = Path.Combine(dir, "file1");
        var file2Uri = Path.Combine(dir, "file2");

        using var vfs = new VFS();

        WriteRandomData(vfs, file1Uri, DataSize, out var dataWrite);
        Assert.AreEqual((ulong)DataSize, vfs.FileSize(file1Uri));
        vfs.MoveFile(file1Uri, file2Uri);
        Assert.IsFalse(vfs.IsFile(file1Uri));
        Assert.AreEqual((ulong)DataSize, vfs.FileSize(file2Uri));
        if (OperatingSystem.IsWindows())
        {
            // Copying files on Windows is not yet supported.
            System.IO.File.Copy(file2Uri, file1Uri);
        }
        else
        {
            vfs.CopyFile(file2Uri, file1Uri);
        }
        Assert.IsTrue(vfs.IsFile(file1Uri));
        Assert.IsTrue(vfs.IsFile(file2Uri));
        Assert.AreEqual((ulong)DataSize * 2, vfs.DirSize(dir));
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

        vfs.VisitChildren(dir, (uri, arg) =>
        {
            string path = new Uri(uri).LocalPath;
            Assert.IsTrue(System.IO.File.Exists(path));
            Assert.AreEqual((string)dir, Path.GetDirectoryName(path));
            Assert.AreEqual(555, arg);

            i++;
            return i != 2;
        }, 555);

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
            }, 0);
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
