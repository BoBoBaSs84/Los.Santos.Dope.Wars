using LSDW.Domain.Extensions;

namespace LSDW.Domain.Tests.Extensions;

[TestClass, ExcludeFromCodeCoverage]
public class IntegerExtensionsTests
{
	[TestMethod]
	public void GetArrayTest()
	{
		int i = 10;

		int[] intArray = i.GetArray();

		Assert.HasCount(i + 1, intArray);
	}

	[TestMethod]
	public void GetListTest()
	{
		int i = 10;

		List<int> ints = i.GetList();

		Assert.HasCount(i + 1, ints);
	}
}