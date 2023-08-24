using LSDW.Domain.Constants;

namespace LSDW.Domain.Tests.Constants;

[TestClass, ExcludeFromCodeCoverage]
public class NameConstantsTests
{
	[TestMethod]
	public void GetMaleFirstNameTest()
	{
		string firstName;

		firstName = NameConstants.GetMaleFirstName();

		Assert.IsFalse(string.IsNullOrWhiteSpace(firstName));
	}

	[TestMethod]
	public void GetFemaleFirstNameTest()
	{
		string firstName;

		firstName = NameConstants.GetFemaleFirstName();

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
	public void GetMaleFullNameTest()
	{
		string fullName;

		fullName = NameConstants.GetMaleFullName();

		Assert.IsFalse(string.IsNullOrWhiteSpace(fullName));
	}

	[TestMethod]
	public void GetFemaleFullNameTest()
	{
		string fullName;

		fullName = NameConstants.GetFemaleFullName();

		Assert.IsFalse(string.IsNullOrWhiteSpace(fullName));
	}
}