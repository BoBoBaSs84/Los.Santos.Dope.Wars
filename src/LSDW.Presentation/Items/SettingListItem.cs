using LemonUI.Menus;

namespace LSDW.Presentation.Items;

/// <summary>
/// The setting list item class.
/// </summary>
/// <typeparam name="T">The type to work with.</typeparam>
public sealed class SettingListItem<T> : NativeListItem<T>
{
	/// <summary>
	/// Initializes a instance of the setting list item class.
	/// </summary>
	/// <param name="title">The title of the item.</param>
	public SettingListItem(string title) : base(title, Array.Empty<T>())
		=> Enabled = true;
}
