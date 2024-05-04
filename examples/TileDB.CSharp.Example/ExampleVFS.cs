using System;
using System.IO;
using System.Collections.Generic;

using TileDB.CSharp;
using TileDB.CSharp.Examples;

public class ExampleVFS
{
    private readonly string _array_path;
    private readonly Context _ctx;
    private readonly VFS _vfs;

    public ExampleVFS()
    {
        var config = new Config();

        // To use S3, set a path starting with `s3://` here:
        _array_path = ExampleUtil.MakeExamplePath("test-example-vfs");

        // To use S3, set credentials here (or in AWS_ environment variables)
        //config.Set("vfs.s3.aws_access_key_id", "...");

        // To use S3 custom headers, set key-value pairs here:
        config.Set("vfs.s3.custom_headers.my_test_header", "test-test-1234");


        _ctx = new Context(config);
        _vfs = new VFS(_ctx);

        if (_vfs.IsDir(_array_path))
        {
            _vfs.RemoveDir(_array_path);
        }
        _vfs.CreateDir(_array_path);
    }

    public void CreateFiles()
    {
        _vfs.Touch(_array_path + "/f1");
    }

    public void ListFiles()
    {
        var files = _vfs.GetChildren(_array_path);
        files.ForEach(Console.WriteLine);
    }

   public static void Run()
    {
        var exampleVFS = new ExampleVFS();

        exampleVFS.CreateFiles();
        exampleVFS.ListFiles();
    }
}
