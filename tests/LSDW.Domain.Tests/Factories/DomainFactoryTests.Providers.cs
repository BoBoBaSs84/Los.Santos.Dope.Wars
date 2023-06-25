using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void LocationProviderTest()
	{
		ILocationProvider? provider;

		provider = DomainFactory.CreateLocationProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void NotificationProviderTest()
	{
		INotificationProvider? provider;

		provider = DomainFactory.CreateNotificationProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreatePlayerProviderTest()
	{
		IPlayerProvider? provider;

		provider = DomainFactory.CreatePlayerProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void TimeProviderTest()
	{
		ITimeProvider? provider;

		provider = DomainFactory.CreateTimeProvider();

		Assert.IsNotNull(provider);
	}
}
