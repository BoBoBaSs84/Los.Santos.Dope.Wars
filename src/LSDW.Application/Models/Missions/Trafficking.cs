﻿using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Extensions;
using LSDW.Application.Models.Missions.Base;
using LSDW.Domain.Extensions;
using RESX = LSDW.Application.Properties.Resources;

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

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	internal Trafficking(IServiceManager serviceManager, IProviderManager providerManager) : base(serviceManager, providerManager, nameof(Trafficking))
	{
		_serviceManager = serviceManager;
		_providerManager = providerManager;
	}

	public IDealMenu? LeftSideMenu { get; set; }
	public IDealMenu? RightSideMenu { get; set; }

	public override void StartMission()
		=> base.StartMission();

	public override void StopMission()
	{
		_ = StateService.Dealers.CleanUp();
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

		if (PlayerProvider.CanControlCharacter && !PlayerProvider.CanStartMission)
			return;

		try
		{
			_ = this.TrackDealers()
				.DiscoverDealers()
				.ChangeDealerInventories()
				.ChangeDealerPrices()
				.InProximity(_providerManager);
		}
		catch (Exception ex)
		{
			LoggerService.Critical(RESX.Trafficking_Error_Critical, ex);
		}
	}
}
