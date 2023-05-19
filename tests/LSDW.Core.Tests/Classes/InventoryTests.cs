using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryTests
{
	[TestMethod]
	public void AddExistingDrugTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 100));
		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 50));

		Assert.AreEqual(20, inventory.TotalQuantity);
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 75));
		inventory.Add(DrugFactory.CreateDrug(DrugType.METH, 10, 100));

		Assert.AreEqual(20, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 75));
		inventory.Remove(DrugFactory.CreateDrug(DrugType.COKE, 5, 100));

		Assert.AreEqual(5, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveDrugIsFalseTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		bool success = inventory.Remove(DrugFactory.CreateDrug(DrugType.COKE, 10, default));

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void AddMoneyTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();
		int moneyToAdd = 10000;

		inventory.Add(moneyToAdd);

		Assert.AreEqual(moneyToAdd, inventory.Money);
	}

	[TestMethod]
	public void AddNoMoneyTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Add(0);

		Assert.AreEqual(0, inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		IInventory inventory = InventoryFactory.CreateInventory(drugs, 1000);

		inventory.Remove(500);

		Assert.AreEqual(500, inventory.Money);
	}

	[TestMethod]
	public void RemoveNoMoneyTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		IInventory inventory = InventoryFactory.CreateInventory(drugs, 1000);

		inventory.Remove(0);

		Assert.AreEqual(1000, inventory.Money);
	}

	[TestMethod()]
	public void AddRangeTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();

		IInventory inventory = InventoryFactory.CreateInventory();
		inventory.Add(drugs);

		Assert.AreEqual(drugs.Count(), inventory.Count);
	}

	[TestMethod()]
	public void RemoveTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		IInventory inventory = InventoryFactory.CreateInventory(drugs, 1000);

		inventory.Remove(drugs);

		Assert.AreEqual(0, inventory.Count);
		Assert.AreEqual(1000, inventory.Money);
	}
}
