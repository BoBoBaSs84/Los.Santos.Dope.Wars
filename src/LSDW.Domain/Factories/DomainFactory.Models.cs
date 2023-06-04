using LSDW.Domain.Classes.Models;
using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Extensions;
using LSDW.Domain.Helpers;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Factories;

/// <summary>
/// The domain factory class.
/// </summary>
public static partial class DomainFactory
{
	/// <summary>
	/// Creates a drug instance from saved drug state.
	/// </summary>
	/// <param name="state">The saved drug state.</param>
	public static IDrug CreateDrug(DrugState state)
		=> CreateDrug(state.DrugType, state.Quantity, state.Price);

	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="price">The price of the drug.</param>
	public static IDrug CreateDrug(DrugType drugType, int quantity, int price)
		=> new Drug(drugType, quantity, price);

	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	public static IDrug CreateDrug(DrugType drugType, int quantity)
		=> new Drug(drugType, quantity, default);

	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	public static IDrug CreateDrug(DrugType drugType)
		=> new Drug(drugType, default, default);

	/// <summary>
	/// Creates a random drug instance.
	/// </summary>
	/// <remarks>
	/// Only the drug type is randomly choosen.
	/// </remarks>
	public static IDrug CreateDrug()
	{
		List<IDrug> drugList = CreateAllDrugs().ToList();
		return drugList[RandomHelper.GetInt(default, drugList.Count)];
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
	/// Creates a drug collection instance of all available drugs.
	/// </summary>
	public static IEnumerable<IDrug> CreateAllDrugs()
	{
		IEnumerable<DrugType> drugTypes = DrugType.COKE.GetList();
		List<IDrug> drugs = new();
		foreach (DrugType drugType in drugTypes)
			drugs.Add(CreateDrug(drugType));
		return drugs;
	}

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	public static IInventory CreateInventory()
		=> new Inventory(CreateAllDrugs().ToList(), default);

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(int money)
		=> new Inventory(CreateAllDrugs().ToList(), money);

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	/// <param name="drugs">The collection of drugs to add to the inventory.</param>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(IEnumerable<IDrug> drugs, int money)
		=> new Inventory(drugs, money);

	/// <summary>
	/// Creates a new inventory instance from a collection of saved drugs.
	/// </summary>
	/// <param name="state">The saved inventory state.</param>
	public static IInventory CreateInventory(InventoryState state)
	{
		IEnumerable<IDrug> drugs = CreateDrugs(state.Drugs);
		return CreateInventory(drugs, state.Money);
	}

	/// <summary>
	/// Creates a new transaction instance.
	/// </summary>
	/// <param name="dateTime">The point in time of the transaction.</param>
	/// <param name="transactionType">The type of the transaction.</param>
	/// <param name="drugType">The drug type of the transaction.</param>
	/// <param name="quantity">The quantity of the transaction.</param>
	/// <param name="price">The unit price of the transaction.</param>
	public static ITransaction CreateTransaction(DateTime dateTime, TransactionType transactionType, DrugType drugType, int quantity, int price)
		=> new Transaction(dateTime, transactionType, drugType, quantity, price);

	/// <summary>
	/// Creates a transaction instance from saved transaction state.
	/// </summary>
	/// <param name="state">The saved transaction state.</param>
	public static ITransaction CreateTransaction(TransactionState state)
		=> CreateTransaction(state.DateTime, state.TransactionType, state.DrugType, state.Quantity, state.Price);

	/// <summary>
	/// Creates a transaction instance collection from saved transaction state collection.
	/// </summary>
	/// <param name="states">The saved transaction state collection.</param>
	public static IEnumerable<ITransaction> CreateTransactions(List<TransactionState> states)
	{
		List<ITransaction> transactions = new();
		foreach (TransactionState state in states)
			transactions.Add(CreateTransaction(state));
		return transactions;
	}

	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	public static IPlayer CreatePlayer()
		=> new Player(CreateInventory(), default, new List<ITransaction>());

	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	/// <param name="experience">The player experience points.</param>
	public static IPlayer CreatePlayer(int experience)
		=> new Player(CreateInventory(), experience, new List<ITransaction>());

	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	public static IPlayer CreatePlayer(IInventory inventory, int experience)
		=> new Player(inventory, experience, new List<ITransaction>());

	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	/// <param name="inventory">The player inventory.</param>
	/// <param name="experience">The player experience points.</param>
	/// <param name="transactions">The transactions for the player.</param>
	public static IPlayer CreatePlayer(IInventory inventory, int experience, IEnumerable<ITransaction> transactions)
		=> new Player(inventory, experience, transactions);

	/// <summary>
	/// Creates a player instance from a saved player state.
	/// </summary>
	/// <param name="state">The saved player state.</param>
	public static IPlayer CreatePlayer(PlayerState state)
	{
		IInventory inventory = CreateInventory(state.Inventory);
		return CreatePlayer(inventory, state.Experience);
	}

	/// <summary>
	/// Creates a player instance from a saved game state.
	/// </summary>
	/// <param name="state">The saved game state.</param>
	public static IPlayer CreatePlayer(GameState state)
		=> CreatePlayer(state.Player);
}
