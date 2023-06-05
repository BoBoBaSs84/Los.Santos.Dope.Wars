using LSDW.Domain.Classes.Persistence;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateDrugFromStateTest()
	{
		DrugState state = GetDrugState();

		IDrug drug = DomainFactory.CreateDrug(state);

		Assert.IsNotNull(drug);
		Assert.AreEqual(state.DrugType, drug.DrugType);
		Assert.AreEqual(state.Quantity, drug.Quantity);
		Assert.AreEqual(state.Price, drug.Price);
	}

	[TestMethod]
	public void CreateDrugsFromStatesTest()
	{
		List<DrugState> states = new() { GetDrugState() };

		IEnumerable<IDrug> drugs = DomainFactory.CreateDrugs(states);

		Assert.IsNotNull(drugs);
		Assert.AreEqual(states.Count, drugs.Count());
	}

	[TestMethod]
	public void CreateDrugWithDrugTypeTest()
	{
		DrugType drugType = DrugType.SMACK;
		IDrug? drug;

		drug = DomainFactory.CreateDrug(drugType);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
	}

	[TestMethod]
	public void CreateDrugWithDrugTypeAndQuantityTest()
	{
		DrugType drugType = DrugType.SMACK;
		int quantity = 10;
		IDrug? drug;

		drug = DomainFactory.CreateDrug(drugType, quantity);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
		Assert.AreEqual(quantity, drug.Quantity);
	}

	[TestMethod]
	public void CreateDrugWithDrugTypeAndQuantityAndPriceTest()
	{
		DrugType drugType = DrugType.SMACK;
		int quantity = 10;
		int price = 150;
		IDrug? drug;

		drug = DomainFactory.CreateDrug(drugType, quantity, price);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
		Assert.AreEqual(quantity, drug.Quantity);
		Assert.AreEqual(price, drug.Price);
	}

	[TestMethod]
	public void CreateRandomDrugTest()
	{
		IDrug? drug;

		drug = DomainFactory.CreateDrug();

		Assert.IsNotNull(drug);
	}

	[TestMethod]
	public void CreateAllDrugsTest()
	{
		IEnumerable<IDrug>? drugs;

		drugs = DomainFactory.CreateAllDrugs();

		Assert.IsNotNull(drugs);
		Assert.IsTrue(drugs.Any());
	}

	[TestMethod]
	public void CreateInventoryTest()
	{
		IInventory? inventory;

		inventory = DomainFactory.CreateInventory();

		Assert.IsNotNull(inventory);
	}

	[TestMethod]
	public void CreateInventoryWithMoneyTest()
	{
		int money = 1000;
		IInventory? inventory;

		inventory = DomainFactory.CreateInventory(money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(money, inventory.Money);
	}

	[TestMethod]
	public void CreateInventoryWithDrugsAndMoneyTest()
	{
		int money = 1000;
		IEnumerable<IDrug> drugs = DomainFactory.CreateAllDrugs();
		IInventory? inventory;

		inventory = DomainFactory.CreateInventory(drugs, money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(money, inventory.Money);
		Assert.AreEqual(drugs.Count(), inventory.Count);
	}

	[TestMethod]
	public void CreateInventoryFromStateTest()
	{
		InventoryState state = GetInventoryState();

		IInventory inventory = DomainFactory.CreateInventory(state);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(state.Money, inventory.Money);
		Assert.AreEqual(state.Drugs.Count, inventory.Count);
	}

	[TestMethod]
	public void CreateTransactionFromStateTest()
	{
		TransactionState state = GetTransactionState();

		ITransaction transaction = DomainFactory.CreateTransaction(state);

		Assert.IsNotNull(transaction);
		Assert.AreEqual(state.DateTime, transaction.DateTime);
		Assert.AreEqual(state.DrugType, transaction.DrugType);
		Assert.AreEqual(state.Type, transaction.Type);
		Assert.AreEqual(state.Price, transaction.Price);
		Assert.AreEqual(state.Quantity, transaction.Quantity);
	}

	[TestMethod]
	public void CreateTransactionTest()
	{
		DateTime date = DateTime.MinValue;
		DrugType drugType = DrugType.COKE;
		int quantity = 10;
		int price = 100;
		TransactionType transactionType = TransactionType.TRAFFIC;

		ITransaction transaction =
			DomainFactory.CreateTransaction(date, transactionType, drugType, quantity, price);

		Assert.IsNotNull(transaction);
		Assert.AreEqual(date, transaction.DateTime);
		Assert.AreEqual(drugType, transaction.DrugType);
		Assert.AreEqual(transactionType, transaction.Type);
		Assert.AreEqual(quantity, transaction.Quantity);
		Assert.AreEqual(price, transaction.Price);
	}

	[TestMethod]
	public void CreateTransactionsFromStatesTest()
	{
		List<TransactionState> states = new() { GetTransactionState() };

		IEnumerable<ITransaction> transactions =
			DomainFactory.CreateTransactions(states);

		Assert.IsNotNull(transactions);
		Assert.AreEqual(states.Count, transactions.Count());
	}

	[TestMethod]
	public void CreatePlayerTest()
	{
		IPlayer? player;

		player = DomainFactory.CreatePlayer();

		Assert.IsNotNull(player);
	}

	[TestMethod]
	public void CreatePlayerWithExperienceTest()
	{
		int experience = 10000;
		IPlayer? player;

		player = DomainFactory.CreatePlayer(experience);

		Assert.IsNotNull(player);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod]
	public void CreatePlayerWithInventoryAndExperienceTest()
	{
		int experience = 10000;
		IInventory inventory = DomainFactory.CreateInventory();
		IPlayer? player;

		player = DomainFactory.CreatePlayer(inventory, experience);

		Assert.IsNotNull(player);
		Assert.IsNotNull(player.Inventory);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod]
	public void CreatePlayerWithInventoryAndExperienceAndTransactionsTest()
	{
		int experience = 10000;
		IInventory inventory = DomainFactory.CreateInventory();
		IEnumerable<ITransaction> transactions = new List<ITransaction>();
		IPlayer? player;

		player = DomainFactory.CreatePlayer(inventory, experience, transactions);

		Assert.IsNotNull(player);
		Assert.IsNotNull(player.Inventory);
		Assert.IsNotNull(player.Transactions);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod]
	public void CreatePLayerFromState()
	{
		PlayerState state = GetPlayerState();

		IPlayer player = DomainFactory.CreatePlayer(state);

		Assert.IsNotNull(player);
		Assert.AreEqual(state.Experience, player.Experience);
	}

	[TestMethod]
	public void CreatePLayerFromGameState()
	{
		GameState state = GetGameState();

		IPlayer player = DomainFactory.CreatePlayer(state);

		Assert.IsNotNull(player);
	}

	private static DrugState GetDrugState()
		=> new()
		{
			DrugType = Enumerators.DrugType.COKE,
			Quantity = 10,
			Price = 100,
		};

	private static InventoryState GetInventoryState()
		=> new()
		{
			Drugs = new(),
			Money = 100
		};

	private static PlayerState GetPlayerState()
		=> new()
		{
			Experience = 10000,
			Inventory = new(),
			Transactions = new()
		};

	private static TransactionState GetTransactionState()
		=> new()
		{
			DateTime = DateTime.MinValue,
			DrugType = DrugType.COKE,
			Quantity = 10,
			Price = 100,
			Type = TransactionType.TRAFFIC
		};

	private static GameState GetGameState()
		=> new()
		{
			Player = new(),
			Dealers = new()
		};
}