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
	IWorldProvider WorldProvider { get; }

	/// <summary>
	/// The player provider.
	/// </summary>
	IPlayerProvider PlayerProvider { get; }

	/// <summary>
	/// The notification provider.
	/// </summary>
	INotificationProvider NotificationProvider { get; }
}
