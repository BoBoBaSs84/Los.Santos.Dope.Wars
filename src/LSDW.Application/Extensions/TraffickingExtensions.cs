using GTA;
using GTA.Math;
using GTA.UI;
using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using MarketSettings = LSDW.Domain.Models.Settings.Market;

namespace LSDW.Application.Extensions;

/// <summary>
/// The trafficking extensions class.
/// </summary>
public static class TraffickingExtensions
{
	private const float TrackDistance = 250;
	private const float DiscoverDistance = 100;

	/// <summary>
	/// Creates and keeps track of the dealers around the world.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	/// <param name="dealers">The collection of dealers to work with.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, ICollection<IDealer> dealers)
	{
		Vector3 playerPosition = Game.Player.Character.Position;
		Vector3 possiblePosition = World.GetNextPositionOnSidewalk(playerPosition.Around(TrackDistance));
		string zone = World.GetZoneDisplayName(possiblePosition);

		if (!dealers.Any(x => World.GetZoneDisplayName(x.Position) == zone) && !dealers.Any(x => x.Position.DistanceTo(possiblePosition) <= TrackDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(possiblePosition);
			dealers.Add(newDealer);
		}

		foreach (IDealer dealer in dealers)
			if (!dealer.Discovered)
			{
				if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
					dealer.CreateBlip();
			}
			else if (!dealer.IsBlipCreated)
				dealer.CreateBlip();

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer drug prices for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	/// <param name="stateService">The game state service interface to use.</param>
	/// <param name="timeProvider">The time provider interface to use.</param>
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking, IGameStateService stateService, ITimeProvider timeProvider)
	{
		if (!stateService.Dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered))
			if (dealer.LastRefresh < timeProvider.Now.AddHours(MarketSettings.PriceChangeInterval))
				_ = dealer.ChangePrices(timeProvider, stateService.Player.Level);

		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	/// <param name="stateService">The game state service interface to use.</param>
	/// <param name="timeProvider">The time provider interface to use.</param>
	public static ITrafficking RestockDealerInventories(this ITrafficking trafficking, IGameStateService stateService, ITimeProvider timeProvider)
	{
		if (!stateService.Dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered))
			if (dealer.LastRefresh < timeProvider.Now.AddHours(MarketSettings.InventoryChangeInterval))
			{
				_ = dealer.RestockInventory(timeProvider, stateService.Player.Level);
				_ = trafficking.NotificationService.Show(NotificationIcon.Default, dealer.Name, "Tip-off", "Hey dude, i got new stuff in stock!");
			}

		return trafficking;
	}
}
