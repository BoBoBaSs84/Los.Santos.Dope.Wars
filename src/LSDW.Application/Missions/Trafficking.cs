using GTA;
using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Missions.Base;
using LSDW.Domain.Enumerators;
using LSDW.Domain.Interfaces.Actors;
using LSDW.Domain.Interfaces.Models;

namespace LSDW.Application.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
internal sealed class Trafficking : Mission, ITrafficking
{
	private readonly ITimeProvider _timeProvider;
	private readonly ILoggerService _logger;
	private readonly IGameStateService _stateService;
	private readonly ICollection<IDealer> _dealers;
	private readonly IPlayer _player;

	private readonly ISideMenu? leftSideMenu;
	private readonly ISideMenu? rightSideMenu;
	private Ped? _character;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="timeProvider">The time provider to use.</param>
	/// <param name="logger">The logger service service to use.</param>
	/// <param name="stateService">The game state service to use.</param>
	internal Trafficking(ITimeProvider timeProvider, ILoggerService logger, IGameStateService stateService) : base()
	{
		_timeProvider = timeProvider;
		_logger = logger;
		_stateService = stateService;
		_dealers = stateService.Dealers.ToList();
		_player = stateService.Player;
	}

	public override void OnAborted(object sender, EventArgs args)
		=> StopMission();

	public override void OnKeyUp(object sender, KeyEventArgs args)
	{
		// condition to start the mission
		// StartMission();
	}

	public override void OnTick(object sender, EventArgs args)
	{
		if (!Game.Player.CanControlCharacter && !Game.Player.CanStartMission)
			return;

		if (Status is not MissionStatusType.Started)
			return;

		_character ??= Game.Player.Character;
	}
}
