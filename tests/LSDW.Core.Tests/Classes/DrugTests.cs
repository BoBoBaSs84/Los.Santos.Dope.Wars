using Microsoft.VisualStudio.TestTools.UnitTesting;
using LSDW.Core.Classes;
using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DrugTests
{
	[TestMethod]
	public void AddSuccessTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 10, 100);

		drug.Add(10, 50);

		Assert.IsTrue(drug.Quantity.Equals(20));
		Assert.IsTrue(drug.Price.Equals(75));
		Assert.IsTrue(drug.PossibleProfit.Equals(500));
	}

	[TestMethod]
	public void AddPriceLessThanZeroExceptionTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 10, 1000);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => drug.Add(1, -1));
	}

	[TestMethod]
	public void AddFailureTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Add(5, 0);

		Assert.IsFalse(drug.Quantity.Equals(20));
		Assert.IsFalse(drug.Price.Equals(750));
		Assert.IsFalse(drug.PossibleProfit.Equals(25000));
	}

	[TestMethod()]
	public void AddEarlyExitTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.METH, 5);

		drug.Add(-5, 0);

		Assert.AreEqual(5, drug.Quantity);
	}

	[TestMethod]
	public void RemoveSuccessTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Remove(5);

		Assert.IsTrue(drug.Quantity.Equals(5));
	}

	[TestMethod]
	public void RemoveFailureTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Remove(1);

		Assert.IsFalse(drug.Quantity.Equals(5));
	}

	[TestMethod]
	public void RemoveEarlyExitTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.HASH);

		drug.Remove(-1);

		Assert.AreEqual(0, drug.Quantity);
  }

	[TestMethod]
	public void RemovePriceSetZeroTest()
	{
		IDrug drug = DrugFactory.CreateDrug(DrugType.LSD, 100, 20);

		drug.Remove(100);

		Assert.AreEqual(0, drug.Quantity);
		Assert.AreEqual(0, drug.Price);
	}
}
