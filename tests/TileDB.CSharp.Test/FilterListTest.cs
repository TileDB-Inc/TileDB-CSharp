﻿using System;
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
            var ctx = TileDB.Context.GetDefault();
            using var filter_list = new FilterList(ctx);
            var gzipFilter = new Filter(ctx, FilterType.TILEDB_FILTER_GZIP);
            var gzip_compression_level = 3;
            gzipFilter.set_option(FilterOption.TILEDB_COMPRESSION_LEVEL, gzip_compression_level);
            filter_list.add_filter(gzipFilter);

            var bit_shuffle_filter = new Filter(ctx, FilterType.TILEDB_FILTER_BITSHUFFLE);
            filter_list.add_filter(bit_shuffle_filter);

            var positive_delta_filter = new Filter(ctx, FilterType.TILEDB_FILTER_POSITIVE_DELTA);
            const uint positive_delta_max_window = 1024;
            positive_delta_filter.set_option(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW, positive_delta_max_window);
            filter_list.add_filter(positive_delta_filter);

            const uint max_chunk_size = 512;
            filter_list.set_max_chunk_size(max_chunk_size);

            const uint num_filters = 3;

            Assert.AreEqual(num_filters, filter_list.nfilters());
            Assert.AreEqual(max_chunk_size, filter_list.max_chunk_size());

            var filter0 = filter_list.filter(0);
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filter0.filter_type());
            Assert.AreEqual(gzip_compression_level, filter0.get_option<int>(FilterOption.TILEDB_COMPRESSION_LEVEL));

            var filter1 = filter_list.filter(1);
            Assert.AreEqual(FilterType.TILEDB_FILTER_BITSHUFFLE, filter1.filter_type());

            var filter2 = filter_list.filter(2);
            Assert.AreEqual(FilterType.TILEDB_FILTER_POSITIVE_DELTA, filter2.filter_type());
            Assert.AreEqual(positive_delta_max_window, filter2.get_option<uint>(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW));
        }
    }


}
