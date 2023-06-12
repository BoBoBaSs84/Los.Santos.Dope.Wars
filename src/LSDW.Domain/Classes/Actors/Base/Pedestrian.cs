using GTA;
using GTA.Math;
using LSDW.Domain.Constants;
using LSDW.Domain.Helpers;
using LSDW.Domain.Interfaces.Actors;

namespace LSDW.Domain.Classes.Actors.Base;

/// <summary>
/// The pedestrian actor base class.
/// </summary>
/// <remarks>
/// Implements the members of the <see cref="IPedestrian"/> interface.
/// </remarks>
internal abstract class Pedestrian : IPedestrian
{
	private Ped? ped;

	/// <summary>
	/// Initializes a instance of the pedestrian class.
	/// </summary>
	/// <param name="position">The position of the pedestrian.</param>
	/// <param name="pedHash">The ped hash of the pedestrian.</param>
	protected Pedestrian(Vector3 position, PedHash pedHash)
	{
		Position = position;
		Hash = pedHash;
		Name = NameConstants.GetFullName();
	}

	/// <summary>
	/// Initializes a instance of the pedestrian class.
	/// </summary>
	/// <param name="position">The position of the pedestrian.</param>
	/// <param name="pedHash">The ped hash of the pedestrian.</param>
	/// <param name="name">The name of the pedestrian.</param>
	protected Pedestrian(Vector3 position, PedHash pedHash, string name) : this(position, pedHash)
		=> Name = name;

	public bool IsCreated => ped is not null;
	public Vector3 Position { get; }
	public PedHash Hash { get; }
	public string Name { get; }

	public void Attack(Ped ped)
	{
		if (ped is null)
			return;

		ped.Task.FightAgainst(ped, -1);
	}

	public virtual void Create(float healthValue = 100)
	{
		if (ped is null)
			return;

		Model model = ScriptHookHelper.RequestModel(Hash);
		ped = World.CreatePed(model, Position);
		ped.HealthFloat = healthValue;
	}

	public virtual void Delete()
	{
		if (ped is null)
			return;

		ped.Delete();
	}

	public virtual void Flee()
	{
		if (ped is null)
			return;

		ped.Task.FleeFrom(Position);
		ped.MarkAsNoLongerNeeded();
	}

	public void GiveArmor(float armorValue)
	{
		if (ped is null)
			return;

		ped.ArmorFloat = armorValue;
	}

	public void GiveWeapon(WeaponHash weaponHash, int ammo = 0)
	{
		if (ped is null)
			return;

		_ = ped.Weapons.Give(weaponHash, ammo, true, true);
	}

	public void SetMoney(int amount)
	{
		if (ped is null)
			return;

		ped.Money = amount;
	}

	public void Update(float health = 100)
	{
		if (ped is null)
			return;

		ped.HealthFloat = health;
	}
}
