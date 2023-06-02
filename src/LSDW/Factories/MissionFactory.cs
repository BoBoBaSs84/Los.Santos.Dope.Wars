using LSDW.Classes.Missions;
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
	/// <param name="stateService">The game state service to use.</param>
	public static IMission CreateTraffickingMission(IDateTimeService timeService, ILoggerService logger, IGameStateService stateService)
		=> new Trafficking(timeService, logger, stateService);
}
