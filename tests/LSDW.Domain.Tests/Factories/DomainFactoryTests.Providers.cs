using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Domain.Tests.Factories;

public partial class DomainFactoryTests
{
	[TestMethod]
	public void GetAudioProviderTest()
	{
		IAudioProvider? provider;

		provider = DomainFactory.GetAudioProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void GetGameProviderTest()
	{
		IGameProvider? provider;

		provider = DomainFactory.GetGameProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void GetNotificationProviderTest()
	{
		INotificationProvider? provider;

		provider = DomainFactory.GetNotificationProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void GetScreenProviderTest()
	{
		IScreenProvider? provider;

		provider = DomainFactory.GetScreenProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void GetPlayerProviderTest()
	{
		IPlayerProvider? provider;

		provider = DomainFactory.GetPlayerProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void GetRandomProviderTest()
	{
		IRandomProvider? provider;

		provider = DomainFactory.GetRandomProvider();

		Assert.IsNotNull(provider);
	}

	[TestMethod]
	public void GetWorldProviderTest()
	{
		IWorldProvider? provider;

		provider = DomainFactory.GetWorldProvider();

		Assert.IsNotNull(provider);
	}
}
