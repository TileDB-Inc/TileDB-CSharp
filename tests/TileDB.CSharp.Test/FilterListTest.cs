using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class FilterListTest
    {
        [TestMethod]
        public void NewFilterListIsValid()
        {
            using(var filterlist = new TileDB.FilterList()) 
            {
                Assert.AreNotEqual(null, filterlist);

                TileDB.Filter gzipFilter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_GZIP);
                int gzip_compression_level = 3;
                gzipFilter.set_option<int>(TileDB.FilterOption.TILEDB_COMPRESSION_LEVEL, gzip_compression_level);
                filterlist.add_filter(gzipFilter);

                TileDB.Filter bitshuffleFilter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_BITSHUFFLE);
                filterlist.add_filter(bitshuffleFilter);

                TileDB.Filter positivedeltaFilter = new TileDB.Filter(TileDB.FilterType.TILEDB_FILTER_POSITIVE_DELTA);
                UInt32 positive_delta_max_window = 1024;
                positivedeltaFilter.set_option<UInt32>(TileDB.FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW, positive_delta_max_window);
                filterlist.add_filter(positivedeltaFilter);

                UInt32 max_chunk_size = 512;
                filterlist.set_max_chunk_size(max_chunk_size);

                UInt32 nfilters = 3;

                Assert.AreEqual(nfilters, filterlist.nfilters());
                Assert.AreEqual(max_chunk_size, filterlist.max_chunk_size());

                TileDB.Filter filter0 = filterlist.filter(0);
    //            Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_GZIP, filter0.filter_type());
    //            Assert.AreEqual(gzip_compression_level, filter0.get_option<int>(TileDB.FilterOption.TILEDB_COMPRESSION_LEVEL));

                TileDB.Filter filter1 = filterlist.filter(1);
                Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_BITSHUFFLE, filter1.filter_type());

                TileDB.Filter filter2 = filterlist.filter(2);
                Assert.AreEqual(TileDB.FilterType.TILEDB_FILTER_POSITIVE_DELTA, filter2.filter_type());
                Assert.AreEqual(positive_delta_max_window, filter2.get_option<UInt32>(TileDB.FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW));


            }
        }
    }


}
