using GTA;
using GTA.Math;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The character provider interface.
/// </summary>
public interface IPlayerProvider
{
	/// <summary>
	/// Gets the <see cref="Ped"/> this <see cref="Player"/> is controlling.
	/// </summary>
	Ped Character { get; }
	
	/// <summary>
	/// Gets or sets how much money this <see cref="Player"/> has.
	/// </summary>
	int Money { get; set; }

	/// <summary>
	/// Gets or sets the wanted level for this <see cref="Player"/>.
	/// </summary>
	int WantedLevel { get; set; }

	/// <summary>
	/// Sets a value indicating whether cops will be dispatched for this <see cref="Player"/>
	/// </summary>
	bool DispatchsCops { set; }

	/// <summary>
	/// Gets a value indicating whether this <see cref="Player"/> can start a mission.
	/// </summary>
	bool CanStartMission { get; }

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Player"/> can control its <see cref="Ped"/>.
	/// </summary>
	bool CanControlCharacter { get; set; }

	/// <summary>
	/// Gets or sets the position of the <see cref="Player"/> current <see cref="Entity"/>.
	/// </summary>
	Vector3 Position { get; set; }

	/// <summary>
	/// Gets a value indicating whether this <see cref="Player"/> is dead.
	/// </summary>
	bool IsDead { get; }

	/// <summary>
	/// Determines whether the <see cref="Player"/> is in range of a specified position.
	/// </summary>
	/// <param name="position">The position to check.</param>
	/// <param name="range">The maximum range.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="Player"/> is in range of the <paramref name="position"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	bool IsInRange(Vector3 position, float range);

	/// <summary>
	/// Determines whether the <see cref="Player"/> is near a specified <see cref="Entity"/>.
	/// </summary>
	/// <param name="entity">The <see cref="Entity"/> to check.</param>
	/// <param name="bounds">The max displacement from the entity.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="Player"/> is near the specified <paramref name="entity"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	bool IsNearEntity(Entity entity, Vector3 bounds);

	/// <summary>
	/// Determines whether this <see cref="Player"/> is targeting the specified <see cref="Entity"/>.
	/// </summary>
	/// <param name="entity">The <see cref="Entity"/> to check.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="Player"/> is targeting the specified <paramref name="entity"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	bool IsTargeting(Entity entity);
}
