using LSDW.Factories;
using LSDW.Interfaces.Services;

namespace LSDW.Tests.Factories;

[TestClass]
public class ServiceFactoryTests
{
	[TestMethod]
	public void CreateSettingsServiceTest()
	{
		ISettingsService? settingsService;

		settingsService = ServiceFactory.CreateSettingsService();

		Assert.IsNotNull(settingsService);
	}

	[TestMethod]
	public void CreateLoggerServiceTest()
	{
		ILoggerService? loggerService;

		loggerService = ServiceFactory.CreateLoggerService();

		Assert.IsNotNull(loggerService);
	}
}