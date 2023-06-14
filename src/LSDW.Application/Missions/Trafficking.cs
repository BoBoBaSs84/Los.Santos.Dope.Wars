using GTA;
using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Actors;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Missions.Base;

namespace LSDW.Application.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
internal sealed class Trafficking : Mission, ITrafficking
{
	private readonly ITimeProvider _timeProvider;
	private readonly ILoggerService _loggerService;
	private readonly IGameStateService _stateService;
	private readonly ICollection<IDealer> _dealers;
	private readonly IPlayer _player;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;
	private Ped? character;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="timeProvider">The time provider to use.</param>
	/// <param name="loggerService">The logger service service to use.</param>
	/// <param name="stateService">The game state service to use.</param>
	internal Trafficking(ITimeProvider timeProvider, ILoggerService loggerService, IGameStateService stateService) : base()
	{
		_timeProvider = timeProvider;
		_loggerService = loggerService;
		_stateService = stateService;
		_dealers = stateService.Dealers.ToList();
		_player = stateService.Player;
	}

	public override void StopMission()
	{
		character = null;
		leftSideMenu = null;
		rightSideMenu = null;
		base.StopMission();
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

		character ??= Game.Player.Character;
	}
}
