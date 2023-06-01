using GTA;
using GTA.Math;
using LSDW.Classes.Actors;
using LSDW.Core.Interfaces.Models;
using LSDW.Interfaces.Actors;

namespace LSDW.Factories;

/// <summary>
/// The actor factory class.
/// </summary>
public static class ActorFactory
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
}
