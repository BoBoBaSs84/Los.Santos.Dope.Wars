using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void CreateAudioProviderTest()
	{
		IAudioProvider? provider;

		provider = DomainFactory.CreateAudioProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreateNotificationProviderTest()
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
	public void CreateRandomProviderTest()
	{
		IRandomProvider? provider;

		provider = DomainFactory.CreateRandomProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void CreateWorldProviderTest()
	{
		IWorldProvider? provider;

		provider = DomainFactory.CreateWorldProvider();

		Assert.IsNotNull(provider);
	}
}
