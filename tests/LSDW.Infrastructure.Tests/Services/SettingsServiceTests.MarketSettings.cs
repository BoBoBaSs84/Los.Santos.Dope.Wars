using LSDW.Domain.Classes.Models;

namespace LSDW.Infrastructure.Tests.Services;

public partial class SettingsServiceTests
{
	[TestMethod]
	public void GetMaximumDrugValueTest()
	{
		float f = _settingsService.MarketSettings.GetMaximumDrugValue();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetMaximumDrugValueTest()
	{
		float value = 1.8f;
		_settingsService.MarketSettings.SetMaximumDrugValue(value);

		float f = _settingsService.MarketSettings.GetMaximumDrugValue();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MaximumDrugValue);
	}

	[TestMethod]
	public void GetMinimumDrugValueTest()
	{
		float f = _settingsService.MarketSettings.GetMinimumDrugValue();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetMinimumDrugValueTest()
	{
		float value = 0.2f;
		_settingsService.MarketSettings.SetMinimumDrugValue(value);

		float f = _settingsService.MarketSettings.GetMinimumDrugValue();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MinimumDrugValue);
	}
}
