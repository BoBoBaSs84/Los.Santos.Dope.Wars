namespace LSDW.Domain.Tests.Models;

public partial class SettingsTests
{
	[TestMethod]
	public void PlayerSettingsInstanceTest()
	{
		Assert.IsNotNull(_settings.Player.ExperienceMultiplier);
		Assert.IsNotNull(_settings.Player.InventoryExpansionPerLevel);
		Assert.IsNotNull(_settings.Player.LooseDrugsOnDeath);
		Assert.IsNotNull(_settings.Player.LooseDrugsWhenBusted);
		Assert.IsNotNull(_settings.Player.LooseMoneyOnDeath);
		Assert.IsNotNull(_settings.Player.LooseMoneyWhenBusted);
		Assert.IsNotNull(_settings.Player.StartingInventory);
	}

	[TestMethod]
	public void GetExperienceMultiplierValuesTest()
	{
		float[]? values = _settings.Player.GetExperienceMultiplierValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetInventoryExpansionPerLevelValuesTest()
	{
		int[]? values = _settings.Player.GetInventoryExpansionPerLevelValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}

	[TestMethod]
	public void GetStartingInventoryValuesTest()
	{
		int[]? values = _settings.Player.GetStartingInventoryValues();

		Assert.IsNotNull(values);
		Assert.AreNotEqual(0, values.Length);
	}
}
