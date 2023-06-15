using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Models;
using System.Diagnostics.CodeAnalysis;

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
	/// <param name="dealer">The dealer to refresh.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer ChangePrices(this IDealer dealer, ITimeProvider timeProvider, int playerLevel)
	{
		dealer.Inventory.ChangePrices(playerLevel);
		dealer.SetLastRefresh(timeProvider.Now);
		return dealer;
	}

	/// <summary>
	/// Changes the drug prices of the dealer collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to refresh.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static ICollection<IDealer> ChangePrices(this ICollection<IDealer> dealers, ITimeProvider timeProvider, int playerLevel)
	{
		foreach (IDealer dealer in dealers)
			dealer.ChangePrices(timeProvider, playerLevel);
		return dealers;
	}

	/// <summary>
	/// Restocks the inventory of the dealer.
	/// </summary>
	/// <param name="dealer">The dealer to refresh.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer RestockInventory(this IDealer dealer, ITimeProvider timeProvider, int playerLevel)
	{
		dealer.Inventory.Restock(playerLevel);
		dealer.SetLastRestock(timeProvider.Now);
		return dealer;
	}

	/// <summary>
	/// Restocks the inventory of the dealer collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to restock.</param>
	/// <param name="timeProvider">The date time provider to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static ICollection<IDealer> RestockInventory(this ICollection<IDealer> dealers, ITimeProvider timeProvider, int playerLevel)
	{
		foreach (IDealer dealer in dealers)
			dealer.RestockInventory(timeProvider, playerLevel);
		return dealers;
	}
}
