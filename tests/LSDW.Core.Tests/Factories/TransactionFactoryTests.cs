using LSDW.Core.Classes;
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
		IEnumerable<TransactionObject> objects = Enumerable.Empty<TransactionObject>();

		transaction = TransactionFactory.CreateTrafficTransaction(dealerInventory, player.Inventory, objects, player.MaximumInventoryQuantity);

		Assert.IsNotNull(transaction);
		Assert.IsNotNull(transaction.Objects);
		Assert.IsFalse(transaction.Result.IsCompleted);
		Assert.AreEqual(player.MaximumInventoryQuantity, transaction.MaximumTargetQuantity);
		Assert.AreEqual(TransactionType.TRAFFIC, transaction.Type);
	}

	[TestMethod]
	public void CreateDepositTransactionNoSizeLimitTest()
	{
		ITransaction? transaction;
		IInventory warehouseInventory = InventoryFactory.CreateInventory();
		IPlayer player = PlayerFactory.CreatePlayer();
		IEnumerable<TransactionObject> objects = Enumerable.Empty<TransactionObject>();

		transaction = TransactionFactory.CreateDepositTransaction(player.Inventory, warehouseInventory, objects);

		Assert.IsNotNull(transaction);
		Assert.IsNotNull(transaction.Objects);
		Assert.IsFalse(transaction.Result.IsCompleted);
		Assert.AreEqual(int.MaxValue, transaction.MaximumTargetQuantity);
		Assert.AreEqual(TransactionType.DEPOSIT, transaction.Type);
	}

	[TestMethod]
	public void CreateDepositTransactionWithSizeLimitTest()
	{
		ITransaction? transaction;
		IInventory warehouseInventory = InventoryFactory.CreateInventory();
		IPlayer player = PlayerFactory.CreatePlayer();
		IEnumerable<TransactionObject> objects = Enumerable.Empty<TransactionObject>();

		transaction = TransactionFactory.CreateDepositTransaction(warehouseInventory, player.Inventory, objects, player.MaximumInventoryQuantity);

		Assert.IsNotNull(transaction);
		Assert.IsNotNull(transaction.Objects);
		Assert.IsFalse(transaction.Result.IsCompleted);
		Assert.AreEqual(player.MaximumInventoryQuantity, transaction.MaximumTargetQuantity);
		Assert.AreEqual(TransactionType.DEPOSIT, transaction.Type);
	}
}