﻿using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using DF = LSDW.Core.Factories.DrugFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DrugTests
{
	[TestMethod]
	public void AddSuccessTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Add(10, 500);

		Assert.IsTrue(drug.Quantity.Equals(20));
		Assert.IsTrue(drug.Price.Equals(750));
		Assert.IsTrue(drug.PossibleProfit.Equals(5000));
	}

	[TestMethod]
	public void AddQuantityLessThenOneExceptionTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => drug.Add(0, drug.Price));
	}

	[TestMethod]
	public void AddPriceLessThanZeroExceptionTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => drug.Add(1, -1));
	}

	[TestMethod]
	public void AddFailureTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Add(5, 0);

		Assert.IsFalse(drug.Quantity.Equals(20));
		Assert.IsFalse(drug.Price.Equals(750));
		Assert.IsFalse(drug.PossibleProfit.Equals(25000));
	}

	[TestMethod]
	public void RemoveSuccessTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Remove(5);

		Assert.IsTrue(drug.Quantity.Equals(5));
	}

	[TestMethod]
	public void RemoveQuantityLessThanOneExceptionTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => drug.Remove(0));
	}

	[TestMethod]
	public void RemoveQuantityLessThanZeroExceptionTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 1, 1000);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => drug.Remove(2));
	}

	[TestMethod]
	public void RemoveFailureTest()
	{
		IDrug drug = DF.CreateDrug(DrugType.COKE, 10, 1000);

		drug.Remove(1);

		Assert.IsFalse(drug.Quantity.Equals(5));
	}
}