using LSDW.Abstractions.Attributes;

namespace LSDW.Abstractions.Tests.Attributes;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class DrugAttributeTests
{
	[TestMethod]
	public void SetProbabilitySuccessTest()
	{
		DrugAttribute drugAttribute = new(string.Empty, string.Empty, default)
		{
			Probability = 0.5f
		};

		Assert.IsNotNull(drugAttribute);
		Assert.AreEqual(default, drugAttribute.AveragePrice);
		Assert.AreEqual(string.Empty, drugAttribute.DisplayName);
		Assert.AreEqual(string.Empty, drugAttribute.Description);
		Assert.AreEqual(0.5f, drugAttribute.Probability);
	}

	[TestMethod]
	public void SetProbabilityFailedTest()
	{
		DrugAttribute drugAttribute = new(string.Empty, string.Empty, default);

		Assert.ThrowsException<ArgumentOutOfRangeException>(() => drugAttribute.Probability = 2);
	}
}