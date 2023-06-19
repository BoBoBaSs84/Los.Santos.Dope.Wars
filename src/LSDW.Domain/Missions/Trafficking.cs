using GTA;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Extensions;
using LSDW.Domain.Missions.Base;

namespace LSDW.Domain.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Mission"/> base class and 
/// implements the members of the <see cref="ITrafficking"/> interface.
/// </remarks>
internal sealed class Trafficking : Mission, ITrafficking
{
	private readonly IPlayer _player;
	private readonly ICollection<IDealer> _dealers;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="player">The player instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="timeProvider">The time provider instance to use.</param>
	/// <param name="loggerService">The logger service instance to use.</param>
	/// <param name="notificationService">The notification service instance to use.</param>
	internal Trafficking(IPlayer player, ICollection<IDealer> dealers, ITimeProvider timeProvider, ILoggerService loggerService, INotificationService notificationService)
		: base(loggerService, nameof(Trafficking))
	{
		_player = player;
		_dealers = dealers;

		TimeProvider = timeProvider;
		LoggerService = loggerService;
		NotificationService = notificationService;
	}

	public ILoggerService LoggerService { get; }
	public INotificationService NotificationService { get; }
	public ITimeProvider TimeProvider { get; }

	public override void StopMission()
	{
		leftSideMenu = null;
		rightSideMenu = null;
		_ = _dealers.DeleteDealers();
		base.StopMission();
	}

	public override void OnAborted(object sender, EventArgs args)
		=> StopMission();

	public override void OnTick(object sender, EventArgs args)
	{
		if (Status is not MissionStatusType.Started)
			return;

		if (!Game.Player.CanControlCharacter && !Game.Player.CanStartMission)
			return;

		try
		{
			_ = this.TrackDealers(_dealers)
				.ChangeDealerPrices(_dealers, _player)
				.ChangeDealerInventories(_dealers, _player);
		}
		catch (Exception ex)
		{
			LoggerService.Critical(ex.Message);
		}
	}
}
