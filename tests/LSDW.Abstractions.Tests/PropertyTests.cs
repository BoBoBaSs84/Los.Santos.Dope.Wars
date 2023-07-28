using LSDW.Abstractions.Domain.Models;
using System.Reflection;

namespace LSDW.Abstractions.Tests;

[TestClass, ExcludeFromCodeCoverage]
public class PropertyTests
{
	[TestMethod]
	public void Test()
	{
		List<PropertyInfo> interfaces = GetProperties(typeof(ISettings));
		foreach (PropertyInfo @interface in interfaces.Where(x => x.PropertyType.IsInterface))
		{
			List<PropertyInfo> properties = GetProperties(@interface.PropertyType);
			foreach (PropertyInfo property in properties)
			{
				string sectionName = @interface.Name;
				string propertyName = property.Name;
				string dataType = TypeNameOrAlias(property.PropertyType);
			}
		}
	}

	private static List<PropertyInfo> GetProperties(Type type)
		=> type.GetProperties().ToList();

	private static string TypeNameOrAlias(Type type)
		=> _typeAlias.TryGetValue(type, out string alias) ? alias : type.Name;

	private static readonly Dictionary<Type, string> _typeAlias = new()
	{
		{ typeof(bool), "bool" },
		{ typeof(byte), "byte" },
		{ typeof(char), "char" },
		{ typeof(decimal), "decimal" },
		{ typeof(double), "double" },
		{ typeof(float), "float" },
		{ typeof(int), "int" },
		{ typeof(long), "long" },
		{ typeof(object), "object" },
		{ typeof(sbyte), "sbyte" },
		{ typeof(short), "short" },
		{ typeof(string), "string" },
		{ typeof(uint), "uint" },
		{ typeof(ulong), "ulong" },
		{ typeof(void), "void" }
	};
}
