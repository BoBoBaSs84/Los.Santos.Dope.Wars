using LSDW.Core.Extensions;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
public class StringExtensionsTests
{
	[TestMethod]
	public void FormatInvariantSuccessTest()
	{
		string unformatedString = "{0}+{1}={2}";
		int a = 1, b = 2, c = 3;

		string formatedString = unformatedString.FormatInvariant(a, b, c);

		Assert.AreEqual($"{a}+{b}={c}", formatedString);
	}

	[TestMethod]
	public void FormatInvariantFailedTest()
	{
		string unformatedString = "{0}+{1}={2}";
		int a = 1, b = 2, c = 3;

		string formatedString = unformatedString.FormatInvariant(a, b, c);

		Assert.AreNotEqual(unformatedString, formatedString);
	}
}