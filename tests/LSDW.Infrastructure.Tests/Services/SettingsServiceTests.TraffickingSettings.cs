using LSDW.Abstractions.Models;

namespace LSDW.Infrastructure.Tests.Services;

public partial class SettingsServiceTests
{
	[TestMethod]
	public void GetDiscoverDealerTest()
	{
		bool b = _settingsService.TraffickingSettings.GetDiscoverDealer();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetDiscoverDealerTest()
	{
		bool value = false;
		_settingsService.TraffickingSettings.SetDiscoverDealer(value);

		bool b = _settingsService.TraffickingSettings.GetDiscoverDealer();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Trafficking.DiscoverDealer);
	}

	[TestMethod]
	public void GetBustChanceTest()
	{
		float f = _settingsService.TraffickingSettings.GetBustChance();

		Assert.AreNotEqual(default, f);
	}

	[TestMethod]
	public void SetBustChanceTest()
	{
		float value = 0.25f;
		_settingsService.TraffickingSettings.SetBustChance(value);

		float f = _settingsService.TraffickingSettings.GetBustChance();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Trafficking.BustChance);
	}

	[TestMethod]
	public void GetBustChanceValuesTest()
	{
		List<float> values = _settingsService.TraffickingSettings.GetBustChanceValues();

		Assert.IsFalse(Equals(values.Count, 0));
	}

	[TestMethod]
	public void GetWantedLevelTest()
	{
		int i = _settingsService.TraffickingSettings.GetWantedLevel();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetWantedLevelTest()
	{
		int value = 3;
		_settingsService.TraffickingSettings.SetWantedLevel(value);

		int i = _settingsService.TraffickingSettings.GetWantedLevel();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Trafficking.WantedLevel);
	}

	[TestMethod]
	public void GetWantedLevelValues()
	{
		List<int> values = _settingsService.TraffickingSettings.GetWantedLevelValues();

		Assert.IsFalse(Equals(values.Count, 0));
	}
}
