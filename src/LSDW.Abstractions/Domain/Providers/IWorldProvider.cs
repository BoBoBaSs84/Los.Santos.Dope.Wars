using GTA;
using GTA.Math;

namespace LSDW.Abstractions.Domain.Providers;

/// <summary>
/// The world provider interface.
/// </summary>
public interface IWorldProvider
{
	/// <summary>
	/// Gets or sets the current date and time in the <see cref="World"/>.
	/// </summary>
	DateTime Now { get; set; }

	/// <summary>
	/// Gets or sets the current time of day in the <see cref="World"/>.
	/// </summary>
	TimeSpan TimeOfDay { get; set; }

	/// <summary>
	/// Creates a <see cref="Blip"/> at the given position on the map.
	/// </summary>
	/// <param name="position">The position of the blip on the map.</param>
	Blip CreateBlip(Vector3 position);

	/// <summary>
	/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
	/// </summary>
	/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
	/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
	/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
	/// <returns></returns>
	Ped CreatePed(Model model, Vector3 position, float heading = 0f);

	/// <summary>
	/// Gets the closest <see cref="Ped"/> to a given position in the <see cref="World"/>.
	/// </summary>
	/// <param name="position">The position to find the nearest <see cref="Ped"/>.</param>
	/// <param name="radius">The maximum distance from the position to detect <see cref="Ped"/>.</param>
	/// <param name="models">The <see cref="Model"/> of <see cref="Ped"/> to get, leave blank for all <see cref="Ped"/> models.</param>
	Ped GetClosestPed(Vector3 position, float radius, params Model[] models);

	/// <summary>
	/// Gets an array of all peds in the <see cref="World"/>.
	/// </summary>
	/// <param name="models">The <see cref="Model"/> of peds to get, leave blank for all <see cref="Ped"/> models.</param>
	Ped[] GetAllPeds(params Model[] models);

	/// <summary>
	/// Gets an array of all <see cref="Ped"/> near a given <see cref="Model"/> in the <see cref="World"/>.
	/// </summary>
	/// <param name="ped">The ped to check.</param>
	/// <param name="radius">The maximun distance from the ped to detect peds.</param>
	/// <param name="models">The <see cref="Model"/> of peds to get, leave blank for all <see cref="Ped"/> models.</param>
	Ped[] GetNearbyPeds(Ped ped, float radius, params Model[] models);

	/// <summary>
	/// Gets an array of all <see cref="Ped"/> in a given region in the <see cref="World"/>.
	/// </summary>
	/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
	/// <param name="radius">The maximun distance from the position to detect peds.</param>
	/// <param name="models">The <see cref="Model"/> of peds to get, leave blank for all <see cref="Ped"/> models.</param>
	Ped[] GetNearbyPeds(Vector3 position, float radius, params Model[] models);

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
