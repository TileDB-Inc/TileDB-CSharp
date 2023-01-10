using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void NewContextIsValid()
        {
            var config = new Config();

            // Set values
            config.Set("sm.memory_budget", "512000000");
            config.Set("vfs.s3.connect_timeout_ms", "5000");
            config.Set("vfs.s3.endpoint_override", "localhost:8888");
            using (var ctx = new Context(config))
            {
                // Get values
                Assert.AreEqual("512000000", ctx.Config().Get("sm.memory_budget"));
                Assert.AreEqual("5000", ctx.Config().Get("vfs.s3.connect_timeout_ms"));
                Assert.AreEqual("localhost:8888", ctx.Config().Get("vfs.s3.endpoint_override"));
            }
        }

        [TestMethod]
        public void ConfigReadWrite()
        {
            var config = new Config();

            // Set values
            config.Set("sm.memory_budget", "512000000");
            config.Set("vfs.s3.connect_timeout_ms", "5000");
            config.Set("vfs.s3.endpoint_override", "localhost:8888");

            // Get values
            Assert.AreEqual("512000000", config.Get("sm.memory_budget"));
            Assert.AreEqual("5000", config.Get("vfs.s3.connect_timeout_ms"));
            Assert.AreEqual("localhost:8888", config.Get("vfs.s3.endpoint_override"));

            var tempConfigPath = TestUtil.MakeTestPath("temp.cfg");
            config.SaveToFile(tempConfigPath);

            var config2 = new Config();
            config2.LoadFromFile(tempConfigPath);

            // Get values from config2
            Assert.AreEqual("512000000", config2.Get("sm.memory_budget"));
            Assert.AreEqual("5000", config2.Get("vfs.s3.connect_timeout_ms"));
            Assert.AreEqual("localhost:8888", config2.Get("vfs.s3.endpoint_override"));
        }

        [TestMethod]
        public void TestIsFileSystemSupported()
        {
            using var ctx = new Context();
            Assert.IsTrue(ctx.IsFileSystemSupported(FileSystemType.InMemory));
        }
    }
}
