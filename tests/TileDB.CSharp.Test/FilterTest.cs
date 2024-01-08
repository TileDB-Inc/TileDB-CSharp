using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class FilterTest
{
    [TestMethod]
    public void NewBitShuffleFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.BitShuffle);
        Assert.AreEqual(FilterType.BitShuffle, filter.FilterType());
    }

    [TestMethod]
    public void NewGzipFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.Gzip);
        Assert.AreEqual(FilterType.Gzip, filter.FilterType());

        const int compressionLevel = 6;
        filter.SetOption(FilterOption.CompressionLevel, compressionLevel);
        Assert.AreEqual(compressionLevel, filter.GetOption<int>(FilterOption.CompressionLevel));
    }

    [TestMethod]
    public void NewPositiveDeltaFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.PositiveDelta);
        Assert.AreEqual(FilterType.PositiveDelta, filter.FilterType());

        const uint positiveDeltaMaxWindow = 1024;
        filter.SetOption(FilterOption.PositiveDeltaMaxWindow, positiveDeltaMaxWindow);
        Assert.AreEqual(positiveDeltaMaxWindow, filter.GetOption<uint>(FilterOption.PositiveDeltaMaxWindow));
    }

    [TestMethod]
    public void NewBitWidthReductionFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.BitWidthReduction);
        Assert.AreEqual(FilterType.BitWidthReduction, filter.FilterType());

        const uint bidWidthMaxWindow = 256;
        filter.SetOption(FilterOption.BitWidthMaxWindow, bidWidthMaxWindow);
        Assert.AreEqual(bidWidthMaxWindow, filter.GetOption<uint>(FilterOption.BitWidthMaxWindow));
    }

    [TestMethod]
    public void NewDictionaryFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.Dictionary);
        Assert.AreEqual(FilterType.Dictionary, filter.FilterType());

        const int compressionLevel = 4;
        filter.SetOption(FilterOption.CompressionLevel, compressionLevel);
        Assert.AreEqual(compressionLevel, filter.GetOption<int>(FilterOption.CompressionLevel));
    }

    [TestMethod]
    public void NewFloatScalingFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.ScaleFloat);
        Assert.AreEqual(FilterType.ScaleFloat, filter.FilterType());

        const double scale = 4.0;
        filter.SetOption(FilterOption.ScaleFloatFactor, scale);
        Assert.AreEqual(scale, filter.GetOption<double>(FilterOption.ScaleFloatFactor));

        const double offset = 1.0;
        filter.SetOption(FilterOption.ScaleFloatOffset, offset);
        Assert.AreEqual(offset, filter.GetOption<double>(FilterOption.ScaleFloatOffset));

        const ulong byteWidth = 4;
        filter.SetOption(FilterOption.ScaleFloatByteWidth, byteWidth);
        Assert.AreEqual(byteWidth, filter.GetOption<ulong>(FilterOption.ScaleFloatByteWidth));
    }

    [TestMethod]
    public void NewWebpFilterIsValid()
    {
        var ctx = Context.GetDefault();
        using var filter = new Filter(ctx, FilterType.Webp);
        Assert.AreEqual(FilterType.Webp, filter.FilterType());

        const float quality = 59.0f;
        filter.SetOption(FilterOption.WebpQuality, quality);
        Assert.AreEqual(quality, filter.GetOption<float>(FilterOption.WebpQuality));

        const WebpInputFormat inputFormat = WebpInputFormat.Bgra;
        filter.SetOption(FilterOption.WebpInputFormat, inputFormat);
        Assert.AreEqual(inputFormat, filter.GetOption<WebpInputFormat>(FilterOption.WebpInputFormat));

        const bool lossless = false;
        filter.SetOption(FilterOption.WebpLossless, lossless);
        Assert.AreEqual(lossless, filter.GetOption<bool>(FilterOption.WebpLossless));
    }
}
