using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Interfaces.Infrastructure.Services;
using LSDW.Abstractions.Interfaces.Presentation.Menus;
using LSDW.Domain.Interfaces.Services;
using LSDW.Presentation.Items;
using RESX = LSDW.Presentation.Properties.Resources;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The settings menu class.
/// </summary>
internal sealed class SettingsMenu : NativeMenu, ISettingsMenu
{
	private readonly ISettingsService _settingsService;
	private readonly ILoggerService _loggerService;
	private readonly ObjectPool _processables = new();

	private readonly int[] _inventoryExpansionPerLevelValues = new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 };
	private readonly int[] _startingInventoryValues = new int[] { 50, 75, 100, 125, 150 };
	private readonly int[] _downTimeInHours = new int[] { 24, 48, 72, 96, 120, 144, 168 };
	private readonly float[] _minimumDrugValue = new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f };
	private readonly float[] _maximumDrugValues = new float[] { 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };
	private readonly float[] _experienceMultiplier = new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f };

	/// <summary>
	/// Initializes a instance of the settings menu class.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	/// <param name="loggerService">The logger service.</param>
	internal SettingsMenu(ISettingsService settingsService, ILoggerService loggerService) : base(RESX.UI_SettingsMenu_Title, RESX.UI_SettingsMenu_Subtitle)
	{
		_settingsService = settingsService;
		_loggerService = loggerService;

		Closing += OnClosing;
		AddMenuItems();

		_processables.Add(this);
	}

	public void OnTick(object sender, EventArgs args)
		=> _processables.Process();

	public void SetVisible(bool value)
		=> Visible = value;

	private void AddMenuItems()
	{
		try
		{
			#region Dealer

			SettingListItem<int> downTimeInHoursItem = new(RESX.UI_Settings_Dealer_DownTimeInHours_Title)
			{
				Description = RESX.UI_Settings_Dealer_DownTimeInHours_Description,
				Items = _downTimeInHours.ToList(),
				SelectedItem = _settingsService.GetDownTimeInHours()
			};
			downTimeInHoursItem.ItemChanged += OnDownTimeInHoursItemChanged;
			Add(downTimeInHoursItem);

			SettingCheckboxItem hasArmorItem = new(RESX.UI_Settings_Dealer_HasArmor_Title)
			{
				Description = RESX.UI_Settings_Dealer_HasArmor_Description,
				Checked = _settingsService.GetHasArmor()
			};
			hasArmorItem.CheckboxChanged += OnHasArmorItemCheckboxChanged;
			Add(hasArmorItem);

			SettingCheckboxItem hasWeaponsItem = new(RESX.UI_Settings_Dealer_HasWeapons_Title)
			{
				Description = RESX.UI_Settings_Dealer_HasWeapons_Description,
				Checked = _settingsService.GetHasWeapons()
			};
			hasWeaponsItem.CheckboxChanged += OnHasWeaponsItemCheckboxChanged;
			Add(hasWeaponsItem);

			#endregion Dealer

			#region Market

			SettingListItem<float> minimumDrugValueItem = new(RESX.UI_Settings_Market_MinimumDrugValue_Title)
			{
				Description = RESX.UI_Settings_Market_MinimumDrugValue_Description,
				Items = _minimumDrugValue.ToList(),
				SelectedItem = _settingsService.GetMinimumDrugValue()
			};
			minimumDrugValueItem.ItemChanged += OnMinimumDrugValueItemChanged;
			Add(minimumDrugValueItem);

			SettingListItem<float> maximumDrugValueItem = new(RESX.UI_Settings_Market_MaximumDrugValue_Title)
			{
				Description = RESX.UI_Settings_Market_MaximumDrugValue_Description,
				Items = _maximumDrugValues.ToList(),
				SelectedItem = _settingsService.GetMaximumDrugValue()
			};
			maximumDrugValueItem.ItemChanged += OnMaximumDrugValueItemChanged;
			Add(maximumDrugValueItem);

			#endregion Market

			#region Player

			SettingListItem<float> experienceMultiplierItem = new(RESX.UI_Settings_Player_ExperienceMultiplier_Title)
			{
				Description = RESX.UI_Settings_Player_ExperienceMultiplier_Description,
				Items = _experienceMultiplier.ToList(),
				SelectedItem = _settingsService.GetExperienceMultiplier()
			};
			experienceMultiplierItem.ItemChanged += OnExperienceMultiplierItemItemChanged;
			Add(experienceMultiplierItem);

			SettingCheckboxItem looseDrugsOnDeathItem = new(RESX.UI_Settings_Player_LooseDrugsOnDeath_Title)
			{
				Description = RESX.UI_Settings_Player_LooseDrugsOnDeath_Description,
				Checked = _settingsService.GetLooseDrugsOnDeath()
			};
			looseDrugsOnDeathItem.CheckboxChanged += OnLooseDrugsOnDeathItemCheckboxChanged;
			Add(looseDrugsOnDeathItem);

			SettingCheckboxItem looseMoneyOnDeathItem = new(RESX.UI_Settings_Player_LooseMoneyOnDeath_Title)
			{
				Description = RESX.UI_Settings_Player_LooseMoneyOnDeath_Description,
				Checked = _settingsService.GetLooseMoneyOnDeath()
			};
			looseMoneyOnDeathItem.CheckboxChanged += OnLooseMoneyOnDeathItemCheckboxChanged;
			Add(looseMoneyOnDeathItem);

			SettingCheckboxItem looseDrugsWhenBustedItem = new(RESX.UI_Settings_Player_LooseDrugsWhenBusted_Title)
			{
				Description = RESX.UI_Settings_Player_LooseDrugsWhenBusted_Description,
				Checked = _settingsService.GetLooseDrugsWhenBusted()
			};
			looseDrugsWhenBustedItem.CheckboxChanged += OnLooseDrugsWhenBustedItemCheckboxChanged;
			Add(looseDrugsWhenBustedItem);

			SettingCheckboxItem looseMoneyWhenBustedItem = new(RESX.UI_Settings_Player_LooseMoneyWhenBusted_Title)
			{
				Description = RESX.UI_Settings_Player_LooseMoneyWhenBusted_Description,
				Checked = _settingsService.GetLooseMoneyWhenBusted()
			};
			looseMoneyWhenBustedItem.CheckboxChanged += OnLooseMoneyWhenBustedItemCheckboxChanged;
			Add(looseMoneyWhenBustedItem);

			SettingListItem<int> inventoryExpansionPerLevelItem = new(RESX.UI_Settings_Player_InventoryExpansionPerLevel_Title)
			{
				Description = RESX.UI_Settings_Player_InventoryExpansionPerLevel_Description,
				Items = _inventoryExpansionPerLevelValues.ToList(),
				SelectedItem = _settingsService.GetInventoryExpansionPerLevel()
			};
			inventoryExpansionPerLevelItem.ItemChanged += InventoryExpansionPerLevelItemChanged;
			Add(inventoryExpansionPerLevelItem);

			SettingListItem<int> startingInventoryItem = new(RESX.UI_Settings_PLayer_StartingInventory_Title)
			{
				Description = RESX.UI_Settings_PLayer_StartingInventory_Description,
				Items = _startingInventoryValues.ToList(),
				SelectedItem = _settingsService.GetStartingInventory()
			};
			startingInventoryItem.ItemChanged += StartingInventoryItemChanged;
			Add(startingInventoryItem);

			#endregion Player
		}
		catch (Exception ex)
		{
			_loggerService.Critical(ex.Message);
		}
	}

	private void OnExperienceMultiplierItemItemChanged(object sender, ItemChangedEventArgs<float> args)
	{
		if (sender is not SettingListItem<float> item)
			return;
		_settingsService.SetExperienceMultiplier(item.SelectedItem);
	}

	private void OnLooseMoneyWhenBustedItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not SettingCheckboxItem item)
			return;
		_settingsService.SetLooseMoneyWhenBusted(item.Checked);
	}

	private void OnLooseMoneyOnDeathItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not SettingCheckboxItem item)
			return;
		_settingsService.SetLooseMoneyOnDeath(item.Checked);
	}

	private void OnHasWeaponsItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not SettingCheckboxItem item)
			return;
		_settingsService.SetHasWeapons(item.Checked);
	}

	private void OnHasArmorItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not SettingCheckboxItem item)
			return;
		_settingsService.SetHasArmor(item.Checked);
	}

	private void OnMaximumDrugValueItemChanged(object sender, ItemChangedEventArgs<float> args)
	{
		if (sender is not SettingListItem<float> item)
			return;
		_settingsService.SetMaximumDrugValue(item.SelectedItem);
	}

	private void OnMinimumDrugValueItemChanged(object sender, ItemChangedEventArgs<float> args)
	{
		if (sender is not SettingListItem<float> item)
			return;
		_settingsService.SetMinimumDrugValue(item.SelectedItem);
	}

	private void OnDownTimeInHoursItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		if (sender is not SettingListItem<int> item)
			return;
		_settingsService.SetDownTimeInHours(item.SelectedItem);
	}

	private void StartingInventoryItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		if (sender is not SettingListItem<int> item)
			return;
		_settingsService.SetStartingInventory(item.SelectedItem);
	}

	private void InventoryExpansionPerLevelItemChanged(object sender, ItemChangedEventArgs<int> args)
	{
		if (sender is not SettingListItem<int> item)
			return;
		_settingsService.SetInventoryExpansionPerLevel(item.SelectedItem);
	}

	private void OnLooseDrugsOnDeathItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not SettingCheckboxItem item)
			return;
		_settingsService.SetLooseDrugsOnDeath(item.Checked);
	}

	private void OnLooseDrugsWhenBustedItemCheckboxChanged(object sender, EventArgs args)
	{
		if (sender is not SettingCheckboxItem item)
			return;
		_settingsService.SetLooseDrugsWhenBusted(item.Checked);
	}

	private void OnClosing(object sender, CancelEventArgs args)
		=> _settingsService.Save();
}
