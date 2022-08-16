using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test
{
    [TestClass]
    public class FilterTest
    {
        [TestMethod]
        public void NewBitShuffleFilterIsValid()
        {
            var ctx = Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_BITSHUFFLE);
            Assert.AreEqual(FilterType.TILEDB_FILTER_BITSHUFFLE, filter.FilterType());
        }

        [TestMethod]
        public void NewGzipFilterIsValid()
        {
            var ctx = Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_GZIP);
            Assert.AreEqual(FilterType.TILEDB_FILTER_GZIP, filter.FilterType());

            const int compressionLevel = 6;
            filter.SetOption(FilterOption.TILEDB_COMPRESSION_LEVEL, compressionLevel);
            Assert.AreEqual(compressionLevel, filter.GetOption<int>(FilterOption.TILEDB_COMPRESSION_LEVEL));
        }

        [TestMethod]
        public void NewPositiveDeltaFilterIsValid()
        {
            var ctx = Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_POSITIVE_DELTA);
            Assert.AreEqual(FilterType.TILEDB_FILTER_POSITIVE_DELTA, filter.FilterType());

            const uint positiveDeltaMaxWindow = 1024;
            filter.SetOption(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW, positiveDeltaMaxWindow);
            Assert.AreEqual(positiveDeltaMaxWindow, filter.GetOption<uint>(FilterOption.TILEDB_POSITIVE_DELTA_MAX_WINDOW));
        }

        [TestMethod]
        public void NewBitWidthReductionFilterIsValid()
        {
            var ctx = Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_BIT_WIDTH_REDUCTION);
            Assert.AreEqual(FilterType.TILEDB_FILTER_BIT_WIDTH_REDUCTION, filter.FilterType());

            const uint bidWidthMaxWindow = 256;
            filter.SetOption(FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW, bidWidthMaxWindow);
            Assert.AreEqual(bidWidthMaxWindow, filter.GetOption<uint>(FilterOption.TILEDB_BIT_WIDTH_MAX_WINDOW));
        }

        [TestMethod]
        public void NewDictionaryFilterIsValid()
        {
            var ctx = Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_DICTIONARY);
            Assert.AreEqual(FilterType.TILEDB_FILTER_DICTIONARY, filter.FilterType());

            const int compressionLevel = 4;
            filter.SetOption(FilterOption.TILEDB_COMPRESSION_LEVEL, compressionLevel);
            Assert.AreEqual(compressionLevel, filter.GetOption<int>(FilterOption.TILEDB_COMPRESSION_LEVEL));
        }

        [TestMethod]
        public void NewFloatScalingFilterIsValid()
        {
            if (!TestUtil.VersionMinimum(2, 11))
            {
                return;
            }

            var ctx = Context.GetDefault();
            using var filter = new Filter(ctx, FilterType.TILEDB_FILTER_SCALE_FLOAT);
            Assert.AreEqual(FilterType.TILEDB_FILTER_SCALE_FLOAT, filter.FilterType());

            const double scale = 4.0;
            filter.SetOption(FilterOption.TILEDB_SCALE_FLOAT_FACTOR, scale);
            Assert.AreEqual(scale, filter.GetOption<double>(FilterOption.TILEDB_SCALE_FLOAT_FACTOR));

            const double offset = 1.0;
            filter.SetOption(FilterOption.TILEDB_SCALE_FLOAT_OFFSET, offset);
            Assert.AreEqual(offset, filter.GetOption<double>(FilterOption.TILEDB_SCALE_FLOAT_OFFSET));

            const ulong byteWidth = 4;
            filter.SetOption(FilterOption.TILEDB_SCALE_FLOAT_BYTEWIDTH, byteWidth);
            Assert.AreEqual(byteWidth, filter.GetOption<ulong>(FilterOption.TILEDB_SCALE_FLOAT_BYTEWIDTH));
        }
    }
}
