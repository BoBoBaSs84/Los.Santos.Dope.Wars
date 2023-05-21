using LSDW.Core.Helpers;

namespace LSDW.Core.Tests.Helpers;

[TestClass]
public class RandomHelperTests
{
	[TestMethod]
	public void GetFullNameTest()
	{
		string nameOne = RandomHelper.GetFullName();
		string nameTwo = RandomHelper.GetFullName();

		Assert.AreNotEqual(nameOne, nameTwo);
	}
}