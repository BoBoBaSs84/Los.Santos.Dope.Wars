using LSDW.Abstractions.Models;

namespace LSDW.Infrastructure.Tests.Services;

public partial class SettingsServiceTests
{
	[TestMethod]
	public void GetExperienceMultiplierTest()
	{
		float f = _settingsService.PlayerSettings.GetExperienceMultiplier();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetExperienceMultiplierTest()
	{
		float value = 0.2f;
		_settingsService.PlayerSettings.SetExperienceMultiplier(value);

		float f = _settingsService.PlayerSettings.GetExperienceMultiplier();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Player.ExperienceMultiplier);
	}

	[TestMethod]
	public void GetExperienceMultiplierValuesTest()
	{
		List<float> values = _settingsService.PlayerSettings.GetExperienceMultiplierValues();

		Assert.IsFalse(Equals(values.Count, 0));
	}

	[TestMethod]
	public void GetLooseDrugsOnDeathTest()
	{
		bool b = _settingsService.PlayerSettings.GetLooseDrugsOnDeath();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseDrugsOnDeathTest()
	{
		bool value = false;
		_settingsService.PlayerSettings.SetLooseDrugsOnDeath(value);

		bool b = _settingsService.PlayerSettings.GetLooseDrugsOnDeath();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseDrugsOnDeath);
	}

	[TestMethod]
	public void GetLooseMoneyOnDeathTest()
	{
		bool b = _settingsService.PlayerSettings.GetLooseMoneyOnDeath();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseMoneyOnDeathTest()
	{
		bool value = false;
		_settingsService.PlayerSettings.SetLooseMoneyOnDeath(value);

		bool b = _settingsService.PlayerSettings.GetLooseMoneyOnDeath();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseMoneyOnDeath);
	}

	[TestMethod]
	public void GetLooseDrugsWhenBustedTest()
	{
		bool b = _settingsService.PlayerSettings.GetLooseDrugsWhenBusted();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseDrugsWhenBustedTest()
	{
		bool value = false;
		_settingsService.PlayerSettings.SetLooseDrugsWhenBusted(value);

		bool b = _settingsService.PlayerSettings.GetLooseDrugsWhenBusted();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseDrugsWhenBusted);
	}

	[TestMethod]
	public void GetLooseMoneyWhenBustedTest()
	{
		bool b = _settingsService.PlayerSettings.GetLooseMoneyWhenBusted();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseMoneyWhenBustedTest()
	{
		bool value = false;
		_settingsService.PlayerSettings.SetLooseMoneyWhenBusted(value);

		bool b = _settingsService.PlayerSettings.GetLooseMoneyWhenBusted();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseMoneyWhenBusted);
	}

	[TestMethod]
	public void GetInventoryExpansionPerLevelTest()
	{
		int i = _settingsService.PlayerSettings.GetInventoryExpansionPerLevel();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetInventoryExpansionPerLevelTest()
	{
		int value = 100;
		_settingsService.PlayerSettings.SetInventoryExpansionPerLevel(value);

		int i = _settingsService.PlayerSettings.GetInventoryExpansionPerLevel();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Player.InventoryExpansionPerLevel);
	}

	[TestMethod]
	public void GetInventoryExpansionPerLevelValuesTest()
	{
		List<int> values = _settingsService.PlayerSettings.GetInventoryExpansionPerLevelValues();

		Assert.IsFalse(Equals(values.Count, 0));
	}

	[TestMethod]
	public void GetStartingInventoryTest()
	{
		int i = _settingsService.PlayerSettings.GetStartingInventory();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetStartingInventoryTest()
	{
		int value = 500;
		_settingsService.PlayerSettings.SetStartingInventory(value);

		int i = _settingsService.PlayerSettings.GetStartingInventory();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Player.StartingInventory);
	}

	[TestMethod]
	public void GetStartingInventoryValuesTest()
	{
		List<int> values = _settingsService.PlayerSettings.GetStartingInventoryValues();

		Assert.IsFalse(Equals(values.Count, 0));
	}
}
