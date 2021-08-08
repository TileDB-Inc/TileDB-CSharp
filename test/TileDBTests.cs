using System;
using Xunit;
using TileDB;
using tdb = TileDB;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace test
{
    public class UnitTest1
    {
        [Fact]
        public void TestConfig()
        {
            var config = new tdb.Config();
            config.set("sm.memory_budget", "1234567");
            var context = new tdb.Context(config);
            Assert.Equal("1234567",
                context.config().get("sm.memory_budget")
            );
        }

        // currently disabled due to ch9397: the C++ TileDBError aborts
        [Fact]
        void TestException()
        {
            /*
            var ctx = new TileDB.Context();
            var _ = Assert.Throws<TileDBError>(
                () => new TileDB.Array(ctx, "/tmptmp/foo/bar", TileDB.QueryType.TILEDB_READ)
            );
            */

            // using try/catch here because the Assert.Throws version won't compile
            try {
                var ctx = new TileDB.Context();
                var _ = new TileDB.Array(ctx, "/tmptmp/foo/bar", TileDB.QueryType.TILEDB_READ);
            } catch (tdb.TileDBError e) {
                //caught TileDBError
                Assert.True(true);
                return;
            } catch (System.Exception e) {
                Assert.True(e.Message.Contains("TileDBError"));
                return;
            }
            Assert.True(false, "unhandled exception");
        }
    }
}
