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
	public static ISideMenu CreateBuyMenu(IProviderManager providerManager)
		=> new SideMenu(TransactionType.BUY, providerManager);

	/// <summary>
	/// Creates a new instance of the sell menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	public static ISideMenu CreateSellMenu(IProviderManager providerManager)
		=> new SideMenu(TransactionType.SELL, providerManager);

	/// <summary>
	/// Creates a new instance of the take menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	public static ISideMenu CreateTakeMenu(IProviderManager providerManager)
		=> new SideMenu(TransactionType.TAKE, providerManager);

	/// <summary>
	/// Creates a new instance of the give menu.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	public static ISideMenu CreateGiveMenu(IProviderManager providerManager)
		=> new SideMenu(TransactionType.GIVE, providerManager);
}
