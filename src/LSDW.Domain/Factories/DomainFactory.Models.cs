using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;
using LSDW.Domain.Extensions;
using LSDW.Domain.Helpers;
using LSDW.Domain.Models;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="type">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	/// <param name="currentPrice">The current price of the drug.</param>
	public static IDrug CreateDrug(DrugType type, int quantity, int currentPrice)
		=> new Drug(type, quantity, currentPrice);

	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="type">The type of the drug.</param>
	/// <param name="quantity">The quantity of the drug.</param>
	public static IDrug CreateDrug(DrugType type, int quantity)
		=> new Drug(type, quantity, default);

	/// <summary>
	/// Creates a new drug instance.
	/// </summary>
	/// <param name="type">The type of the drug.</param>
	public static IDrug CreateDrug(DrugType type)
		=> new Drug(type, default, default);

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
	/// Creates a drug collection instance of all available drugs.
	/// </summary>
	public static ICollection<IDrug> CreateAllDrugs()
	{
		IEnumerable<DrugType> drugTypes = DrugType.COKE.GetList();
		ICollection<IDrug> drugs = new HashSet<IDrug>();
		foreach (DrugType drugType in drugTypes)
			drugs.Add(CreateDrug(drugType));
		return drugs;
	}

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	public static IInventory CreateInventory()
		=> new Inventory(CreateAllDrugs(), default);

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(int money)
		=> new Inventory(CreateAllDrugs(), money);

	/// <summary>
	/// Creates a new inventory instance.
	/// </summary>
	/// <param name="drugs">The collection of drugs to add to the inventory.</param>
	/// <param name="money">The money to add to the inventory.</param>
	public static IInventory CreateInventory(ICollection<IDrug> drugs, int money)
		=> new Inventory(drugs, money);

	/// <summary>
	/// Creates a new transaction instance.
	/// </summary>
	/// <param name="dateTime">The point in time of the transaction.</param>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="drugType">The drug type of the transaction.</param>
	/// <param name="quantity">The quantity of the transaction.</param>
	/// <param name="price">The unit price of the transaction.</param>
	public static ITransaction CreateTransaction(DateTime dateTime, TransactionType type, DrugType drugType, int quantity, int price)
		=> new Transaction(dateTime, type, drugType, quantity, price);

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
	public static IPlayer CreatePlayer(IInventory inventory, int experience, ICollection<ITransaction> transactions)
		=> new Player(inventory, experience, transactions);
}
