using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Application.Properties;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Presentation.Factories;
using System.Diagnostics.CodeAnalysis;
using GTAControl = GTA.Control;

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
	public static ITrafficking TrackDealers(this ITrafficking trafficking)
	{
		Vector3 randomPosition = trafficking.PlayerProvider.Position.Around(TrackDistance);
		Vector3 dealerPosition = trafficking.WorldProvider.GetNextPositionOnSidewalk(randomPosition);

		if (dealerPosition.Equals(Vector3.Zero))
			return trafficking;

		string zoneDisplayName = trafficking.WorldProvider.GetZoneDisplayName(dealerPosition);

		if (!trafficking.StateService.Dealers.Any(x => trafficking.WorldProvider.GetZoneDisplayName(x.SpawnPosition) == zoneDisplayName)
			&& !trafficking.StateService.Dealers.Any(x => x.SpawnPosition.DistanceTo(dealerPosition) <= TerritoryDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(dealerPosition);
			trafficking.StateService.Dealers.Add(newDealer);
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
	public static ITrafficking DiscoverDealers(this ITrafficking trafficking)
	{
		if (ClosestDealer is not null)
			return trafficking;

		Vector3 playerPosition = trafficking.PlayerProvider.Position;

		foreach (IDealer dealer in trafficking.StateService.Dealers.Where(x => x.Discovered.Equals(false)))
		{
			if (!trafficking.SettingsService.Trafficking.DiscoverDealer)
			{
				trafficking.DiscoverDealer(dealer);
				continue;
			}

			if (dealer.SpawnPosition.DistanceTo(playerPosition) <= DiscoverDistance)
				trafficking.DiscoverDealer(dealer);
		}

		foreach (IDealer dealer in trafficking.StateService.Dealers.Where(x => x.Discovered.Equals(true) && x.BlipCreated.Equals(false)))
		{
			if (dealer.Closed)
			{
				if (dealer.ClosedUntil < trafficking.WorldProvider.Now)
					dealer.ClosedUntil = null;
				else
					continue;
			}

			dealer.CreateBlip(trafficking.WorldProvider);
		}

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer drug prices for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking)
	{
		if (!trafficking.StateService.Dealers.Any(x => x.Discovered.Equals(true)))
			return trafficking;

		foreach (IDealer dealer in trafficking.StateService.Dealers.Where(x => x.Discovered && x.Closed.Equals(false) && x.NextPriceChange < trafficking.WorldProvider.Now))
			dealer.ChangePrices(trafficking.WorldProvider, trafficking.StateService.Player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking)
	{
		if (!trafficking.StateService.Dealers.Any(x => x.Discovered.Equals(true)))
			return trafficking;

		foreach (IDealer dealer in trafficking.StateService.Dealers.Where(x => x.Discovered && x.Closed.Equals(false) && x.NextInventoryChange < trafficking.WorldProvider.Now))
		{
			dealer.ChangeInventory(trafficking.WorldProvider, trafficking.StateService.Player.Level);
			trafficking.NotificationProvider.Show(
				sender: dealer.Name,
				subject: Resources.Trafficking_Notification_Restock_Subject,
				message: Resources.Trafficking_Notification_Restock_Message
				);
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of everything that happens to the dealer that is closest to te player.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	public static ITrafficking InProximity(this ITrafficking trafficking, IProviderManager providerManager)
	{
		if (Equals(trafficking.StateService.Dealers.Count, 0))
			return trafficking;

		Vector3 playerPosition = trafficking.PlayerProvider.Position;

		// looking for the closest dealer, if found, no more iterations
		if (ClosestDealer is null)
		{
			foreach (IDealer dealer in trafficking.StateService.Dealers.Where(x => x.Closed.Equals(false)))
			{
				if (dealer.SpawnPosition.DistanceTo(playerPosition) < CreateDistance)
				{
					ClosestDealer = dealer;
					ClosestDealer.Create(trafficking.WorldProvider);
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
				ClosestDealer.TurnTo(trafficking.PlayerProvider.Character);

				if (trafficking.LeftSideMenu is null || trafficking.RightSideMenu is null)
				{
					trafficking.LeftSideMenu = PresentationFactory.CreateBuyMenu(providerManager, trafficking.StateService.Player, ClosestDealer.Inventory);
					trafficking.LeftSideMenu.SwitchItemActivated += (sender, args) => OnSwitchItemActivated(trafficking);

					trafficking.RightSideMenu = PresentationFactory.CreateSellMenu(providerManager, trafficking.StateService.Player, ClosestDealer.Inventory);
					trafficking.RightSideMenu.SwitchItemActivated += (sender, args) => OnSwitchItemActivated(trafficking);
				}
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) is > RealCloseRangeDistance)
			{
				if (trafficking.LeftSideMenu is not null || trafficking.RightSideMenu is not null)
					trafficking.LeftSideMenu = trafficking.RightSideMenu = null;
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) <= InteractionDistance && trafficking.LeftSideMenu is not null && trafficking.RightSideMenu is not null)
			{
				if (trafficking.PlayerProvider.WantedLevel > 0)
				{
					CloseMenus(trafficking);
					trafficking.LetDealerFlee(ClosestDealer);
					ClosestDealer = null;
					return trafficking;
				}

				if (!trafficking.LeftSideMenu.Visible || !trafficking.RightSideMenu.Visible)
				{
					trafficking.NotificationProvider.ShowHelpText(Resources.Trafficking_HelpText_DealMenu, 1, true, false);

					if (trafficking.GameProvider.IsControlJustPressed(GTAControl.Context))
						trafficking.LeftSideMenu?.Toggle();
				}
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) > InteractionDistance && trafficking.LeftSideMenu is not null && trafficking.RightSideMenu is not null)
				CloseMenus(trafficking);

			if (ClosestDealer.IsDead)
			{
				trafficking.CloseDealer(ClosestDealer);
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

	private static void OnSwitchItemActivated(ITrafficking trafficking)
	{
		trafficking.LeftSideMenu?.Toggle();
		trafficking.RightSideMenu?.Toggle();
	}

	private static void CloseMenus(ITrafficking trafficking)
	{
		if (trafficking.LeftSideMenu is not null && trafficking.LeftSideMenu.Visible)
			trafficking.LeftSideMenu.Toggle();
		if (trafficking.RightSideMenu is not null && trafficking.RightSideMenu.Visible)
			trafficking.RightSideMenu.Toggle();
	}

	/// <summary>
	/// Discovers the dealer, creates the blip on the map and show the notification.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	internal static void DiscoverDealer(this ITrafficking trafficking, IDealer dealer)
	{
		dealer.Discovered = true;
		dealer.ChangeInventory(trafficking.WorldProvider, trafficking.StateService.Player.Level);
		dealer.CreateBlip(trafficking.WorldProvider);
		string locationName = trafficking.WorldProvider.GetZoneLocalizedName(dealer.SpawnPosition);
		trafficking.NotificationProvider.Show(
			sender: dealer.Name,
			subject: Resources.Trafficking_Notification_Discovery_Subject,
			message: Resources.Trafficking_Notification_Discovery_Message.FormatInvariant(locationName)
			);
	}

	/// <summary>
	/// Closes the dealer, deletes the blip on the map and show the notification.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	internal static void CloseDealer(this ITrafficking trafficking, IDealer dealer)
	{
		int downTimeInHours = trafficking.SettingsService.Dealer.DownTimeInHours;
		dealer.ClosedUntil = trafficking.WorldProvider.Now.AddHours(downTimeInHours);
		dealer.CleanUp();
		trafficking.NotificationProvider.Show(
			subject: Resources.Trafficking_Notification_Iced_Subject,
			message: Resources.Trafficking_Notification_Iced_Message.FormatInvariant(dealer.Name)
			);
	}

	/// <summary>
	/// Lets the dealer flee and show the notification.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to extend.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	internal static void LetDealerFlee(this ITrafficking trafficking, IDealer dealer)
	{
		if (dealer.CurrentTask is TaskType.FLEE)
			return;

		dealer.Flee();
		trafficking.NotificationProvider.Show(
			subject: Resources.Trafficking_Notification_Bust_Subject,
			message: Resources.Trafficking_Notification_Bust_Message.FormatInvariant(dealer.Name)
			);
	}
}
