using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TileDB.CSharp.Test;

[TestClass]
public class CoreUtilTest
{
    [TestMethod]
    public void BuildConfigurationNonEmpty()
    {
        var buildConfiguration = CoreUtil.GetBuildConfiguration();

        Assert.IsFalse(string.IsNullOrWhiteSpace(buildConfiguration));
    }

    [TestMethod]
    public void TestStats()
    {
        Stats.Disable();
        Assert.IsFalse(Stats.IsEnabled);
        Stats.Enable();
        Assert.IsTrue(Stats.IsEnabled);
    }
}
