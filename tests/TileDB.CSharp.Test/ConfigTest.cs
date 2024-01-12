using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class ConfigTest
{
    [TestMethod]
    public void ConfigSet()
    {
        var config = new Config();
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);
    }

    [TestMethod]
    public void ConfigGetNonExistent()
    {
        var config = new Config();
        var val = config.Get("sm.does_not_exist");
        Assert.AreEqual("", val);
    }

    [TestMethod]
    public void ConfigUnSet()
    {
        var config = new Config();
        var defaultVal = config.Get("sm.tile_cache_size");
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);
        config.Unset("sm.tile_cache_size");
        val = config.Get("sm.tile_cache_size");
        Assert.AreEqual(defaultVal, val);
    }

    [TestMethod]
    public void ConfigFromFile()
    {
        var config = new Config();
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);

        // Create temporary path for testing configuration writing/reading
        var tmpPath = TestUtil.MakeTestPath("tmp.cfg");

        config.SaveToFile(tmpPath);

        var config2 = new Config();
        config2.LoadFromFile(tmpPath);

        val = config2.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);
    }

    [TestMethod]
    public void ConfigCompare()
    {
        var config = new Config();
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);

        var config2 = new Config();
        config2.Set("sm.tile_cache_size", "10");
        var val2 = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val2);

        var cmp2 = config.Cmp(ref config2);
        Assert.IsTrue(cmp2);

        var config3 = new Config();
        config3.Set("sm.tile_cache_size", "11");
        var val3 = config3.Get("sm.tile_cache_size");
        Assert.AreEqual("11", val3);

        var cmp3 = config.Cmp(ref config3);
        Assert.IsFalse(cmp3);
    }

    [TestMethod]
    public void ConfigIterator()
    {
        var config = new Config();

        // Iterate the configuration
        var iter = config.Iterate("vfs.s3.");

        while (!iter.Done())
        {

            // Get current param, value from iterator
            var config_entry_pair = iter.Here();

            switch (config_entry_pair.Item1)
            {
                case "aws_access_key_id":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "aws_external_id":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "aws_load_frequency":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "aws_role_arn":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "aws_secret_access_key":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "aws_session_name":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "aws_session_token":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "bucket_canned_acl":
                    Assert.AreEqual("NOT_SET", config_entry_pair.Item2);
                    break;
                case "ca_file":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "ca_path":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "connect_max_tries":
                    Assert.AreEqual("5", config_entry_pair.Item2);
                    break;
                case "connect_scale_factor":
                    Assert.AreEqual("25", config_entry_pair.Item2);
                    break;
                case "connect_timeout_ms":
                    Assert.AreEqual("10800", config_entry_pair.Item2);
                    break;
                case "endpoint_override":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "logging_level":
                    Assert.AreEqual("Off", config_entry_pair.Item2);
                    break;
                case "max_parallel_ops":
                    Assert.AreEqual(Environment.ProcessorCount.ToString(), config_entry_pair.Item2);
                    break;
                case "multipart_part_size":
                    Assert.AreEqual("5242880", config_entry_pair.Item2);
                    break;
                case "object_canned_acl":
                    Assert.AreEqual("NOT_SET", config_entry_pair.Item2);
                    break;
                case "proxy_host":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "proxy_password":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "proxy_port":
                    Assert.AreEqual("0", config_entry_pair.Item2);
                    break;
                case "proxy_scheme":
                    Assert.AreEqual("http", config_entry_pair.Item2);
                    break;
                case "proxy_username":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "region":
                    Assert.AreEqual("us-east-1", config_entry_pair.Item2);
                    break;
                case "request_timeout_ms":
                    Assert.AreEqual("3000", config_entry_pair.Item2);
                    break;
                case "requester_pays":
                    Assert.AreEqual("false", config_entry_pair.Item2);
                    break;
                case "scheme":
                    Assert.AreEqual("https", config_entry_pair.Item2);
                    break;
                case "skip_init":
                    Assert.AreEqual("false", config_entry_pair.Item2);
                    break;
                case "sse":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "sse_kms_key_id":
                    Assert.AreEqual("", config_entry_pair.Item2);
                    break;
                case "use_multipart_upload":
                    Assert.AreEqual("true", config_entry_pair.Item2);
                    break;
                case "use_virtual_addressing":
                    Assert.AreEqual("true", config_entry_pair.Item2);
                    break;
                case "verify_ssl":
                    Assert.AreEqual("true", config_entry_pair.Item2);
                    break;
            }
            iter.Next();
        }
    }
}