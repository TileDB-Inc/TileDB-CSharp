using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileDB;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void NewContextIsValid()
        {

            TileDB.Config config = new TileDB.Config();

            // Set values
            config.set("sm.memory_budget", "512000000");
            config.set("vfs.s3.connect_timeout_ms", "5000");
            config.set("vfs.s3.endpoint_override", "localhost:8888");
            using (var ctx = new Context(config)) 
            {
                // Get values
                Assert.AreEqual<string>(ctx.config().get("sm.memory_budget"), "512000000");
                Assert.AreEqual<string>(ctx.config().get("vfs.s3.connect_timeout_ms"), "5000");
                Assert.AreEqual<string>(ctx.config().get("vfs.s3.endpoint_override"), "localhost:8888");
            }

        }

        [TestMethod]
        public void ConfigReadWrite() 
        {
            TileDB.Config config = new TileDB.Config();
 
            // Set values
            config.set("sm.memory_budget", "512000000");
            config.set("vfs.s3.connect_timeout_ms", "5000");
            config.set("vfs.s3.endpoint_override", "localhost:8888");

            // Get values
            Assert.AreEqual<string>(config.get("sm.memory_budget"), "512000000");
            Assert.AreEqual<string>(config.get("vfs.s3.connect_timeout_ms"), "5000");
            Assert.AreEqual<string>(config.get("vfs.s3.endpoint_override"), "localhost:8888");

            config.save_to_file("temp.cfg");

            TileDB.Config config2 = new TileDB.Config();
            config2.load_from_file("temp.cfg");

            // Get values from config2
            Assert.AreEqual<string>(config2.get("sm.memory_budget"), "512000000");
            Assert.AreEqual<string>(config2.get("vfs.s3.connect_timeout_ms"), "5000");
            Assert.AreEqual<string>(config2.get("vfs.s3.endpoint_override"), "localhost:8888");

        }
    }
}