namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void TraffickingSettingsInstanceTest()
	{
		Assert.IsNotNull(_settings.Trafficking.BustChance);
		Assert.IsNotNull(_settings.Trafficking.DiscoverDealer);
		Assert.IsNotNull(_settings.Trafficking.WantedLevel);
	}

	[TestMethod]
	public void GetBustChanceValuesTest()
	{
		float[]? values = _settings.Trafficking.GetBustChanceValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetWantedLevelValuesTest()
	{
		int[]? values = _settings.Trafficking.GetWantedLevelValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}
}
