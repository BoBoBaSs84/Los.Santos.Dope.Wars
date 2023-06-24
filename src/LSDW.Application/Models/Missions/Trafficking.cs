using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Extensions;
using LSDW.Application.Models.Missions.Base;
using LSDW.Domain.Extensions;

namespace LSDW.Application.Models.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Mission"/> base class and 
/// implements the members of the <see cref="ITrafficking"/> interface.
/// </remarks>
internal sealed class Trafficking : Mission, ITrafficking
{
	private readonly IServiceManager _serviceManager;
	private readonly IProviderManager _providerManager;
	private readonly ICollection<IDealer> _dealers;
	private readonly IPlayer _player;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	internal Trafficking(IServiceManager serviceManager, IProviderManager providerManager) : base(serviceManager, nameof(Trafficking))
	{
		_serviceManager = serviceManager;
		_providerManager = providerManager;
		_dealers = _serviceManager.StateService.Dealers;
		_player = _serviceManager.StateService.Player;

		LoggerService = _serviceManager.LoggerService;
		LocationProvider = _providerManager.LocationProvider;
		NotificationProvider = _providerManager.NotificationProvider;
		TimeProvider = _providerManager.TimeProvider;
	}

	public bool MenusInitialized => leftSideMenu is not null && rightSideMenu is not null;
	public ILocationProvider LocationProvider { get; }
	public ILoggerService LoggerService { get; }
	public INotificationProvider NotificationProvider { get; }
	public ITimeProvider TimeProvider { get; }

	public void CleanUpMenus()
	{
		leftSideMenu = null;
		rightSideMenu = null;
	}

	public void SetMenus(ISideMenu leftSideMenu, ISideMenu rightSideMenu)
	{
		this.leftSideMenu = leftSideMenu;
		this.rightSideMenu = rightSideMenu;
	}

	public override void StopMission()
	{
		CleanUpMenus();
		_ = _dealers.CleanUpDealers();
		base.StopMission();
	}

	public override void OnAborted(object sender, EventArgs args)
	{
		if (Status is MissionStatusType.Started)
			StopMission();
	}

	public override void OnTick(object sender, EventArgs args)
	{
		if (Status is not MissionStatusType.Started)
			return;

		if (!Game.Player.CanControlCharacter && !Game.Player.CanStartMission)
			return;

		try
		{
			_ = this.TrackDealers(_dealers)
				.DiscoverDealers(_dealers, _player)
				.DiscoveredDealers(_dealers)
				.ChangeDealerInventories(_dealers, _player)
				.ChangeDealerPrices(_dealers, _player)
				.CreateDealers(_dealers)
				.CloseRange(_serviceManager, _providerManager, _dealers, _player)
				.DealerInteraction(_dealers);
		}
		catch (Exception ex)
		{
			LoggerService.Critical($"There was an error.", ex);
		}
	}
}
