using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Abstractions.Application.Services;

/// <summary>
/// Represents the script hook service contract.
/// </summary>
public interface IScriptHookService
{
	/// <summary>
	/// Gets the audio provider.
	/// </summary>
	public IAudioProvider Audio { get; }

	/// <summary>
	/// Gets the game provider.
	/// </summary>
	public IGameProvider Game { get; }

	/// <summary>
	/// Gets the notification provider.
	/// </summary>
	public INotificationProvider Notification { get; }

	/// <summary>
	/// Gets the player provider.
	/// </summary>
	public IPlayerProvider Player { get; }

	/// <summary>
	/// Gets the screen provider.
	/// </summary>
	public IScreenProvider Screen { get; }

	/// <summary>
	/// Gets the world provider.
	/// </summary>
	public IWorldProvider World { get; }
}
