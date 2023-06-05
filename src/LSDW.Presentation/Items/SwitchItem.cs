using LemonUI.Menus;
using LSDW.Abstractions.Interfaces.Presentation.Items;
using LSDW.Domain.Enumerators;
using RESX = LSDW.Presentation.Properties.Resources;

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
		Title = GetTitle(menuType);
		Description = GetDescription(menuType);
		Enabled = true;
	}

	private static string GetTitle(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_Switch_Item_Title_Buy,
			MenuType.SELL => RESX.UI_Switch_Item_Title_Sell,
			MenuType.TAKE => RESX.UI_Switch_Item_Title_Take,
			MenuType.GIVE => RESX.UI_Switch_Item_Title_Give,
			MenuType.RETRIEVE => RESX.UI_Switch_Item_Title_Retrieve,
			MenuType.STORE => RESX.UI_Switch_Item_Title_Store,
			_ => string.Empty
		};

	private static string GetDescription(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_Switch_Item_Description_Buy,
			MenuType.SELL => RESX.UI_Switch_Item_Description_Sell,
			MenuType.TAKE => RESX.UI_Switch_Item_Description_Take,
			MenuType.GIVE => RESX.UI_Switch_Item_Description_Give,
			MenuType.RETRIEVE => RESX.UI_Switch_Item_Description_Retrieve,
			MenuType.STORE => RESX.UI_Switch_Item_Description_Store,
			_ => string.Empty
		};
}
