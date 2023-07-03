using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Application.Managers;

/// <summary>
/// The provider manager service.
/// </summary>
internal sealed class ProviderManager : IProviderManager
{
	private readonly Lazy<IAudioProvider> _lazyAudioProvider;
	private readonly Lazy<INotificationProvider> _lazyNotificationProvider;
	private readonly Lazy<IPlayerProvider> _lazyPlayerProvider;
	private readonly Lazy<IRandomProvider> _lazyRandomProvider;
	private readonly Lazy<IWorldProvider> _lazyWorldProvider;

	/// <summary>
	/// Initializes a instance of the location provider manager class.
	/// </summary>
	internal ProviderManager()
	{
		_lazyAudioProvider = new Lazy<IAudioProvider>(DomainFactory.CreateAudioProvider);
		_lazyNotificationProvider = new Lazy<INotificationProvider>(DomainFactory.CreateNotificationProvider);
		_lazyPlayerProvider = new Lazy<IPlayerProvider>(DomainFactory.CreatePlayerProvider);
		_lazyRandomProvider = new Lazy<IRandomProvider>(DomainFactory.CreateRandomProvider);
		_lazyWorldProvider = new Lazy<IWorldProvider>(DomainFactory.CreateWorldProvider);
	}

	public IAudioProvider AudioProvider => _lazyAudioProvider.Value;

	public INotificationProvider NotificationProvider => _lazyNotificationProvider.Value;

	public IPlayerProvider PlayerProvider => _lazyPlayerProvider.Value;

	public IRandomProvider RandomProvider => _lazyRandomProvider.Value;

	public IWorldProvider WorldProvider => _lazyWorldProvider.Value;
}
