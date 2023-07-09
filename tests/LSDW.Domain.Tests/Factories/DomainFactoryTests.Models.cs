using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateDrugWithDrugTypeTest()
	{
		DrugType drugType = DrugType.SMACK;
		IDrug? drug;

		drug = DomainFactory.CreateDrug(drugType);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.Type);
	}

	[TestMethod]
	public void CreateDrugWithDrugTypeAndQuantityTest()
	{
		DrugType drugType = DrugType.SMACK;
		int quantity = 10;
		IDrug? drug;

		drug = DomainFactory.CreateDrug(drugType, quantity);

		Assert.IsNotNull(drug);
		Assert.AreEqual(drugType, drug.Type);
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
		Assert.AreEqual(drugType, drug.Type);
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
		ICollection<IDrug>? drugs;

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
		ICollection<IDrug> drugs = DomainFactory.CreateAllDrugs();
		IInventory? inventory;

		inventory = DomainFactory.CreateInventory(drugs, money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(money, inventory.Money);
		Assert.AreEqual(drugs.Count, inventory.Count);
	}

	[TestMethod]
	public void CreateTransactionTest()
	{
		DateTime date = DateTime.MinValue;
		DrugType drugType = DrugType.COKE;
		int quantity = 10;
		int price = 100;
		TransactionType transactionType = TransactionType.BUY;

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
		ICollection<ITransaction> transactions = new HashSet<ITransaction>();
		IPlayer? player;

		player = DomainFactory.CreatePlayer(inventory, experience, transactions);

		Assert.IsNotNull(player);
		Assert.IsNotNull(player.Inventory);
		Assert.AreEqual(experience, player.Experience);
	}

	[TestMethod]
	public void CreateDealerTest()
	{
		IDealer? dealer;

		dealer = DomainFactory.CreateDealer(_zeroVector);

		Assert.IsNotNull(dealer);
		Assert.AreEqual(dealer.SpawnPosition, _zeroVector);
		Assert.AreEqual(TaskType.NOTASK, dealer.CurrentTask);
	}

	[TestMethod]
	public void CreateDealerWithParamsTest()
	{
		IDealer? dealer;

		dealer = DomainFactory.CreateDealer(_zeroVector, _pedHash);

		Assert.IsNotNull(dealer);
		Assert.AreEqual(dealer.SpawnPosition, _zeroVector);
		Assert.AreEqual(dealer.Hash, _pedHash);
		Assert.AreEqual(TaskType.NOTASK, dealer.CurrentTask);
	}

	[TestMethod]
	public void CreateDealersTest()
	{
		ICollection<IDealer>? dealers;

		dealers = DomainFactory.CreateDealers();

		Assert.IsNotNull(dealers);
	}
}