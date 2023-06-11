using LSDW.Domain.Classes.Models;

namespace LSDW.Infrastructure.Tests.Services;

public partial class SettingsServiceTests
{
	[TestMethod]
	public void GetMaximumDrugValueTest()
	{
		float f = _settingsService.MarketSettings.GetMaximumDrugPrice();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetMaximumDrugValueTest()
	{
		float value = 1.8f;
		_settingsService.MarketSettings.SetMaximumDrugPrice(value);

		float f = _settingsService.MarketSettings.GetMaximumDrugPrice();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MaximumDrugPrice);
	}

	[TestMethod]
	public void GetMaximumDrugPriceValuesTest()
	{
		List<float> values = _settingsService.MarketSettings.GetMaximumDrugPriceValues();

		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetMinimumDrugValueTest()
	{
		float f = _settingsService.MarketSettings.GetMinimumDrugPrice();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetMinimumDrugValueTest()
	{
		float value = 0.2f;
		_settingsService.MarketSettings.SetMinimumDrugPrice(value);

		float f = _settingsService.MarketSettings.GetMinimumDrugPrice();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MinimumDrugPrice);
	}

	[TestMethod]
	public void GetMinimumDrugPriceValuesTest()
	{
		List<float> values = _settingsService.MarketSettings.GetMinimumDrugPriceValues();

		Assert.IsTrue(values.Any());
	}
}
