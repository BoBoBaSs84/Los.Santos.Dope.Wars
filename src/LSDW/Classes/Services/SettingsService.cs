using GTA;
using LSDW.Core.Classes;
using LSDW.Factories;
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
	private readonly ILoggerService _loggerService;
	private readonly ScriptSettings _scriptSettings;

	/// <summary>
	/// Initializes a instance of the settings service class.
	/// </summary>
	public SettingsService()
	{
		string settingsFileName = Path.Combine(AppContext.BaseDirectory, Settings.SettingsFileName);
		_loggerService = ServiceFactory.CreateLoggerService();
		_scriptSettings = ScriptSettings.Load(settingsFileName);
    
    Load();
    Save();
	}

  public void Load()
  {
    Dealer.DownTimeInHours = GetDownTimeInHours();
    Dealer.WearsArmor = GetWearsArmor();
    Dealer.WearsWeapons = GetWearsWeapons();
    Market.MaximumDrugValue = GetMaximumDrugValue();
    Market.MinimumDrugValue = GetMinimumDrugValue();
    Player.ExperienceMultiplier = GetExperienceMultiplier();
    Player.LooseDrugsOnDeath = GetLooseDrugsOnDeath();
    Player.LooseMoneyOnDeath = GetLooseMoneyOnDeath();
    Player.LooseDrugsWhenBusted = GetLooseDrugsWhenBusted();
    Player.LooseMoneyWhenBusted = GetLooseMoneyWhenBusted();
    Player.InventoryExpansionPerLevel = GetInventoryExpansionPerLevel();
    Player.StartingInventory = GetStartingInventory();
  }

  public void Save()
    => _scriptSettings.Save();

	public int GetDownTimeInHours()
  {
    int value = _scriptSettings.GetValue(nameof(Dealer), nameof(Dealer.DownTimeInHours), 48);
    return value;
  }

	public void SetDownTimeInHours(int value)
  {
		_scriptSettings.SetValue(nameof(Dealer), nameof(Dealer.DownTimeInHours), value);
		Dealer.DownTimeInHours = value;
  }

	public bool GetWearsArmor()
  {
    bool value = _scriptSettings.GetValue(nameof(Dealer), nameof(Dealer.WearsArmor), true);
    return value;
  }

	public void SetWearsArmor(bool value)
  {
		_scriptSettings.SetValue(nameof(Dealer), nameof(Dealer.WearsArmor), value);
		Dealer.WearsArmor = value;
  }

	public bool GetWearsWeapons()
  {
    bool value = _scriptSettings.GetValue(nameof(Dealer), nameof(Dealer.WearsWeapons), true);
    return value;
  }

	public void SetWearsWeapons(bool value)
  {
		_scriptSettings.SetValue(nameof(Dealer), nameof(Dealer.WearsWeapons), value);
		Dealer.WearsWeapons = value;
  }

	public float GetMaximumDrugValue()
  {
    float value = _scriptSettings.GetValue(nameof(Market), nameof(Market.MaximumDrugValue), 1.2f);
    return value;
  }

	public void SetMaximumDrugValue(float value)
  {
		_scriptSettings.SetValue(nameof(Market), nameof(Market.MaximumDrugValue), value);
		Market.MaximumDrugValue = value;
  }

	public float GetMinimumDrugValue()
  {
    float value = _scriptSettings.GetValue(nameof(Market), nameof(Market.MinimumDrugValue), 0.8f);
    return value;
  }

	public void SetMinimumDrugValue(float value)
  {
		_scriptSettings.SetValue(nameof(Market), nameof(Market.MinimumDrugValue), value);
		Market.MinimumDrugValue = value;
  }

	public float GetExperienceMultiplier()
  {
    float value = _scriptSettings.GetValue(nameof(Player), nameof(Player.ExperienceMultiplier), 1f);
    return value;
  }

	public void SetExperienceMultiplier(float value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.ExperienceMultiplier), value);
		Player.ExperienceMultiplier = value;
  }

	public bool GetLooseDrugsOnDeath()
  {
    bool value = _scriptSettings.GetValue(nameof(Player), nameof(Player.LooseDrugsOnDeath), true);
    return value;
  }

	public void SetLooseDrugsOnDeath(bool value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.LooseDrugsOnDeath), value);
		Player.LooseDrugsOnDeath = value;
  }

	public bool GetLooseMoneyOnDeath()
  {
    bool value = _scriptSettings.GetValue(nameof(Player), nameof(Player.LooseMoneyOnDeath), true);
    return value;
  }

	public void SetLooseMoneyOnDeath(bool value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.LooseMoneyOnDeath), value);
		Player.LooseMoneyOnDeath = value;
  }

	public bool GetLooseDrugsWhenBusted()
  {
    bool value = _scriptSettings.GetValue(nameof(Player), nameof(Player.LooseDrugsWhenBusted), true);
    return value;
  }

	public void SetLooseDrugsWhenBusted(bool value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.LooseDrugsWhenBusted), value);
		Player.LooseDrugsWhenBusted = value;
  }

	public bool GetLooseMoneyWhenBusted()
  {
    bool value = _scriptSettings.GetValue(nameof(Player), nameof(Player.LooseMoneyWhenBusted), true);
    return value;
  }

	public void SetLooseMoneyWhenBusted(bool value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.LooseMoneyWhenBusted), value);
		Player.LooseMoneyWhenBusted = value;
  }

	public int GetInventoryExpansionPerLevel()
  {
    int value = _scriptSettings.GetValue(nameof(Player), nameof(Player.InventoryExpansionPerLevel), 10);
    return value;
  }

	public void SetInventoryExpansionPerLevel(int value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.InventoryExpansionPerLevel), value);
		Player.InventoryExpansionPerLevel = value;
  }

	public int GetStartingInventory()
  {
    int value = _scriptSettings.GetValue(nameof(Player), nameof(Player.StartingInventory), 100);
    return value;
  }

	public void SetStartingInventory(int value)
  {
		_scriptSettings.SetValue(nameof(Player), nameof(Player.StartingInventory), value);
		Player.StartingInventory = value;
  }
}
