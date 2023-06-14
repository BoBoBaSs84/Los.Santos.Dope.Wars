using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Presentation.Menus;

namespace LSDW.Presentation.Factories;

/// <summary>
/// The presentation factory class.
/// </summary>
public static class PresentationFactory
{
	/// <summary>
	/// Creates a new instance of the settings menu.
	/// </summary>
	/// <param name="settingsService">The settings service.</param>
	public static ISettingsMenu CreateSettingsMenu(ISettingsService settingsService)
		=> new SettingsMenu(settingsService);

	/// <summary>
	/// Creates a new instance of the buy menu.
	/// </summary>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	public static ISideMenu CreateBuyMenu(IPlayer player, IInventory inventory)
		=> new SideMenu(TransactionType.BUY, player, inventory);

	/// <summary>
	/// Creates a new instance of the sell menu.
	/// </summary>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	public static ISideMenu CreateSellMenu(IPlayer player, IInventory inventory)
		=> new SideMenu(TransactionType.SELL, player, inventory);

	/// <summary>
	/// Creates a new instance of the take menu.
	/// </summary>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	public static ISideMenu CreateTakeMenu(IPlayer player, IInventory inventory)
		=> new SideMenu(TransactionType.TAKE, player, inventory);

	/// <summary>
	/// Creates a new instance of the give menu.
	/// </summary>
	/// <param name="player">The current player.</param>
	/// <param name="inventory">The opposition inventory.</param>
	public static ISideMenu CreateGiveMenu(IPlayer player, IInventory inventory)
		=> new SideMenu(TransactionType.GIVE, player, inventory);
}
