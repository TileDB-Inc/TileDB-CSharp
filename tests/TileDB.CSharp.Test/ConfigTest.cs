using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB;
using System.IO;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void Set() {
            TileDB.Config config = new TileDB.Config();
            config.set("sm.tile_cache_size", "10");
            string val = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");
        }

        [TestMethod]
        public void Get() {
            TileDB.Config config = new TileDB.Config();
            string val = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10000000");
            val = config.get("sm.does_not_exists");
            Assert.AreEqual<string>(val, "");
        }

        [TestMethod]
        public void UnSet() {
            TileDB.Config config = new TileDB.Config();
            config.set("sm.tile_cache_size", "10");
            string val = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");
            config.unset("sm.tile_cache_size");
            val = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10000000");
        }

        [TestMethod]
        public void File() {
            TileDB.Config config = new TileDB.Config();
            config.set("sm.tile_cache_size", "10");
            string val = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");

            // Create temporary path for testing configuration writing/reading
            string tmpPath = Path.Join( Path.GetTempPath(), "tmp.cfg");

            config.save_to_file(tmpPath);

            TileDB.Config config2 = new TileDB.Config();
            config2.load_from_file(tmpPath);

            val = config2.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");
        }

        [TestMethod]
        public void Compare() {
            TileDB.Config config = new TileDB.Config();
            config.set("sm.tile_cache_size", "10");
            string val = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val, "10");

            TileDB.Config config2 = new TileDB.Config();
            config2.set("sm.tile_cache_size", "10");
            string val2 = config.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val2, "10");

            bool cmp2 = config.cmp(ref config2);
            Assert.AreEqual<bool>(cmp2, true);

            TileDB.Config config3 = new TileDB.Config();
            config3.set("sm.tile_cache_size", "11");
            string val3 = config3.get("sm.tile_cache_size");
            Assert.AreEqual<string>(val3, "11");

            bool cmp3 = config.cmp(ref config3);
            Assert.AreEqual<bool>(cmp3, false);            
        }        

        [TestMethod]
        public void ConfigIterator() {
            TileDB.Config config = new TileDB.Config();

            // Iterate the configuration
            TileDB.ConfigIterator iter = config.iterate("vfs.s3.");

            while (!iter.done()) {

                // Get current param, value from iterator
                Tuple<string, string> config_entry_pair = iter.here();

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
                        Assert.AreEqual<string>(config_entry_pair.Item2, System.Environment.ProcessorCount.ToString());
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
                iter.next();
            }
        }
    }
}