using GTA;
using GTA.Math;
using LSDW.Domain.Classes.Actors;
using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Factories;

/// <summary>
/// The domain factory class.
/// </summary>
public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new dealer instance.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	/// <param name="pedHash">The ped hash of the dealer.</param>
	public static IDealer CreateDealer(Vector3 position, PedHash pedHash = PedHash.Dealer01SMY)
		=> new Dealer(position, pedHash);

	/// <summary>
	/// Creates a new dealer instance.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	/// <param name="pedHash">The ped hash of the dealer.</param>
	/// <param name="closedUntil">The dealer is gone until this date time.</param>
	/// <param name="discovered">Has the dealer already been discovered?</param>
	/// <param name="inventory">The dealer inventory.</param>
	/// <param name="name">The name of the dealer.</param>
	public static IDealer CreateDealer(Vector3 position, PedHash pedHash, DateTime? closedUntil, bool discovered, IInventory inventory, string name)
		=> new Dealer(position, pedHash, closedUntil, discovered, inventory, name);

	/// <summary>
	/// Creates a dealer instance from a saved dealer state.
	/// </summary>
	/// <param name="state">The saved dealer state.</param>
	public static IDealer CreateDealer(DealerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return CreateDealer(state.Position, state.Hash, state.ClosedUntil, state.Discovered, inventory, state.Name);
	}

	/// <summary>
	/// Creates a dealer instance collection from a saved dealer state collection.
	/// </summary>
	/// <param name="states">The saved dealer state collection.</param>
	public static ICollection<IDealer> CreateDealers(List<DealerState> states)
	{
		List<IDealer> dealers = new();
		foreach (DealerState state in states)
			dealers.Add(CreateDealer(state));
		return dealers;
	}

	/// <summary>
	/// Creates a new dealer instance collection.
	/// </summary>
	public static ICollection<IDealer> CreateDealers()
		=> new List<IDealer>();

	/// <summary>
	/// Creates a dealer instance collection from a saved game state.
	/// </summary>
	/// <param name="state">The saved game state.</param>
	public static ICollection<IDealer> CreateDealers(GameState state)
		=> CreateDealers(state.Dealers);
}
