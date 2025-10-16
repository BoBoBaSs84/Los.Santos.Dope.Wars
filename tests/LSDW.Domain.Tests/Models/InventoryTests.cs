using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class InventoryTests
{
	[TestMethod]
	public void AddExistingDrugTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Add(DomainFactory.CreateDrug(DrugType.COKE, 10, 100));
		inventory.Add(DomainFactory.CreateDrug(DrugType.COKE, 10, 50));

		Assert.AreEqual(20, inventory.TotalQuantity);
	}

	[TestMethod]
	public void AddNewDrugTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Add(DomainFactory.CreateDrug(DrugType.COKE, 10, 75));
		inventory.Add(DomainFactory.CreateDrug(DrugType.METH, 10, 100));

		Assert.AreEqual(20, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveExistingDrugTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Add(DomainFactory.CreateDrug(DrugType.COKE, 10, 75));
		inventory.Remove(DomainFactory.CreateDrug(DrugType.COKE, 5, 100));

		Assert.AreEqual(5, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveDrugQuantityExceptionTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		IDrug drugToRemove = DomainFactory.CreateDrug(DrugType.COKE, 10, 50);

		Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Remove(drugToRemove));
	}

	[TestMethod]
	public void AddMoneyTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		int moneyToAdd = 10000;

		inventory.Add(moneyToAdd);

		Assert.AreEqual(moneyToAdd, inventory.Money);
	}

	[TestMethod]
	public void AddNoMoneyTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Add(0);

		Assert.AreEqual(0, inventory.Money);
	}

	[TestMethod]
	public void RemoveMoneyTest()
	{
		ICollection<IDrug> drugs = DomainFactory.CreateAllDrugs();
		IInventory inventory = DomainFactory.CreateInventory(drugs, 1000);

		inventory.Remove(500);

		Assert.AreEqual(500, inventory.Money);
	}

	[TestMethod]
	public void RemoveNoMoneyTest()
	{
		ICollection<IDrug> drugs = DomainFactory.CreateAllDrugs();
		IInventory inventory = DomainFactory.CreateInventory(drugs, 1000);

		inventory.Remove(0);

		Assert.AreEqual(1000, inventory.Money);
	}

	[TestMethod]
	public void AddRangeTest()
	{
		ICollection<IDrug> drugs = DomainFactory.CreateAllDrugs();

		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drugs);

		Assert.AreEqual(drugs.Count, inventory.Count);
	}

	[TestMethod]
	public void RemoveTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		inventory = inventory.Restock();

		inventory.Remove(inventory);

		Assert.AreEqual(15, inventory.Count);
		Assert.AreEqual(0, inventory.TotalQuantity);
		Assert.AreEqual(0, inventory.TotalValue);
	}

	[TestMethod]
	public void AddWithParamsTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.HASH, 50, 10);
		IInventory inventory = DomainFactory.CreateInventory();

		inventory.Add(drug.Type, drug.Quantity, drug.Price);

		Assert.AreEqual(drug.Quantity * drug.Price, inventory.TotalValue);
		Assert.AreEqual(drug.Quantity, inventory.TotalQuantity);
	}

	[TestMethod]
	public void RemoveWithParamsTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.HASH, 50, 10);
		IInventory inventory = DomainFactory.CreateInventory();
		inventory.Add(drug);

		inventory.Remove(drug.Type, drug.Quantity);

		Assert.AreEqual(default, inventory.TotalValue);
		Assert.AreEqual(default, inventory.TotalQuantity);
	}

	[TestMethod]
	public void ClearTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		int oldCount = inventory.Count;

		inventory.Clear();
		int newCount = inventory.Count;

		Assert.AreNotEqual(oldCount, newCount);
	}

	[TestMethod]
	public void ContainsTest()
	{
		IDrug drug = DomainFactory.CreateDrug(DrugType.COKE, 50, 10);
		ICollection<IDrug> drugs = new HashSet<IDrug>() { drug };

		IInventory inventory = DomainFactory.CreateInventory(drugs, default);

		Assert.IsTrue(inventory.Contains(drug));
	}

	[TestMethod]
	public void CopyToTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();
		IDrug[] drugs = new IDrug[inventory.Count];

		inventory.CopyTo(drugs, 0);

		Assert.AreEqual(inventory.Count, drugs.Length);
	}

	[TestMethod]
	public void GetEnumeratorTest()
	{
		IInventory inventory = DomainFactory.CreateInventory();

		IEnumerator<IDrug> enumerator = inventory.GetEnumerator();

		Assert.IsNotNull(enumerator);
	}
}
