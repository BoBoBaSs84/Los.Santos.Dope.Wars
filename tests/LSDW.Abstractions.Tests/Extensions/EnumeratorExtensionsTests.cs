using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;

namespace LSDW.Abstractions.Tests.Extensions;

[TestClass]
public class EnumeratorExtensionsTests
{
	[TestMethod]
	public void GetDisplayNameSuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		string displayName = drugType.GetDrugName();

		Assert.AreNotEqual(displayName, drugType.ToString());
	}

	[TestMethod]
	public void GetAveragePriceSuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		int averagePrice = drugType.GetAverageDrugPrice();

		Assert.AreNotEqual(default, averagePrice);
	}

	[TestMethod]
	public void GetProbabilitySuccessTest()
	{
		DrugType drugType = DrugType.COKE;

		float probability = drugType.GetDrugProbability();

		Assert.AreNotEqual(0, probability);
	}

	[TestMethod]
	public void GetDescriptionSuccessTest()
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