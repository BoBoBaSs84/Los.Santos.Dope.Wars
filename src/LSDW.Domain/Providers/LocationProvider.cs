using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The location provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="World"/> methods and properties.
/// </remarks>
internal sealed class LocationProvider : ILocationProvider
{
	public Vector3 GetNextPositionOnSidewalk(Vector3 position)
		=> World.GetSafeCoordForPed(position, true, 1);

	public Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied = false)
		=> World.GetNextPositionOnStreet(position, unoccupied);

	public string GetZoneDisplayName(Vector3 position)
		=> World.GetZoneDisplayName(position);

	public string GetZoneLocalizedName(Vector3 position)
		=> World.GetZoneLocalizedName(position);
}
