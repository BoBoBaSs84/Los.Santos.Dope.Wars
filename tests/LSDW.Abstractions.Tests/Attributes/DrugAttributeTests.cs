using LSDW.Abstractions.Attributes;

namespace LSDW.Abstractions.Tests.Attributes;

[TestClass, ExcludeFromCodeCoverage]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DrugAttributeTests
{
	[TestMethod]
	public void SetProbabilitySuccessTest()
	{
		DrugAttribute drugAttribute = new(string.Empty, string.Empty)
		{
			Probability = 0.5f
		};

		Assert.IsNotNull(drugAttribute);
		Assert.AreEqual(default, drugAttribute.AveragePrice);
		Assert.AreEqual(string.Empty, drugAttribute.Name);
		Assert.AreEqual(string.Empty, drugAttribute.Description);
		Assert.AreEqual(0.5f, drugAttribute.Probability);
	}

	[TestMethod]
	public void SetProbabilityFailedTest()
	{
		DrugAttribute drugAttribute = new(string.Empty, string.Empty);

		Assert.Throws<ArgumentOutOfRangeException>(() => drugAttribute.Probability = 2);
	}

	[TestMethod]
	public void SetAveragePriceSuccessTest()
	{
		DrugAttribute drugAttribute = new(string.Empty, string.Empty)
		{
			AveragePrice = 1
		};

		Assert.IsNotNull(drugAttribute);
		Assert.AreEqual(1, drugAttribute.AveragePrice);
		Assert.AreEqual(string.Empty, drugAttribute.Name);
		Assert.AreEqual(string.Empty, drugAttribute.Description);
		Assert.AreEqual(default, drugAttribute.Probability);
	}

	[TestMethod]
	public void SetAveragePriceFailedTest()
	{
		DrugAttribute drugAttribute = new(string.Empty, string.Empty);

		Assert.Throws<ArgumentOutOfRangeException>(() => drugAttribute.AveragePrice = -1);
	}
}