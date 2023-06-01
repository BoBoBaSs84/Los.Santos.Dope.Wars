using LSDW.Classes.Persistence;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Interfaces.Actors;

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
	/// Creates a drug instance from saved drug state.
	/// </summary>
	/// <param name="state">The saved drug state.</param>
	public static IDrug CreateDrug(DrugState state)
		=> ModelFactory.CreateDrug(state.DrugType, state.Quantity, state.Price);

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
	/// Creates a drug collection instance from a saved drug collection state.
	/// </summary>
	/// <param name="states">The saved drug collection state.</param>
	public static IEnumerable<IDrug> CreateDrugs(List<DrugState> states)
	{
		List<IDrug> drugList = new();
		foreach (DrugState state in states)
			drugList.Add(CreateDrug(state));
		return drugList;
	}

	/// <summary>
	/// Creates a new saveable inventory state from a inventory instance.
	/// </summary>
	/// <param name="inventory">The inventory instance to save.</param>
	public static InventoryState CreateInventoryState(IInventory inventory)
		=> new(inventory);

	/// <summary>
	/// Creates a new inventory instance from a collection of saved drugs.
	/// </summary>
	/// <param name="state">The saved inventory state.</param>
	public static IInventory CreateInventory(InventoryState state)
	{
		IEnumerable<IDrug> drugs = CreateDrugs(state.Drugs);
		return ModelFactory.CreateInventory(drugs, state.Money);
	}

	/// <summary>
	/// Creates a new saveable player state from a player instance.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	public static PlayerState CreatePlayerState(IPlayer player)
		=> new(player);

	/// <summary>
	/// Creates a player instance from a saved player state.
	/// </summary>
	/// <param name="state">The saved player state.</param>
	public static IPlayer CreatePlayer(PlayerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return ModelFactory.CreatePlayer(inventory, state.Experience);
	}

	/// <summary>
	/// Creates a new saveable dealer state from a dealer instance.
	/// </summary>
	/// <param name="dealer">The dealer instance to save.</param>
	public static DealerState CreateDealerState(IDealer dealer)
		=> new(dealer);

	/// <summary>
	/// Creates a new saveable dealer state collection from a dealer instance collection.
	/// </summary>
	/// <param name="dealers">The dealer instance colection to save.</param>
	public static List<DealerState> CreateDealerStates(IEnumerable<IDealer> dealers)
	{
		List<DealerState> states = new();
		foreach (IDealer dealer in dealers)
			states.Add(CreateDealerState(dealer));
		return states;
	}

	/// <summary>
	/// Creates a dealer instance from a saved dealer state.
	/// </summary>
	/// <param name="state">The saved dealer state.</param>
	public static IDealer CreateDealer(DealerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return ActorFactory.CreateDealer(state.Position, state.Hash, state.ClosedUntil, state.Discovered, inventory, state.Name);
	}

	/// <summary>
	/// Creates a dealer instance collection from a saved dealer state collection.
	/// </summary>
	/// <param name="states">The saved dealer state collection.</param>
	public static IEnumerable<IDealer> CreateDealers(List<DealerState> states)
	{
		List<IDealer> dealers = new();
		foreach (DealerState state in states)
			dealers.Add(CreateDealer(state));
		return dealers;
	}

	/// <summary>
	/// Creates a new saveable transaction state from a transaction instance.
	/// </summary>
	/// <param name="transaction">The transaction instance to save.</param>
	public static TransactionState CreateTransactionState(ITransaction transaction)
		=> new(transaction);

	/// <summary>
	/// Creates a new saveable transaction state collection from a transaction instance collection.
	/// </summary>
	/// <param name="transactions">The transaction instance collection to save.</param>
	public static List<TransactionState> CreateTransactionStates(IEnumerable<ITransaction> transactions)
	{
		List<TransactionState> states = new();
		foreach (ITransaction transaction in transactions)
			states.Add(CreateTransactionState(transaction));
		return states;
	}

	/// <summary>
	/// Creates a transaction instance from saved transaction state.
	/// </summary>
	/// <param name="state">The saved transaction state.</param>
	public static ITransaction CreateTransaction(TransactionState state)
		=> ModelFactory.CreateTransaction(state.DateTime, state.TransactionType, state.DrugType, state.Quantity, state.TotalValue);

	/// <summary>
	/// Creates a transaction instance collection from saved transaction state collection.
	/// </summary>
	/// <param name="states">The saved transaction state collection.</param>
	public static IEnumerable<ITransaction> CreateTransaction(List<TransactionState> states)
	{
		List<ITransaction> transactions = new();
		foreach(TransactionState state in states)
			transactions.Add(CreateTransaction(state));
		return transactions;
	}

	/// <summary>
	/// Creates a new saveable game state.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	/// <param name="dealers">The dealer instance colection to save.</param>
	public static GameState CreateGameState(IPlayer player, IEnumerable<IDealer> dealers)
		=> new(player, dealers);

	/// <summary>
	/// Creates a player instance from a saved game state.
	/// </summary>
	/// <param name="state">The saved game state.</param>
	public static IPlayer CreatePlayer(GameState state)
		=> CreatePlayer(state.Player);

	/// <summary>
	/// Creates a dealer instance collection from a saved game state.
	/// </summary>
	/// <param name="state">The saved game state.</param>
	public static IEnumerable<IDealer> CreateDealers(GameState state)
		=> CreateDealers(state.Dealers);
}
