using LSDW.Abstractions.Application.Managers;
using LSDW.Application.Factories;

namespace LSDW.Application.Tests.Factories;

public partial class ApplicationFactoryTests
{
	[TestMethod]
	public void GetProviderManagerTest()
	{
		IProviderManager? manager;

		manager = ApplicationFactory.GetProviderManager();

		Assert.IsNotNull(manager);
		Assert.IsNotNull(manager.AudioProvider);
		Assert.IsNotNull(manager.NotificationProvider);
		Assert.IsNotNull(manager.PlayerProvider);
		Assert.IsNotNull(manager.RandomProvider);
		Assert.IsNotNull(manager.WorldProvider);
	}

	[TestMethod]
	public void GetServiceManagerTest()
	{
		IServiceManager? manager;

		manager = ApplicationFactory.GetServiceManager();

		Assert.IsNotNull(manager);
		Assert.IsNotNull(manager.LoggerService);
		Assert.IsNotNull(manager.SettingsService);
		Assert.IsNotNull(manager.StateService);
	}
}
