using GTA;
using GTA.Math;
using GTA.UI;
using LSDW.Abstractions.Application.Missions;
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
	/// <param name="stateService">The game state service interface to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, IGameStateService stateService)
	{
		Vector3 playerPosition = Game.Player.Character.Position;
		Vector3 possiblePosition = World.GetNextPositionOnSidewalk(playerPosition.Around(TrackDistance));
		string zone = World.GetZoneDisplayName(possiblePosition);

		if (!stateService.Dealers.Any(x => World.GetZoneDisplayName(x.Position) == zone) && !stateService.Dealers.Any(x => x.Position.DistanceTo(possiblePosition) <= TrackDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(possiblePosition);
			stateService.Dealers.Add(newDealer);
		}

		foreach (IDealer dealer in stateService.Dealers)
		{
			if (!dealer.Discovered)
			{
				if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				{
					dealer.CreateBlip();
					trafficking.NotificationService.Show(dealer.Name, "Greetings", $"Hey, if your around {World.GetZoneLocalizedName(dealer.Position)} come see me.");
				}
			}

			if (dealer.Discovered && !dealer.IsBlipCreated)
			{
				dealer.CreateBlip();
			}
		}
		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer drug prices for each discovered dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	/// <param name="stateService">The game state service interface to use.</param>
	public static ITrafficking ChangeDealerPrices(this ITrafficking trafficking, IGameStateService stateService)
	{
		if (!stateService.Dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered))
		{
			if (dealer.LastRefresh < trafficking.TimeProvider.Now.AddHours(MarketSettings.PriceChangeInterval))
			{
				_ = dealer.ChangePrices(trafficking.TimeProvider, stateService.Player.Level);
			}
		}
		return trafficking;
	}

	/// <summary>
	/// Checks and changes the dealer inventories.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	/// <param name="stateService">The game state service interface to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking, IGameStateService stateService)
	{
		if (!stateService.Dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in stateService.Dealers.Where(x => x.Discovered))
		{
			if (dealer.LastRefresh < trafficking.TimeProvider.Now.AddHours(MarketSettings.InventoryChangeInterval))
			{
				_ = dealer.ChangeInventory(trafficking.TimeProvider, stateService.Player.Level);
				trafficking.NotificationService.Show(dealer.Name, "Tip-off", "Hey dude, i got new stuff in stock!");
			}
		}
		return trafficking;
	}
}
