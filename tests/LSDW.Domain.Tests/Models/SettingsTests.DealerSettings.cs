namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void DealerSettingsInstanceTest()
	{
		Assert.IsNotNull(_settings.Dealer.DownTimeInHours);
		Assert.IsNotNull(_settings.Dealer.HasArmor);
		Assert.IsNotNull(_settings.Dealer.HasWeapons);
	}

	[TestMethod]
	public void GetDownTimeInHoursValuesTest()
	{
		int[] values = _settings.Dealer.GetDownTimeInHoursValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}
}