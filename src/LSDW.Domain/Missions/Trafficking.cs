using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
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
	private readonly ILoggerService _loggerService;

	private ISideMenu? leftSideMenu;
	private ISideMenu? rightSideMenu;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	internal Trafficking(IServiceManager serviceManager, IProviderManager providerManager) : base(serviceManager.LoggerService, nameof(Trafficking))
	{
		_loggerService = serviceManager.LoggerService;
		ServiceManager = serviceManager;
		ProviderManager = providerManager;
	}

	public IServiceManager ServiceManager { get; }
	public IProviderManager ProviderManager { get; }

	public override void StopMission()
	{
		leftSideMenu = null;
		rightSideMenu = null;
		_ = ServiceManager.StateService.Dealers.DeleteDealers();
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
			_ = this.TrackDealers()
				.ChangeDealerInventories()
				.ChangeDealerPrices();
		}
		catch (Exception ex)
		{
			_loggerService.Critical(ex.Message);
		}
	}
}
