using LSDW.Domain.Classes.Models;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Services;

namespace LSDW.Domain.Tests.Services;

[TestClass]
public class SettingsServiceTests
{
	private static readonly ISettingsService _settingsService = DomainFactory.CreateSettingsService();
	private static readonly string _settingsFileName = Settings.SettingsFileName;

	[ClassCleanup]
	public static void ClassCleanup()
	{
		if (File.Exists(_settingsFileName))
			File.Delete(_settingsFileName);
	}

	[TestMethod]
	public void GetDownTimeInHoursTest()
	{
		int i = _settingsService.GetDownTimeInHours();

		Assert.AreNotEqual(0, i);
	}

	[TestMethod]
	public void SetDownTimeInHoursTest()
	{
		int value = 72;
		_settingsService.SetDownTimeInHours(value);

		int i = _settingsService.GetDownTimeInHours();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Dealer.DownTimeInHours);
	}

	[TestMethod]
	public void GetHasArmorTest()
	{
		bool b = _settingsService.GetHasArmor();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetHasArmorTest()
	{
		bool value = false;
		_settingsService.SetHasArmor(value);

		bool b = _settingsService.GetHasArmor();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Dealer.HasArmor);
	}

	[TestMethod]
	public void GetHasWeaponsTest()
	{
		bool b = _settingsService.GetHasWeapons();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetHasWeaponsTest()
	{
		bool value = false;
		_settingsService.SetHasWeapons(value);

		bool b = _settingsService.GetHasWeapons();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Dealer.HasArmor);
	}

	[TestMethod]
	public void GetMaximumDrugValueTest()
	{
		float f = _settingsService.GetMaximumDrugValue();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetMaximumDrugValueTest()
	{
		float value = 1.8f;
		_settingsService.SetMaximumDrugValue(value);

		float f = _settingsService.GetMaximumDrugValue();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MaximumDrugValue);
	}

	[TestMethod]
	public void GetMinimumDrugValueTest()
	{
		float f = _settingsService.GetMinimumDrugValue();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetMinimumDrugValueTest()
	{
		float value = 0.2f;
		_settingsService.SetMinimumDrugValue(value);

		float f = _settingsService.GetMinimumDrugValue();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MinimumDrugValue);
	}

	[TestMethod]
	public void GetExperienceMultiplierTest()
	{
		float f = _settingsService.GetExperienceMultiplier();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void SetExperienceMultiplierTest()
	{
		float value = 0.2f;
		_settingsService.SetExperienceMultiplier(value);

		float f = _settingsService.GetExperienceMultiplier();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Player.ExperienceMultiplier);
	}

	[TestMethod]
	public void GetLooseDrugsOnDeathTest()
	{
		bool b = _settingsService.GetLooseDrugsOnDeath();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseDrugsOnDeathTest()
	{
		bool value = false;
		_settingsService.SetLooseDrugsOnDeath(value);

		bool b = _settingsService.GetLooseDrugsOnDeath();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseDrugsOnDeath);
	}

	[TestMethod]
	public void GetLooseMoneyOnDeathTest()
	{
		bool b = _settingsService.GetLooseMoneyOnDeath();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseMoneyOnDeathTest()
	{
		bool value = false;
		_settingsService.SetLooseMoneyOnDeath(value);

		bool b = _settingsService.GetLooseMoneyOnDeath();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseMoneyOnDeath);
	}

	[TestMethod]
	public void GetLooseDrugsWhenBustedTest()
	{
		bool b = _settingsService.GetLooseDrugsWhenBusted();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseDrugsWhenBustedTest()
	{
		bool value = false;
		_settingsService.SetLooseDrugsWhenBusted(value);

		bool b = _settingsService.GetLooseDrugsWhenBusted();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseDrugsWhenBusted);
	}

	[TestMethod]
	public void GetLooseMoneyWhenBustedTest()
	{
		bool b = _settingsService.GetLooseMoneyWhenBusted();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetLooseMoneyWhenBustedTest()
	{
		bool value = false;
		_settingsService.SetLooseMoneyWhenBusted(value);

		bool b = _settingsService.GetLooseMoneyWhenBusted();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Player.LooseMoneyWhenBusted);
	}

	[TestMethod]
	public void GetInventoryExpansionPerLevelTest()
	{
		int i = _settingsService.GetInventoryExpansionPerLevel();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetInventoryExpansionPerLevelTest()
	{
		int value = 100;
		_settingsService.SetInventoryExpansionPerLevel(value);

		int i = _settingsService.GetInventoryExpansionPerLevel();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Player.InventoryExpansionPerLevel);
	}

	[TestMethod]
	public void GetStartingInventoryTest()
	{
		int i = _settingsService.GetStartingInventory();

		Assert.AreNotEqual(default, i);
	}

	[TestMethod]
	public void SetStartingInventoryTest()
	{
		int value = 500;
		_settingsService.SetStartingInventory(value);

		int i = _settingsService.GetStartingInventory();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Player.StartingInventory);
	}
}