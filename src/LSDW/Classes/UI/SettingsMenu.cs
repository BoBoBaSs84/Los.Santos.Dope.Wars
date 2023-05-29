using LemonUI;
using LemonUI.Menus;
using LSDW.Interfaces.Services;
using RESX = LSDW.Properties.Resources;

namespace LSDW.Classes.UI;

/// <summary>
/// The settings menu class.
/// </summary>
public sealed class SettingsMenu : NativeMenu
{
	private readonly ISettingsService _settingsService;
	private readonly ILoggerService _loggerService;
	private readonly ObjectPool _processables = new();

	/// <summary>
	/// Initializes a instance of the settings menu class.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	/// <param name="loggerService">The logger service.</param>
	internal SettingsMenu(ISettingsService settingsService, ILoggerService loggerService) : base(RESX.UI_SettingsMenu_Title, RESX.UI_SettingsMenu_Subtitle)
	{
		_settingsService = settingsService;
		_loggerService = loggerService;

		AddMenuItems();
		_processables.Add(this);

		Closing += OnClosing;
	}

	private void AddMenuItems()
	{
		try
		{
			NativeCheckboxItem looseDrugsWhenBustedItem = new(RESX.UI_Settings_Player_LooseDrugsWhenBusted_Title, true)
			{
				Description = RESX.UI_Settings_Player_LooseDrugsWhenBusted_Description,
				Checked = _settingsService.GetLooseDrugsWhenBusted()
			};
			looseDrugsWhenBustedItem.CheckboxChanged += LooseDrugsWhenBustedItemCheckboxChanged;
			Add(looseDrugsWhenBustedItem);

			NativeCheckboxItem looseDrugsOnDeathItem = new(RESX.UI_Settings_Player_LooseDrugsOnDeath_Title, true)
			{
				Description = RESX.UI_Settings_Player_LooseDrugsOnDeath_Description,
				Checked = _settingsService.GetLooseDrugsOnDeath()
			};
			looseDrugsOnDeathItem.CheckboxChanged += LooseDrugsOnDeathItemCheckboxChanged;
			Add(looseDrugsOnDeathItem);

			NativeListItem<int> inventoryExpansionPerLevelItem = new(RESX.UI_Settings_Player_InventoryExpansionPerLevel_Title, new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 })
			{
				Description = RESX.UI_Settings_Player_InventoryExpansionPerLevel_Description,
				Enabled = true,
				SelectedItem = _settingsService.GetInventoryExpansionPerLevel()
			};
			inventoryExpansionPerLevelItem.ItemChanged += InventoryExpansionPerLevelItemChanged;
			Add(inventoryExpansionPerLevelItem);

			NativeListItem<int> startingInventoryItem = new(RESX.UI_Settings_PLayer_StartingInventory_Title, new int[] { 50, 75, 100, 125, 150 })
			{
				Description = RESX.UI_Settings_PLayer_StartingInventory_Description,
				Enabled = true,
				SelectedItem = _settingsService.GetStartingInventory()
			};
			startingInventoryItem.ItemChanged += StartingInventoryItemChanged;
			Add(startingInventoryItem);

			NativeListItem<int> downTimeInHoursItem = new(RESX.UI_Settings_Dealer_DownTimeInHours_Title, new int[] { 24, 48, 72, 96, 120, 144, 168 })
			{
				Description = RESX.UI_Settings_Dealer_DownTimeInHours_Description,
				Enabled = true,
				SelectedItem = _settingsService.GetDownTimeInHours()
			};
			downTimeInHoursItem.ItemChanged += DownTimeInHoursItemChanged;
			Add(downTimeInHoursItem);

			NativeListItem<float> minimumDrugValueItem = new(RESX.UI_Settings_Market_MinimumDrugValue_Title, new float[] { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f })
			{
				Description = RESX.UI_Settings_Market_MinimumDrugValue_Description,
				Enabled = true,
				SelectedItem = _settingsService.GetMinimumDrugValue()
			};
			minimumDrugValueItem.ItemChanged += MinimumDrugValueItemChanged;
			Add(minimumDrugValueItem);

			NativeListItem<float> maximumDrugValueItem = new(RESX.UI_Settings_Market_MaximumDrugValue_Title, new float[] { 1.1f, 1.2f, 1.3f, 1.4f, 1.5f })
			{
				Description = RESX.UI_Settings_Market_MaximumDrugValue_Description,
				Enabled = true,
				SelectedItem = _settingsService.GetMaximumDrugValue()
			};
			maximumDrugValueItem.ItemChanged += MaximumDrugValueItemChanged;
			Add(maximumDrugValueItem);
		}
		catch (Exception ex)
		{
			_loggerService.Critical(ex.Message);
		}
	}

	private void MaximumDrugValueItemChanged(object sender, ItemChangedEventArgs<float> args)
	{
		if (sender is not NativeListItem<float> maximumDrugValueItem)
			return;
		_settingsService.SetMaximumDrugValue(maximumDrugValueItem.SelectedItem);
	}

	private void MinimumDrugValueItemChanged(object sender, ItemChangedEventArgs<float> args)
	{
		if (sender is not NativeListItem<float> minimumDrugValueItem)
			return;
		_settingsService.SetMinimumDrugValue(minimumDrugValueItem.SelectedItem);
	}

	private void DownTimeInHoursItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		if (sender is not NativeListItem<int> downTimeInHoursItem)
			return;
		_settingsService.SetDownTimeInHours(downTimeInHoursItem.SelectedItem);
	}

	private void StartingInventoryItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		if (sender is not NativeListItem<int> startingInventoryItem)
			return;
		_settingsService.SetStartingInventory(startingInventoryItem.SelectedItem);
	}

	private void InventoryExpansionPerLevelItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		if (sender is not NativeListItem<int> inventoryExpansionPerLevelItem)
			return;
		_settingsService.SetInventoryExpansionPerLevel(inventoryExpansionPerLevelItem.SelectedItem);
	}

	private void LooseDrugsOnDeathItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not NativeCheckboxItem looseDrugsOnDeathItem)
			return;
		_settingsService.SetLooseDrugsOnDeath(looseDrugsOnDeathItem.Checked);
	}

	private void LooseDrugsWhenBustedItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not NativeCheckboxItem looseDrugsWhenBustedItem)
			return;
		_settingsService.SetLooseDrugsWhenBusted(looseDrugsWhenBustedItem.Checked);
	}

	private void OnClosing(object sender, CancelEventArgs args)
		=> _settingsService.Save();

	internal void OnTick(object sender, EventArgs args)
		=> _processables.Process();
}
