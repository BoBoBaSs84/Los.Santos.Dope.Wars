using LemonUI.Menus;
using LSDW.Abstractions.Presentation.Items;
using LSDW.Domain.Enumerators;
using LSDW.Presentation.Helpers;

namespace LSDW.Presentation.Items;

/// <summary>
/// The switch item class.
/// </summary>
internal sealed class SwitchItem : NativeItem, ISwitchItem
{
	/// <summary>
	/// Initializes a instance of the switch item class.
	/// </summary>
	/// <param name="menuType">The menu type for the switch item.</param>
	internal SwitchItem(MenuType menuType) : base(string.Empty)
	{
		Enabled = true;
		Title = SwitchItemHelper.GetTitle(menuType);
		Description = SwitchItemHelper.GetDescription(menuType);
	}
}
