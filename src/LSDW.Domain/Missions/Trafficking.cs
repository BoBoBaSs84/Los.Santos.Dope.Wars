using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
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
		_dealers = serviceManager.StateService.Dealers;
		_player = serviceManager.StateService.Player;

		LoggerService = serviceManager.LoggerService;
		LocationProvider = providerManager.LocationProvider;
		NotificationProvider = providerManager.NotificationProvider;
		TimeProvider = providerManager.TimeProvider;
	}

	public ILocationProvider LocationProvider { get; }
	public ILoggerService LoggerService { get; }
	public INotificationProvider NotificationProvider { get; }
	public ITimeProvider TimeProvider { get; }

	public override void StopMission()
	{
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
				.CloseRange(_dealers);
		}
		catch (Exception ex)
		{
			LoggerService.Critical($"There was an error.", ex);
		}
	}
}
