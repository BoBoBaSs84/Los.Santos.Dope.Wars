using LSDW.Application.Abstractions.Application.Providers;
using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Providers;

namespace LSDW.Application.Services;

/// <summary>
/// Represents the implementation of the script hook service.
/// </summary>
internal sealed class ScriptHookService : IScriptHookService
{
	private readonly Lazy<IAudioProvider> _audioProvider = new(() => new AudioProvider());
	private readonly Lazy<IGameProvider> _gameProvider = new(() => new GameProvider());
	private readonly Lazy<INotificationProvider> _notificationProvider = new(() => new NotificationProvider());
	private readonly Lazy<IPlayerProvider> _playerProvider = new(() => new PlayerProvider());
	private readonly Lazy<IScreenProvider> _screenProvider = new(() => new ScreenProvider());
	private readonly Lazy<IWorldProvider> _worldProvider = new(() => new WorldProvider());

	public IAudioProvider Audio => _audioProvider.Value;
	public IGameProvider Game => _gameProvider.Value;
	public INotificationProvider Notification => _notificationProvider.Value;
	public IPlayerProvider Player => _playerProvider.Value;
	public IScreenProvider Screen => _screenProvider.Value;
	public IWorldProvider World => _worldProvider.Value;
}
