using GTA;
using LSDW.Core.Classes;
using LSDW.Interfaces.Services;
using Dealer = LSDW.Core.Classes.Settings.Dealer;
using Market = LSDW.Core.Classes.Settings.Market;
using Player = LSDW.Core.Classes.Settings.Player;

namespace LSDW.Classes.Services;

/// <summary>
/// The settings service class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="ScriptSettings"/>.
/// </remarks>
public sealed class SettingsService : ISettingsService
{
	private readonly ScriptSettings _scriptSettings;

	/// <summary>
	/// Initializes a instance of the settings service class.
	/// </summary>
	public SettingsService()
	{
		string settingsFileName = Path.Combine(AppContext.BaseDirectory, Settings.SettingsFileName);
		_scriptSettings = ScriptSettings.Load(settingsFileName);
    
    Load();
    Save();
	}

  public void Load()
  {
    int downtimeinhours = GetDownTimeInHours();
    SetDownTimeInHours(downtimeinhours);
    bool hasarmor = GetHasArmor();
    SetHasArmor(hasarmor);
    bool hasweapons = GetHasWeapons();
    SetHasWeapons(hasweapons);
    float maximumdrugvalue = GetMaximumDrugValue();
    SetMaximumDrugValue(maximumdrugvalue);
    float minimumdrugvalue = GetMinimumDrugValue();
    SetMinimumDrugValue(minimumdrugvalue);
    float experiencemultiplier = GetExperienceMultiplier();
    SetExperienceMultiplier(experiencemultiplier);
    bool loosedrugsondeath = GetLooseDrugsOnDeath();
    SetLooseDrugsOnDeath(loosedrugsondeath);
    bool loosemoneyondeath = GetLooseMoneyOnDeath();
    SetLooseMoneyOnDeath(loosemoneyondeath);
    bool loosedrugswhenbusted = GetLooseDrugsWhenBusted();
    SetLooseDrugsWhenBusted(loosedrugswhenbusted);
    bool loosemoneywhenbusted = GetLooseMoneyWhenBusted();
    SetLooseMoneyWhenBusted(loosemoneywhenbusted);
    int inventoryexpansionperlevel = GetInventoryExpansionPerLevel();
    SetInventoryExpansionPerLevel(inventoryexpansionperlevel);
    int startinginventory = GetStartingInventory();
    SetStartingInventory(startinginventory);
  }

  public void Save()
    => _scriptSettings.Save();

	public int GetDownTimeInHours()
    => _scriptSettings.GetValue("DEALERSETTINGS", "DOWNTIMEINHOURS", 48);

	public void SetDownTimeInHours(int value)
  {
		_scriptSettings.SetValue("DEALERSETTINGS", "DOWNTIMEINHOURS", value);
		Dealer.DownTimeInHours = value;
  }

	public bool GetHasArmor()
    => _scriptSettings.GetValue("DEALERSETTINGS", "HASARMOR", true);

	public void SetHasArmor(bool value)
  {
		_scriptSettings.SetValue("DEALERSETTINGS", "HASARMOR", value);
		Dealer.HasArmor = value;
  }

	public bool GetHasWeapons()
    => _scriptSettings.GetValue("DEALERSETTINGS", "HASWEAPONS", true);

	public void SetHasWeapons(bool value)
  {
		_scriptSettings.SetValue("DEALERSETTINGS", "HASWEAPONS", value);
		Dealer.HasWeapons = value;
  }

	public float GetMaximumDrugValue()
    => _scriptSettings.GetValue("MARKETSETTINGS", "MAXIMUMDRUGVALUE", 1.2f);

	public void SetMaximumDrugValue(float value)
  {
		_scriptSettings.SetValue("MARKETSETTINGS", "MAXIMUMDRUGVALUE", value);
		Market.MaximumDrugValue = value;
  }

	public float GetMinimumDrugValue()
    => _scriptSettings.GetValue("MARKETSETTINGS", "MINIMUMDRUGVALUE", 0.8f);

	public void SetMinimumDrugValue(float value)
  {
		_scriptSettings.SetValue("MARKETSETTINGS", "MINIMUMDRUGVALUE", value);
		Market.MinimumDrugValue = value;
  }

	public float GetExperienceMultiplier()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "EXPERIENCEMULTIPLIER", 1);

	public void SetExperienceMultiplier(float value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "EXPERIENCEMULTIPLIER", value);
		Player.ExperienceMultiplier = value;
  }

	public bool GetLooseDrugsOnDeath()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "LOOSEDRUGSONDEATH", true);

	public void SetLooseDrugsOnDeath(bool value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "LOOSEDRUGSONDEATH", value);
		Player.LooseDrugsOnDeath = value;
  }

	public bool GetLooseMoneyOnDeath()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "LOOSEMONEYONDEATH", true);

	public void SetLooseMoneyOnDeath(bool value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "LOOSEMONEYONDEATH", value);
		Player.LooseMoneyOnDeath = value;
  }

	public bool GetLooseDrugsWhenBusted()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "LOOSEDRUGSWHENBUSTED", true);

	public void SetLooseDrugsWhenBusted(bool value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "LOOSEDRUGSWHENBUSTED", value);
		Player.LooseDrugsWhenBusted = value;
  }

	public bool GetLooseMoneyWhenBusted()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "LOOSEMONEYWHENBUSTED", true);

	public void SetLooseMoneyWhenBusted(bool value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "LOOSEMONEYWHENBUSTED", value);
		Player.LooseMoneyWhenBusted = value;
  }

	public int GetInventoryExpansionPerLevel()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "INVENTORYEXPANSIONPERLEVEL", 10);

	public void SetInventoryExpansionPerLevel(int value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "INVENTORYEXPANSIONPERLEVEL", value);
		Player.InventoryExpansionPerLevel = value;
  }

	public int GetStartingInventory()
    => _scriptSettings.GetValue("PLAYERSETTINGS", "STARTINGINVENTORY", 100);

	public void SetStartingInventory(int value)
  {
		_scriptSettings.SetValue("PLAYERSETTINGS", "STARTINGINVENTORY", value);
		Player.StartingInventory = value;
  }
}
