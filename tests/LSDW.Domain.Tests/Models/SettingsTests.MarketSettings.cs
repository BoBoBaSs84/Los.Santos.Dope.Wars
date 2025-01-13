namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void MarketSettingsInstanceTest()
	{
		Assert.IsNotNull(_settings.Market.InventoryChangeInterval);
		Assert.IsNotNull(_settings.Market.MaximumDrugPrice);
		Assert.IsNotNull(_settings.Market.MinimumDrugPrice);
		Assert.IsNotNull(_settings.Market.PriceChangeInterval);
		Assert.IsNotNull(_settings.Market.SpecialOfferChance);
	}

	[TestMethod]
	public void GetSpecialOfferChanceValuesTest()
	{
		float[]? values = _settings.Market.GetSpecialOfferChanceValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetInventoryChangeIntervalValuesTest()
	{
		int[]? values = _settings.Market.GetInventoryChangeIntervalValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetPriceChangeIntervalValuesTest()
	{
		int[]? values = _settings.Market.GetPriceChangeIntervalValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetMaximumDrugPriceValuesTest()
	{
		float[]? values = _settings.Market.GetMaximumDrugPriceValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetMinimumDrugPriceValuesTest()
	{
		float[]? values = _settings.Market.GetMinimumDrugPriceValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}
}
