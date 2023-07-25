using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using System.Reflection;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
public class SettingsServiceTests
{
	private readonly ISettingsService _settingsService;
	private readonly string _iniFileName;

	public SettingsServiceTests()
	{
		_settingsService = InfrastructureFactory.GetSettingsService();
		_iniFileName = DomainFactory.GetSettings().IniFileName;
	}

	[TestMethod]
	public void SaveTest()
	{
		_settingsService.Save();

		string iniFilePath = Path.Combine(AppContext.BaseDirectory, _iniFileName);

		Assert.IsTrue(File.Exists(iniFilePath));
	}

	[TestMethod]
	public void PlayTimeTest()
	{
		List<PropertyInfo> interfaces = GetProperties(_settingsService.GetType());
		foreach (PropertyInfo @interface in interfaces)
		{
			List<PropertyInfo> properties = GetProperties(@interface.PropertyType);
			foreach (PropertyInfo property in properties)
			{
				var dataType = TypeNameOrAlias(property.PropertyType.GetProperties().First().PropertyType);
				var name = property.Name;
				var title = $"RESX.UI_Settings_{@interface.Name}_{property.Name}_Title";
				var desription = $"RESX.UI_Settings_{@interface.Name}_{property.Name}_Description";
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
