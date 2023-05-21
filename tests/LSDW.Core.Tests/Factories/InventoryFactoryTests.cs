using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class InventoryFactoryTests
{
	[TestMethod]
	public void CreateInventoryTest()
	{
		IInventory? inventory;

		inventory = InventoryFactory.CreateInventory();

		Assert.IsNotNull(inventory);
	}

	[TestMethod]
	public void CreateInventoryWithParamsTest()
	{
		IInventory? inventory;
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		int money = 1000;
		
		inventory = InventoryFactory.CreateInventory(drugs, money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(money, inventory.Money);
		Assert.AreEqual(drugs.Count(), inventory.Count);
	}
}