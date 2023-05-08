using LSDW.Classes;
using LSDW.Enumerators;
using LSDW.Interfaces.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LSDW.Tests.Classes
{
	[TestClass]
	public class DrugTests
	{
		[TestMethod]
		public void AddSuccessTest()
		{
			IDrug drug = new Drug(DrugType.COKE, 10, 1000);

			drug.Add(10, 500);

			Assert.IsTrue(drug.Quantity.Equals(20));
			Assert.IsTrue(drug.Price.Equals(750));
			Assert.IsTrue(drug.Profit.Equals(25000));
		}

		[TestMethod]
		public void AddFailureTest()
		{
			IDrug drug = new Drug(DrugType.COKE, 10, 1000);

			drug.Add(5, 0);

			Assert.IsFalse(drug.Quantity.Equals(20));
			Assert.IsFalse(drug.Price.Equals(750));
			Assert.IsFalse(drug.Profit.Equals(25000));
		}

		[TestMethod]
		public void RemoveSuccessTest()
		{
			IDrug drug = new Drug(DrugType.COKE, 10, 1000);

			drug.Remove(5);

			Assert.IsTrue(drug.Quantity.Equals(5));
		}

		[TestMethod]
		public void RemoveFailureTest()
		{
			IDrug drug = new Drug(DrugType.COKE, 10, 1000);

			drug.Remove(1);

			Assert.IsFalse(drug.Quantity.Equals(5));
		}
	}
}
