using GTA.UI;
using LemonUI.Menus;
using LSDW.Core.Extensions;
using LSDW.Core.Interfaces.Classes;
using LSDW.Enumerators;
using RESX = LSDW.Properties.Resources;

namespace LSDW.UserInterfaces.Trafficking;

/// <summary>
/// The menu class.
/// </summary>
public sealed class Menu : NativeMenu
{
	private readonly Size ScreenSize = GTA.UI.Screen.Resolution;
	
	/// <summary>
	/// The menu inventory.
	/// </summary>
	public IInventory Inventory { get; }

	/// <summary>
	/// The menu switch item.
	/// </summary>
	public SwitchItem SwitchItem { get; }

	/// <summary>
	/// Initializes a instance of the menu class.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	/// <param name="color">The color of the menu.</param>
	/// <param name="drugs">The drugs for the menu.</param>
	public Menu(MenuType menuType, Color color, IInventory drugs) : base(GetTitle(menuType))
	{
		Inventory = drugs;

		Alignment = GetAlignment(menuType);
		Banner.Color = color;
		ItemCount = CountVisibility.Always;
		Offset = new PointF(ScreenSize.Width / 64, ScreenSize.Height / 36);
		UseMouse = false;
		TitleFont = GTA.UI.Font.Pricedown;
		Subtitle = GetSubtitle(menuType);
		SwitchItem = new(menuType);
	}

	/// <summary>
	/// Returns the alignment for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	private static Alignment GetAlignment(MenuType menuType)
		=> menuType is MenuType.BUY or MenuType.RETRIEVE or MenuType.TAKE ? Alignment.Left : Alignment.Right;

	/// <summary>
	/// Returns the title for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	private static string GetTitle(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_Menu_Title_Buy,
			MenuType.SELL => RESX.UI_Menu_Title_Sell,
			MenuType.RETRIEVE => RESX.UI_Menu_Title_Retrieve,
			MenuType.STORE => RESX.UI_Menu_Title_Store,
			MenuType.GIVE => RESX.UI_Menu_Title_Give,
			MenuType.TAKE => RESX.UI_Menu_Title_Take,
			_ => string.Empty
		};

	/// <summary>
	/// Returns the subtitle for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	private string GetSubtitle(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_Menu_Subtitle_Buy.FormatInvariant(Inventory.Money),
			MenuType.SELL => RESX.UI_Menu_Subtitle_Sell.FormatInvariant(Inventory.Money),
			_ => string.Empty
		};
}
