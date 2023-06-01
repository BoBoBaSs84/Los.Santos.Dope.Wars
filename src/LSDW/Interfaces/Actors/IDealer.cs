using GTA;
using LSDW.Core.Interfaces.Models;

namespace LSDW.Interfaces.Actors;

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
	/// <param name="closedUntil">Value for closed or <see langword="null"/> for open again.</param>
	void SetClosed(DateTime? closedUntil);
}
