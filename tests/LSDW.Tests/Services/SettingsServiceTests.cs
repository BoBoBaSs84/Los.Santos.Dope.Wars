using System.Reflection;
using LSDW.Core.Classes;

namespace LSDW.Tests.Services;

[TestClass]
public class SettingsServiceTests
{
	[TestMethod]
	public void Test()
	{
		List<Type> nestedTypes = typeof(Settings).GetNestedTypes().ToList();
		foreach (Type nestedType in nestedTypes)
		{
			List<PropertyInfo> properties = nestedType.GetProperties().ToList();
			foreach (PropertyInfo prop in properties)
			{
				

			}
		}
	}
}