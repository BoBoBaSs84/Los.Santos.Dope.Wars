using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;

namespace LSDW.Abstractions.Tests.Extensions;

[TestClass]
public class EnumeratorExtensionsTests
{
	[TestMethod]
	public void GetDrugNameTest()
	{
		DrugType drugType = DrugType.COKE;

		string displayName = drugType.GetName();

		Assert.AreNotEqual(displayName, drugType.ToString());
	}

	[TestMethod]
	public void GetAverageDrugPriceTest()
	{
		DrugType drugType = DrugType.COKE;

		int averagePrice = drugType.GetAveragePrice();

		Assert.AreNotEqual(default, averagePrice);
	}

	[TestMethod]
	public void GetDrugProbabilityTest()
	{
		DrugType drugType = DrugType.COKE;

		float probability = drugType.GetDrugProbability();

		Assert.AreNotEqual(0, probability);
	}

	[TestMethod]
	public void GetDrugDescriptionTest()
	{
		DrugType drugType = DrugType.COKE;

		string description = drugType.GetDrugDescription();

		Assert.AreNotEqual(description, drugType.ToString());
	}

	[TestMethod]
	public void GetDescriptionTest()
	{
		string description = Test.WithDescription.GetDescription();

		Assert.AreEqual("TestDescription", description);
	}

	[TestMethod]
	public void GetNoDescriptionTest()
	{
		string description = Test.NoDescription.GetDescription();

		Assert.AreEqual(Test.NoDescription.ToString(), description);
	}

	private enum Test
	{
		[System.ComponentModel.Description("TestDescription")]
		WithDescription,
		NoDescription,
	}
}