using LemonUI.Menus;

namespace LSDW.Presentation.Items;

/// <summary>
/// The setting checkbox item class.
/// </summary>
internal sealed class SettingCheckboxItem : NativeCheckboxItem
{
	/// <summary>
	/// Initializes a instance of the setting checkbox item class.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	internal SettingCheckboxItem(string title) : base(title)
		=> Enabled = true;
}
