using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class TransactionFactoryTests
{
	[TestMethod]
	public void CreateTrafficTransactionTest()
	{
		ITransaction? transaction;
		IInventory dealerInventory = InventoryFactory.CreateInventory();
		IPlayer player = PlayerFactory.CreatePlayer();

		transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, player.MaximumInventoryQuantity);

		Assert.IsNotNull(transaction);
		Assert.AreEqual(player.MaximumInventoryQuantity, transaction.MaximumTargetQuantity);
		Assert.AreEqual(TransactionType.TRAFFIC, transaction.Type);
	}

	[TestMethod]
	public void CreateDepositTransactionNoSizeLimitTest()
	{
		ITransaction? transaction;
		IInventory warehouseInventory = InventoryFactory.CreateInventory();
		IPlayer player = PlayerFactory.CreatePlayer();

		transaction = TransactionFactory.CreateDepositTransaction(player.Inventory, warehouseInventory);

		Assert.IsNotNull(transaction);
		Assert.AreEqual(int.MaxValue, transaction.MaximumTargetQuantity);
		Assert.AreEqual(TransactionType.DEPOSIT, transaction.Type);
	}

	[TestMethod]
	public void CreateDepositTransactionWithSizeLimitTest()
	{
		ITransaction? transaction;
		IInventory warehouseInventory = InventoryFactory.CreateInventory();
		IPlayer player = PlayerFactory.CreatePlayer();

		transaction = TransactionFactory.CreateDepositTransaction(warehouseInventory, player.Inventory, player.MaximumInventoryQuantity);

		Assert.IsNotNull(transaction);
		Assert.AreEqual(player.MaximumInventoryQuantity, transaction.MaximumTargetQuantity);
		Assert.AreEqual(TransactionType.DEPOSIT, transaction.Type);
	}
}