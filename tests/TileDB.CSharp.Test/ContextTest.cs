using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                Assert.AreEqual<string>(ctx.Config().Get("sm.memory_budget"), "512000000");
                Assert.AreEqual<string>(ctx.Config().Get("vfs.s3.connect_timeout_ms"), "5000");
                Assert.AreEqual<string>(ctx.Config().Get("vfs.s3.endpoint_override"), "localhost:8888");
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
            Assert.AreEqual<string>(config.Get("sm.memory_budget"), "512000000");
            Assert.AreEqual<string>(config.Get("vfs.s3.connect_timeout_ms"), "5000");
            Assert.AreEqual<string>(config.Get("vfs.s3.endpoint_override"), "localhost:8888");

            var tempConfigPath = TestUtil.MakeTestPath("temp.cfg");
            config.SaveToFile(tempConfigPath);

            var config2 = new Config();
            config2.LoadFromFile(tempConfigPath);

            // Get values from config2
            Assert.AreEqual<string>(config2.Get("sm.memory_budget"), "512000000");
            Assert.AreEqual<string>(config2.Get("vfs.s3.connect_timeout_ms"), "5000");
            Assert.AreEqual<string>(config2.Get("vfs.s3.endpoint_override"), "localhost:8888");
        }
    }
}