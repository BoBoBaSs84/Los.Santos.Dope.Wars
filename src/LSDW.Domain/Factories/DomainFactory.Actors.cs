using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Actors;
using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Classes.Actors;

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
	/// Creates a new dealer instance collection.
	/// </summary>
	public static ICollection<IDealer> CreateDealers()
		=> new List<IDealer>();
}
