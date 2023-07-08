using GTA;
using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Presentation.Factories;
using System.Diagnostics.CodeAnalysis;
using DealerSettings = LSDW.Abstractions.Models.Settings.Dealer;
using RESX = LSDW.Application.Properties.Resources;

namespace LSDW.Application.Extensions;

/// <summary>
/// The trafficking extensions class.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Extension methods.")]
public static class TraffickingExtensions
{
	private const float TrackDistance = 400;
	private const float TerritoryDistance = 250;
	private const float DiscoverDistance = 150;
	private const float CreateDistance = 100;
	private const float CloseRangeDistance = 50;
	private const float RealCloseRangeDistance = 10;
	private const float InteractionDistance = 2;
	private static IDealer? ClosestDealer;

	/// <summary>
	/// Tracks new dealers around the world and adds them to the dealer collection.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, IProviderManager providerManager, IServiceManager serviceManager)
	{
		Vector3 randomPosition = providerManager.PlayerProvider.Position.Around(TrackDistance);
		Vector3 dealerPosition = providerManager.WorldProvider.GetNextPositionOnSidewalk(randomPosition);

		if (dealerPosition.Equals(Vector3.Zero))
			return trafficking;

		string zoneDisplayName = providerManager.WorldProvider.GetZoneDisplayName(dealerPosition);

		if (!serviceManager.StateService.Dealers.Any(x => providerManager.WorldProvider.GetZoneDisplayName(x.SpawnPosition) == zoneDisplayName)
			&& !serviceManager.StateService.Dealers.Any(x => x.SpawnPosition.DistanceTo(dealerPosition) <= TerritoryDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(dealerPosition);
			serviceManager.StateService.Dealers.Add(newDealer);
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of the undiscovered dealers and the already discovered dealers.
	/// </summary>
	/// <remarks>
	/// Does the following things:
	/// <list type="bullet">
	/// <item>Set the dealer to discovered to true</item>
	/// <item>Creates a blip on the map for the dealer</item>
	/// <item>Notificates the player about the discovery</item>
	/// <item>Checks if the dealers is already discovered</item>
	/// <item>Checks if the dealer is closed and tries to reopen</item>
	/// <item>Checks if the dealer blip can be created</item>	
	/// </list>
	/// </remarks>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	public static ITrafficking DiscoverDealers(this ITrafficking trafficking, IProviderManager providerManager, IServiceManager serviceManager)
	{
		if (ClosestDealer is not null)
			return trafficking;

		Vector3 playerPosition = providerManager.PlayerProvider.Position;

		foreach (IDealer dealer in serviceManager.StateService.Dealers.Where(x => x.Discovered.Equals(false)))
		{
			if (!Settings.Trafficking.DiscoverDealer)
			{
				DiscoverDealer(providerManager, dealer, serviceManager.StateService.Player);
				continue;
			}

			if (dealer.SpawnPosition.DistanceTo(playerPosition) <= DiscoverDistance)
				DiscoverDealer(providerManager, dealer, serviceManager.StateService.Player);
		}

		foreach (IDealer dealer in serviceManager.StateService.Dealers.Where(x => x.Discovered.Equals(true) && x.BlipCreated.Equals(false)))
		{
			if (dealer.Closed)
			{
				if (dealer.ClosedUntil < providerManager.WorldProvider.Now)
					dealer.ClosedUntil = null;
				else
					continue;
			}

			dealer.CreateBlip(providerManager.WorldProvider);
		}

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer drug prices for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking, IProviderManager providerManager, IServiceManager serviceManager)
	{
		if (!serviceManager.StateService.Dealers.Any(x => x.Discovered.Equals(true)))
			return trafficking;

		foreach (IDealer dealer in serviceManager.StateService.Dealers.Where(x => x.Discovered && x.Closed.Equals(false) && x.NextPriceChange < providerManager.WorldProvider.Now))
			dealer.ChangePrices(providerManager.WorldProvider, serviceManager.StateService.Player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking, IProviderManager providerManager, IServiceManager serviceManager)
	{
		if (!serviceManager.StateService.Dealers.Any(x => x.Discovered.Equals(true)))
			return trafficking;

		foreach (IDealer dealer in serviceManager.StateService.Dealers.Where(x => x.Discovered && x.Closed.Equals(false) && x.NextInventoryChange < providerManager.WorldProvider.Now))
		{
			dealer.ChangeInventory(providerManager.WorldProvider, serviceManager.StateService.Player.Level);
			providerManager.NotificationProvider.Show(
				sender: dealer.Name,
				subject: RESX.Trafficking_Notification_Restock_Subject,
				message: RESX.Trafficking_Notification_Restock_Message
				);
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of everything that happens to the dealer that is closest to te player.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	public static ITrafficking InProximity(this ITrafficking trafficking, IProviderManager providerManager, IServiceManager serviceManager)
	{
		if (Equals(serviceManager.StateService.Dealers.Count, 0))
			return trafficking;

		Vector3 playerPosition = providerManager.PlayerProvider.Position;

		// looking for the closest dealer, if found, no more iterations
		if (ClosestDealer is null)
		{
			foreach (IDealer dealer in serviceManager.StateService.Dealers.Where(x => x.Closed.Equals(false)))
			{
				if (dealer.SpawnPosition.DistanceTo(playerPosition) < CreateDistance)
				{
					ClosestDealer = dealer;
					ClosestDealer.Create(providerManager.WorldProvider);
					break;
				}
			}
		}

		if (ClosestDealer is not null)
		{
			if (ClosestDealer.Position.DistanceTo(playerPosition) is < CloseRangeDistance and > RealCloseRangeDistance)
				ClosestDealer.GuardPosition();

			if (ClosestDealer.Position.DistanceTo(playerPosition) is < RealCloseRangeDistance and > InteractionDistance)
			{
				ClosestDealer.TurnTo(providerManager.PlayerProvider.Character);

				if (trafficking.LeftSideMenu is null || trafficking.RightSideMenu is null)
				{
					trafficking.LeftSideMenu =
						PresentationFactory.CreateBuyMenu(providerManager, serviceManager.StateService.Player, ClosestDealer.Inventory);

					trafficking.RightSideMenu =
						PresentationFactory.CreateSellMenu(providerManager, serviceManager.StateService.Player, ClosestDealer.Inventory);
				}
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) is > RealCloseRangeDistance)
			{
				if (trafficking.LeftSideMenu is not null || trafficking.RightSideMenu is not null)
					trafficking.LeftSideMenu = trafficking.RightSideMenu = null;
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) <= InteractionDistance && trafficking.LeftSideMenu is not null && trafficking.RightSideMenu is not null)
			{
				if (providerManager.PlayerProvider.WantedLevel > 0)
				{
					trafficking.LeftSideMenu.Visible = trafficking.RightSideMenu.Visible = false;
					LetDealerFlee(providerManager, ClosestDealer);
					ClosestDealer = null;
					return trafficking;
				}

				if (!trafficking.LeftSideMenu.Visible || !trafficking.RightSideMenu.Visible)
				{
					providerManager.NotificationProvider.ShowHelpText(RESX.Trafficking_HelpText_DealMenu, 1, true, false);

					if (Game.IsControlJustPressed(GTA.Control.Context))
						trafficking.LeftSideMenu.Visible = true;
				}
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) > InteractionDistance)
			{
				if (trafficking.LeftSideMenu.Visible || trafficking.RightSideMenu.Visible)
					trafficking.LeftSideMenu.Visible = trafficking.RightSideMenu.Visible = false;
			}

			if (ClosestDealer.IsDead)
			{
				CloseDealer(providerManager, ClosestDealer);
				ClosestDealer = null;
				return trafficking;
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) > CreateDistance)
			{
				ClosestDealer.Delete();
				ClosestDealer = null;
				return trafficking;
			}
		}

		return trafficking;
	}

	/// <summary>
	/// Discovers the dealer, creates the blip on the map and show the notification.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	private static void DiscoverDealer(IProviderManager providerManager, IDealer dealer, IPlayer player)
	{
		dealer.Discovered = true;
		dealer.ChangeInventory(providerManager.WorldProvider, player.Level);
		dealer.CreateBlip(providerManager.WorldProvider);
		string locationName = providerManager.WorldProvider.GetZoneLocalizedName(dealer.SpawnPosition);
		providerManager.NotificationProvider.Show(
			sender: dealer.Name,
			subject: RESX.Trafficking_Notification_Discovery_Subject,
			message: RESX.Trafficking_Notification_Discovery_Message.FormatInvariant(locationName)
			);
	}

	/// <summary>
	/// Closes the dealer, deletes the blip on the map and show the notification.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	private static void CloseDealer(IProviderManager providerManager, IDealer dealer)
	{
		dealer.ClosedUntil = providerManager.WorldProvider.Now.AddHours(DealerSettings.DownTimeInHours);
		dealer.CleanUp();
		providerManager.NotificationProvider.Show(
			subject: RESX.Trafficking_Notification_Iced_Subject,
			message: RESX.Trafficking_Notification_Iced_Message.FormatInvariant(dealer.Name)
			);
	}

	/// <summary>
	/// Lets the dealer flee and show the notification.
	/// </summary>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	private static void LetDealerFlee(IProviderManager providerManager, IDealer dealer)
	{
		if (dealer.CurrentTask is TaskType.FLEE)
			return;

		dealer.Flee();
		providerManager.NotificationProvider.Show(
			subject: RESX.Trafficking_Notification_Bust_Subject,
			message: RESX.Trafficking_Notification_Bust_Message.FormatInvariant(dealer.Name)
			);
	}
}
