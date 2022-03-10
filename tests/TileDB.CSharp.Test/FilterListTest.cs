using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class FilterListTest
    {
        [TestMethod]
        public void NewFilterListIsValid()
        {
            var ctx = Context.GetDefault();
            using var filter_list = new FilterList(ctx);
            var gzipFilter = new Filter(ctx, FilterType.TILEDB_FILTER_GZIP);
            var gzip_compression_level = 3;
            gzipFilter.SetOption(FilterOption.TILEDB_COMPRESSION_LEVEL, gzip_compression_level);
            filter_list.AddFilter(gzipFilter);

            var bit_shuffle_filter = new Filter(ctx, FilterType.TILEDB_FILTER_BITSHUFFLE);
            filter_list.AddFilter(bit_shuffle_filter);

            var positive_delta_filter = new Filter(ctx, FilterType.TILEDB_FILTER_POSITIVE_DELTA);
            const uint positiveDeltaMaxWindow = 1024;
            positive_delta_filter.SetOption(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW, positiveDeltaMaxWindow);
            filter_list.AddFilter(positive_delta_filter);

            const uint maxChunkSize = 512;
            filter_list.SetMaxChunkSize(maxChunkSize);

            const uint numFilters = 3;

            Assert.AreEqual(numFilters, filter_list.NFilters());
            Assert.AreEqual(maxChunkSize, filter_list.MaxChunkSize());

            var filter0 = filter_list.Filter(0);
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filter0.FilterType());
            Assert.AreEqual(gzip_compression_level, filter0.GetOption<int>(FilterOption.TILEDB_COMPRESSION_LEVEL));

            var filter1 = filter_list.Filter(1);
            Assert.AreEqual(FilterType.TILEDB_FILTER_BITSHUFFLE, filter1.FilterType());

            var filter2 = filter_list.Filter(2);
            Assert.AreEqual(FilterType.TILEDB_FILTER_POSITIVE_DELTA, filter2.FilterType());
            Assert.AreEqual(positiveDeltaMaxWindow, filter2.GetOption<uint>(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW));
        }
    }


}
