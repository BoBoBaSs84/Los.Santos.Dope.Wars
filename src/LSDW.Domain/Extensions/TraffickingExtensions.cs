using GTA.Math;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Domain.Factories;
using System.Diagnostics.CodeAnalysis;

namespace LSDW.Domain.Extensions;

/// <summary>
/// The trafficking extensions class.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Extension methods.")]
public static class TraffickingExtensions
{
	private const float TrackDistance = 250;
	private const float DiscoverDistance = 100;
	private const float CreateDistance = 100;

	/// <summary>
	/// Creates and keeps track of the dealers around the world.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking)
	{
		ICollection<IDealer> dealers = trafficking.ServiceManager.StateService.Dealers;
		IPlayer player = trafficking.ServiceManager.StateService.Player;
		ITimeProvider timeProvider = trafficking.ProviderManager.TimeProvider;
		ILocationProvider locationProvider = trafficking.ProviderManager.LocationProvider;
		INotificationService notificationService = trafficking.ServiceManager.NotificationService;

		Vector3 playerPosition = locationProvider.PlayerPosition;
		Vector3 possiblePosition = locationProvider.GetNextPositionOnSidewalk(playerPosition.Around(TrackDistance));
		string zone = locationProvider.GetZoneDisplayName(possiblePosition);

		if (!dealers.Any(x => locationProvider.GetZoneDisplayName(x.Position) == zone) && !dealers.Any(x => x.Position.DistanceTo(possiblePosition) <= TrackDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(possiblePosition);
			dealers.Add(newDealer);
		}

		foreach (IDealer dealer in dealers)
		{
			if (!dealer.Discovered)
			{
				if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				{
					dealer.CreateBlip();
					dealer.ChangeInventory(timeProvider, player.Level);
					notificationService.Show(dealer.Name, "Greetings", $"Hey, if your around {locationProvider.GetZoneLocalizedName(dealer.Position)} come see me.");
				}
			}

			if (dealer.Discovered && !dealer.ClosedUntil.HasValue)
			{
				dealer.CreateBlip();

				if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				{
					dealer.Create();
				}
				else
				{
					dealer.Delete();
				}
			}
		}
		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer drug prices for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>	
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking)
	{
		ICollection<IDealer> dealers = trafficking.ServiceManager.StateService.Dealers;
		IPlayer player = trafficking.ServiceManager.StateService.Player;
		ITimeProvider timeProvider = trafficking.ProviderManager.TimeProvider;
		INotificationService notificationService = trafficking.ServiceManager.NotificationService;

		if (!dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered && x.NextPriceChange < timeProvider.Now))
			dealer.ChangePrices(timeProvider, player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking)
	{
		ICollection<IDealer> dealers = trafficking.ServiceManager.StateService.Dealers;
		IPlayer player = trafficking.ServiceManager.StateService.Player;
		ITimeProvider timeProvider = trafficking.ProviderManager.TimeProvider;
		INotificationService notificationService = trafficking.ServiceManager.NotificationService;

		if (!dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered && x.NextInventoryChange < timeProvider.Now))
		{
			dealer.ChangeInventory(timeProvider, player.Level);
			notificationService.Show(dealer.Name, "Tip-off", "Hey dude, i got new stuff in stock!");
		}

		return trafficking;
	}
}
