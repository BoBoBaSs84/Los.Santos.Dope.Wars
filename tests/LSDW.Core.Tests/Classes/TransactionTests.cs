using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Factories;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class TransactionTests
{
	[TestMethod]
	public void CommitSuccessTest()
	{
		IDrug dealerDrug = DrugFactory.CreateDrug(DrugType.COKE, 15, 100);
		IInventory dealerInventory = InventoryFactory.CreateInventory();
		dealerInventory.Add(dealerDrug);

		IPlayer player = PlayerFactory.CreatePlayer();
		player.Inventory.Add(500);

		List<TransactionObject> objects = new() { new(DrugType.COKE, 5, 90) };
		ITransaction transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, objects, player.MaximumInventoryQuantity);

		transaction.Commit();

		Assert.IsTrue(transaction.Result.Successful);
		Assert.IsTrue(transaction.Result.IsCompleted);
		Assert.AreEqual(1, transaction.Result.Messages.Count);
		Assert.AreEqual(450, dealerInventory.Money);
		Assert.AreEqual(10, dealerInventory.TotalQuantity);
		Assert.AreEqual(50, player.Inventory.Money);
		Assert.AreEqual(5, player.Inventory.TotalQuantity);
	}

	[TestMethod]
	public void CommitFailedQuantityTest()
	{
		IDrug dealerDrug = DrugFactory.CreateDrug(DrugType.COKE, 250, 100);
		IInventory dealerInventory = InventoryFactory.CreateInventory();
		dealerInventory.Add(dealerDrug);

		IPlayer player = PlayerFactory.CreatePlayer();
		player.Inventory.Add(500000);

		List<TransactionObject> objects = new() { new(DrugType.COKE, 150, 90) };
		ITransaction transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, objects, player.MaximumInventoryQuantity);

		transaction.Commit();

		Assert.IsFalse(transaction.Result.Successful);
		Assert.IsTrue(transaction.Result.IsCompleted);
		Assert.IsTrue(transaction.Result.Messages.Any());
	}

	[TestMethod]
	public void CommitFailedMoneyTest()
	{
		IDrug dealerDrug = DrugFactory.CreateDrug(DrugType.COKE, 25, 100);
		IInventory dealerInventory = InventoryFactory.CreateInventory();
		dealerInventory.Add(dealerDrug);

		IPlayer player = PlayerFactory.CreatePlayer();
		player.Inventory.Add(50);

		List<TransactionObject> objects = new() { new(DrugType.COKE, 5, 90) };
		ITransaction transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, objects, player.MaximumInventoryQuantity);

		transaction.Commit();

		Assert.IsFalse(transaction.Result.Successful);
		Assert.IsTrue(transaction.Result.IsCompleted);
		Assert.IsTrue(transaction.Result.Messages.Any());
	}

	[TestMethod]
	public void CommitDepositSuccessTest()
	{
		IDrug cokeToDeposit = DrugFactory.CreateDrug(DrugType.COKE, 50, 85);
		IPlayer player = PlayerFactory.CreatePlayer();
		IInventory warehouse = InventoryFactory.CreateInventory();
		player.Inventory.Add(cokeToDeposit);
		player.Inventory.Add(1200);

		List<TransactionObject> objects = new() { new(cokeToDeposit) };
		ITransaction transaction = TransactionFactory.CreateDepositTransaction(player.Inventory, warehouse, objects);

		transaction.Commit();

		Assert.IsTrue(transaction.Result.Successful);
		Assert.IsTrue(transaction.Result.IsCompleted);
		Assert.AreEqual(1, transaction.Result.Messages.Count);
		Assert.AreEqual(0, player.Inventory.TotalQuantity);
		Assert.AreEqual(1200, player.Inventory.Money);
		Assert.AreEqual(50, warehouse.TotalQuantity);
	}
}