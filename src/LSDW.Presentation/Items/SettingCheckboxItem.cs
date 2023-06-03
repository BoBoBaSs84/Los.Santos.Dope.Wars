using LemonUI.Menus;

namespace LSDW.Presentation.Items;

/// <summary>
/// The setting checkbox item class.
/// </summary>
public sealed class SettingCheckboxItem : NativeCheckboxItem
{
	/// <summary>
	/// Initializes a instance of the setting checkbox item class.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	public SettingCheckboxItem(string title) : base(title)
		=> Enabled = true;
}
