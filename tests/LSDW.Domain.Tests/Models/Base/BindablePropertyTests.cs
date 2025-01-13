using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Domain.Models.Base;

namespace LSDW.Domain.Tests.Models.Base;

[TestClass, ExcludeFromCodeCoverage]
public class BindablePropertyTests
{
	private readonly string testValue = "Test";

	[TestMethod]
	public void BindablePropertyNewValueTest()
	{
		bool propertChanging = false,
				propertChanged = false;

		string oldValue = string.Empty,
			newVlaue = string.Empty;

		TestClass testClass = new(string.Empty);
		testClass.TestProperty.Changing += (sender, args) => { propertChanging = true; oldValue = args.Value; };
		testClass.TestProperty.Changed += (sender, args) => { propertChanged = true; newVlaue = args.Value; };

		testClass.TestProperty.Value = testValue;

		Assert.IsTrue(propertChanging);
		Assert.AreEqual(string.Empty, oldValue);
		Assert.IsTrue(propertChanged);
		Assert.AreEqual(testValue, newVlaue);
		Assert.AreEqual(testValue, testClass.TestProperty.Value);
	}

	[TestMethod]
	public void BindablePropertySameValueTest()
	{
		bool propertChanging = false,
				propertChanged = false;

		string oldValue = string.Empty,
			newVlaue = string.Empty;

		TestClass testClass = new(oldValue);
		testClass.TestProperty.Changing += (sender, args) => { propertChanging = true; oldValue = args.Value; };
		testClass.TestProperty.Changed += (sender, args) => { propertChanged = true; newVlaue = args.Value; };

		testClass.TestProperty.Value = newVlaue;

		Assert.IsFalse(propertChanging);
		Assert.AreEqual(string.Empty, oldValue);
		Assert.IsFalse(propertChanged);
		Assert.AreEqual(oldValue, newVlaue);
		Assert.AreEqual(oldValue, testClass.TestProperty.Value);
	}

	private sealed class TestClass(string testProperty)
	{
		public IBindableProperty<string> TestProperty { get; set; } = new BindableProperty<string>(testProperty);
	}
}