using GTA;
using GTA.Math;

namespace LSDW.Application.Abstractions.Application.Providers;

/// <summary>
/// Represents an abstraction contract for the <see cref="Player"/> class.
/// </summary>
public interface IPlayerProvider
{
	/// <inheritdoc cref="Player.Handle"/>
	int Handle { get; }

	/// <inheritdoc cref="Player.NativeValue"/>
	ulong NativeValue { get; set; }

	/// <inheritdoc cref="Player.Character"/>
	Ped Character { get; }

	/// <inheritdoc cref="Player.Name"/>
	string Name { get; }

	/// <inheritdoc cref="Player.Money"/>
	int Money { get; set; }

	/// <inheritdoc cref="Player.WantedLevel"/>
	int WantedLevel { get; set; }

	/// <inheritdoc cref="Player.WantedCenterPosition"/>
	Vector3 WantedCenterPosition { get; set; }

	/// <inheritdoc cref="Player.MaxArmor"/>
	int MaxArmor { get; set; }

	/// <inheritdoc cref="Player.PrimaryParachuteTint"/>
	ParachuteTint PrimaryParachuteTint { get; set; }

	/// <inheritdoc cref="Player.ReserveParachuteTint"/>
	ParachuteTint ReserveParachuteTint { get; set; }

	/// <inheritdoc cref="Player.CanLeaveParachuteSmokeTrail"/>
	bool CanLeaveParachuteSmokeTrail { set; }

	/// <inheritdoc cref="Player.ParachuteSmokeTrailColor"/>
	Color ParachuteSmokeTrailColor { get; set; }

	/// <inheritdoc cref="Player.IsDead"/>
	bool IsDead { get; }

	/// <inheritdoc cref="Player.IsAlive"/>
	bool IsAlive { get; }

	/// <inheritdoc cref="Player.IsAiming"/>
	bool IsAiming { get; }

	/// <inheritdoc cref="Player.IsClimbing"/>
	bool IsClimbing { get; }

	/// <inheritdoc cref="Player.IsRidingTrain"/>
	bool IsRidingTrain { get; }

	/// <inheritdoc cref="Player.IsPressingHorn"/>
	bool IsPressingHorn { get; }

	/// <inheritdoc cref="Player.IsPlaying"/>
	bool IsPlaying { get; }

	/// <inheritdoc cref="Player.IsInvincible"/>
	bool IsInvincible { get; set; }

	/// <inheritdoc cref="Player.IgnoredByPolice"/>
	bool IgnoredByPolice { set; }

	/// <inheritdoc cref="Player.IgnoredByEveryone"/>
	bool IgnoredByEveryone { set; }

	/// <inheritdoc cref="Player.DispatchsCops"/>
	bool DispatchsCops { set; }

	/// <inheritdoc cref="Player.CanUseCover"/>
	bool CanUseCover { set; }

	/// <inheritdoc cref="Player.CanStartMission"/>
	bool CanStartMission { get; }

	/// <inheritdoc cref="Player.CanControlRagdoll"/>
	bool CanControlRagdoll { set; }

	/// <inheritdoc cref="Player.CanControlCharacter"/>
	bool CanControlCharacter { get; set; }

	/// <inheritdoc cref="Player.RemainingSprintTime"/>
	float RemainingSprintTime { get; }

	/// <inheritdoc cref="Player.RemainingSprintStamina"/>
	float RemainingSprintStamina { get; }

	/// <inheritdoc cref="Player.RemainingUnderwaterTime"/>
	float RemainingUnderwaterTime { get; }

	/// <inheritdoc cref="Player.IsSpecialAbilityActive"/>
	bool IsSpecialAbilityActive { get; }

	/// <inheritdoc cref="Player.IsSpecialAbilityEnabled"/>
	bool IsSpecialAbilityEnabled { get; set; }

	/// <inheritdoc cref="Player.LastVehicle"/>
	Vehicle LastVehicle { get; }

	/// <inheritdoc cref="Player.IsTargetingAnything"/>
	bool IsTargetingAnything { get; }

	/// <inheritdoc cref="Player.TargetedEntity"/>
	Entity TargetedEntity { get; }

	/// <inheritdoc cref="Player.LockedOnEntity"/>
	Entity LockedOnEntity { get; }

	/// <inheritdoc cref="Player.ForcedAim"/>
	bool ForcedAim { set; }

	/// <inheritdoc cref="Player.ChangeModel(Model)"/>
	bool ChangeModel(Model model);

	/// <inheritdoc cref="Player.ChargeSpecialAbility(int)"/>
	void ChargeSpecialAbility(int absoluteAmount);

	/// <inheritdoc cref="Player.ChargeSpecialAbility(float)"/>
	void ChargeSpecialAbility(float normalizedRatio);

	/// <inheritdoc cref="Player.RefillSpecialAbility()"/>
	void RefillSpecialAbility();

	/// <inheritdoc cref="Player.DepleteSpecialAbility()"/>
	void DepleteSpecialAbility();

	/// <inheritdoc cref="Player.IsTargeting(Entity)"/>
	bool IsTargeting(Entity entity);

	/// <inheritdoc cref="Player.DisableFiringThisFrame()"/>
	void DisableFiringThisFrame();

	/// <inheritdoc cref="Player.SetRunSpeedMultThisFrame(float)"/>
	void SetRunSpeedMultThisFrame(float mult);

	/// <inheritdoc cref="Player.SetSwimSpeedMultThisFrame(float)"/>
	void SetSwimSpeedMultThisFrame(float mult);

	/// <inheritdoc cref="Player.SetFireAmmoThisFrame()"/>
	void SetFireAmmoThisFrame();

	/// <inheritdoc cref="Player.SetExplosiveAmmoThisFrame()"/>
	void SetExplosiveAmmoThisFrame();

	/// <inheritdoc cref="Player.SetExplosiveMeleeThisFrame()"/>
	void SetExplosiveMeleeThisFrame();

	/// <inheritdoc cref="Player.SetSuperJumpThisFrame()"/>
	void SetSuperJumpThisFrame();

	/// <inheritdoc cref="Player.SetMayNotEnterAnyVehicleThisFrame()"/>
	void SetMayNotEnterAnyVehicleThisFrame();

	/// <inheritdoc cref="Player.SetMayOnlyEnterThisVehicleThisFrame(Vehicle)"/>
	void SetMayOnlyEnterThisVehicleThisFrame(Vehicle vehicle);
}
