using GTA.Math;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace LSDW.Domain.Extensions;

/// <summary>
/// The trafficking extensions class.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Extension methods.")]
public static class TraffickingExtensions
{
	private const float TrackDistance = 500;
	private const float TerritoryDistance = 250;
	private const float DiscoverDistance = 150;
	private const float CreateDistance = 100;

	/// <summary>
	/// Tracks new dealers around the world and adds them to the dealer collection.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, ICollection<IDealer> dealers, IPlayer player)
	{
		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;
		Vector3 possiblePosition = trafficking.LocationProvider.GetNextPositionOnSidewalk(playerPosition.Around(TrackDistance));
		string zoneDisplayName = trafficking.LocationProvider.GetZoneDisplayName(possiblePosition);

		if (!dealers.Any(x => trafficking.LocationProvider.GetZoneDisplayName(x.Position) == zoneDisplayName) && !dealers.Any(x => x.Position.DistanceTo(possiblePosition) <= TerritoryDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(possiblePosition);
			newDealer.ChangeInventory(trafficking.TimeProvider, player.Level);
			dealers.Add(newDealer);

			string message = $"A dealer has appeared in '{trafficking.LocationProvider.GetZoneLocalizedName(possiblePosition)}' at '{possiblePosition}'.";
			trafficking.LoggerService.Debug(message);
		}

		return trafficking;
	}

	/// <summary>
	/// Discovers dealers and does the following things:
	/// <list type="bullet">
	/// <item>Set the dealer to discovered to <see langword="true"/></item>
	/// <item>Creates a blip on the map for the dealer</item>
	/// <item>Notificates the player about the discovery</item>
	/// </list>
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	public static ITrafficking DiscoverDealers(this ITrafficking trafficking, ICollection<IDealer> dealers)
	{
		if (!dealers.Any(x => x.Discovered.Equals(false)))
			return trafficking;

		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered.Equals(false)))
		{
			if (!Settings.Trafficking.DiscoverDealer)
			{
				DiscoverDealer(dealer, trafficking.LoggerService, trafficking.LocationProvider, trafficking.NotificationProvider);
				continue;
			}

			if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				DiscoverDealer(dealer, trafficking.LoggerService, trafficking.LocationProvider, trafficking.NotificationProvider);
		}

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer drug prices for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking, ICollection<IDealer> dealers, IPlayer player)
	{
		if (!dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered && x.NextPriceChange < trafficking.TimeProvider.Now))
			dealer.ChangePrices(trafficking.TimeProvider, player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking, ICollection<IDealer> dealers, IPlayer player)
	{
		if (!dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered && x.NextInventoryChange < trafficking.TimeProvider.Now))
		{
			dealer.ChangeInventory(trafficking.TimeProvider, player.Level);
			trafficking.NotificationProvider.Show(dealer.Name, "Tip-off", "Hey dude, i got new stuff in stock!");
		}

		return trafficking;
	}

	/// <summary>
	/// Discovers the dealer, creates the blip on the map and show the notification.
	/// </summary>
	/// <param name="dealer">The dealer instance to use.</param>
	/// <param name="loggerService">The logger service instance to use.</param>
	/// <param name="locationProvider">The location provider instance to use.</param>
	/// <param name="notificationProvider">The notification provider instance to use.</param>
	private static void DiscoverDealer(IDealer dealer, ILoggerService loggerService, ILocationProvider locationProvider, INotificationProvider notificationProvider)
	{
		dealer.SetDiscovered(true);
		dealer.CreateBlip();
		notificationProvider.Show(dealer.Name, "Greetings", $"Hey, if your around {locationProvider.GetZoneLocalizedName(dealer.Position)} come see me.");
		loggerService.Information($"Dealer {dealer.Name} found at {dealer.Position}");
	}
}
