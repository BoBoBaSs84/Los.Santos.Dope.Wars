using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using RESX = LSDW.Presentation.Properties.Resources;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The settings menu class.
/// </summary>
internal sealed partial class SettingsMenu : NativeMenu, ISettingsMenu
{
	private readonly ISettingsService _settingsService;
	private readonly ObjectPool _processables = new();

	private readonly List<int> _playerInventoryExpansionPerLevelValues = new int[] { 0, 5, 10, 15, 25, 30, 35, 40, 45, 50 }.ToList();
	private readonly List<int> _playerStartingInventoryValues = new int[] { 50, 75, 100, 125, 150 }.ToList();
	private readonly List<int> _dealerDownTimeInHoursValues = new int[] { 24, 48, 72, 96, 120, 144, 168 }.ToList();
	private readonly List<bool> _dealerHasArmorValues = new bool[] { true, false }.ToList();
	private readonly List<bool> _dealerHasWeaponsValues = new bool[] { true, false }.ToList();
	private readonly List<float> _marketMinimumDrugValueValues = new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f }.ToList();
	private readonly List<float> _marketMaximumDrugValueValues = new float[] { 1.05f, 1.1f, 1.15f, 1.2f, 1.25f }.ToList();
	private readonly List<float> _playerExperienceMultiplierValues = new float[] { 0.75f, 0.8f, 0.85f, 0.9f, 0.95f, 1f, 1.05f, 1.1f, 1.15f, 1.2f, 1.25f }.ToList();
	private readonly List<bool> _playerLooseDrugsOnDeathValues = new bool[] { true, false }.ToList();
	private readonly List<bool> _playerLooseMoneyOnDeathValues = new bool[] { true, false }.ToList();
	private readonly List<bool> _playerLooseDrugsWhenBustedValues = new bool[] { true, false }.ToList();
	private readonly List<bool> _playerLooseMoneyWhenBustedValues = new bool[] { true, false }.ToList();
	private readonly List<float> _traffickingBustChanceValues = new float[] { 0.05f, 0.1f, 0.15f, 0.2f, 0.25f, 0.3f, 0.35f, 0.4f, 0.45f, 0.5f }.ToList();
	private readonly List<int> _traffickingWantedLevelValues = new int[] { 1, 2, 3, 4, 5 }.ToList();

	/// <summary>
	/// Initializes a instance of the settings menu class.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	internal SettingsMenu(ISettingsService settingsService) : base(RESX.UI_SettingsMenu_Title)
	{
		_settingsService = settingsService;

		Subtitle = RESX.UI_SettingsMenu_Subtitle;
		Closing += OnClosing;

		AddMenuItems();

		_processables.Add(this);
	}

	public void OnTick(object sender, EventArgs args)
		=> _processables.Process();

	public void SetVisible(bool value)
		=> Visible = value;

	private void OnClosing(object sender, CancelEventArgs args)
		=> _settingsService.Save();

	//private void AddMenuItems()
	//{
	//SettingCheckboxItem hasArmorItem = new(RESX.UI_Settings_Dealer_HasArmor_Title)
	//{
	//	Description = RESX.UI_Settings_Dealer_HasArmor_Description,
	//	Checked = _settingsService.GetHasArmor()
	//};
	//hasArmorItem.CheckboxChanged += OnHasArmorItemCheckboxChanged;
	//Add(hasArmorItem);

	//SettingListItem<float> minimumDrugValueItem = new(RESX.UI_Settings_Market_MinimumDrugValue_Title)
	//{
	//	Description = RESX.UI_Settings_Market_MinimumDrugValue_Description,
	//	Items = _minimumDrugValues.ToList(),
	//	SelectedItem = _settingsService.GetMinimumDrugValue()
	//};
	//minimumDrugValueItem.ItemChanged += OnMinimumDrugValueItemChanged;
	//Add(minimumDrugValueItem);
	//}

	//private void OnHasArmorItemCheckboxChanged(object sender, EventArgs args)
	//{
	//	if (sender is not SettingCheckboxItem item)
	//		return;
	//	_settingsService.SetHasArmor(item.Checked);
	//}

	//private void OnMinimumDrugValueItemChanged(object sender, ItemChangedEventArgs<float> args)
	//{
	//	if (sender is not SettingListItem<float> item)
	//		return;
	//	_settingsService.SetMinimumDrugValue(item.SelectedItem);
	//}
}
