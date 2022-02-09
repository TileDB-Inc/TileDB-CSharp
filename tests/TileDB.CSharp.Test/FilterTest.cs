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
        public void NewBitshuffleFilterIsValid()
        {
            using(var filter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_BITSHUFFLE)) 
            {
                Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_BITSHUFFLE, filter.filter_type());
            }
        }

        [TestMethod]
        public void NewGzipFilterIsValid()
        {
            using (var filter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_GZIP)) 
            {

                Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_GZIP, filter.filter_type());
                
                int compression_level = 6;
                filter.set_option<int>(TileDB.FilterOption.TILEDB_COMPRESSION_LEVEL, compression_level);
                Assert.AreEqual(compression_level, filter.get_option<int>(TileDB.FilterOption.TILEDB_COMPRESSION_LEVEL));
            }
        }

        [TestMethod]
        public void NewPositiveDeltaFilterIsValid()
        {
            using (var filter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_POSITIVE_DELTA)) 
            {
                Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_POSITIVE_DELTA, filter.filter_type());

                UInt32 positive_delta_max_window = 1024;
                filter.set_option<UInt32>(TileDB.FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW, positive_delta_max_window);
                Assert.AreEqual(positive_delta_max_window, filter.get_option<UInt32>(TileDB.FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW));
            }

        }

        [TestMethod]
        public void NewBitWidthReductionFilterIsValid()
        {
            using (var filter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_BIT_WIDTH_REDUCTION)) 
            {

                Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_BIT_WIDTH_REDUCTION, filter.filter_type());

                UInt32 bid_width_max_window = 256;
                filter.set_option<UInt32>(TileDB.FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW, bid_width_max_window);
                Assert.AreEqual(bid_width_max_window, filter.get_option<UInt32>(TileDB.FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW));
            }
        }


    }
}
