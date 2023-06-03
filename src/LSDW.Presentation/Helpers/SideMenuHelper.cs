﻿using GTA.UI;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Extensions;
using LSDW.Domain.Interfaces.Models;
using LSDW.Presentation.Menus;
using RESX = LSDW.Presentation.Properties.Resources;

namespace LSDW.Presentation.Helpers;

/// <summary>
/// The side menu helper class.
/// </summary>
/// <remarks>
/// Contains only things relevant for the <see cref="SideMenu"/> class.
/// </remarks>
internal static class SideMenuHelper
{
	/// <summary>
	/// Returns the transaction type for the provided menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	internal static TransactionType GetTransactionType(MenuType menuType)
		=> menuType is MenuType.BUY or MenuType.SELL ? TransactionType.TRAFFIC : TransactionType.DEPOSIT;

	/// <summary>
	/// Returns the maximum quantity for the transaction based on the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	internal static int GetMaximumQuantity(MenuType menuType, IPlayer player)
		=> menuType is MenuType.SELL or MenuType.STORE or MenuType.GIVE
		? int.MaxValue
		: player.MaximumInventoryQuantity;

	/// <summary>
	/// Returns the source and target inventory based on the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	/// <param name="drugs">The opposing inventory.</param>
	internal static (IInventory source, IInventory target) GetInventories(MenuType menuType, IPlayer player, IInventory drugs)
		=> menuType is MenuType.SELL or MenuType.STORE or MenuType.GIVE
		? ((IInventory source, IInventory target))(player.Inventory, drugs)
		: ((IInventory source, IInventory target))(drugs, player.Inventory);

	/// <summary>
	/// Returns the alignment for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	internal static Alignment GetAlignment(MenuType menuType)
		=> menuType is MenuType.SELL or MenuType.STORE or MenuType.GIVE
		? Alignment.Right
		: Alignment.Left;

	/// <summary>
	/// Returns the title for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	internal static string GetTitle(MenuType menuType)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_SideMenu_Title_Buy,
			MenuType.SELL => RESX.UI_SideMenu_Title_Sell,
			MenuType.RETRIEVE => RESX.UI_SideMenu_Title_Retrieve,
			MenuType.STORE => RESX.UI_SideMenu_Title_Store,
			MenuType.GIVE => RESX.UI_SideMenu_Title_Give,
			MenuType.TAKE => RESX.UI_SideMenu_Title_Take,
			_ => string.Empty
		};

	/// <summary>
	/// Returns the subtitle for the menu type.
	/// </summary>
	/// <param name="menuType">The type of the menu.</param>
	/// <param name="targetMoney">The amount of money of the target.</param>
	internal static string GetSubtitle(MenuType menuType, int targetMoney)
		=> menuType switch
		{
			MenuType.BUY => RESX.UI_SideMenu_Subtitle_Buy.FormatInvariant(targetMoney),
			MenuType.SELL => RESX.UI_SideMenu_Subtitle_Sell.FormatInvariant(targetMoney),
			_ => string.Empty
		};
}
