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
	/// <summary>
	/// Initializes a instance of the location provider class.
	/// </summary>
	public LocationProvider()
		=> PlayerPosition = Game.Player.Character.Position;

	public Vector3 PlayerPosition { get; }

	public Vector3 GetNextPositionOnSidewalk(Vector3 position)
		=> World.GetNextPositionOnSidewalk(position);

	public string GetZoneDisplayName(Vector3 position)
		=> World.GetZoneDisplayName(position);

	public string GetZoneLocalizedName(Vector3 position)
		=> World.GetZoneLocalizedName(position);
}
