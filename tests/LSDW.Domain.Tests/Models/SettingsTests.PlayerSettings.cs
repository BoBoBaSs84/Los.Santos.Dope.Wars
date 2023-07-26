namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void GetExperienceMultiplierValuesTest()
	{
		float[] values = _settings.Player.GetExperienceMultiplierValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetInventoryExpansionPerLevelValuesTest()
	{
		int[] values = _settings.Player.GetInventoryExpansionPerLevelValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}

	[TestMethod]
	public void GetStartingInventoryValuesTest()
	{
		int[] values = _settings.Player.GetStartingInventoryValues();

		Assert.IsNotNull(values);
		Assert.IsTrue(values.Any());
	}
}
