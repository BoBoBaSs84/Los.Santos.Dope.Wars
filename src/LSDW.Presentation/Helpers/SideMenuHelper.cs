using GTA.UI;
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
public static class SideMenuHelper
{
	/// <summary>
	/// Returns the maximum quantity for the transaction based on the menu type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	internal static int GetMaximumQuantity(TransactionType type, IPlayer player)
		=> type is TransactionType.SELL or TransactionType.GIVE
		? int.MaxValue
		: player.MaximumInventoryQuantity;

	/// <summary>
	/// Returns the source and target inventory based on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	/// <param name="drugs">The opposing inventory.</param>
	internal static (IInventory source, IInventory target) GetInventories(TransactionType type, IPlayer player, IInventory drugs)
		=> type is TransactionType.SELL or TransactionType.GIVE
		? ((IInventory source, IInventory target))(player.Inventory, drugs)
		: ((IInventory source, IInventory target))(drugs, player.Inventory);

	/// <summary>
	/// Returns the alignment for the menu based on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	internal static Alignment GetAlignment(TransactionType type)
		=> type is TransactionType.SELL or TransactionType.GIVE
		? Alignment.Right
		: Alignment.Left;

	/// <summary>
	/// Returns the title for the menu based on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	internal static string GetTitle(TransactionType type)
		=> type switch
		{
			TransactionType.BUY => RESX.UI_SideMenu_Title_Buy,
			TransactionType.SELL => RESX.UI_SideMenu_Title_Sell,
			TransactionType.GIVE => RESX.UI_SideMenu_Title_Give,
			TransactionType.TAKE => RESX.UI_SideMenu_Title_Take,
			_ => string.Empty
		};

	/// <summary>
	/// Returns the subtitle for the menu based on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	/// <param name="targetMoney">The amount of money of the target.</param>
	internal static string GetSubtitle(TransactionType type, int targetMoney)
		=> type switch
		{
			TransactionType.BUY => RESX.UI_SideMenu_Subtitle_Buy.FormatInvariant(targetMoney),
			TransactionType.SELL => RESX.UI_SideMenu_Subtitle_Sell.FormatInvariant(targetMoney),
			_ => string.Empty
		};
}
