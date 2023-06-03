using LSDW.Core.Models;
using LSDW.Factories;
using LSDW.Interfaces.Services;

namespace LSDW.Tests.Classes.Services;

[TestClass]
public class SettingsServiceTests
{
	private static readonly ISettingsService _settingsService = ServiceFactory.CreateSettingsService();

	[ClassCleanup]
	public static void ClassCleanup()
	{
		if (File.Exists(Settings.SettingsFileName))
			File.Delete(Settings.SettingsFileName);
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
	public void SetMinimumDrugValueTest()
	{
		float f = _settingsService.GetMinimumDrugValue();

		Assert.AreNotEqual(0, f);
	}

	[TestMethod]
	public void GetExperienceMultiplierTest()
	{
		float value = 0.2f;
		_settingsService.SetMinimumDrugValue(value);

		float f = _settingsService.GetMinimumDrugValue();

		Assert.AreEqual(value, f);
		Assert.AreEqual(value, Settings.Market.MinimumDrugValue);
	}

	[TestMethod]
	public void SetExperienceMultiplierTest()
	{

	}

	[TestMethod]
	public void GetLooseDrugsOnDeathTest()
	{

	}

	[TestMethod]
	public void SetLooseDrugsOnDeathTest()
	{

	}

	[TestMethod]
	public void GetLooseMoneyOnDeathTest()
	{

	}

	[TestMethod]
	public void SetLooseMoneyOnDeathTest()
	{

	}

	[TestMethod]
	public void GetLooseDrugsWhenBustedTest()
	{

	}

	[TestMethod]
	public void SetLooseDrugsWhenBustedTest()
	{

	}

	[TestMethod]
	public void GetLooseMoneyWhenBustedTest()
	{

	}

	[TestMethod]
	public void SetLooseMoneyWhenBustedTest()
	{

	}

	[TestMethod]
	public void GetInventoryExpansionPerLevelTest()
	{

	}

	[TestMethod]
	public void SetInventoryExpansionPerLevelTest()
	{

	}

	[TestMethod]
	public void GetStartingInventoryTest()
	{

	}

	[TestMethod]
	public void SetStartingInventoryTest()
	{

	}
}