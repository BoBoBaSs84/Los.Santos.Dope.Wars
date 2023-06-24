using GTA;
using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Managers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;
using LSDW.Presentation.Factories;
using System.Diagnostics.CodeAnalysis;

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

	/// <summary>
	/// Tracks new dealers around the world and adds them to the dealer collection.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	public static ITrafficking TrackDealers(this ITrafficking trafficking, ICollection<IDealer> dealers)
	{
		Vector3 randomPosition = trafficking.LocationProvider.PlayerPosition.Around(TrackDistance);
		Vector3 dealerPosition = trafficking.LocationProvider.GetNextPositionOnSidewalk(randomPosition);

		if (dealerPosition.Equals(new(0, 0, 0)))
			return trafficking;

		string zoneDisplayName = trafficking.LocationProvider.GetZoneDisplayName(dealerPosition);

		if (!dealers.Any(x => trafficking.LocationProvider.GetZoneDisplayName(x.Position) == zoneDisplayName) && !dealers.Any(x => x.Position.DistanceTo(dealerPosition) <= TerritoryDistance))
		{
			IDealer newDealer = DomainFactory.CreateDealer(dealerPosition);
			dealers.Add(newDealer);

			string message = $"A dealer has appeared in '{trafficking.LocationProvider.GetZoneLocalizedName(dealerPosition)}' at '{dealerPosition}'.";
			trafficking.LoggerService.Debug(message);
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of the undiscovered dealers.
	/// </summary>
	/// <remarks>
	/// Does the following things:
	/// <list type="bullet">
	/// <item>Set the dealer to discovered to true</item>
	/// <item>Creates a blip on the map for the dealer</item>
	/// <item>Notificates the player about the discovery</item>
	/// </list>
	/// </remarks>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static ITrafficking DiscoverDealers(this ITrafficking trafficking, ICollection<IDealer> dealers, IPlayer player)
	{
		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

		foreach (IDealer dealer in dealers.Where(x => x.Discovered.Equals(false)))
		{
			if (!Settings.Trafficking.DiscoverDealer)
			{
				trafficking.DiscoverDealer(dealer, player);
				continue;
			}

			if (dealer.Position.DistanceTo(playerPosition) <= DiscoverDistance)
				trafficking.DiscoverDealer(dealer, player);
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of the already discovered dealers.
	/// </summary>
	/// <remarks>
	/// Does the following things:
	/// <list type="bullet">
	/// <item>Checks if the dealers is discovered</item>
	/// <item>Checks if the dealer is closed and tries to reopen</item>
	/// <item>Checks if the dealer blip can be created</item>
	/// </list>
	/// </remarks>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	public static ITrafficking DiscoveredDealers(this ITrafficking trafficking, ICollection<IDealer> dealers)
	{
		foreach (IDealer dealer in dealers.Where(x => x.Discovered && !x.BlipCreated))
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
	/// Takes care of the creation and deletion of the discovered dealers.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	public static ITrafficking CreateDealers(this ITrafficking trafficking, ICollection<IDealer> dealers)
	{
		if (!dealers.Any())
			return trafficking;

		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

		foreach (IDealer dealer in dealers)
		{
			if (dealer.Position.DistanceTo(playerPosition) < CreateDistance)
			{
				if (dealer.Created || dealer.Closed)
					continue;

				dealer.Create();
			}

			if (dealer.Position.DistanceTo(playerPosition) > CreateDistance)
				if (dealer.Created)
					dealer.Delete();
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of every thing if the player is in close range to the dealer.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	public static ITrafficking CloseRange(this ITrafficking trafficking, IServiceManager serviceManager, IProviderManager providerManager, ICollection<IDealer> dealers, IPlayer player)
	{
		if (!dealers.Any())
			return trafficking;

		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

		foreach (IDealer dealer in dealers)
		{
			if (dealer.IsDead && dealer.Created)
				dealer.SetClosed(trafficking.TimeProvider);

			if (dealer.Position.DistanceTo(playerPosition) < CloseRangeDistance)
				dealer.GuardPosition();

			if (dealer.Position.DistanceTo(playerPosition) < RealCloseRangeDistance)
			{
				dealer.TurnTo(Game.Player.Character);

				if (trafficking.MenusInitialized.Equals(false))
				{
					trafficking.LoggerService.Debug($"{trafficking.MenusInitialized}");
					InitializeMenus(trafficking, serviceManager, providerManager, dealer, player);
					trafficking.LoggerService.Debug($"{trafficking.MenusInitialized}");
				}

				//if (Game.Player.WantedLevel > 0)
				//{
				//	dealer.Flee();
				//	dealer.SetClosed(trafficking.TimeProvider);
				//}
			}
			else
			{
				if (trafficking.MenusInitialized.Equals(true))
				{
					trafficking.LoggerService.Debug($"{trafficking.MenusInitialized}");
					trafficking.CleanUpMenus();
					trafficking.LoggerService.Debug($"{trafficking.MenusInitialized}");
				}
			}
		}

		return trafficking;
	}

	/// <summary>
	/// Takes care of the interactions between the player and the dealers.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="dealers">The dealer collection instance to use.</param>
	public static ITrafficking DealerInteraction(this ITrafficking trafficking, ICollection<IDealer> dealers)
	{
		if (!dealers.Any())
			return trafficking;

		Vector3 playerPosition = trafficking.LocationProvider.PlayerPosition;

		foreach (IDealer dealer in dealers.Where(x => x.Closed.Equals(false)))
		{
			if (dealer.Position.DistanceTo(playerPosition) <= InteractionDistance)
			{
				if (trafficking.MenusInitialized)
				{
					trafficking.NotificationProvider.ShowHelpTextThisFrame($"~b~Press ~INPUT_CONTEXT~ ~w~to start a warehouse mission.");
					if (Game.IsControlJustPressed(GTA.Control.Context))
					{
					}
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

	/// <summary>
	/// Initializes the trafficking menus.
	/// </summary>
	/// <param name="trafficking">The trafficking instance to use.</param>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	/// <param name="dealer">The dealer instance to use.</param>
	/// <param name="player">The player instance to use.</param>
	private static void InitializeMenus(ITrafficking trafficking, IServiceManager serviceManager, IProviderManager providerManager, IDealer dealer, IPlayer player)
	{
		ISideMenu leftSideMenu = PresentationFactory.CreateBuyMenu(serviceManager, providerManager, player.Inventory);
		ISideMenu rightSideMenu = PresentationFactory.CreateSellMenu(serviceManager, providerManager, dealer.Inventory);
		trafficking.SetMenus(leftSideMenu, rightSideMenu);
	}
}
