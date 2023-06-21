using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Application.Managers;

/// <summary>
/// The provider manager service.
/// </summary>
internal sealed class ProviderManager : IProviderManager
{
	private readonly Lazy<ILocationProvider> _lazyLocationProvider;
	private readonly Lazy<ITimeProvider> _lazyTimeProvider;
	private readonly Lazy<INotificationProvider> _lazyNotificationProvider;

	/// <summary>
	/// Initializes a instance of the location provider manager class.
	/// </summary>
	public ProviderManager()
	{
		_lazyLocationProvider = new Lazy<ILocationProvider>(DomainFactory.CreateLocationProvider);
		_lazyTimeProvider = new Lazy<ITimeProvider>(DomainFactory.CreateTimeProvider);
		_lazyNotificationProvider = new Lazy<INotificationProvider>(DomainFactory.CreateNotificationProvider);
	}

	public ILocationProvider LocationProvider
		=> _lazyLocationProvider.Value;

	public ITimeProvider TimeProvider
		=> _lazyTimeProvider.Value;

	public INotificationProvider NotificationProvider
		=> _lazyNotificationProvider.Value;
}
