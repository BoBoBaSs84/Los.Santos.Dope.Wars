using LSDW.Core.Classes;
using LSDW.Factories;
using LSDW.Interfaces.Services;

namespace LSDW.Tests.Classes.Services;

[TestClass]
public class SettingsServiceTests
{
	private static readonly ISettingsService _settingsService = ServiceFactory.CreateSettingsService();

	[ClassInitialize]
	public static void ClassInitialize(TestContext context)
	{
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

	}

	[TestMethod]
	public void SetHasWeaponsTest()
	{

	}

	[TestMethod]
	public void GetMaximumDrugValueTest()
	{

	}

	[TestMethod]
	public void SetMaximumDrugValueTest()
	{

	}

	[TestMethod]
	public void SetMinimumDrugValueTest()
	{

	}

	[TestMethod]
	public void GetExperienceMultiplierTest()
	{

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