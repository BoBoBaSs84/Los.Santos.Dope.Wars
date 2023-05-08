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
		private readonly List<Drug> _drugList = new List<Drug>();

		[TestMethod]
		public void AddExistingDrugTest()
		{
			IInventory inventory = new PlayerInventory(_drugList);
			Assert.IsFalse(inventory.Any());

			inventory.Add(new Drug(DrugType.COKE, 10, 1000));
			inventory.Add(new Drug(DrugType.COKE, 10, 500));

			Assert.IsTrue(inventory.Count.Equals(1));
			Assert.IsTrue(inventory.TotalQuantity.Equals(20));
			Assert.IsTrue(inventory.TotalValue.Equals(15000));
			Assert.IsTrue(inventory.TotalProfit.Equals(25000));
		}

		[TestMethod]
		public void AddNewDrugTest()
		{
			IInventory inventory = new PlayerInventory(_drugList);
			Assert.IsFalse(inventory.Any());

			inventory.Add(new Drug(DrugType.COKE, 10, 1000));
			inventory.Add(new Drug(DrugType.METH, 15, 1500));

			Assert.IsTrue(inventory.Count.Equals(2));
			Assert.IsTrue(inventory.TotalQuantity.Equals(25));
			Assert.IsTrue(inventory.TotalValue.Equals(32500));
			Assert.IsTrue(inventory.TotalProfit.Equals(21250));
		}

		[TestMethod]
		public void RemoveExistingDrugTest()
		{
			IInventory inventory = new PlayerInventory(_drugList);
			Assert.IsFalse(inventory.Any());

			inventory.Add(new Drug(DrugType.COKE, 10, 1500));
			inventory.Remove(new Drug(DrugType.COKE, 5, 1000));

			Assert.IsTrue(inventory.Count.Equals(1));
			Assert.IsTrue(inventory.TotalQuantity.Equals(5));
			Assert.IsTrue(inventory.TotalValue.Equals(7500));
			Assert.IsTrue(inventory.TotalProfit.Equals(2500));
		}
	}
}
