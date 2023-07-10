using LSDW.Abstractions.Application.Managers;
using LSDW.Application.Managers;

namespace LSDW.Application.Tests.Managers;

[TestClass]
public class ProviderManagerTests
{
	[TestMethod]
	public void ProviderManagerTest()
	{
		IProviderManager? manager;

		manager = new ProviderManager();

		Assert.IsNotNull(manager);
		Assert.IsNotNull(manager.AudioProvider);
		Assert.IsNotNull(manager.GameProvider);
		Assert.IsNotNull(manager.PlayerProvider);
		Assert.IsNotNull(manager.ScreenProvider);
		Assert.IsNotNull(manager.RandomProvider);
		Assert.IsNotNull(manager.NotificationProvider);
		Assert.IsNotNull(manager.WorldProvider);
	}

	[TestMethod]
	public void ServiceManagerTest()
	{
		IServiceManager? manager;

		manager = new ServiceManager();

		Assert.IsNotNull(manager);
		Assert.IsNotNull(manager.SettingsService);
		Assert.IsNotNull(manager.LoggerService);
		Assert.IsNotNull(manager.StateService);
	}
}