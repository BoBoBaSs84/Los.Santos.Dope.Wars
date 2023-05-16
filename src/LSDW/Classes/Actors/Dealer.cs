using GTA;
using GTA.Math;
using LSDW.Classes.Actors.Base;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using LSDW.Helpers;
using LSDW.Interfaces.Actors;

namespace LSDW.Classes.Actors;

/// <summary>
/// The dealer class.
/// </summary>
internal sealed class Dealer : Pedestrian, IDealer
{
	private const int ClosingHours = 48;

	/// <summary>
	/// Initializes a instance of the dealer class.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	public Dealer(Vector3 position) : base(position)
	{
		Inventory = InventoryFactory.CreateInventory();
		Discovered = false;
	}

	/// <summary>
	/// Initializes a instance of the dealer class.
	/// </summary>
	/// <param name="position">The position of the dealer.</param>
	/// <param name="closedUntil">The dealer is gone until this date time.</param>
	/// <param name="discovered">Has the dealer already been discovered?</param>
	/// <param name="inventory">The dealer inventory.</param>
	/// <param name="name">The name of the dealer.</param>
	public Dealer(Vector3 position, DateTime? closedUntil, bool discovered, IInventory inventory, string name) : base(position, name)
	{
		ClosedUntil = closedUntil;
		Discovered = discovered;
		Inventory = inventory;
	}

	public Blip? Blip { get; private set; }
	public DateTime? ClosedUntil { get; private set; }
	public bool Discovered { get; private set; }
	public IInventory Inventory { get; }

	public void CreateBlip(BlipSprite sprite = BlipSprite.Drugs, BlipColor color = BlipColor.White)
	{
		if (Blip is not null)
			return;

		Blip = World.CreateBlip(Position);
		Blip.Sprite = sprite;
		Blip.Scale = 0.8f;
		Blip.Color = color;
		Blip.Name = Name;
	}

	public void DeleteBlip()
		=> Blip?.Delete();

	public void Flee()
	{
		if (!Created)
			return;

		Ped?.Task.FleeFrom(Position);
		ClosedUntil = ScriptHookHelper.GetCurrentDateTime().AddHours(ClosingHours);
	}

	public void Update(WeaponHash weaponHash, int ammo)
	{
		if (!Created)
			return;
		_ = Ped?.Weapons.Give(weaponHash, ammo, true, true);
	}
}
