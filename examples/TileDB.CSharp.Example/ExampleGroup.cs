using System;
using System.Collections.Generic;
using System.IO;

namespace TileDB.CSharp.Examples;

public class ExampleGroup
{
    private readonly Context _ctx;
    private readonly string _nameSpace;

    // Local access
    public ExampleGroup()
    {
        _ctx = Context.GetDefault();
        _nameSpace = String.Empty;
    }

    // S3 access
    public ExampleGroup(string key, string secret)
    {
        var config = new Config();
        config.Set("vfs.s3.aws_access_key_id", key);
        config.Set("vfs.s3.aws_secret_access_key", secret);
        _ctx = new Context(config);
        _nameSpace = String.Empty;
    }

    // TileDB Cloud access with token
    public ExampleGroup(string host, string token, string nameSpace)
    {
        var config = new Config();
        config.Set("rest.server_address", host);
        config.Set("rest.token", token);
        _ctx = new Context(config);
        _nameSpace = nameSpace;
    }

    // TileDB Cloud access with basic authentication
    public ExampleGroup(string host, string username, string password, string nameSpace)
    {
        var config = new Config();
        config.Set("rest.server_address", host);
        config.Set("rest.username", username);
        config.Set("rest.password", password);
        _ctx = new Context(config);
        _nameSpace = nameSpace;
    }

    public void CreateGroups(string pathOrBucket, string groupName1, string groupName2)
    {
        var groupURI1 = string.Join("/", new List<string>(new string[] {pathOrBucket, groupName1}));
        var groupURI2 = string.Join("/", new List<string>(new string[] { pathOrBucket, groupName2 }));

        if (_nameSpace == String.Empty)
        {
            if (Directory.Exists(groupURI1))
            {
                Directory.Delete(groupURI1, true);
            }
            Group.Create(_ctx, groupURI1);

            if (Directory.Exists(groupURI2))
            {
                Directory.Delete(groupURI2, true);
            }
            Group.Create(_ctx, groupURI2);
            return;
        }

        var groupURIToCreate1 = string.Format("tiledb://{0}/{1}", _nameSpace, groupURI1);
        Group.Create(_ctx, groupURIToCreate1);
        var groupURIToCreate2 = string.Format("tiledb://{0}/{1}", _nameSpace, groupURI2);
        Group.Create(_ctx, groupURIToCreate2);
    }

    public void NestGroup(string pathOrBucket, string parentGroupName, string childGroupName)
    {
        string parentGroupURI;
        string childGroupURI;
        if (_nameSpace == String.Empty)
        {
            parentGroupURI = string.Join("/", new List<string>(new string[] { pathOrBucket, parentGroupName }));
            childGroupURI = string.Join("/", new List<string>(new string[] { pathOrBucket, childGroupName }));
        }
        else
        {
            parentGroupURI = string.Format("tiledb://{0}/{1}", _nameSpace, parentGroupName);
            childGroupURI = string.Format("tiledb://{0}/{1}", _nameSpace, childGroupName);
        }

        var parentGroup = new Group(_ctx, parentGroupURI);
        parentGroup.Open(QueryType.Write);
        parentGroup.AddMember(childGroupURI, false, "groupMember2");
        parentGroup.Close();
    }

    public static void RunLocal()
    {
        var localGroupPath = ExampleUtil.MakeExamplePath("groups-local");
        if (!Directory.Exists(localGroupPath))
        {
            Directory.CreateDirectory(localGroupPath);
        }
        var localGroupName1 = "group1";
        var localGroupName2 = "group2";

        var exampleGroup = new ExampleGroup();
        exampleGroup.CreateGroups(localGroupPath, localGroupName1, localGroupName2);
        exampleGroup.NestGroup(localGroupPath, localGroupName1, localGroupName2);
    }

    public static void RunCloud(string token, string nameSpace, string s3BucketPrefix,
        string host = "https://api.tiledb.com")
    {
        var cloudGroupName1 = "cloud_group1";
        var cloudGroupName2 = "cloud_group2";

        var exampleGroup = new ExampleGroup(host, token, nameSpace);
        exampleGroup.CreateGroups(s3BucketPrefix, cloudGroupName1, cloudGroupName2);
        exampleGroup.NestGroup(s3BucketPrefix, cloudGroupName1, cloudGroupName2);
    }
}