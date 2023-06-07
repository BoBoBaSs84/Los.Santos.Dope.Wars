using LSDW.Domain.Enumerators;
using LSDW.Presentation.Properties;

namespace LSDW.Presentation.Helpers;

/// <summary>
/// The switch item Helper class.
/// </summary>
internal static class SwitchItemHelper
{
	/// <summary>
	/// Gets the title for the switch item depending on the menu type.
	/// </summary>
	/// <param name="menuType">The menu type for the switch item.</param>
	/// <returns>The title for the switch item.</returns>
	internal static string GetTitle(MenuType menuType)
	{
		string title = menuType switch
		{
			MenuType.BUY => Resources.UI_Switch_Item_Title_Buy,
			MenuType.SELL => Resources.UI_Switch_Item_Title_Sell,
			MenuType.TAKE => Resources.UI_Switch_Item_Title_Take,
			MenuType.GIVE => Resources.UI_Switch_Item_Title_Give,
			MenuType.RETRIEVE => Resources.UI_Switch_Item_Title_Retrieve,
			MenuType.STORE => Resources.UI_Switch_Item_Title_Store,
			_ => string.Empty
		};

		return title;
	}

	/// <summary>
	/// Gets the description for the switch item depending on the menu type.
	/// </summary>
	/// <param name="menuType">The menu type for the switch item.</param>
	/// <returns>The description for the switch item.</returns>
	internal static string GetDescription(MenuType menuType)
	{
		string description = menuType switch
		{
			MenuType.BUY => Resources.UI_Switch_Item_Description_Buy,
			MenuType.SELL => Resources.UI_Switch_Item_Description_Sell,
			MenuType.TAKE => Resources.UI_Switch_Item_Description_Take,
			MenuType.GIVE => Resources.UI_Switch_Item_Description_Give,
			MenuType.RETRIEVE => Resources.UI_Switch_Item_Description_Retrieve,
			MenuType.STORE => Resources.UI_Switch_Item_Description_Store,
			_ => string.Empty
		};

		return description;
	}
}
