using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Providers;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new world provider instance.
	/// </summary>
	public static IWorldProvider CreateWorldProvider()
		=> new WorldProvider();

	/// <summary>
	/// Creates a new player provider instance.
	/// </summary>
	public static IPlayerProvider CreatePlayerProvider()
		=> new PlayerProvider();

	/// <summary>
	/// Creates a new notification provider instance.
	/// </summary>
	public static INotificationProvider CreateNotificationProvider()
		=> new NotificationProvider();
}
