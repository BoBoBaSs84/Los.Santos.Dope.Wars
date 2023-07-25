using LSDW.Abstractions.Domain.Models;
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
}
