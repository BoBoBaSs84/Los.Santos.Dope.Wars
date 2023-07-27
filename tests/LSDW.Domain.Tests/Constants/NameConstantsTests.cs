using LSDW.Domain.Constants;

namespace LSDW.Domain.Tests.Constants;

[TestClass, ExcludeFromCodeCoverage]
public class NameConstantsTests
{
	[TestMethod]
	public void GetFirstNameTest()
	{
		string firstName;

		firstName = NameConstants.GetFirstName();

		Assert.IsFalse(string.IsNullOrWhiteSpace(firstName));
	}

	[TestMethod]
	public void GetLastNameTest()
	{
		string lastName;

		lastName = NameConstants.GetLastName();

		Assert.IsFalse(string.IsNullOrWhiteSpace(lastName));
	}

	[TestMethod]
	public void GetFullNameTest()
	{
		string fullName;

		fullName = NameConstants.GetFullName();

		Assert.IsFalse(string.IsNullOrWhiteSpace(fullName));
	}
}