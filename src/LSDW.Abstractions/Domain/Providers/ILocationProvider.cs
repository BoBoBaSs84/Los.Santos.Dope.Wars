using GTA;
using GTA.Math;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The location provider interface.
/// </summary>
public interface ILocationProvider
{
	/// <summary>
	/// Gets the position of the player character.
	/// </summary>
	Vector3 PlayerPosition { get; }

	/// <summary>
	/// Gets the next position on the street where a <see cref="Vehicle"/> can be placed.
	/// </summary>
	/// <param name="position">The position to check around.</param>
	/// <param name="unoccupied">If set to true only find positions that dont already have a vehicle in them.</param>
	Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied = false);

	/// <summary>
	/// Gets the next position on the street where a <see cref="Ped"/> can be placed.
	/// </summary>
	/// <param name="position">The position to check around.</param>
	Vector3 GetNextPositionOnSidewalk(Vector3 position);

	/// <summary>
	/// Gets the display name of the a zone in the map.
	/// </summary>
	/// <param name="position">The position on the map.</param>
	string GetZoneDisplayName(Vector3 position);

	/// <summary>
	/// Gets the localized name of the a zone in the map.
	/// </summary>
	/// <param name="position">The position on the map.</param>
	string GetZoneLocalizedName(Vector3 position);
}
