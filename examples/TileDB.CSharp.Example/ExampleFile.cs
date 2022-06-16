using System;
using System.Collections.Generic;

namespace TileDB.CSharp.Examples
{
    public class ExampleFile
    {
        private Context _ctx;
        private string _nameSpace;
        
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
            string uriToCreate;
            string uriToAccess;

            var arrayURI = string.Join("/", new List<string>(new string[] {pathOrBucket, arrayName}));
            if (_nameSpace == String.Empty)
            {
                uriToCreate = arrayURI;
                uriToAccess = arrayURI;
            }
            else
            {
                uriToCreate = string.Format("tiledb://{0}/{1}", _nameSpace, arrayURI);
                uriToAccess = string.Format("tiledb://{0}/{1}", _nameSpace, arrayName);
            }
            
            File f = new File(_ctx);
            var arraySchema = f.SchemaCreate(fileToSave);
            Array.Create(_ctx, uriToCreate, arraySchema);
            f.URIImport(
                uriToAccess,
                fileToSave,
                MIMEType.TILEDB_MIME_AUTODETECT
            );
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

            File f = new File(_ctx);
            f.URIExport(
                fileToExport,
                uriToAccess
            );
        }
        
        public static void RunLocal()
        {
            var localFile = "local_file";
            var localPath = "local_path";
            var localArrayName = "array_name";
            var localOutputFile = "local_out_file";

            var exampleFile = new ExampleFile();
            exampleFile.SaveFileToArray(localPath, localArrayName, localFile);
            exampleFile.ExportArrayToFile(localOutputFile, localArrayName, localPath);
        }

        public static void RunCloud()
        {
            var localFile = "local_file";
            var bucket = "bucket";
            var cloudArrayName = "cloud_array_name";
            var nameSpace = "userNameOrOrgName";
            var localOutputFileFromCloud = "local_out";
            
            var exampleFile = new ExampleFile(
                "https://api.tiledb.com",
                "token",
                nameSpace
            );
            exampleFile.SaveFileToArray(bucket, cloudArrayName, localFile);
            exampleFile.ExportArrayToFile(localOutputFileFromCloud, cloudArrayName);
        }
    }
}