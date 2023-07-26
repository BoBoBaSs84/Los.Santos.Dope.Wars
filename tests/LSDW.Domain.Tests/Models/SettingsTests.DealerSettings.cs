namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void GetDownTimeInHoursValuesTest()
	{
		int[] values = _settings.Dealer.GetDownTimeInHoursValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}
}