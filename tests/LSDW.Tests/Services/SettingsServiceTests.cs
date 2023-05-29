using LSDW.Core.Classes;
using System.Globalization;
using System.Reflection;

namespace LSDW.Tests.Services;

[TestClass]
public class SettingsServiceTests
{


	[TestMethod]
	public void Test()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

		List<Type> nestedTypes = typeof(Settings).GetNestedTypes().ToList();
		foreach (Type nestedType in nestedTypes)
		{
			List<PropertyInfo> properties = nestedType.GetProperties().ToList();
			foreach (PropertyInfo prop in properties)
			{
				_ = prop.PropertyType;
				Trace.WriteLine($"{prop.Name}={prop.GetValue(this, null).ToString().ToLower(Thread.CurrentThread.CurrentCulture)}");
			}
		}
	}
}