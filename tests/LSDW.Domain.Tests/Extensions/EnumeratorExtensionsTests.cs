﻿using LSDW.Domain.Enumerators;
using LSDW.Domain.Extensions;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
public class EnumeratorExtensionsTests
{
	[TestMethod]
	public void GetDisplayNameSuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		string displayName = drugType.GetDisplayName();

		Assert.AreNotEqual(displayName, drugType.ToString());
	}

	[TestMethod]
	public void GetDisplayNameFailedTest()
	{
		TestType testType = TestType.Test;

		string displayName = testType.GetDisplayName();

		Assert.AreEqual(displayName, testType.ToString());
	}

	[TestMethod]
	public void GetMarketPriceSuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		int marketPrice = drugType.GetAveragePrice();

		Assert.AreNotEqual(marketPrice, 0);
	}

	[TestMethod]
	public void GetMarketPriceFailedTest()
	{
		TestType testType = TestType.Test;

		int marketPrice = testType.GetAveragePrice();

		Assert.AreEqual(marketPrice, 0);
	}

	[TestMethod]
	public void GetRankSuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		float probability = drugType.GetProbability();

		Assert.AreNotEqual(0, probability);
	}

	[TestMethod]
	public void GetRankFailedTest()
	{
		TestType testType = TestType.Test;

		float probability = testType.GetProbability();

		Assert.AreEqual(0, probability);
	}

	[TestMethod]
	public void GetDescriptionSuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		string description = drugType.GetDescription();

		Assert.AreNotEqual(description, drugType.ToString());
	}

	[TestMethod]
	public void GetDescriptionFailedTest()
	{
		TestType testType = TestType.Test;

		string description = testType.GetDescription();

		Assert.AreEqual(description, testType.ToString());
	}

	internal enum TestType
	{
		Test
	}
}