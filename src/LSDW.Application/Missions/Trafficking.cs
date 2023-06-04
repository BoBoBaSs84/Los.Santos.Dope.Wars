using GTA;
using LSDW.Abstractions.Interfaces.Application;
using LSDW.Abstractions.Interfaces.Infrastructure;
using LSDW.Abstractions.Interfaces.Presentation;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Application.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
internal sealed class Trafficking : IMission
{
	private readonly IDateTimeService _timeService;
	private readonly ILoggerService _logger;
	private readonly IGameStateService _stateService;
	private readonly ICollection<IDealer> _dealers;
	private readonly IPlayer _player;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;
	private Ped? _character;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="timeService">The current date and time service to use.</param>
	/// <param name="logger">The logger service service to use.</param>
	/// <param name="stateService">The game state service to use.</param>
	internal Trafficking(IDateTimeService timeService, ILoggerService logger, IGameStateService stateService)
	{
		_timeService = timeService;
		_logger = logger;
		_stateService = stateService;
		_dealers = stateService.Dealers.ToList();
		_player = stateService.Player;

		Status = MissionStatusType.Stopped;
	}

	public MissionStatusType Status { get; private set; }

	public void CleanUp()
	{
		foreach (IDealer dealer in _dealers)
			dealer.Delete();

		Status = MissionStatusType.Stopped;
	}

	public void OnAborted(object sender, EventArgs args)
	{
		CleanUp();

		Status = MissionStatusType.Aborted;
	}

	public void OnTick(object sender, EventArgs args)
	{
		while (Game.IsLoading)
			return;

		if (!Game.Player.CanStartMission)
			return;

		if (Status is not MissionStatusType.Stopped or MissionStatusType.Aborted)
			return;

		_character ??= Game.Player.Character;
	}
}
