using GTA;
using LSDW.Abstractions.Domain.Models.Base;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Abstractions.Domain.Models;

/// <summary>
/// The dealer interface.
/// </summary>
public interface IDealer : IPedestrianBase
{
	/// <summary>
	/// The dealer is gone until this date time.
	/// </summary>
	DateTime? ClosedUntil { get; set; }

	/// <summary>
	/// When does only the inventory prices change the next time?
	/// </summary>
	DateTime NextPriceChange { get; set; }

	/// <summary>
	/// When does the whole inventory changes itself next time?
	/// </summary>
	DateTime NextInventoryChange { get; set; }

	/// <summary>
	/// Is the dealer closed for business?
	/// </summary>
	bool Closed { get; }

	/// <summary>
	/// Has the dealer already been discovered?
	/// </summary>
	bool Discovered { get; set; }

	/// <summary>
	/// Is the blip created?
	/// </summary>
	bool BlipCreated { get; }

	/// <summary>
	/// The dealer inventory.
	/// </summary>
	IInventory Inventory { get; }

	/// <summary>
	/// Creates the blip on the map.
	/// </summary>
	/// <param name="worldProvider">The world provider instance to use.</param>
	/// <param name="sprite">The sprite to use for the blip.</param>
	/// <param name="color">The color tu use for the blip.</param>
	void CreateBlip(IWorldProvider worldProvider, BlipSprite sprite = BlipSprite.Drugs, BlipColor color = BlipColor.White);

	/// <summary>
	/// Deletes the blip on the map.
	/// </summary>
	void DeleteBlip();
}
