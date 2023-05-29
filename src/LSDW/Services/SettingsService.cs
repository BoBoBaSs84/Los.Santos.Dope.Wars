using GTA;
using LSDW.Interfaces.Services;
using DealerSettings = LSDW.Core.Classes.Settings.DealerSettings;
using PlayerSettings = LSDW.Core.Classes.Settings.PlayerSettings;
using MarketSettings = LSDW.Core.Classes.Settings.MarketSettings;
using Settings = LSDW.Core.Classes.Settings;
using LSDW.Factories;

namespace LSDW.Services;

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

		if (!File.Exists(settingsFileName))
			Init();
		else
			Apply();
	}

	private void Init()
	{
		SetLooseDrugsWhenBusted(PlayerSettings.LooseDrugsWhenBusted);
		SetLooseDrugsOnDeath(PlayerSettings.LooseDrugsOnDeath);
		SetInventoryExpansionPerLevel(PlayerSettings.InventoryExpansionPerLevel);
		SetStartingInventory(PlayerSettings.StartingInventory);
		SetDownTimeInHours(DealerSettings.DownTimeInHours);
		SetMinimumDrugValue(MarketSettings.MinimumDrugValue);
		SetMaximumDrugValue(MarketSettings.MaximumDrugValue);
		_ = Save();
	}

	private void Apply()
	{
		PlayerSettings.LooseDrugsWhenBusted = GetLooseDrugsWhenBusted();
		PlayerSettings.LooseDrugsOnDeath = GetLooseDrugsOnDeath();
		PlayerSettings.InventoryExpansionPerLevel = GetInventoryExpansionPerLevel();
		PlayerSettings.StartingInventory = GetStartingInventory();
		DealerSettings.DownTimeInHours = GetDownTimeInHours();
		MarketSettings.MinimumDrugValue = GetMinimumDrugValue();
		MarketSettings.MaximumDrugValue = GetMaximumDrugValue();
	}

	public bool GetLooseDrugsWhenBusted()
		=> _scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsWhenBusted), true);

	public bool GetLooseDrugsOnDeath()
		=> _scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsOnDeath), true);

	public int GetInventoryExpansionPerLevel()
		=> _scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.InventoryExpansionPerLevel), 10);

	public int GetStartingInventory()
		=> _scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.StartingInventory), 100);

	public int GetDownTimeInHours()
		=> _scriptSettings.GetValue(nameof(DealerSettings), nameof(DealerSettings.DownTimeInHours), 48);

	public decimal GetMinimumDrugValue()
		=> _scriptSettings.GetValue(nameof(MarketSettings), nameof(MarketSettings.MinimumDrugValue), 0.8M);

	public decimal GetMaximumDrugValue()
		=> _scriptSettings.GetValue(nameof(MarketSettings), nameof(MarketSettings.MaximumDrugValue), 1.2M);

	public void SetLooseDrugsWhenBusted(bool value)
	{
		_scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsWhenBusted), value);
		PlayerSettings.LooseDrugsWhenBusted = value;
	}

	public void SetLooseDrugsOnDeath(bool value)
	{
		_scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsOnDeath), value);
		PlayerSettings.LooseDrugsOnDeath = value;
	}

	public void SetInventoryExpansionPerLevel(int value)
	{
		_scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.InventoryExpansionPerLevel), value);
		PlayerSettings.InventoryExpansionPerLevel = value;
	}

	public void SetStartingInventory(int value)
	{
		_scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.StartingInventory), value);
		PlayerSettings.StartingInventory = value;
	}

	public void SetDownTimeInHours(int value)
	{
		_scriptSettings.SetValue(nameof(DealerSettings), nameof(DealerSettings.DownTimeInHours), value);
		DealerSettings.DownTimeInHours = value;
	}

	public void SetMinimumDrugValue(decimal value)
	{
		_scriptSettings.SetValue(nameof(MarketSettings), nameof(MarketSettings.MinimumDrugValue), value);
		MarketSettings.MinimumDrugValue = value;
	}

	public void SetMaximumDrugValue(decimal value)
	{
		_scriptSettings.SetValue(nameof(MarketSettings), nameof(MarketSettings.MaximumDrugValue), value);
		MarketSettings.MaximumDrugValue = value;
	}

	public bool Save()
		=> _scriptSettings.Save();
}
