namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void GetSpecialOfferChanceValuesTest()
	{
		float[] values = _settings.Market.GetSpecialOfferChanceValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetInventoryChangeIntervalValuesTest()
	{
		int[] values = _settings.Market.GetInventoryChangeIntervalValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetPriceChangeIntervalValuesTest()
	{
		int[] values = _settings.Market.GetPriceChangeIntervalValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetMaximumDrugPriceValuesTest()
	{
		float[] values = _settings.Market.GetMaximumDrugPriceValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetMinimumDrugPriceValuesTest()
	{
		float[] values = _settings.Market.GetMinimumDrugPriceValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}
}
