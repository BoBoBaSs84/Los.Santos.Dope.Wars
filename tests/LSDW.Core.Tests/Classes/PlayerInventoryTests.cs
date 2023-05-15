using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using DF = LSDW.Core.Factories.DrugFactory;
using IF = LSDW.Core.Factories.InventoryFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class PlayerInventoryTests
{
	[TestMethod]
	public void AddExistingDrugTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 1000));
		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 500));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 750));
		inventory.Add(DF.CreateDrug(DrugType.METH, 10, 1000));

		Assert.IsTrue(inventory.Count.Equals(2));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 3000));
		inventory.Remove(DF.CreateDrug(DrugType.COKE, 5, 1000));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(5));
	}

	[TestMethod]
	public void RemoveExistingDrugCompletelyTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, default);

		inventory.Add(drug);
		inventory.Remove(drug);

		Assert.IsTrue(inventory.Count.Equals(0));
		Assert.IsTrue(inventory.TotalQuantity.Equals(0));
	}

	[TestMethod]
	public void RemoveDrugIsFalseTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();

		bool success = inventory.Remove(DF.CreateDrug(DrugType.COKE, 10, default));

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void AddMoneyTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();
		int moneyToAdd = 10000;

		inventory.Add(moneyToAdd);

		Assert.AreEqual(moneyToAdd, inventory.Money);
	}

	[TestMethod]
	public void AddMoneyExceptionTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();
		int moneyToAdd = -10000;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Add(moneyToAdd));
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();
		int moneyToRemove = 10000;

		inventory.Remove(moneyToRemove);

		Assert.AreEqual(moneyToRemove * -1, inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyExceptionTest()
	{
		IInventoryCollection inventory = IF.CreatePlayerInventory();
		int moneyToRemove = -10000;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Remove(moneyToRemove));
	}
}
