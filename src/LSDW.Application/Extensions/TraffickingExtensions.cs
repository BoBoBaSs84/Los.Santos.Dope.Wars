using GTA;
using GTA.Math;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using System.Diagnostics.CodeAnalysis;
using DealerSettings = LSDW.Abstractions.Models.Settings.Dealer;


namespace LSDW.Application.Extensions;

/// <summary>
/// The trafficking extensions class.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Extension methods.")]
// Todo: Resources!
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
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="stateService">The state service instance to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, IStateService stateService)
	{
		Vector3 randomPosition = trafficking.PlayerProvider.Position.Around(TrackDistance);
		Vector3 dealerPosition = trafficking.WorldProvider.GetNextPositionOnSidewalk(randomPosition);

		if (dealerPosition.Equals(new(0, 0, 0)))
			return trafficking;

		string zoneDisplayName = trafficking.WorldProvider.GetZoneDisplayName(dealerPosition);

		if (!stateService.Dealers.Any(x => trafficking.WorldProvider.GetZoneDisplayName(x.Position) == zoneDisplayName) && !stateService.Dealers.Any(x => x.Position.DistanceTo(dealerPosition) <= TerritoryDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(dealerPosition);
			stateService.Dealers.Add(newDealer);
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
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="stateService">The state service instance to use.</param>
	public static ITrafficking DiscoverDealers(this ITrafficking trafficking, IStateService stateService)
	{
		if (ClosestDealer is not null)
			return trafficking;

		Vector3 playerPosition = trafficking.PlayerProvider.Position;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered.Equals(false)))
		{
			if (!Settings.Trafficking.DiscoverDealer)
			{
				trafficking.DiscoverDealer(dealer, stateService.Player);
				continue;
			}

			if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				trafficking.DiscoverDealer(dealer, stateService.Player);
		}

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered.Equals(true) && x.BlipCreated.Equals(false)))
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
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="stateService">The state service instance to use.</param>
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking, IStateService stateService)
	{
		if (!stateService.Dealers.Any(x => x.Discovered.Equals(true)))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered && x.Closed.Equals(false) && x.NextPriceChange < trafficking.WorldProvider.Now))
			dealer.ChangePrices(trafficking.WorldProvider, stateService.Player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="stateService">The state service instance to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking, IStateService stateService)
	{
		if (!stateService.Dealers.Any(x => x.Discovered.Equals(true)))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered && x.Closed.Equals(false) && x.NextInventoryChange < trafficking.WorldProvider.Now))
		{
			dealer.ChangeInventory(trafficking.WorldProvider, stateService.Player.Level);
			trafficking.NotificationProvider.Show(dealer.Name, "Tip-off", "Hey dude, i got new stuff in stock!");
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of everything that happens to the dealer that is closest to te player.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="stateService">The state service instance to use.</param>
	public static ITrafficking InProximity(this ITrafficking trafficking, IStateService stateService)
	{
		if (Equals(stateService.Dealers.Count, 0))
			return trafficking;

		Vector3 playerPosition = trafficking.PlayerProvider.Position;

		// looking for the closest dealer, if found, no more iterations
		if (ClosestDealer is null)
		{
			foreach (IDealer dealer in stateService.Dealers.Where(x => x.Closed.Equals(false)))
			{
				if (dealer.Position.DistanceTo(playerPosition) <= CreateDistance)
				{
					ClosestDealer = dealer;
					break;
				}
			}
		}

		if (ClosestDealer is not null)
		{
			if (ClosestDealer.Position.DistanceTo(playerPosition) < CreateDistance)
			{
				ClosestDealer.Create(trafficking.WorldProvider);
				ClosestDealer.Wait();
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) is < CloseRangeDistance and > RealCloseRangeDistance)
				ClosestDealer.Wait();

			if (ClosestDealer.Position.DistanceTo(playerPosition) is < RealCloseRangeDistance and > InteractionDistance)
			{
				ClosestDealer.TurnTo(trafficking.PlayerProvider.Character);

				if (trafficking.LeftSideMenu.Initialized.Equals(false))
					trafficking.LeftSideMenu.Initialize(stateService.Player, ClosestDealer.Inventory);

				if (trafficking.RightSideMenu.Initialized.Equals(false))
					trafficking.RightSideMenu.Initialize(stateService.Player, ClosestDealer.Inventory);
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) <= InteractionDistance)
			{
				if (!trafficking.LeftSideMenu.Visible && !trafficking.RightSideMenu.Visible)
				{
					trafficking.NotificationProvider.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to start a warehouse mission.");

					if (Game.IsControlJustPressed(GTA.Control.Context))
						trafficking.LeftSideMenu.Visible = true;
				}
			}

			// cleanup everything
			if (ClosestDealer.Position.DistanceTo(playerPosition) > CreateDistance || ClosestDealer.IsDead || trafficking.PlayerProvider.WantedLevel > 0)
			{
				if (!ClosestDealer.Closed)
				{
					if (!ClosestDealer.IsDead)
					{
						if (trafficking.PlayerProvider.WantedLevel > 0)
						{
							ClosestDealer.Flee();
							string message = $"{ClosestDealer.Name} had to flee from the cops!";
							trafficking.NotificationProvider.Show("BUST!", message);
						}
					}

					if (ClosestDealer.IsDead)
					{
						ClosestDealer.DeleteBlip();
						ClosestDealer.ClosedUntil = trafficking.WorldProvider.Now.AddHours(DealerSettings.DownTimeInHours);
						string message = $"{ClosestDealer.Name} was made cold, the store is closed for the time being!";
						trafficking.NotificationProvider.Show("ICED!", message);
					}
				}

				if (ClosestDealer.Position.DistanceTo(playerPosition) > CreateDistance)
				{
					ClosestDealer.Delete();
					ClosestDealer = null;
				}
			}
		}

		return trafficking;
	}

	/// <summary>
	/// Discovers the dealer, creates the blip on the map and show the notification.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	private static void DiscoverDealer(this ITrafficking trafficking, IDealer dealer, IPlayer player)
	{
		dealer.Discovered = true;
		dealer.ChangeInventory(trafficking.WorldProvider, player.Level);
		dealer.CreateBlip(trafficking.WorldProvider);
		string locationName = trafficking.WorldProvider.GetZoneLocalizedName(dealer.Position);
		string message = $"Hey dude, if your around {locationName}, come see me.";
		trafficking.NotificationProvider.Show(dealer.Name, "Greetings", message);
	}
}
