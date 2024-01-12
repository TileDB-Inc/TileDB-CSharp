using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class FilterListTest
{
    [TestMethod]
    public void NewFilterListIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter_list = new FilterList(ctx);
        var gzipFilter = new Filter(ctx, FilterType.Gzip);
        var gzip_compression_level = 3;
        gzipFilter.SetOption(FilterOption.CompressionLevel, gzip_compression_level);
        filter_list.AddFilter(gzipFilter);

        var bit_shuffle_filter = new Filter(ctx, FilterType.BitShuffle);
        filter_list.AddFilter(bit_shuffle_filter);

        var positive_delta_filter = new Filter(ctx, FilterType.PositiveDelta);
        const uint positiveDeltaMaxWindow = 1024;
        positive_delta_filter.SetOption(FilterOption.PositiveDeltaMaxWindow, positiveDeltaMaxWindow);
        filter_list.AddFilter(positive_delta_filter);

        const uint maxChunkSize = 512;
        filter_list.SetMaxChunkSize(maxChunkSize);

        const uint numFilters = 3;

        Assert.AreEqual(numFilters, filter_list.NFilters());
        Assert.AreEqual(maxChunkSize, filter_list.MaxChunkSize());

        var filter0 = filter_list.Filter(0);
        Assert.AreEqual(FilterType.Gzip, filter0.FilterType());
        Assert.AreEqual(gzip_compression_level, filter0.GetOption<int>(FilterOption.CompressionLevel));

        var filter1 = filter_list.Filter(1);
        Assert.AreEqual(FilterType.BitShuffle, filter1.FilterType());

        var filter2 = filter_list.Filter(2);
        Assert.AreEqual(FilterType.PositiveDelta, filter2.FilterType());
        Assert.AreEqual(positiveDeltaMaxWindow, filter2.GetOption<uint>(FilterOption.PositiveDeltaMaxWindow));
    }
}
