using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Models;
using System.Diagnostics.CodeAnalysis;
using MarketSettings = LSDW.Domain.Models.Settings.Market;

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
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer ChangePrices(this IDealer dealer, ITimeProvider timeProvider, int playerLevel)
	{
		dealer.Inventory.ChangePrices(playerLevel);
		dealer.SetNextPriceChange(timeProvider.Now.AddHours(MarketSettings.PriceChangeInterval));
		return dealer;
	}

	/// <summary>
	/// Changes the drug prices of the dealer collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to change.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static ICollection<IDealer> ChangePrices(this ICollection<IDealer> dealers, ITimeProvider timeProvider, int playerLevel)
	{
		foreach (IDealer dealer in dealers)
			dealer.ChangePrices(timeProvider, playerLevel);
		return dealers;
	}

	/// <summary>
	/// Changes the inventory of the dealer.
	/// </summary>
	/// <param name="dealer">The dealer to change.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer ChangeInventory(this IDealer dealer, ITimeProvider timeProvider, int playerLevel)
	{
		dealer.Inventory.Restock(playerLevel);
		dealer.SetNextInventoryChange(timeProvider.Now.AddHours(MarketSettings.InventoryChangeInterval));
		dealer.SetNextPriceChange(timeProvider.Now.AddHours(MarketSettings.PriceChangeInterval));
		return dealer;
	}

	/// <summary>
	/// Changes the inventories of the dealer collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to change.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static ICollection<IDealer> ChangeInventories(this ICollection<IDealer> dealers, ITimeProvider timeProvider, int playerLevel)
	{
		foreach (IDealer dealer in dealers)
			dealer.ChangeInventory(timeProvider, playerLevel);
		return dealers;
	}

	/// <summary>
	/// Deletes all the dealers within the collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to delete.</param>
	public static ICollection<IDealer> DeleteDealers(this ICollection<IDealer> dealers)
	{
		foreach (IDealer dealer in dealers)
			dealer.Delete();
		return dealers;
	}
}
