using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Abstractions.Application.Managers;

/// <summary>
/// The provider manager interface.
/// </summary>
public interface IProviderManager
{
	/// <summary>
	/// The audio provider
	/// </summary>
	IAudioProvider AudioProvider { get; }

	/// <summary>
	/// The world provider.
	/// </summary>
	INotificationProvider NotificationProvider { get; }

	/// <summary>
	/// The screen provider.
	/// </summary>
	IScreenProvider ScreenProvider { get; }

	/// <summary>
	/// The player provider.
	/// </summary>
	IPlayerProvider PlayerProvider { get; }

	/// <summary>
	/// The random provider.
	/// </summary>
	IRandomProvider RandomProvider { get; }

	/// <summary>
	/// The world provider.
	/// </summary>
	IWorldProvider WorldProvider { get; }
}
