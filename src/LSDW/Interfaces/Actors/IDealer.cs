using GTA;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Interfaces.Actors;

/// <summary>
/// The dealer interface.
/// </summary>
public interface IDealer : IPedestrian
{
	/// <summary>
	/// The blip on the map for the dealer.
	/// </summary>
	Blip? Blip { get; }

	/// <summary>
	/// The dealer is gone until this date time.
	/// </summary>
	DateTime? ClosedUntil { get; }

	/// <summary>
	/// Has the dealer already been discovered?
	/// </summary>
	bool Discovered { get; }

	/// <summary>
	/// The dealer inventory.
	/// </summary>
	IInventory Inventory { get; }

	/// <summary>
	/// The dealer needs to flee.
	/// </summary>
	void Flee();

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
	/// Updates the dealer by giving him a weapon.
	/// </summary>
	/// <param name="weaponHash">The weapon to give.</param>
	/// <param name="ammo">The amount of ammo to give.</param>
	void Update(WeaponHash weaponHash, int ammo);
}
