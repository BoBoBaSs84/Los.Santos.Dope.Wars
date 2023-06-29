using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Constants;
using LSDW.Domain.Helpers;
using System.Security.Cryptography.X509Certificates;

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
		CurrentTask = TaskType.NOTASK;
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
		CurrentTask = TaskType.NOTASK;
		Name = name;
	}

	public TaskType CurrentTask { get; private set; }
	public bool Created => ped is not null;
	public bool IsDead => ped is not null && ped.IsDead;
	public Vector3 Position { get; }
	public PedHash Hash { get; }
	public string Name { get; }

	public virtual void Attack(Ped ped)
	{
		if (ped is null || CurrentTask is TaskType.FIGHT)
			return;

		ped.Task.FightAgainst(ped, -1);
		SetTaskType(TaskType.FIGHT);
	}

	public virtual void Create(IWorldProvider worldProvider, int health = 100)
	{
		if (ped is not null)
			return;

		Model model = ScriptHookHelper.GetPedModel(Hash);
		ped = worldProvider.CreatePed(model, Position);
		ped.Health = health;
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
		if (ped is null || CurrentTask is TaskType.FLEE)
			return;

		ped.Task.FleeFrom(Position);
		ped.MarkAsNoLongerNeeded();
		SetTaskType(TaskType.FLEE);
	}

	public void GiveArmor(int armor)
	{
		if (ped is null)
			return;

		ped.Armor = armor;
	}

	public void GiveWeapon(WeaponHash weaponHash, int ammo = 0)
	{
		if (ped is null)
			return;

		_ = ped.Weapons.Give(weaponHash, ammo, true, true);
		ped.CanSwitchWeapons = true;
	}

	public void GuardPosition()
	{
		if (ped is null || CurrentTask is TaskType.GUARD)
			return;

		ped.Task.GuardCurrentPosition();
		SetTaskType(TaskType.GUARD);
	}

	public void Idle()
	{
		if (ped is null || CurrentTask is TaskType.IDLE)
			return;

		TaskSequence taskSequence = new();
		taskSequence.AddTask.UseMobilePhone(20000);
		taskSequence.AddTask.PutAwayMobilePhone();
		taskSequence.AddTask.Wait(20000);
		taskSequence.AddTask.WanderAround(Position, 0.5f);
		taskSequence.Close(true);

		using (taskSequence)
		{
			ped.Task.PerformSequence(taskSequence);
		}
	}

	public void TurnTo(Ped entity, int duration = -1)
	{
		if (ped is null || CurrentTask is TaskType.TURNTO)
			return;

		ped.Task.TurnTo(entity, duration);
		SetTaskType(TaskType.TURNTO);
	}

	public void SetMoney(int amount)
	{
		if (ped is null)
			return;

		ped.Money = amount;
	}

	public void StandStill(int duartion = -1)
	{
		if (ped is null || CurrentTask is TaskType.STAND)
			return;

		ped.Task.StandStill(duartion);
		SetTaskType(TaskType.STAND);
	}

	public void Update(int health = 100)
	{
		if (ped is null)
			return;

		ped.Health = health;
	}

	public void WanderAround()
	{
		if (ped is null || CurrentTask is TaskType.WANDER)
			return;

		ped.Task.WanderAround();
		SetTaskType(TaskType.WANDER);
	}

	public void WanderAround(float radius = 0)
	{
		if (ped is null || CurrentTask is TaskType.WANDER)
			return;

		ped.Task.WanderAround(Position, radius);
		SetTaskType(TaskType.WANDER);
	}

	public void Wait(int duration)
	{
		if (ped is null || CurrentTask is TaskType.WAIT)
			return;

		ped.Task.Wait(duration);
		SetTaskType(TaskType.WAIT);
	}

	/// <summary>
	/// Sets the current task of the pedestrian.
	/// </summary>
	/// <param name="type">The type to set.</param>
	private void SetTaskType(TaskType type)
		=> CurrentTask = type;
}
