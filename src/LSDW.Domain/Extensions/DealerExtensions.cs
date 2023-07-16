using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Properties;
using System.Diagnostics.CodeAnalysis;
using MarketSettings = LSDW.Abstractions.Models.Settings.Market;

namespace LSDW.Domain.Extensions;

/// <summary>
/// The dealer extensions class.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Extension methods.")]
public static class DealerExtensions
{
	/// <summary>
	/// Changes the drug prices of the dealer.
	/// </summary>
	/// <param name="dealer">The dealer to change.</param>
	/// <param name="worldProvider">The world provider instance to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer ChangePrices(this IDealer dealer, IWorldProvider worldProvider, int playerLevel)
	{
		dealer.Inventory.ChangePrices(playerLevel);
		dealer.NextPriceChange = worldProvider.Now.AddHours(MarketSettings.PriceChangeInterval);
		return dealer;
	}

	/// <summary>
	/// Changes the inventory of the dealer.
	/// </summary>
	/// <param name="dealer">The dealer to change.</param>
	/// <param name="worldProvider">The world provider instance to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer ChangeInventory(this IDealer dealer, IWorldProvider worldProvider, int playerLevel)
	{
		dealer.Inventory.Restock(playerLevel);
		dealer.NextInventoryChange = worldProvider.Now.AddHours(MarketSettings.InventoryChangeInterval);
		dealer.NextPriceChange = worldProvider.Now.AddHours(MarketSettings.PriceChangeInterval);
		return dealer;
	}

	/// <summary>
	/// This will pick a random drug from the dealers inventory and make a special offer on it.
	/// </summary>
	/// <remarks>
	/// There is a 50:50 chance whether it is a buy or sell offer.
	/// </remarks>
	/// <param name="dealer">The dealer to make a speical offer.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	// TODO: Special blip icon ?
	public static IDealer MakeSpecialOffer(this IDealer dealer, IProviderManager providerManager, int playerLevel)
	{
		IRandomProvider randomProvider = providerManager.RandomProvider;
		INotificationProvider notificationProvider = providerManager.NotificationProvider;
		DrugType drugType = dealer.Inventory.Select(x => x.Type).ToList()[randomProvider.GetInt(dealer.Inventory.Count)];

		IDrug drug = dealer.Inventory.First(x => x.Type == drugType);
		float random = randomProvider.GetFloat();

		// special buy offer
		if (random < 0.5f)
		{
			drug.SpecialBuyOffer(playerLevel);
			notificationProvider.Show(
				sender: dealer.Name,
				subject: Resources.Dealer_Message_SpecialBuyOffer_Subject,
				message: Resources.Dealer_Message_SpecialBuyOffer_Message.FormatInvariant(drug.Type.GetName()),
				blinking: true
				);
		}

		// special sell offer
		if (random >= 0.5f)
		{
			drug.SpecialSellOffer(playerLevel);
			notificationProvider.Show(
				sender: dealer.Name,
				subject: Resources.Dealer_Message_SpecialSellOffer_Subject,
				message: Resources.Dealer_Message_SpecialSellOffer_Message.FormatInvariant(drug.Type.GetName()),
				blinking: true
				);
		}

		return dealer;
	}

	/// <summary>
	/// Cleans up the dealer.
	/// </summary>
	/// <remarks>
	/// Removing the <see cref="Blip"/> and deleting the <see cref="Ped"/>.
	/// </remarks>
	/// <param name="dealer"></param>
	public static IDealer CleanUp(this IDealer dealer)
	{
		dealer.DeleteBlip();
		dealer.Delete();
		return dealer;
	}

	/// <summary>
	/// Cleans up all the dealers within the collection.
	/// </summary>
	/// <remarks>
	/// This removes the <see cref="Blip"/> from the map and deletes the dealer <see cref="Ped"/>.
	/// </remarks>
	/// <param name="dealers">The dealer collection to clean up.</param>
	public static ICollection<IDealer> CleanUp(this ICollection<IDealer> dealers)
	{
		foreach (IDealer dealer in dealers)
			dealer.CleanUp();
		return dealers;
	}
}
