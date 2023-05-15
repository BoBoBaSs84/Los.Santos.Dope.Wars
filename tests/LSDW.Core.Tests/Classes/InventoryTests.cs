using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using DF = LSDW.Core.Factories.DrugFactory;
using IF = LSDW.Core.Factories.InventoryFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryTests
{
	[TestMethod]
	public void AddExistingDrugTest()
	{
		IInventory inventory = IF.CreateInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 1000));
		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 500));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventory inventory = IF.CreateInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 750));
		inventory.Add(DF.CreateDrug(DrugType.METH, 10, 1000));

		Assert.IsTrue(inventory.Count.Equals(2));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventory inventory = IF.CreateInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 3000));
		inventory.Remove(DF.CreateDrug(DrugType.COKE, 5, 1000));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(5));
	}

	[TestMethod]
	public void RemoveExistingDrugCompletelyTest()
	{
		IInventory inventory = IF.CreateInventory();
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, default);

		inventory.Add(drug);
		inventory.Remove(drug);

		Assert.IsTrue(inventory.Count.Equals(0));
		Assert.IsTrue(inventory.TotalQuantity.Equals(0));
	}

	[TestMethod]
	public void RemoveDrugIsFalseTest()
	{
		IInventory inventory = IF.CreateInventory();

		bool success = inventory.Remove(DF.CreateDrug(DrugType.COKE, 10, default));

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void AddMoneyTest()
	{
		IInventory inventory = IF.CreateInventory();
		int moneyToAdd = 10000;

		inventory.Add(moneyToAdd);

		Assert.AreEqual(moneyToAdd, inventory.Money);
	}

	[TestMethod]
	public void AddMoneyExceptionTest()
	{
		IInventory inventory = IF.CreateInventory();
		int moneyToAdd = -10000;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Add(moneyToAdd));
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		IInventory inventory = IF.CreateInventory();
		int moneyToRemove = 10000;

		inventory.Remove(moneyToRemove);

		Assert.AreEqual(moneyToRemove * -1, inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyExceptionTest()
	{
		IInventory inventory = IF.CreateInventory();
		int moneyToRemove = -10000;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Remove(moneyToRemove));
	}
}
