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
        /*
        [Fact]
        void TestError()
        {
            // using try/catch here because the Assert.Throws version won't compile
            //   Assert.Throws<TileDBError>(() => new TileDB.Array(...))
            try {
                var ctx = new TileDB.Context();
                //var _ = new TileDB.Array(ctx, "/tmptmp/foo/bar", TileDB.tiledb_query_type_t.TILEDB_READ);
            } catch (Exception e) {
                Assert.True(true);
                return;
            }
            Assert.True(false, "unhandled exception");
        }
        */

    }
}
