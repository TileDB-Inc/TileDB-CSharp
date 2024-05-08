using System;
using TileDB.CSharp;
using TileDB.CSharp.Examples;

public static class ExampleVFS
{
    public static void Run()
    {
        var config = new Config();

        // To use S3, set a path starting with `s3://` here:
        string array_path = ExampleUtil.MakeExamplePath("test-example-vfs");

        // To use S3, set credentials here (or in AWS_ environment variables)
        //config.Set("vfs.s3.aws_access_key_id", "...");

        // To use S3 custom headers, set key-value pairs here:
        config.Set("vfs.s3.custom_headers.my_test_header", "test-test-1234");


        using Context ctx = new Context(config);
        using VFS vfs = new VFS(ctx);

        if (vfs.IsDir(array_path))
        {
            vfs.RemoveDir(array_path);
        }
        vfs.CreateDir(array_path);

        CreateFiles();
        ListFiles();

        void CreateFiles()
        {
            vfs.Touch(array_path + "/f1");
        }

        void ListFiles()
        {
            var files = vfs.GetChildren(array_path);
            files.ForEach(Console.WriteLine);
        }
    }
}
