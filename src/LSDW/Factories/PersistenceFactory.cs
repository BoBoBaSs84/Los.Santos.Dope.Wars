using LSDW.Classes.Persistence;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
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
		=> DrugFactory.CreateDrug(state.DrugType, state.Quantity, state.Price);

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
		return InventoryFactory.CreateInventory(drugs, state.Money);
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
		return PlayerFactory.CreatePlayer(inventory, state.Experience);
	}

	/// <summary>
	/// Creates a new saveable dealer state from a dealer instance.
	/// </summary>
	/// <param name="dealer">The dealer instance to save.</param>
	public static DealerState CreateDealerState(IDealer dealer)
		=> new(dealer);

	/// <summary>
	/// Creates a dealer instance from a saved dealer state.
	/// </summary>
	/// <param name="state">The saved dealer state.</param>
	public static IDealer CreateDealer(DealerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return ActorFactory.CreateDealer(state.Position, state.ClosedUntil, state.Discovered, inventory, state.Name);
	}

	/// <summary>
	/// Creates a new saveable log entry state from a log entry instance.
	/// </summary>
	/// <param name="logEntry">The log entry instance to save.</param>
	public static LogEntryState CreateLogEntryState(ILogEntry logEntry)
		=> new(logEntry);

	/// <summary>
	/// Creates a new saveable log entry state collection from a log entry instance collection.
	/// </summary>
	/// <param name="logEntries">The log entry instance collection to save.</param>
	public static List<LogEntryState> CreateLogEntryStates(IEnumerable<ILogEntry> logEntries)
	{
		List<LogEntryState> states = new();
		foreach (ILogEntry logEntry in logEntries)
			states.Add(CreateLogEntryState(logEntry));
		return states;
	}

	/// <summary>
	/// Creates a log entry instance from saved log entry state.
	/// </summary>
	/// <param name="state">The saved log entry state.</param>
	public static ILogEntry CreateLogEntry(LogEntryState state)
		=> LogEntryFactory.CreateLogEntry(state.DateTime, state.TransactionType, state.DrugType, state.Quantity, state.TotalValue);

	/// <summary>
	/// Creates a log entry instance collection from saved log entry state collection.
	/// </summary>
	/// <param name="states">The saved log entry state collection.</param>
	public static IEnumerable<ILogEntry> CreateLogEntries(List<LogEntryState> states)
	{
		List<ILogEntry> logEntries = new();
		foreach(LogEntryState state in states)
			logEntries.Add(CreateLogEntry(state));
		return logEntries;
	}
}
