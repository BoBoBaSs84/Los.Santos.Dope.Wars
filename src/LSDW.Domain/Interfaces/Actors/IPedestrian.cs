using GTA;
using GTA.Math;

namespace LSDW.Domain.Interfaces.Actors;

/// <summary>
/// The pedestrian interface.
/// </summary>
public interface IPedestrian
{
	/// <summary>
	/// Is the pedestrian created?
	/// </summary>
	bool IsCreated { get; }

	/// <summary>
	/// The name of the pedestrian.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The position of the pedestrian.
	/// </summary>
	Vector3 Position { get; }

	/// <summary>
	/// The peds hash.
	/// </summary>
	PedHash Hash { get; }

	/// <summary>
	/// Attacks the target.
	/// </summary>
	/// <param name="ped">The ped to attack.</param>
	void Attack(Ped ped);

	/// <summary>
	/// Creates the pedestrian.
	/// </summary>
	/// <param name="healthValue">The health points to give.</param>
	void Create(float healthValue = 100f);

	/// <summary>
	/// Deletes the pedestrian.
	/// </summary>
	void Delete();

	/// <summary>
	/// Lets pedestrian the pedestrian flee from the current position.
	/// </summary>
	void Flee();

	/// <summary>
	/// Gives the pedestrian an protective armor.
	/// </summary>
	/// <param name="armorValue">The armor value.</param>
	void GiveArmor(float armorValue);

	/// <summary>
	/// Gives the pedestrian a weapon.
	/// </summary>
	/// <param name="weaponHash">The weapon hash to use.</param>
	/// <param name="ammo">The amount of ammo to give.</param>
	void GiveWeapon(WeaponHash weaponHash, int ammo = 0);

	/// <summary>
	/// Give the pedestrian the amount of money.
	/// </summary>
	/// <param name="amount">The amount of money to give.</param>
	void SetMoney(int amount);

	/// <summary>
	/// Updates the pedestrian.
	/// </summary>
	/// <param name="healthValue">The health points to give.</param>
	void Update(float healthValue = 100f);
}
