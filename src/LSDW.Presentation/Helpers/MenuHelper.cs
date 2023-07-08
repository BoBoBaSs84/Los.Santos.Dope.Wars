using GTA.UI;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Extensions;
using RESX = LSDW.Presentation.Properties.Resources;

namespace LSDW.Presentation.Helpers;

/// <summary>
/// The side menu helper class.
/// </summary>
/// <remarks>
/// Contains only things relevant for menus.
/// </remarks>
public static class MenuHelper
{
	/// <summary>
	/// Returns the source and target inventory based on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	/// <param name="player">The player and his inventory.</param>
	/// <param name="inventory">The opposing inventory.</param>
	public static (IInventory source, IInventory target) GetInventories(TransactionType type, IPlayer player, IInventory inventory)
		=> type is TransactionType.SELL or TransactionType.GIVE
		? ((IInventory source, IInventory target))(player.Inventory, inventory)
		: ((IInventory source, IInventory target))(inventory, player.Inventory);

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
	/// Returns the name for the menu based on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the menu.</param>
	/// <param name="targetMoney">The amount of money of the target.</param>
	internal static string GetName(TransactionType type, int targetMoney)
		=> type switch
		{
			TransactionType.BUY => RESX.UI_SideMenu_Name_Buy.FormatInvariant(targetMoney),
			TransactionType.SELL => RESX.UI_SideMenu_Name_Sell.FormatInvariant(targetMoney),
			_ => string.Empty
		};
}
