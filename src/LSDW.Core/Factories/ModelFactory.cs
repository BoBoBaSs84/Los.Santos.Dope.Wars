using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Models;

namespace LSDW.Core.Factories;

/// <summary>
/// The model factory class.
/// </summary>
public static class ModelFactory
{
	#region IDrug
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
		=> new Drug(drugType, quantity, 0);

	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="drugType">The type of the drug.</param>
	public static IDrug CreateDrug(DrugType drugType)
		=> new Drug(drugType, 0, 0);

	/// <summary>
	/// Creates a random drug instance.
	/// </summary>
	/// <remarks>
	/// Only the drug type is randomly choosen.
	/// </remarks>
	public static IDrug CreateDrug()
	{
		List<IDrug> drugList = CreateAllDrugs().ToList();
		return drugList[RandomHelper.GetInt(0, drugList.Count)];
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
	#endregion

	#region IInventory
	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	public static IInventory CreateInventory()
		=> new Inventory(CreateAllDrugs().ToList(), 0);

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
	#endregion

	#region IPlayer
	/// <summary>
	/// Creates a new player instance.
	/// </summary>
	public static IPlayer CreatePlayer()
		=> new Player(CreateInventory(), 0, new List<ITransaction>());

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
	/// <param name="logEntries">The transaction log entries for the player.</param>
	public static IPlayer CreatePlayer(IInventory inventory, int experience, IEnumerable<ITransaction> logEntries)
		=> new Player(inventory, experience, logEntries);
	#endregion

	#region ITransaction
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
	#endregion
}
