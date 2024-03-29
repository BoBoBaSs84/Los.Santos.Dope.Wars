﻿using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Domain.Models.Base;

/// <summary>
/// The pedestrian base interface.
/// </summary>
public interface IPedestrianBase
{
	/// <summary>
	/// The current task of the pedestrian.
	/// </summary>
	TaskType CurrentTask { get; }

	/// <summary>
	/// Is the pedestrian created?
	/// </summary>
	bool Created { get; }

	/// <summary>
	/// Is the pedestrian dead?
	/// </summary>
	bool IsDead { get; }

	/// <summary>
	/// The name of the pedestrian.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The current position of the pedestrian.
	/// </summary>
	Vector3 Position { get; }

	/// <summary>
	/// The spawn position of the pedestrian.
	/// </summary>
	Vector3 SpawnPosition { get; }

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
	/// <param name="worldProvider">The world provider insatnce to use.</param>
	/// <param name="health">The health points to give.</param>
	void Create(IWorldProvider worldProvider, int health = 100);

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
	/// <param name="armor">The armor value.</param>
	void GiveArmor(int armor);

	/// <summary>
	/// Gives the pedestrian a weapon.
	/// </summary>
	/// <param name="weaponHash">The weapon hash to use.</param>
	/// <param name="ammo">The amount of ammo to give.</param>
	void GiveWeapon(WeaponHash weaponHash, int ammo = 0);

	/// <summary>
	/// Lets the pedestrian guard the current position.
	/// </summary>
	void GuardPosition();

	/// <summary>
	/// Lets the pedestrian stand still.
	/// </summary>
	/// <param name="duration">The duration in milliseconds.</param>
	void StandStill(int duration = -1);

	/// <summary>
	/// Turns to and looks at the provided ped.
	/// </summary>
	/// <param name="entity">The ped to look at.</param>
	/// <param name="duration">The duration in milliseconds.</param>
	void TurnTo(Ped entity, int duration = -1);

	/// <summary>
	/// Give the pedestrian the amount of money.
	/// </summary>
	/// <param name="amount">The amount of money to give.</param>
	void SetMoney(int amount);

	/// <summary>
	/// Updates the pedestrian.
	/// </summary>
	/// <param name="health">The health points to give.</param>
	void Update(int health = 100);

	/// <summary>
	/// Lets the pedestrian wander around.
	/// </summary>
	void WanderAround();

	/// <summary>
	/// Lets the pedestrian wander around.
	/// </summary>
	/// <param name="radius">The radius to wander around.</param>
	void WanderAround(float radius = 5);

	/// <summary>
	/// Lets the pedestrian wait.
	/// </summary>
	/// <param name="duration">The duration in milliseconds.</param>
	void Wait(int duration = -1);
}
