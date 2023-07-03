using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Models;

namespace LSDW.Infrastructure.Factories;

public static partial class InfrastructureFactory
{
	/// <summary>
	/// Creates a dealer instance from a saved dealer state.
	/// </summary>
	/// <param name="state">The saved dealer state.</param>
	public static IDealer CreateDealer(DealerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return DomainFactory.CreateDealer(state.Position, state.Hash, state.Name, state.ClosedUntil, state.Discovered, inventory, state.NextPriceChange, state.NextInventoryChange);
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
	/// Creates a dealer instance collection from a saved game state.
	/// </summary>
	/// <param name="state">The saved game state.</param>
	public static ICollection<IDealer> CreateDealers(GameState state)
		=> CreateDealers(state.Dealers);

	/// <summary>
	/// Creates a drug instance from saved drug state.
	/// </summary>
	/// <param name="state">The saved drug state.</param>
	public static IDrug CreateDrug(DrugState state)
		=> DomainFactory.CreateDrug(state.Type, state.Quantity, state.CurrentPrice);

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
	/// Creates a new saveable dealer state from a dealer instance.
	/// </summary>
	/// <param name="dealer">The dealer instance to save.</param>
	public static DealerState CreateDealerState(IDealer dealer)
		=> new(dealer);

	/// <summary>
	/// Creates a new saveable dealer state collection from a dealer instance collection.
	/// </summary>
	/// <param name="dealers">The dealer instance colection to save.</param>
	public static List<DealerState> CreateDealerStates(ICollection<IDealer> dealers)
	{
		List<DealerState> states = new();
		foreach (IDealer dealer in dealers)
			states.Add(CreateDealerState(dealer));
		return states;
	}

	/// <summary>
	/// Creates a new saveable transaction state from a transaction instance.
	/// </summary>
	/// <param name="transaction">The transaction instance to save.</param>
	public static TransactionState CreateTransactionState(ITransaction transaction)
		=> new(transaction);

	/// <summary>
	/// Creates a new inventory instance from a collection of saved drugs.
	/// </summary>
	/// <param name="state">The saved inventory state.</param>
	public static IInventory CreateInventory(InventoryState state)
	{
		IEnumerable<IDrug> drugs = CreateDrugs(state.Drugs);
		return DomainFactory.CreateInventory(drugs, state.Money);
	}

	/// <summary>
	/// Creates a transaction instance from saved transaction state.
	/// </summary>
	/// <param name="state">The saved transaction state.</param>
	public static ITransaction CreateTransaction(TransactionState state)
		=> DomainFactory.CreateTransaction(state.DateTime, state.Type, state.DrugType, state.Quantity, state.Price);

	/// <summary>
	/// Creates a transaction collection instance from saved transaction state collection.
	/// </summary>
	/// <param name="states">The saved transaction states.</param>
	public static ICollection<ITransaction> CreateTransactions(List<TransactionState> states)
	{
		ICollection<ITransaction> transactions = new HashSet<ITransaction>();
		foreach (TransactionState state in states)
			transactions.Add(CreateTransaction(state));
		return transactions;
	}

	/// <summary>
	/// Creates a new saveable transaction state collection from a transaction instance collection.
	/// </summary>
	/// <param name="transactions">The transaction instance collection to save.</param>
	public static List<TransactionState> CreateTransactionStates(ICollection<ITransaction> transactions)
	{
		List<TransactionState> states = new();
		foreach (ITransaction transaction in transactions)
			states.Add(CreateTransactionState(transaction));
		return states;
	}

	/// <summary>
	/// Creates a player instance from a saved player state.
	/// </summary>
	/// <param name="state">The saved player state.</param>
	public static IPlayer CreatePlayer(PlayerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return DomainFactory.CreatePlayer(inventory, state.Experience);
	}

	/// <summary>
	/// Creates a player instance from a saved game state.
	/// </summary>
	/// <param name="state">The saved game state.</param>
	public static IPlayer CreatePlayer(GameState state)
		=> CreatePlayer(state.Player);

	/// <summary>
	/// Creates a new saveable player state from a player instance.
	/// </summary>
	/// <param name="player">The player instance to save.</param>
	public static PlayerState CreatePlayerState(IPlayer player)
		=> new(player);

	/// <summary>
	/// Creates a new saveable game state.
	/// </summary>
	/// <param name="dealers">The dealer instance colection to save.</param>
	/// <param name="player">The player instance to save.</param>
	public static GameState CreateGameState(ICollection<IDealer> dealers, IPlayer player)
		=> new(dealers, player);
}
