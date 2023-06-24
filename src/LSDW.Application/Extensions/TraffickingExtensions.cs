using GTA;
using GTA.Math;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;
using System.Diagnostics.CodeAnalysis;

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
		Vector3 randomPosition = trafficking.LocationProvider.PlayerPosition.Around(TrackDistance);
		Vector3 dealerPosition = trafficking.LocationProvider.GetNextPositionOnSidewalk(randomPosition);

		if (dealerPosition.Equals(new(0, 0, 0)))
			return trafficking;

		string zoneDisplayName = trafficking.LocationProvider.GetZoneDisplayName(dealerPosition);

		if (!stateService.Dealers.Any(x => trafficking.LocationProvider.GetZoneDisplayName(x.Position) == zoneDisplayName) && !stateService.Dealers.Any(x => x.Position.DistanceTo(dealerPosition) <= TerritoryDistance))
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

		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

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
				if (dealer.ClosedUntil.HasValue && dealer.ClosedUntil < trafficking.TimeProvider.Now)
					dealer.SetOpen();
				else
					continue;

			dealer.CreateBlip();
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
		if (ClosestDealer is not null || !stateService.Dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered && x.NextPriceChange < trafficking.TimeProvider.Now))
			dealer.ChangePrices(trafficking.TimeProvider, stateService.Player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="stateService">The state service instance to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking, IStateService stateService)
	{
		if (ClosestDealer is not null || !stateService.Dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered && x.NextInventoryChange < trafficking.TimeProvider.Now))
		{
			dealer.ChangeInventory(trafficking.TimeProvider, stateService.Player.Level);
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
		if (!stateService.Dealers.Any())
			return trafficking;

		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

		// looking for the closest dealer, if found, no more iterations
		if (ClosestDealer is null)
		{
			foreach (IDealer dealer in stateService.Dealers.Where(x => x.Closed.Equals(false)))
			{
				if (dealer.Position.DistanceTo(playerPosition) < CreateDistance)
					ClosestDealer = dealer;
				break;
			}
		}

		if (ClosestDealer is not null)
		{
			if (ClosestDealer.Position.DistanceTo(playerPosition) < CreateDistance && !ClosestDealer.Created)
				ClosestDealer.Create();

			if (ClosestDealer.Position.DistanceTo(playerPosition) is < CloseRangeDistance and > RealCloseRangeDistance)
				ClosestDealer.GuardPosition();

			if (ClosestDealer.Position.DistanceTo(playerPosition) is < RealCloseRangeDistance and > InteractionDistance)
			{
				// Todo: IPlayerProvider
				ClosestDealer.TurnTo(Game.Player.Character);

				if (trafficking.LeftSideMenu.Initialized.Equals(false))
					trafficking.LeftSideMenu.Initialize(stateService.Player, ClosestDealer.Inventory);

				if (trafficking.RightSideMenu.Initialized.Equals(false))
					trafficking.RightSideMenu.Initialize(stateService.Player, ClosestDealer.Inventory);
			}

			if (ClosestDealer.Position.DistanceTo(playerPosition) <= InteractionDistance)
			{
				trafficking.NotificationProvider.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to start a warehouse mission.");
				if (Game.IsControlJustPressed(GTA.Control.Context))
				{
				}
			}

			// cleanup everything
			if (ClosestDealer.Position.DistanceTo(playerPosition) > CreateDistance || ClosestDealer.IsDead || Game.Player.WantedLevel > 0)
			{
				if (ClosestDealer.IsDead)
				{
					ClosestDealer.SetClosed(trafficking.TimeProvider);
					string message = $"{ClosestDealer.Name} was made cold, the store is closed for the time being!";
					trafficking.NotificationProvider.Show("ICED!", message);
				}

				// Todo: IPlayerProvider
				if (Game.Player.WantedLevel > 0)
				{
					ClosestDealer.Flee();
					string message = $"{ClosestDealer.Name} had to flee from the cops!";
					trafficking.NotificationProvider.Show("BUST!", message);
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
		dealer.SetDiscovered(true);
		dealer.ChangeInventory(trafficking.TimeProvider, player.Level);
		dealer.CreateBlip();
		string locationName = trafficking.LocationProvider.GetZoneLocalizedName(dealer.Position);
		string message = $"Hey dude, if your around {locationName}, come see me.";
		trafficking.NotificationProvider.Show(dealer.Name, "Greetings", message);
	}
}
