namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void GetBustChanceValuesTest()
	{
		float[] values = _settings.Trafficking.GetBustChanceValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetWantedLevelValuesTest()
	{
		int[] values = _settings.Trafficking.GetWantedLevelValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}
}
