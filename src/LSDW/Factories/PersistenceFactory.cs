using LSDW.Classes.Persistence;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Factories;

/// <summary>
/// The persistence factory class.
/// </summary>
public static class PersistenceFactory
{
	/// <summary>
	/// Creates a new saveable drug state from a drug instance.
	/// </summary>
	/// <param name="drug">The drug instance to save.</param>
	public static DrugState CreateDrugState(IDrug drug)
		=> new(drug);

	/// <summary>
	/// Should create a drug instance from saved drug state.
	/// </summary>
	/// <param name="drugState">The saved drug state.</param>
	public static IDrug CreateDrug(DrugState drugState)
		=> DrugFactory.CreateDrug(drugState.DrugType, drugState.Quantity, drugState.Price);

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
	/// Should create a drug collection instance from a saved drug collection state.
	/// </summary>
	/// <param name="drugs">The saved drug collection state.</param>
	public static IEnumerable<IDrug> CreateDrugs(List<DrugState> drugs)
	{
		List<IDrug> drugList = new();
		foreach (DrugState drug in drugs)
			drugList.Add(CreateDrug(drug));
		return drugList;
	}

	/// <summary>
	/// Creates a new saveable inventory state from a inventory instance.
	/// </summary>
	/// <param name="inventory">The inventory instance to save.</param>
	public static InventoryState CreateInventoryState(IInventory inventory)
		=> new(inventory);

	/// <summary>
	/// Should create a new inventory instance from a collection of saved drugs.
	/// </summary>
	/// <param name="inventoryState">The saved inventory state.</param>
	public static IInventory CreateInventory(InventoryState inventoryState)
	{
		IEnumerable<IDrug> drugs = CreateDrugs(inventoryState.Drugs);
		return InventoryFactory.CreateInventory(drugs, inventoryState.Money);
	}

	/// <summary>
	/// Creates a new saveable player state from a player instance.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	public static PlayerState CreatePlayerState(IPlayer player)
		=> new(player);

	/// <summary>
	/// Should create a player character instance from a saved player state.
	/// </summary>
	/// <param name="playerState">The saved player state.</param>
	public static IPlayer CreatePlayer(PlayerState playerState)
	{
		IInventory inventory = CreateInventory(playerState.Inventory);
		return PlayerFactory.CreatePlayer(inventory, playerState.Experience);
	}
}
