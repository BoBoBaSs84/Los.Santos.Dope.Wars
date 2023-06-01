using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;

namespace LSDW.Core.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryTests
{
	[TestMethod]
	public void AddExistingDrugTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();

		inventory.Add(ModelFactory.CreateDrug(DrugType.COKE, 10, 100));
		inventory.Add(ModelFactory.CreateDrug(DrugType.COKE, 10, 50));

		Assert.AreEqual(20, inventory.TotalQuantity);
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();

		inventory.Add(ModelFactory.CreateDrug(DrugType.COKE, 10, 75));
		inventory.Add(ModelFactory.CreateDrug(DrugType.METH, 10, 100));

		Assert.AreEqual(20, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();

		inventory.Add(ModelFactory.CreateDrug(DrugType.COKE, 10, 75));
		inventory.Remove(ModelFactory.CreateDrug(DrugType.COKE, 5, 100));

		Assert.AreEqual(5, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveDrugQuantityExceptionTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();
		IDrug drugToRemove = ModelFactory.CreateDrug(DrugType.COKE, 10, 50);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => inventory.Remove(drugToRemove));
	}

	[TestMethod]
	public void AddMoneyTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();
		int moneyToAdd = 10000;

		inventory.Add(moneyToAdd);

		Assert.AreEqual(moneyToAdd, inventory.Money);
	}

	[TestMethod]
	public void AddNoMoneyTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();

		inventory.Add(0);

		Assert.AreEqual(0, inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		IEnumerable<IDrug> drugs = ModelFactory.CreateAllDrugs();
		IInventory inventory = ModelFactory.CreateInventory(drugs, 1000);

		inventory.Remove(500);

		Assert.AreEqual(500, inventory.Money);
	}

	[TestMethod]
	public void RemoveNoMoneyTest()
	{
		IEnumerable<IDrug> drugs = ModelFactory.CreateAllDrugs();
		IInventory inventory = ModelFactory.CreateInventory(drugs, 1000);

		inventory.Remove(0);

		Assert.AreEqual(1000, inventory.Money);
	}

	[TestMethod()]
	public void AddRangeTest()
	{
		IEnumerable<IDrug> drugs = ModelFactory.CreateAllDrugs();

		IInventory inventory = ModelFactory.CreateInventory();
		inventory.Add(drugs);

		Assert.AreEqual(drugs.Count(), inventory.Count);
	}

	[TestMethod()]
	public void RemoveTest()
	{
		IInventory inventory = ModelFactory.CreateInventory();
		inventory = inventory.Randomize();

		inventory.Remove(inventory);

		Assert.AreEqual(15, inventory.Count);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(0, inventory.TotalProfit);
		Assert.AreEqual(0, inventory.TotalValue);
	}
}
