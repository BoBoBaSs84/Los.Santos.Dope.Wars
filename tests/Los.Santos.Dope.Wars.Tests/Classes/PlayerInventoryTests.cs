using LSDW.Classes;
using LSDW.Enumerators;
using LSDW.Interfaces.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace LSDW.Tests.Classes
{
	[TestClass]
	public class PlayerInventoryTests
	{
		private readonly List<IDrug> _drugList = new List<IDrug>();

		[TestMethod]
		public void AddExistingDrugTest()
		{
			IInventory inventory = new PlayerInventory(_drugList);
			Assert.IsFalse(inventory.Any());

			inventory.Add(100000);
			inventory.Add(new Drug(DrugType.COKE, 10, 1000));
			inventory.Add(new Drug(DrugType.COKE, 10, 500));

			Assert.IsTrue(inventory.Count.Equals(1));
			Assert.IsTrue(inventory.TotalQuantity.Equals(20));
			Assert.IsTrue(inventory.TotalMarketValue.Equals(15000));
			Assert.IsTrue(inventory.TotalProfit.Equals(5000));
			Assert.IsTrue(inventory.Money.Equals(85000));
		}

		[TestMethod]
		public void AddNewDrugTest()
		{
			IInventory inventory = new PlayerInventory(_drugList);
			Assert.IsFalse(inventory.Any());

			inventory.Add(100000);
			inventory.Add(new Drug(DrugType.COKE, 10, 750));
			inventory.Add(new Drug(DrugType.METH, 10, 1000));

			Assert.IsTrue(inventory.Count.Equals(2));
			Assert.IsTrue(inventory.TotalQuantity.Equals(20));
			Assert.IsTrue(inventory.TotalMarketValue.Equals(22500));
			Assert.IsTrue(inventory.TotalProfit.Equals(-3750));
			Assert.IsTrue(inventory.Money.Equals(67500));
		}

		[TestMethod]
		public void RemoveExistingDrugTest()
		{
			IInventory inventory = new PlayerInventory(_drugList);
			Assert.IsFalse(inventory.Any());

			inventory.Add(100000);
			inventory.Add(new Drug(DrugType.COKE, 10, 3000));
			inventory.Remove(new Drug(DrugType.COKE, 5, 1000));

			Assert.IsTrue(inventory.Count.Equals(1));
			Assert.IsTrue(inventory.TotalQuantity.Equals(5));
			Assert.IsTrue(inventory.TotalMarketValue.Equals(15000));
			Assert.IsTrue(inventory.TotalProfit.Equals(-10000));
			Assert.IsTrue(inventory.Money.Equals(75000));
		}
	}
}
