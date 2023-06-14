using GTA;

namespace LSDW.Abstractions.Domain.Models;

/// <summary>
/// The dealer interface.
/// </summary>
public interface IDealer : IPedestrian
{
	/// <summary>
	/// The dealer is gone until this date time.
	/// </summary>
	DateTime? ClosedUntil { get; }

	/// <summary>
	/// When was the inventory prices the last time refreshed?
	/// </summary>
	DateTime LastRefresh { get; }

	/// <summary>
	/// When was the inventory the last time restocked?
	/// </summary>
	DateTime LastRestock { get; }

	/// <summary>
	/// Has the dealer already been discovered?
	/// </summary>
	bool Discovered { get; }

	/// <summary>
	/// Is the blip created?
	/// </summary>
	bool IsBlipCreated { get; }

	/// <summary>
	/// The dealer inventory.
	/// </summary>
	IInventory Inventory { get; }

	/// <summary>
	/// Creates the blip on the map.
	/// </summary>
	/// <param name="sprite"></param>
	/// <param name="color"></param>
	void CreateBlip(BlipSprite sprite = BlipSprite.Drugs, BlipColor color = BlipColor.White);

	/// <summary>
	/// Deletes the blip on the map.
	/// </summary>
	void DeleteBlip();

	/// <summary>
	/// Set the closed or not state.
	/// </summary>
	/// <param name="value">The value for closed, <see langword="null"/> for open again.</param>
	void SetClosed(DateTime? value);

	/// <summary>
	/// Sets if the dealer is discovered or not, maybe good for rediscovering too.
	/// </summary>
	/// <param name="value"><see langword="true"/> or <see langword="false"/></param>
	void SetDiscovered(bool value);

	/// <summary>
	/// Sets the last refresh date time.
	/// </summary>
	/// <param name="value">The date time to set.</param>
	void SetLastRefresh(DateTime value);

	/// <summary>
	/// Sets the last restock date time. 
	/// </summary>
	/// <param name="value">The date time to set.</param>
	void SetLastRestock(DateTime value);
}
