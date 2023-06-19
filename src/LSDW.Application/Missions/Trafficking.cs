using GTA;
using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Extensions;
using LSDW.Application.Missions.Base;
using LSDW.Domain.Extensions;

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
	private readonly IGameStateService _stateService;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="timeProvider">The time provider interface to use.</param>
	/// <param name="loggerService">The logger service interface to use.</param>
	/// <param name="stateService">The game state service interface to use.</param>
	/// <param name="notificationService">The notification service interface to use.</param>
	internal Trafficking(ITimeProvider timeProvider, ILoggerService loggerService, IGameStateService stateService, INotificationService notificationService)
		: base(loggerService, nameof(Trafficking))
	{
		TimeProvider = timeProvider;
		LoggerService = loggerService;
		_stateService = stateService;

		NotificationService = notificationService;
	}

	public ILoggerService LoggerService { get; }
	public INotificationService NotificationService { get; }
	public ITimeProvider TimeProvider { get; }
	public DateTime LastChange { get; private set; }
	public DateTime LastRenew { get; private set; }

	public override void StopMission()
	{
		leftSideMenu = null;
		rightSideMenu = null;
		_ = _stateService.Dealers.DeleteDealers();
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
			_ = this.TrackDealers(_stateService)
				.ChangeDealerPrices(_stateService)
				.ChangeDealerInventories(_stateService);
		}
		catch (Exception ex)
		{
			LoggerService.Critical(ex.Message);
		}
	}
}
