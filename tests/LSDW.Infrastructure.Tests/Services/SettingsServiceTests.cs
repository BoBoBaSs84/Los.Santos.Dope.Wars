using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Classes.Models;
using LSDW.Infrastructure.Factories;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
public partial class SettingsServiceTests
{
	private static readonly ISettingsService _settingsService = InfrastructureFactory.CreateSettingsService();
	private static readonly string _iniFileName = Settings.IniFileName;

	[ClassCleanup]
	public static void ClassCleanup()
	{
		if (File.Exists(_iniFileName))
			File.Delete(_iniFileName);
	}
}
