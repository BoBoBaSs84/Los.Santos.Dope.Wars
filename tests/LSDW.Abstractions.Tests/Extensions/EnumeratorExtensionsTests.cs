using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Extensions;

namespace LSDW.Abstractions.Tests.Extensions;

[TestClass]
public class EnumeratorExtensionsTests
{
	[TestMethod]
	public void GetDrugTypeNameTest()
	{
		DrugType drugType = DrugType.COKE;

		string name = drugType.GetName();

		Assert.AreNotEqual(name, drugType.ToString());
	}

	[TestMethod]
	public void GetDrugTypeAveragePriceTest()
	{
		DrugType drugType = DrugType.COKE;

		int averagePrice = drugType.GetAveragePrice();

		Assert.AreNotEqual(default, averagePrice);
	}

	[TestMethod]
	public void GetDrugTypeProbabilityTest()
	{
		DrugType drugType = DrugType.COKE;

		float probability = drugType.GetProbability();

		Assert.AreNotEqual(0, probability);
	}

	[TestMethod]
	public void GetDrugTypeDescriptionTest()
	{
		DrugType drugType = DrugType.COKE;

		string description = drugType.GetDescription();

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