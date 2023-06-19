using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Missions;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new trafficking mission instance.
	/// </summary>
	/// <param name="player">The player instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="timeProvider">The time provider instance to use.</param>
	/// <param name="loggerService">The logger service instance to use.</param>
	/// <param name="notificationService">The notification service instance to use.</param>
	public static ITrafficking CreateTraffickingMission(IPlayer player, ICollection<IDealer> dealers, ITimeProvider timeProvider, ILoggerService loggerService, INotificationService notificationService)
		=> new Trafficking(player, dealers, timeProvider, loggerService, notificationService);
}
