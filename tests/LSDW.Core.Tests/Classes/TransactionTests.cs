using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Exceptions;
using LSDW.Core.Interfaces.Classes;
using CF = LSDW.Core.Factories.CharacterFactory;
using DF = LSDW.Core.Factories.DrugFactory;
using IF = LSDW.Core.Factories.InventoryFactory;
using TF = LSDW.Core.Factories.TransactionFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class TransactionTests
{
	[TestMethod]
	public void TransactSuccessTest()
	{
		IDrug cokeInStock = DF.CreateDrug(DrugType.COKE, 15, 1000);
		IInventoryCollection dealerInventory = IF.CreateInventory(new List<IDrug>() { cokeInStock });

		IPlayerCharacter player = CF.CreatePlayer();
		player.Inventory.Add(5000);

		List<TransactionObject> objects = new() { new(DrugType.COKE, 5, 900) };
		ITransaction transaction = TF.CreateTrafficTransaction(objects, player.MaximumInventoryQuantity);

		TransactionResult result = transaction.Commit(dealerInventory, player.Inventory);

		Assert.IsTrue(result.Successful);
		Assert.IsFalse(result.Messages.Any());
		Assert.AreEqual(4500, dealerInventory.Money);
		Assert.AreEqual(10, dealerInventory.TotalQuantity);
		Assert.AreEqual(500, player.Inventory.Money);
		Assert.AreEqual(5, player.Inventory.TotalQuantity);
	}

	[TestMethod]
	public void TransactionFailedQuantityTest()
	{
		IDrug cokeInStock = DF.CreateDrug(DrugType.COKE, 250, 1000);
		IInventoryCollection dealerInventory = IF.CreateInventory(new List<IDrug>() { cokeInStock });

		IPlayerCharacter player = CF.CreatePlayer();
		player.Inventory.Add(500000);

		List<TransactionObject> objects = new() { new(DrugType.COKE, 150, 900) };
		ITransaction transaction = TF.CreateTrafficTransaction(objects, player.MaximumInventoryQuantity);

		TransactionResult result = transaction.Commit(dealerInventory, player.Inventory);

		Assert.IsFalse(result.Successful);
		Assert.IsTrue(result.Messages.Any());
	}

	[TestMethod]
	public void TransactionFailedMoneyTest()
	{
		IDrug cokeInStock = DF.CreateDrug(DrugType.COKE, 25, 1000);
		IInventoryCollection dealerInventory = IF.CreateInventory(new List<IDrug>() { cokeInStock });

		IPlayerCharacter player = CF.CreatePlayer();
		player.Inventory.Add(500);

		List<TransactionObject> objects = new() { new(DrugType.COKE, 5, 900) };
		ITransaction transaction = TF.CreateTrafficTransaction(objects, player.MaximumInventoryQuantity);

		TransactionResult result = transaction.Commit(dealerInventory, player.Inventory);

		Assert.IsFalse(result.Successful);
		Assert.IsTrue(result.Messages.Any());
	}
}