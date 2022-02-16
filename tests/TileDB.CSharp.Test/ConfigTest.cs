using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void ConfigSet() {
            var config = new Config();
            config.Set("sm.tile_cache_size", "10");
            var val = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");
        }

        [TestMethod]
        public void ConfigGet() {
            var config = new Config();
            var val = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10000000");
            val = config.Get("sm.does_not_exists");
            Assert.AreEqual<string>(val, "");
        }

        [TestMethod]
        public void ConfigUnSet() {
            var config = new Config();
            config.Set("sm.tile_cache_size", "10");
            var val = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");
            config.Unset("sm.tile_cache_size");
            val = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10000000");
        }

        [TestMethod]
        public void ConfigFromFile() {
            var config = new Config();
            config.Set("sm.tile_cache_size", "10");
            var val = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");

            // Create temporary path for testing configuration writing/reading
            var tmpPath = Path.Join( Path.GetTempPath(), "tmp.cfg");

            config.save_to_file(tmpPath);

            var config2 = new Config();
            config2.load_from_file(tmpPath);

            val = config2.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");
        }

        [TestMethod]
        public void ConfigCompare() {
            var config = new Config();
            config.Set("sm.tile_cache_size", "10");
            var val = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");

            var config2 = new Config();
            config2.Set("sm.tile_cache_size", "10");
            var val2 = config.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val2, "10");

            var cmp2 = config.Cmp(ref config2);
            Assert.AreEqual(cmp2, true);

            var config3 = new Config();
            config3.Set("sm.tile_cache_size", "11");
            var val3 = config3.Get("sm.tile_cache_size");
            Assert.AreEqual<string>(val3, "11");

            var cmp3 = config.Cmp(ref config3);
            Assert.AreEqual(cmp3, false);
        }

        [TestMethod]
        public void ConfigIterator() {
            var config = new Config();

            // Iterate the configuration
            var iter = config.Iterate("vfs.s3.");

            while (!iter.Done()) {

                // Get current param, value from iterator
                var config_entry_pair = iter.Here();

                switch (config_entry_pair.Item1) {
                    case "aws_access_key_id":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "aws_external_id":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "aws_load_frequency":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "aws_role_arn":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "aws_secret_access_key":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "aws_session_name":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "aws_session_token":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "bucket_canned_acl":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "NOT_SET");
                        break;
                    case "ca_file":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "ca_path":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "connect_max_tries":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "5");
                        break;
                    case "connect_scale_factor":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "25");
                        break;
                    case "connect_timeout_ms":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "10800");
                        break;
                    case "endpoint_override":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "logging_level":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "Off");
                        break;
                    case "max_parallel_ops":
                        Assert.AreEqual(config_entry_pair.Item2, Environment.ProcessorCount.ToString());
                        break;
                    case "multipart_part_size":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "5242880");
                        break;
                    case "object_canned_acl":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "NOT_SET");
                        break;
                    case "proxy_host":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "proxy_password":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "proxy_port":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "0");
                        break;
                    case "proxy_scheme":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "http");
                        break;
                    case "proxy_username":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "region":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "us-east-1");
                        break;
                    case "request_timeout_ms":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "3000");
                        break;
                    case "requester_pays":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "false");
                        break;
                    case "scheme":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "https");
                        break;
                    case "skip_init":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "false");
                        break;
                    case "sse":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "sse_kms_key_id":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "");
                        break;
                    case "use_multipart_upload":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "true");
                        break;
                    case "use_virtual_addressing":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "true");
                        break;
                    case "verify_ssl":
                        Assert.AreEqual<string>(config_entry_pair.Item2, "true");
                        break;
                }
                iter.Next();
            }
        }
    }
}