using GTA;
using LSDW.Interfaces.Services;
using DealerSettings = LSDW.Core.Classes.Settings.DealerSettings;
using PlayerSettings = LSDW.Core.Classes.Settings.PlayerSettings;
using Settings = LSDW.Core.Classes.Settings;

namespace LSDW.Services;

/// <summary>
/// The settings service class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="ScriptSettings"/>.
/// </remarks>
public sealed class SettingsService : ISettingsService
{
	private readonly ScriptSettings scriptSettings;

	/// <summary>
	/// Initializes a instance of the settings service class.
	/// </summary>
	public SettingsService()
	{
		string settingsFileName = Path.Combine(AppContext.BaseDirectory, Settings.SettingsFileName);
		scriptSettings = ScriptSettings.Load(settingsFileName);

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
		SetMinimumDrugValue(DealerSettings.MinimumDrugValue);
		SetMaximumDrugValue(DealerSettings.MaximumDrugValue);
		_ = Save();
	}

	private void Apply()
	{
		PlayerSettings.LooseDrugsWhenBusted = GetLooseDrugsWhenBusted();
		PlayerSettings.LooseDrugsOnDeath = GetLooseDrugsOnDeath();
		PlayerSettings.InventoryExpansionPerLevel = GetInventoryExpansionPerLevel();
		PlayerSettings.StartingInventory = GetStartingInventory();
		DealerSettings.DownTimeInHours = GetDownTimeInHours();
		DealerSettings.MaximumDrugValue = GetMinimumDrugValue();
		DealerSettings.MaximumDrugValue = GetMinimumDrugValue();
	}

	public bool GetLooseDrugsWhenBusted()
		=> scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsWhenBusted), PlayerSettings.LooseDrugsWhenBusted);

	public bool GetLooseDrugsOnDeath()
		=> scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsOnDeath), PlayerSettings.LooseDrugsOnDeath);

	public int GetInventoryExpansionPerLevel()
		=> scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.InventoryExpansionPerLevel), PlayerSettings.InventoryExpansionPerLevel);

	public int GetStartingInventory()
		=> scriptSettings.GetValue(nameof(PlayerSettings), nameof(PlayerSettings.StartingInventory), PlayerSettings.StartingInventory);

	public int GetDownTimeInHours()
		=> scriptSettings.GetValue(nameof(DealerSettings), nameof(DealerSettings.DownTimeInHours), DealerSettings.DownTimeInHours);

	public decimal GetMinimumDrugValue()
		=> scriptSettings.GetValue(nameof(DealerSettings), nameof(DealerSettings.MinimumDrugValue), DealerSettings.MinimumDrugValue);

	public decimal GetMaximumDrugValue()
		=> scriptSettings.GetValue(nameof(DealerSettings), nameof(DealerSettings.MaximumDrugValue), DealerSettings.MaximumDrugValue);

	public void SetLooseDrugsWhenBusted(bool value)
	{
		scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsWhenBusted), value);
		PlayerSettings.LooseDrugsWhenBusted = value;
	}

	public void SetLooseDrugsOnDeath(bool value)
	{
		scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.LooseDrugsOnDeath), value);
		PlayerSettings.LooseDrugsOnDeath = value;
	}

	public void SetInventoryExpansionPerLevel(int value)
	{
		scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.InventoryExpansionPerLevel), value);
		PlayerSettings.InventoryExpansionPerLevel = value;
	}

	public void SetStartingInventory(int value)
	{
		scriptSettings.SetValue(nameof(PlayerSettings), nameof(PlayerSettings.StartingInventory), value);
		PlayerSettings.StartingInventory = value;
	}

	public void SetDownTimeInHours(int value)
	{
		scriptSettings.SetValue(nameof(DealerSettings), nameof(DealerSettings.DownTimeInHours), value);
		DealerSettings.DownTimeInHours = value;
	}

	public void SetMinimumDrugValue(decimal value)
	{
		scriptSettings.SetValue(nameof(DealerSettings), nameof(DealerSettings.MinimumDrugValue), value);
		DealerSettings.MinimumDrugValue = value;
	}

	public void SetMaximumDrugValue(decimal value)
	{
		scriptSettings.SetValue(nameof(DealerSettings), nameof(DealerSettings.MaximumDrugValue), value);
		DealerSettings.MaximumDrugValue = value;
	}

	public bool Save()
		=> scriptSettings.Save();
}
