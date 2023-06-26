using LSDW.Domain.Models;

namespace LSDW.Infrastructure.Tests.Services;

public partial class SettingsServiceTests
{
	[TestMethod]
	public void GetDownTimeInHoursTest()
	{
		int i = _settingsService.DealerSettings.GetDownTimeInHours();

		Assert.AreNotEqual(0, i);
	}

	[TestMethod]
	public void SetDownTimeInHoursTest()
	{
		int value = 72;
		_settingsService.DealerSettings.SetDownTimeInHours(value);

		int i = _settingsService.DealerSettings.GetDownTimeInHours();

		Assert.AreEqual(value, i);
		Assert.AreEqual(value, Settings.Dealer.DownTimeInHours);
	}

	[TestMethod]
	public void GetHasArmorTest()
	{
		bool b = _settingsService.DealerSettings.GetHasArmor();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetHasArmorTest()
	{
		bool value = false;
		_settingsService.DealerSettings.SetHasArmor(value);

		bool b = _settingsService.DealerSettings.GetHasArmor();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Dealer.HasArmor);
	}

	[TestMethod]
	public void GetHasWeaponsTest()
	{
		bool b = _settingsService.DealerSettings.GetHasWeapons();

		Assert.IsTrue(b);
	}

	[TestMethod]
	public void SetHasWeaponsTest()
	{
		bool value = false;
		_settingsService.DealerSettings.SetHasWeapons(value);

		bool b = _settingsService.DealerSettings.GetHasWeapons();

		Assert.IsFalse(b);
		Assert.IsFalse(Settings.Dealer.HasWeapons);
	}

	[TestMethod]
	public void GetDownTimeInHoursValuesTest()
	{
		List<int> values = _settingsService.DealerSettings.GetDownTimeInHoursValues();

		Assert.IsTrue(!Equals(values.Count, 0));
	}
}
