using LSDW.Enumerators;
using LSDW.Interfaces.Classes;
using DF = LSDW.Factories.DrugFactory;
using IF = LSDW.Factories.InventoryFactory;

namespace LSDW.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class PlayerInventoryTests
{
	[TestMethod]
	public void AddExistingDrugTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 1000));
		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 500));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 750));
		inventory.Add(DF.CreateDrug(DrugType.METH, 10, 1000));

		Assert.IsTrue(inventory.Count.Equals(2));
		Assert.IsTrue(inventory.TotalQuantity.Equals(20));
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();

		inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 3000));
		inventory.Remove(DF.CreateDrug(DrugType.COKE, 5, 1000));

		Assert.IsTrue(inventory.Count.Equals(1));
		Assert.IsTrue(inventory.TotalQuantity.Equals(5));
	}

	[TestMethod]
	public void AddMoneyTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();		
		int moneyToAdd = 10000;

		inventory.Add(moneyToAdd);

		Assert.AreEqual(moneyToAdd, inventory.Money);
	}

	[TestMethod]
	public void AddMoneyExceptionTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();
		int moneyToAdd = -10000;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Add(moneyToAdd));
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();
		int moneyToRemove = 10000;

		inventory.Remove(moneyToRemove);

		Assert.AreEqual(moneyToRemove * (-1), inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyExceptionTest()
	{
		IInventory inventory = IF.CreatePlayerInventory();
		int moneyToRemove = -10000;

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Add(moneyToRemove));
	}
}
