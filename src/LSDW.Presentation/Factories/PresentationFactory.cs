using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
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
	/// <param name="serviceManager">The service manager instance to use.</param>
	public static ISettingsMenu CreateSettingsMenu(IServiceManager serviceManager)
		=> new SettingsMenu(serviceManager);

	/// <summary>
	/// Creates a new instance of the buy menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	public static IDealMenu CreateBuyMenu(IProviderManager providerManager, IPlayer player, IInventory inventory)
		=> new DealMenu(providerManager, TransactionType.BUY, player, inventory);

	/// <summary>
	/// Creates a new instance of the sell menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	public static IDealMenu CreateSellMenu(IProviderManager providerManager, IPlayer player, IInventory inventory)
		=> new DealMenu(providerManager, TransactionType.SELL, player, inventory);

	/// <summary>
	/// Creates a new instance of the take menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	public static IDealMenu CreateTakeMenu(IProviderManager providerManager, IPlayer player, IInventory inventory)
		=> new DealMenu(providerManager, TransactionType.TAKE, player, inventory);

	/// <summary>
	/// Creates a new instance of the give menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	/// <param name="inventory">The inventory instance to use.</param>
	public static IDealMenu CreateGiveMenu(IProviderManager providerManager, IPlayer player, IInventory inventory)
		=> new DealMenu(providerManager, TransactionType.TAKE, player, inventory);
}
