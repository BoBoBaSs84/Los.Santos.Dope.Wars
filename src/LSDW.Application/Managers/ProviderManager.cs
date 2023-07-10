using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Factories;

namespace LSDW.Application.Managers;

/// <summary>
/// The provider manager class.
/// </summary>
internal sealed class ProviderManager : IProviderManager
{
	private readonly Lazy<IAudioProvider> _lazyAudioProvider;
	private readonly Lazy<IGameProvider> _lazyGameProvider;
	private readonly Lazy<INotificationProvider> _lazyNotificationProvider;
	private readonly Lazy<IScreenProvider> _lazyScreenScreenProvider;
	private readonly Lazy<IPlayerProvider> _lazyPlayerProvider;
	private readonly Lazy<IRandomProvider> _lazyRandomProvider;
	private readonly Lazy<IWorldProvider> _lazyWorldProvider;

	/// <summary>
	/// Initializes a instance of the provider manager class.
	/// </summary>
	internal ProviderManager()
	{
		_lazyAudioProvider = new Lazy<IAudioProvider>(DomainFactory.GetAudioProvider);
		_lazyGameProvider = new Lazy<IGameProvider>(DomainFactory.GetGameProvider);
		_lazyNotificationProvider = new Lazy<INotificationProvider>(DomainFactory.GetNotificationProvider);
		_lazyScreenScreenProvider = new Lazy<IScreenProvider>(DomainFactory.GetScreenProvider);
		_lazyPlayerProvider = new Lazy<IPlayerProvider>(DomainFactory.GetPlayerProvider);
		_lazyRandomProvider = new Lazy<IRandomProvider>(DomainFactory.GetRandomProvider);
		_lazyWorldProvider = new Lazy<IWorldProvider>(DomainFactory.GetWorldProvider);
	}

	/// <summary>
	/// The provider manager singleton instance.
	/// </summary>
	public static readonly ProviderManager Instance = new();

	public IAudioProvider AudioProvider
		=> _lazyAudioProvider.Value;

	public IGameProvider GameProvider
		=> _lazyGameProvider.Value;

	public INotificationProvider NotificationProvider
		=> _lazyNotificationProvider.Value;

	public IScreenProvider ScreenProvider
		=> _lazyScreenScreenProvider.Value;

	public IPlayerProvider PlayerProvider
		=> _lazyPlayerProvider.Value;

	public IRandomProvider RandomProvider
		=> _lazyRandomProvider.Value;

	public IWorldProvider WorldProvider
		=> _lazyWorldProvider.Value;
}
