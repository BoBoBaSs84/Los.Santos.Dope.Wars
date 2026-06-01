using LSDW.Application.Abstractions.Application.Providers;

using GTA;
using GTA.Math;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents the implementation of the <see cref="IPlayerProvider"/> interface.
/// </summary>
internal sealed class PlayerProvider : IPlayerProvider
{
	public int Handle
	{
		get => Game.Player.Handle;
	}
	public ulong NativeValue
	{
		get => Game.Player.NativeValue;
		set => Game.Player.NativeValue = value;
	}
	public Ped Character
	{
		get => Game.Player.Character;
	}
	public string Name
	{
		get => Game.Player.Name;
	}
	public int Money
	{
		get => Game.Player.Money;
		set => Game.Player.Money = value;
	}
	public int WantedLevel
	{
		get => Game.Player.WantedLevel;
		set => Game.Player.WantedLevel = value;
	}
	public Vector3 WantedCenterPosition
	{
		get => Game.Player.WantedCenterPosition;
		set => Game.Player.WantedCenterPosition = value;
	}
	public int MaxArmor
	{
		get => Game.Player.MaxArmor;
		set => Game.Player.MaxArmor = value;
	}
	public ParachuteTint PrimaryParachuteTint
	{
		get => Game.Player.PrimaryParachuteTint;
		set => Game.Player.PrimaryParachuteTint = value;
	}
	public ParachuteTint ReserveParachuteTint
	{
		get => Game.Player.ReserveParachuteTint;
		set => Game.Player.ReserveParachuteTint = value;
	}
	public bool CanLeaveParachuteSmokeTrail
	{
		set => Game.Player.CanLeaveParachuteSmokeTrail = value;
	}
	public Color ParachuteSmokeTrailColor
	{
		get => Game.Player.ParachuteSmokeTrailColor;
		set => Game.Player.ParachuteSmokeTrailColor = value;
	}
	public bool IsDead
	{
		get => Game.Player.IsDead;
	}
	public bool IsAlive
	{
		get => Game.Player.IsAlive;
	}
	public bool IsAiming
	{
		get => Game.Player.IsAiming;
	}
	public bool IsClimbing
	{
		get => Game.Player.IsClimbing;
	}
	public bool IsRidingTrain
	{
		get => Game.Player.IsRidingTrain;
	}
	public bool IsPressingHorn
	{
		get => Game.Player.IsPressingHorn;
	}
	public bool IsPlaying
	{
		get => Game.Player.IsPlaying;
	}
	public bool IsInvincible
	{
		get => Game.Player.IsInvincible;
		set => Game.Player.IsInvincible = value;
	}
	public bool IgnoredByPolice
	{
		set => Game.Player.IgnoredByPolice = value;
	}
	public bool IgnoredByEveryone
	{
		set => Game.Player.IgnoredByEveryone = value;
	}
	public bool DispatchsCops
	{
		set => Game.Player.DispatchsCops = value;
	}
	public bool CanUseCover
	{
		set => Game.Player.CanUseCover = value;
	}
	public bool CanStartMission
	{
		get => Game.Player.CanStartMission;
	}
	public bool CanControlRagdoll
	{
		set => Game.Player.CanControlRagdoll = value;
	}
	public bool CanControlCharacter
	{
		get => Game.Player.CanControlCharacter;
		set => Game.Player.CanControlCharacter = value;
	}
	public float RemainingSprintTime
	{
		get => Game.Player.RemainingSprintTime;
	}
	public float RemainingSprintStamina
	{
		get => Game.Player.RemainingSprintStamina;
	}
	public float RemainingUnderwaterTime
	{
		get => Game.Player.RemainingUnderwaterTime;
	}
	public bool IsSpecialAbilityActive
	{
		get => Game.Player.IsSpecialAbilityActive;
	}
	public bool IsSpecialAbilityEnabled
	{
		get => Game.Player.IsSpecialAbilityEnabled;
		set => Game.Player.IsSpecialAbilityEnabled = value;
	}
	public Vehicle LastVehicle
	{
		get => Game.Player.LastVehicle;
	}
	public bool IsTargetingAnything
	{
		get => Game.Player.IsTargetingAnything;
	}
	public Entity TargetedEntity
	{
		get => Game.Player.TargetedEntity;
	}
	public Entity LockedOnEntity
	{
		get => Game.Player.LockedOnEntity;
	}
	public bool ForcedAim
	{
		set => Game.Player.ForcedAim = value;
	}
	public bool ChangeModel(Model model) => Game.Player.ChangeModel(model);

	public void ChargeSpecialAbility(int absoluteAmount) => Game.Player.ChargeSpecialAbility(absoluteAmount);

	public void ChargeSpecialAbility(float normalizedRatio) => Game.Player.ChargeSpecialAbility(normalizedRatio);

	public void RefillSpecialAbility() => Game.Player.RefillSpecialAbility();

	public void DepleteSpecialAbility() => Game.Player.DepleteSpecialAbility();

	public bool IsTargeting(Entity entity) => Game.Player.IsTargeting(entity);

	public void DisableFiringThisFrame() => Game.Player.DisableFiringThisFrame();

	public void SetRunSpeedMultThisFrame(float mult) => Game.Player.SetRunSpeedMultThisFrame(mult);

	public void SetSwimSpeedMultThisFrame(float mult) => Game.Player.SetSwimSpeedMultThisFrame(mult);

	public void SetFireAmmoThisFrame() => Game.Player.SetFireAmmoThisFrame();

	public void SetExplosiveAmmoThisFrame() => Game.Player.SetExplosiveAmmoThisFrame();

	public void SetExplosiveMeleeThisFrame() => Game.Player.SetExplosiveMeleeThisFrame();

	public void SetSuperJumpThisFrame() => Game.Player.SetSuperJumpThisFrame();

	public void SetMayNotEnterAnyVehicleThisFrame() => Game.Player.SetMayNotEnterAnyVehicleThisFrame();

	public void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle) => Game.Player.SetMayOnlyEnterThisVehicleThisFrame(vehicle);
}
