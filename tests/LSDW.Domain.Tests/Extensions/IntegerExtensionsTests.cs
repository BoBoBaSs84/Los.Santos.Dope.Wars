using LSDW.Domain.Extensions;

namespace LSDW.Domain.Tests.Extensions;

[TestClass]
public class IntegerExtensionsTests
{
	[TestMethod]
	public void GetArrayTest()
	{
		int i = 10;

		int[] intArray = i.GetArray();

		Assert.AreEqual(i + 1, intArray.Length);
	}

	[TestMethod]
	public void GetListTest()
	{
		int i = 10;

		IList<int> ints = i.GetList();

		Assert.AreEqual(i + 1, ints.Count);
	}
}