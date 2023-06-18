using GTA;
using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Extensions;
using LSDW.Application.Missions.Base;

namespace LSDW.Application.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Mission"/> base class and 
/// implements the members of the <see cref="ITrafficking"/> interface.
/// </remarks>
internal sealed class Trafficking : Mission, ITrafficking
{
	private readonly ITimeProvider _timeProvider;
	private readonly ILoggerService _loggerService;
	private readonly IGameStateService _stateService;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;
	private Ped? character;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="timeProvider">The time provider interface to use.</param>
	/// <param name="loggerService">The logger service interface to use.</param>
	/// <param name="stateService">The game state service interface to use.</param>
	/// <param name="notificationService">The notification service interface to use.</param>
	internal Trafficking(ITimeProvider timeProvider, ILoggerService loggerService, IGameStateService stateService, INotificationService notificationService)
	{
		_timeProvider = timeProvider;
		_loggerService = loggerService;
		_stateService = stateService;

		NotificationService = notificationService;
	}

	public INotificationService NotificationService { get; }

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
		if (args.KeyCode == Keys.F9)
		{
			if (Game.Player.CanControlCharacter && Game.Player.CanStartMission)
				StartMission();
		}
	}

	public override void OnTick(object sender, EventArgs args)
	{
		if (Status is not MissionStatusType.Started)
			return;

		if (!Game.Player.CanControlCharacter && !Game.Player.CanStartMission)
			return;

		try
		{
			_ = this.TrackDealers(_stateService.Dealers);
			_ = this.RestockDealerInventories(_stateService, _timeProvider);
			_ = this.ChangeDealerPrices(_stateService, _timeProvider);
		}
		catch (Exception ex)
		{
			_loggerService.Critical(ex.Message);
		}
	}
}
