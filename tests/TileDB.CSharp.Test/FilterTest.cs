using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TileDB.CSharp.Test
{
    [TestClass]
    public class FilterTest
    {

        [TestMethod]
        public void NewBitShuffleFilterIsValid()
        {
            var ctx = TileDB.Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_BITSHUFFLE);
            Assert.AreEqual(FilterType.TILEDB_FILTER_BITSHUFFLE, filter.filter_type());
        }

        [TestMethod]
        public void NewGzipFilterIsValid()
        {
            var ctx = TileDB.Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_GZIP);
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filter.filter_type());

            const int compression_level = 6;
            filter.set_option(FilterOption.TILEDB_COMPRESSION_LEVEL, compression_level);
            Assert.AreEqual(compression_level, filter.get_option<int>(FilterOption.TILEDB_COMPRESSION_LEVEL));
        }

        [TestMethod]
        public void NewPositiveDeltaFilterIsValid()
        {
            var ctx = TileDB.Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_POSITIVE_DELTA);
            Assert.AreEqual(FilterType.TILEDB_FILTER_POSITIVE_DELTA, filter.filter_type());

            const uint positive_delta_max_window = 1024;
            filter.set_option(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW, positive_delta_max_window);
            Assert.AreEqual(positive_delta_max_window, filter.get_option<uint>(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW));
        }

        [TestMethod]
        public void NewBitWidthReductionFilterIsValid()
        {
            var ctx = TileDB.Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_BIT_WIDTH_REDUCTION);
            Assert.AreEqual(FilterType.TILEDB_FILTER_BIT_WIDTH_REDUCTION, filter.filter_type());

            const uint bid_width_max_window = 256;
            filter.set_option(FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW, bid_width_max_window);
            Assert.AreEqual(bid_width_max_window, filter.get_option<uint>(FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW));
        }


    }
}
