using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class ConfigTest
{
    [TestMethod]
    public void ConfigSet()
    {
        var config = new Config();
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);
    }

    [TestMethod]
    public void ConfigGetNonExistent()
    {
        var config = new Config();
        var val = config.Get("sm.does_not_exist");
        Assert.AreEqual("", val);
    }

    [TestMethod]
    public void ConfigUnSet()
    {
        var config = new Config();
        var defaultVal = config.Get("sm.tile_cache_size");
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);
        config.Unset("sm.tile_cache_size");
        val = config.Get("sm.tile_cache_size");
        Assert.AreEqual(defaultVal, val);
    }

    [TestMethod]
    public void ConfigFromFile()
    {
        var config = new Config();
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);

        // Create temporary path for testing configuration writing/reading
        var tmpPath = TestUtil.MakeTestPath("tmp.cfg");

        config.SaveToFile(tmpPath);

        var config2 = new Config();
        config2.LoadFromFile(tmpPath);

        val = config2.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);
    }

    [TestMethod]
    public void ConfigCompare()
    {
        var config = new Config();
        config.Set("sm.tile_cache_size", "10");
        var val = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val);

        var config2 = new Config();
        config2.Set("sm.tile_cache_size", "10");
        var val2 = config.Get("sm.tile_cache_size");
        Assert.AreEqual("10", val2);

        var cmp2 = config.Cmp(ref config2);
        Assert.IsTrue(cmp2);

        var config3 = new Config();
        config3.Set("sm.tile_cache_size", "11");
        var val3 = config3.Get("sm.tile_cache_size");
        Assert.AreEqual("11", val3);

        var cmp3 = config.Cmp(ref config3);
        Assert.IsFalse(cmp3);
    }

    [TestMethod]
    public void ConfigIterator()
    {
        const string Prefix = "vfs.s3.";
        var config = new Config();
        bool sawItem = false;

        foreach (var config_entry_pair in config.EnumerateOptions(Prefix))
        {
            sawItem = true;
            Assert.AreEqual(config_entry_pair.Value, config.Get(Prefix + config_entry_pair.Key));
        }
        Assert.IsTrue(sawItem);
    }
}
