using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Constants;
using LSDW.Domain.Helpers;

namespace LSDW.Domain.Models.Base;

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
		CurrentTask = TaskType.NoTask;
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
	{
		CurrentTask = TaskType.NoTask;
		Name = name;
	}

	public TaskType CurrentTask { get; private set; }
	public bool Created => ped is not null;
	public bool IsDead => ped is null || ped.IsDead;
	public Vector3 Position { get; }
	public PedHash Hash { get; }
	public string Name { get; }

	public virtual void Attack(Ped ped)
	{
		if (ped is null || CurrentTask is TaskType.FightAgainst)
			return;

		ped.Task.FightAgainst(ped, -1);
		SetTaskType(TaskType.FightAgainst);
	}

	public virtual void Create(float healthValue = 100)
	{
		if (ped is not null)
			return;

		Model model = ScriptHookHelper.GetPedModel(Hash);
		ped = World.CreatePed(model, Position);
		ped.HealthFloat = healthValue;
		StandStill();
	}

	public virtual void Delete()
	{
		if (ped is null)
			return;

		ped.Delete();
	}

	public virtual void Flee()
	{
		if (ped is null || CurrentTask is TaskType.FleeFrom)
			return;

		ped.Task.FleeFrom(Position);
		ped.MarkAsNoLongerNeeded();
		SetTaskType(TaskType.FleeFrom);
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

	public void GuardPosition()
	{
		if (ped is null || CurrentTask is TaskType.Guard)
			return;

		ped.Task.GuardCurrentPosition();
		SetTaskType(TaskType.Guard);
	}

	public void TurnTo(Ped pedToTurnTo)
	{
		if (ped is null || CurrentTask is TaskType.TurnTo)
			return;

		ped.Task.TurnTo(pedToTurnTo);
		SetTaskType(TaskType.TurnTo);
	}

	public void SetMoney(int amount)
	{
		if (ped is null)
			return;

		ped.Money = amount;
	}

	public void StandStill(int duartion = -1)
	{
		if (ped is null || CurrentTask is TaskType.StandStill)
			return;

		ped.Task.StandStill(duartion);
		SetTaskType(TaskType.StandStill);
	}

	public void Update(float health = 100)
	{
		if (ped is null)
			return;

		ped.HealthFloat = health;
	}

	public void WanderAround()
	{
		if (ped is null || CurrentTask is TaskType.WanderAround)
			return;

		ped.Task.WanderAround();
		SetTaskType(TaskType.WanderAround);
	}

	public void WanderAround(float radius = 0)
	{
		if (ped is null || CurrentTask is TaskType.WanderAround)
			return;

		ped.Task.WanderAround(Position, radius);
		SetTaskType(TaskType.WanderAround);
	}

	public void Wait(int duration)
	{
		if (ped is null || CurrentTask is TaskType.Wait)
			return;

		ped.Task.Wait(duration);
		SetTaskType(TaskType.Wait);
	}

	/// <summary>
	/// Sets the current task of the pedestrian.
	/// </summary>
	/// <param name="type">The type to set.</param>
	private void SetTaskType(TaskType type)
		=> CurrentTask = type;
}
