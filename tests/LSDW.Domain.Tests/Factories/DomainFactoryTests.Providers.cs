using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateAudioProviderTest()
	{
		IAudioProvider? provider;

		provider = DomainFactory.GetAudioProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreateNotificationProviderTest()
	{
		INotificationProvider? provider;

		provider = DomainFactory.GetNotificationProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreatePlayerProviderTest()
	{
		IPlayerProvider? provider;

		provider = DomainFactory.GetPlayerProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreateRandomProviderTest()
	{
		IRandomProvider? provider;

		provider = DomainFactory.GetRandomProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreateWorldProviderTest()
	{
		IWorldProvider? provider;

		provider = DomainFactory.GetWorldProvider();

		Assert.IsNotNull(provider);
	}
}
