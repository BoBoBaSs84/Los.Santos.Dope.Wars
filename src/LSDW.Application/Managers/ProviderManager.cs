using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Application.Managers;

/// <summary>
/// The provider manager service.
/// </summary>
internal sealed class ProviderManager : IProviderManager
{
	private readonly Lazy<IWorldProvider> _lazyLocationProvider;
	private readonly Lazy<INotificationProvider> _lazyNotificationProvider;
	private readonly Lazy<IPlayerProvider> _lazyPlayerProvider;


	/// <summary>
	/// Initializes a instance of the location provider manager class.
	/// </summary>
	public ProviderManager()
	{
		_lazyLocationProvider = new Lazy<IWorldProvider>(DomainFactory.CreateWorldProvider);
		_lazyNotificationProvider = new Lazy<INotificationProvider>(DomainFactory.CreateNotificationProvider);
		_lazyPlayerProvider = new Lazy<IPlayerProvider>(DomainFactory.CreatePlayerProvider);
	}

	public IWorldProvider WorldProvider
		=> _lazyLocationProvider.Value;

	public INotificationProvider NotificationProvider
		=> _lazyNotificationProvider.Value;

	public IPlayerProvider PlayerProvider
		=> _lazyPlayerProvider.Value;
}
