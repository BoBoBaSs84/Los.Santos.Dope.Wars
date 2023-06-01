using LSDW.Classes.Missions;
using LSDW.Core.Interfaces.Models;
using LSDW.Interfaces.Missions;
using LSDW.Interfaces.Services;

namespace LSDW.Factories;

/// <summary>
/// The mission factory class.
/// </summary>
public static class MissionFactory
{
	/// <summary>
	/// Creates a new trafficking mission instance.
	/// </summary>
	/// <param name="timeService">The current date and time service to use.</param>
	/// <param name="logger">The logger service service to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static IMission CreateTraffickingMission(IDateTimeService timeService, ILoggerService logger, IPlayer player)
		=> new Trafficking(timeService, logger, player);
}
