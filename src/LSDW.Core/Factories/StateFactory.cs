using LSDW.Core.Classes;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Factories;

/// <summary>
/// The state factory class.
/// </summary>
public static class StateFactory
{
	/// <summary>
	/// Creates a new saveable drug state from a drug instance.
	/// </summary>
	/// <param name="drug">The drug instance to save.</param>
	public static DrugState CreateDrugState(IDrug drug)
		=> new(drug);

	/// <summary>
	/// Creates a new saveable drug state collection from a drug instance collection.
	/// </summary>
	/// <param name="drugs">The drug instance collection to save.</param>
	public static List<DrugState> CreateDrugStates(IEnumerable<IDrug> drugs)
	{
		List<DrugState> drugStates = new();
		foreach (IDrug drug in drugs)
			drugStates.Add(CreateDrugState(drug));
		return drugStates;
	}

	/// <summary>
	/// Creates a new saveable inventory state from a inventory instance.
	/// </summary>
	/// <param name="inventory">The inventory instance to save.</param>
	public static InventoryState CreateInventoryState(IInventory inventory)
		=> new(inventory);

	/// <summary>
	/// Creates a new saveable player state from a player instance.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	public static PlayerState CreatePlayerState(IPlayer player)
		=> new(player);
}
