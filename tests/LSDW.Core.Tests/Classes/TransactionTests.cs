using LSDW.Core.Enumerators;
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

		IDrug cokeToBuy = DF.CreateDrug(DrugType.COKE, 5, 900);
		ITransaction transaction = TF.CreateTrafficTransaction(new List<IDrug>() { cokeToBuy }, player.MaximumInventoryQuantity);

		transaction.Transact(dealerInventory, player.Inventory);

		Assert.AreEqual(4500, dealerInventory.Money);
		Assert.AreEqual(10, dealerInventory.TotalQuantity);
		Assert.AreEqual(500, player.Inventory.Money);
		Assert.AreEqual(5, player.Inventory.TotalQuantity);
	}
}