using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The location provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="World"/> methods and <see cref="Game"/> properties.
/// </remarks>
internal sealed class LocationProvider : ILocationProvider
{
	public Vector3 PlayerPosition
		=> Game.Player.Character.Position;

	public Vector3 GetNextPositionOnSidewalk(Vector3 position)
		=> World.GetNextPositionOnSidewalk(position);
	
	public Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied = false)
		=> World.GetNextPositionOnStreet(position, unoccupied);

	public string GetZoneDisplayName(Vector3 position)
		=> World.GetZoneDisplayName(position);

	public string GetZoneLocalizedName(Vector3 position)
		=> World.GetZoneLocalizedName(position);
}
