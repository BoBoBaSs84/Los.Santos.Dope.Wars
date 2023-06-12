using LSDW.Domain.Classes.Models;

namespace LSDW.Infrastructure.Tests.Services;

public partial class SettingsServiceTests
{
	[TestMethod]
	public void GetInventoryChangeIntervalTest()
	{
		int i = _settingsService.MarketSettings.GetInventoryChangeInterval();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetInventoryChangeIntervalTest()
	{
		int value = 99;
		_settingsService.MarketSettings.SetInventoryChangeInterval(value);

		int i = _settingsService.MarketSettings.GetInventoryChangeInterval();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Market.InventoryChangeInterval);
	}

	[TestMethod]
	public void GetInventoryChangeIntervalValues()
	{
		List<int> values = _settingsService.MarketSettings.GetInventoryChangeIntervalValues();

		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetPriceChangeIntervalTest()
	{
		int i = _settingsService.MarketSettings.GetPriceChangeInterval();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetPriceChangeIntervalTest()
	{
		int value = 99;
		_settingsService.MarketSettings.SetPriceChangeInterval(value);

		int i = _settingsService.MarketSettings.GetPriceChangeInterval();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Market.PriceChangeInterval);
	}

	[TestMethod]
	public void GetPriceChangeIntervalValuesTest()
	{
		List<int> values = _settingsService.MarketSettings.GetPriceChangeIntervalValues();

		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetMaximumDrugValueTest()
	{
		float f = _settingsService.MarketSettings.GetMaximumDrugPrice();

		Assert.AreNotEqual(default, f);
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

		Assert.AreNotEqual(default, f);
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
