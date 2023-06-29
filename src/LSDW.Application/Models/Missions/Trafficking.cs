using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Extensions;
using LSDW.Application.Models.Missions.Base;
using LSDW.Domain.Extensions;
using LSDW.Presentation.Factories;

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
	private readonly IStateService _stateService;
	private readonly Lazy<ISideMenu> _lazyLeftSideMenu;
	private readonly Lazy<ISideMenu> _lazyRightSideMenu;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	internal Trafficking(IServiceManager serviceManager, IProviderManager providerManager)
		: base(serviceManager, providerManager, nameof(Trafficking))
	{
		_serviceManager = serviceManager;
		_providerManager = providerManager;
		_stateService = serviceManager.StateService;
		_lazyLeftSideMenu = new Lazy<ISideMenu>(() => PresentationFactory.CreateBuyMenu(_providerManager));
		_lazyRightSideMenu = new Lazy<ISideMenu>(() => PresentationFactory.CreateSellMenu(_providerManager));
	}

	public ISideMenu LeftSideMenu => _lazyLeftSideMenu.Value;
	public ISideMenu RightSideMenu => _lazyRightSideMenu.Value;

	public override void StartMission()
	{
		LeftSideMenu.SwitchItem.Activated += OnSwitchItemActivated;
		RightSideMenu.SwitchItem.Activated -= OnSwitchItemActivated;
		base.StartMission();
	}

	public override void StopMission()
	{
		_ = _stateService.Dealers.CleanUp();
		LeftSideMenu.SwitchItem.Activated -= OnSwitchItemActivated;
		RightSideMenu.SwitchItem.Activated -= OnSwitchItemActivated;
		LeftSideMenu.CleanUp();
		RightSideMenu.CleanUp();
		base.StopMission();
	}

	public override void OnAborted(object sender, EventArgs args)
	{
		if (Status is MissionStatusType.STARTED)
			StopMission();
	}

	public override void OnTick(object sender, EventArgs args)
	{
		if (Status is not MissionStatusType.STARTED)
			return;

		if (!PlayerProvider.CanControlCharacter && !PlayerProvider.CanStartMission)
			return;

		try
		{
			_ = this.TrackDealers(_stateService)
				.DiscoverDealers(_stateService)
				.ChangeDealerInventories(_stateService)
				.ChangeDealerPrices(_stateService)
				.InProximity(_stateService);
		}
		catch (Exception ex)
		{
			LoggerService.Critical($"There was an error.", ex);
		}
	}

	private void OnSwitchItemActivated(object sender, EventArgs e)
	{
		LeftSideMenu.Visible = !LeftSideMenu.Visible;
		RightSideMenu.Visible = !RightSideMenu.Visible;
	}
}
