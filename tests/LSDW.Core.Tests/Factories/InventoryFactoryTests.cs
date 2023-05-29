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
		Assert.AreEqual(15, inventory.Count);
		Assert.AreEqual(0, inventory.Money);
	}

	[TestMethod]
	public void CreateInventoryWithDrugsAndMoneyTest()
	{
		IInventory? inventory;
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		int money = 1000;

		inventory = InventoryFactory.CreateInventory(drugs, money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(money, inventory.Money);
		Assert.AreEqual(drugs.Count(), inventory.Count);
	}

	[TestMethod]
	public void CreateInventoryWithMoneyTest()
	{
		IInventory? inventory;
		int money = 10000;

		inventory = InventoryFactory.CreateInventory(money);

		Assert.IsNotNull(inventory);
		Assert.AreEqual(15, inventory.Count);
		Assert.AreEqual(money, inventory.Money);
	}
}