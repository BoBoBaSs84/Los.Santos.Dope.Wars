using GTA;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
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
	/// <param name="dealer">The dealer to change.</param>
	/// <param name="worldProvider">The world provider instance to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static IDealer ChangePrices(this IDealer dealer, IWorldProvider worldProvider, int playerLevel)
	{
		dealer.Inventory.ChangePrices(playerLevel);
		dealer.SetNextPriceChange(worldProvider);
		return dealer;
	}

	/// <summary>
	/// Changes the drug prices of the dealer collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to change.</param>
	/// <param name="worldProvider">The world provider instance to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static ICollection<IDealer> ChangePrices(this ICollection<IDealer> dealers, IWorldProvider worldProvider, int playerLevel)
	{
		foreach (IDealer dealer in dealers)
			dealer.ChangePrices(worldProvider, playerLevel);
		return dealers;
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
		dealer.SetNextInventoryChange(worldProvider);
		dealer.SetNextPriceChange(worldProvider);
		return dealer;
	}

	/// <summary>
	/// Changes the inventories of the dealer collection.
	/// </summary>
	/// <param name="dealers">The dealer collection to change.</param>
	/// <param name="worldProvider">The world provider instance to use.</param>
	/// <param name="playerLevel">The current player level.</param>
	public static ICollection<IDealer> ChangeInventories(this ICollection<IDealer> dealers, IWorldProvider worldProvider, int playerLevel)
	{
		foreach (IDealer dealer in dealers)
			dealer.ChangeInventory(worldProvider, playerLevel);
		return dealers;
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
