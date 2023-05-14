using LSDW.Enumerators;
using LSDW.Interfaces.Classes;
using System.Linq;
using DF = LSDW.Factories.DrugFactory;
using IF = LSDW.Factories.InventoryFactory;

namespace LSDW.Tests.Classes
{
	[TestClass]
	public class PlayerInventoryTests
	{
		[TestMethod]
		public void AddExistingDrugTest()
		{
			IInventory inventory = IF.CreatePlayerInventory();
			Assert.IsFalse(inventory.Any());

			inventory.Add(100000);
			inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 1000));
			inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 500));

			Assert.IsTrue(inventory.Count.Equals(1));
			Assert.IsTrue(inventory.TotalQuantity.Equals(20));
			Assert.IsTrue(inventory.TotalMarketValue.Equals(15000));
			Assert.IsTrue(inventory.TotalProfit.Equals(5000));
			Assert.IsTrue(inventory.Money.Equals(85000));
		}

		[TestMethod]
		public void AddNewDrugTest()
		{
			IInventory inventory = IF.CreatePlayerInventory();
			Assert.IsFalse(inventory.Any());

			inventory.Add(100000);
			inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 750));
			inventory.Add(DF.CreateDrug(DrugType.METH, 10, 1000));

			Assert.IsTrue(inventory.Count.Equals(2));
			Assert.IsTrue(inventory.TotalQuantity.Equals(20));
			Assert.IsTrue(inventory.TotalMarketValue.Equals(22500));
			Assert.IsTrue(inventory.TotalProfit.Equals(-3750));
			Assert.IsTrue(inventory.Money.Equals(67500));
		}

		[TestMethod]
		public void RemoveExistingDrugTest()
		{
			IInventory inventory = IF.CreatePlayerInventory();
			Assert.IsFalse(inventory.Any());

			inventory.Add(100000);
			inventory.Add(DF.CreateDrug(DrugType.COKE, 10, 3000));
			inventory.Remove(DF.CreateDrug(DrugType.COKE, 5, 1000));

			Assert.IsTrue(inventory.Count.Equals(1));
			Assert.IsTrue(inventory.TotalQuantity.Equals(5));
			Assert.IsTrue(inventory.TotalMarketValue.Equals(15000));
			Assert.IsTrue(inventory.TotalProfit.Equals(-10000));
			Assert.IsTrue(inventory.Money.Equals(75000));
		}
	}
}
