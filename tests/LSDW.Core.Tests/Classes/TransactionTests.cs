using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

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

		ITransaction transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, player.MaximumInventoryQuantity);
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 5, 90);

		transaction.Add(drug);
		transaction.Commit();

		Assert.IsTrue(transaction.Result.Successful);
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

		ITransaction transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, player.MaximumInventoryQuantity);
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 150, 90);

		transaction.Add(drug);
		transaction.Commit();

		Assert.IsFalse(transaction.Result.Successful);
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

		List<IDrug> drugList = new();
		IDrug cokeToAdd = DrugFactory.CreateDrug(DrugType.COKE, 5, 90);
		drugList.Add(cokeToAdd);
		IDrug methToAdd = DrugFactory.CreateDrug(DrugType.METH, 10, 110);
		drugList.Add(methToAdd);

		ITransaction transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, player.MaximumInventoryQuantity);

		transaction.Add(drugList);
		transaction.Commit();

		Assert.IsFalse(transaction.Result.Successful);
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

		ITransaction transaction = TransactionFactory.CreateDepositTransaction(player.Inventory, warehouse);
		transaction.Add(cokeToDeposit);

		transaction.Commit();

		Assert.IsTrue(transaction.Result.Successful);
		Assert.AreEqual(1, transaction.Result.Messages.Count);
		Assert.AreEqual(0, player.Inventory.TotalQuantity);
		Assert.AreEqual(1200, player.Inventory.Money);
		Assert.AreEqual(50, warehouse.TotalQuantity);
	}
}