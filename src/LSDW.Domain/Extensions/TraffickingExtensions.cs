using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
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
	/// <param name="dealers">The dealer collection instance to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, ICollection<IDealer> dealers, IPlayer player)
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
		{
			if (!dealer.Discovered)
			{
				if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				{
					dealer.CreateBlip();
					dealer.ChangeInventory(trafficking.TimeProvider, player.Level);
					trafficking.NotificationService.Show(dealer.Name, "Greetings", $"Hey, if your around {World.GetZoneLocalizedName(dealer.Position)} come see me.");
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
	/// Checks and changes the dealer inventories.
	/// </summary>
	/// <param name="trafficking">The trafficking interface to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static ITrafficking ChangeDealerInventories(this ITrafficking trafficking, ICollection<IDealer> dealers, IPlayer player)
	{
		if (!dealers.Any(x => x.Discovered))
			return trafficking;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered && x.NextInventoryChange < trafficking.TimeProvider.Now))
		{
			dealer.ChangeInventory(trafficking.TimeProvider, player.Level);
			trafficking.NotificationService.Show(dealer.Name, "Tip-off", "Hey dude, i got new stuff in stock!");
		}

		return trafficking;
	}
}
