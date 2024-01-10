using System;
using System.Collections.Generic;
using System.IO;

namespace TileDB.CSharp.Examples;

public class ExampleFile
{
    private readonly Context _ctx;
    private readonly string _nameSpace;

    // Local access
    public ExampleFile()
    {
        _ctx = Context.GetDefault();
        _nameSpace = String.Empty;
    }

    // S3 access
    public ExampleFile(string key, string secret)
    {
        var config = new Config();
        config.Set("vfs.s3.aws_access_key_id", key);
        config.Set("vfs.s3.aws_secret_access_key", secret);
        _ctx = new Context(config);
        _nameSpace = String.Empty;
    }

    // TileDB Cloud access with token
    public ExampleFile(string host, string token, string nameSpace)
    {
        var config = new Config();
        config.Set("rest.server_address", host);
        config.Set("rest.token", token);
        _ctx = new Context(config);
        _nameSpace = nameSpace;
    }

    // TileDB Cloud access with basic authentication
    public ExampleFile(string host, string username, string password, string nameSpace)
    {
        var config = new Config();
        config.Set("rest.server_address", host);
        config.Set("rest.username", username);
        config.Set("rest.password", password);
        _ctx = new Context(config);
        _nameSpace = nameSpace;
    }

    public void SaveFileToArray(string pathOrBucket, string arrayName, string fileToSave)
    {
        var arrayURI = string.Join("/", new List<string>(new string[] {pathOrBucket, arrayName}));

        // If no fileToSave exists, create one with some text data
        if (!System.IO.File.Exists(fileToSave))
        {
            System.IO.File.WriteAllText(fileToSave, "Some text data");
        }

        if (_nameSpace == String.Empty)
        {
            // If exported array exists, recreate it
            if (Directory.Exists(arrayURI))
            {
                Directory.Delete(arrayURI, true);
            }

            FileStoreUtil.SaveFileToArray(_ctx, arrayURI, fileToSave);
            return;
        }

        FileStoreUtil.SaveFileToCloudArray(_ctx, _nameSpace, arrayURI, fileToSave);
    }

    public void ExportArrayToFile(string fileToExport, string arrayName, string pathOrBucket="")
    {
        string uriToAccess;
        if (_nameSpace == String.Empty)
        {
            uriToAccess = string.Join("/", new List<string>(new string[] {pathOrBucket, arrayName}));
        }
        else
        {
            uriToAccess = string.Format("tiledb://{0}/{1}", _nameSpace, arrayName);
        }

        FileStoreUtil.ExportArrayToFile(_ctx, fileToExport, uriToAccess);
    }

    public static void RunLocal()
    {
        var localPath = ExampleUtil.MakeExamplePath("file-local");
        var localOutputFile = Path.Join(localPath, "local_out_file");
        var localFile = Path.Join(localPath, "local_file");
        var localArrayName = "array_name";
        if (!Directory.Exists(localPath))
        {
            Directory.CreateDirectory(localPath);
        }

        var exampleFile = new ExampleFile();
        // Import localFile to new TileDB Array
        exampleFile.SaveFileToArray(localPath, localArrayName, localFile);
        // Export TileDB Array to new localOutputFile
        exampleFile.ExportArrayToFile(localOutputFile, localArrayName, localPath);
    }

    public static void RunCloud(string token, string nameSpace, string cloudArrayName, string s3Bucket,
        string host = "https://api.tiledb.com")
    {
        var localPath = ExampleUtil.MakeExamplePath("file-cloud");
        if (!Directory.Exists(localPath))
        {
            Directory.CreateDirectory(localPath);
        }
        // Local file to import to new TileDB Cloud Array
        var localFile = Path.Join(localPath, "local_file");
        // Local file to store exported TileDB Cloud Array
        var localOutputFileFromCloud = Path.Join(localPath, "local_out_cloud_file");

        // Convert a local file to new TileDB Cloud Array
        // + Creates TileDB Array: tiledb://<nameSpace>/<cloudArrayName>
        // + Stored on S3: <s3bucket>/<cloudArrayName>
        var exampleFile = new ExampleFile(host, token, nameSpace);
        // Import localFile to new TileDB Cloud Array
        exampleFile.SaveFileToArray(s3Bucket, cloudArrayName, localFile);
        // Export TileDB Cloud Array to new local file
        exampleFile.ExportArrayToFile(localOutputFileFromCloud, cloudArrayName);
    }
}