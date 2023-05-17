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

		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 1000));
		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 500));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 750));
		inventory.Add(DrugFactory.CreateDrug(DrugType.METH, 10, 1000));

		Assert.IsTrue(inventory.Count.Equals(2));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();

		inventory.Add(DrugFactory.CreateDrug(DrugType.COKE, 10, 3000));
		inventory.Remove(DrugFactory.CreateDrug(DrugType.COKE, 5, 1000));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(5));
	}

	[TestMethod]
	public void RemoveExistingDrugCompletelyTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory();
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 10, default);

		inventory.Add(drug);
		inventory.Remove(drug);

		Assert.IsTrue(inventory.Count.Equals(0));
		Assert.IsTrue(inventory.TotalQuantity.Equals(0));
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
		IInventory inventory = InventoryFactory.CreateInventory(1000);

		inventory.Add(0);

		Assert.AreEqual(1000, inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory(1000);

		inventory.Remove(500);

		Assert.AreEqual(500, inventory.Money);
	}

	[TestMethod]
	public void RemoveNoMoneyTest()
	{
		IInventory inventory = InventoryFactory.CreateInventory(1000);

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

		IInventory inventory = InventoryFactory.CreateInventory(drugs);

		inventory.Remove(drugs);

		Assert.AreEqual(0, inventory.Count);
	}
}
