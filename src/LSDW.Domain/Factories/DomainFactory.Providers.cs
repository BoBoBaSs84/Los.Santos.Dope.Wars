using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Providers;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Returns the audio provider singleton instance.
	/// </summary>
	public static IAudioProvider GetAudioProvider()
		=> AudioProvider.Instance;

	/// <summary>
	/// Returns the world provider singleton instance.
	/// </summary>
	public static INotificationProvider GetNotificationProvider()
		=> NotificationProvider.Instance;

	/// <summary>
	/// Returns the screen provider singleton instance.
	/// </summary>
	public static IScreenProvider GetScreenProvider()
		=> ScreenProvider.Instance;

	/// <summary>
	/// Returns the player provider singleton instance.
	/// </summary>
	public static IPlayerProvider GetPlayerProvider()
		=> PlayerProvider.Instance;

	/// <summary>
	/// Returns the random provider singleton instance.
	/// </summary>
	public static IRandomProvider GetRandomProvider()
		=> RandomProvider.Instance;

	/// <summary>
	/// Returns the world provider singleton instance.
	/// </summary>
	public static IWorldProvider GetWorldProvider()
		=> WorldProvider.Instance;
}
