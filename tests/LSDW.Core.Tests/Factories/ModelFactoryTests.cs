using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class ModelFactoryTests
{
	[TestMethod]
	public void CreateRandomDrugTest()
	{
		IDrug? drug;

		drug = ModelFactory.CreateDrug();

		Assert.IsNotNull(drug);
	}

	[TestMethod]
	public void CreateAllDrugsTest()
	{
		IEnumerable<IDrug>? drugs;

		drugs = ModelFactory.CreateAllDrugs();

		Assert.IsNotNull(drugs);
		Assert.IsTrue(drugs.Any());
	}

	[TestMethod]
	public void CreateDrugTest()
	{
		DrugType drugType = DrugType.COKE;
		int quantity = 10;
		int price = 1000;

		IDrug drug = ModelFactory.CreateDrug(drugType, quantity, price);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
		Assert.AreEqual(quantity, drug.Quantity);
		Assert.AreEqual(price, drug.Price);
	}

	[TestMethod]
	public void CreateDrugWithoutPriceTest()
	{
		DrugType drugType = DrugType.COKE;
		int quantity = 10;

		IDrug drug = ModelFactory.CreateDrug(drugType, quantity);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
		Assert.AreEqual(quantity, drug.Quantity);
		Assert.AreEqual(0, drug.Price);
	}

	[TestMethod]
	public void CreateDrugWithoutQuantityAndPriceTest()
	{
		DrugType drugType = DrugType.COKE;

		IDrug drug = ModelFactory.CreateDrug(drugType);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.DrugType);
		Assert.AreEqual(0, drug.Quantity);
		Assert.AreEqual(0, drug.Price);
	}

	[TestMethod]
	public void CreateInventoryTest()
	{
		IInventory? inventory;

		inventory = ModelFactory.CreateInventory();

		Assert.IsNotNull(inventory);
		Assert.AreEqual(15, inventory.Count);
		Assert.AreEqual(0, inventory.Money);
	}

	[TestMethod]
	public void CreateInventoryWithDrugsAndMoneyTest()
	{
		IInventory? inventory;
		IEnumerable<IDrug> drugs = ModelFactory.CreateAllDrugs();
		int money = 1000;

		inventory = ModelFactory.CreateInventory(drugs, money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(money, inventory.Money);
		Assert.AreEqual(drugs.Count(), inventory.Count);
	}

	[TestMethod]
	public void CreateInventoryWithMoneyTest()
	{
		IInventory? inventory;
		int money = 10000;

		inventory = ModelFactory.CreateInventory(money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(15, inventory.Count);
		Assert.AreEqual(money, inventory.Money);
	}

	[TestMethod]
	public void CreatePlayerTest()
	{
		IPlayer? player;

		player = ModelFactory.CreatePlayer();

		Assert.IsNotNull(player);
		Assert.AreEqual(0, player.Experience);
	}

	[TestMethod]
	public void CreatePlayerWithExperienceTest()
	{
		IPlayer? player;
		int experience = 38934;

		player = ModelFactory.CreatePlayer(experience);

		Assert.IsNotNull(player);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod]
	public void CreatePlayerWithExperienceAndInventoryTest()
	{
		IPlayer? player;
		IInventory inventory = ModelFactory.CreateInventory();
		int money = 12000;
		inventory.Add(money);
		int experience = money;

		player = ModelFactory.CreatePlayer(inventory, experience);

		Assert.IsNotNull(player);
		Assert.AreEqual(money, player.Inventory.Money);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod()]
	public void CreateExistingPlayerTest()
	{
		IPlayer? player;
		IInventory inventory = ModelFactory.CreateInventory();
		_ = inventory.Randomize();
		int experience = 500;
		IEnumerable<ITransaction> logEntries = new List<ITransaction>()
		{
			ModelFactory.CreateTransaction(DateTime.Now, TransactionType.TRAFFIC, DrugType.COKE, 10, 1000),
			ModelFactory.CreateTransaction(DateTime.Now.AddDays(-1), TransactionType.DEPOSIT, DrugType.METH, 10, 0),
		};

		player = ModelFactory.CreatePlayer(inventory, experience, logEntries);

		Assert.IsNotNull(player);
		Assert.AreEqual(experience, player.Experience);
		Assert.AreEqual(inventory.Money, player.Inventory.Money);
		Assert.AreEqual(inventory.Count, player.Inventory.Count);
		Assert.AreEqual(inventory.TotalQuantity, player.Inventory.TotalQuantity);
		Assert.AreEqual(logEntries.Count(), player.Transactions.Count);
	}

	[TestMethod]
	public void CreateTransactionTest()
	{
		ITransaction? transaction;

		transaction = ModelFactory.CreateTransaction(DateTime.Now, TransactionType.DEPOSIT, DrugType.COKE, 10, 100);

		Assert.IsNotNull(transaction);
	}
}