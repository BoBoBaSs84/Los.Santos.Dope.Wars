using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Domain.Providers;

/// <summary>
/// The world provider class.
/// </summary>
/// <remarks>
/// Wrapper for the <see cref="World"/> methods and properties.
/// </remarks>
internal sealed class WorldProvider : IWorldProvider
{
	/// <summary>
	/// The world provider singleton instance.
	/// </summary>
	public static WorldProvider Instance => new();

	public DateTime Now
	{
		get => World.CurrentDate;
		set => World.CurrentDate = value;
	}

	public TimeSpan TimeOfDay
	{
		get => World.CurrentTimeOfDay;
		set => World.CurrentTimeOfDay = value;
	}

	public Blip CreateBlip(Vector3 position)
		=> World.CreateBlip(position);

	public Ped CreatePed(Model model, Vector3 position, float heading = 0f)
		=> World.CreatePed(model, position, heading);

	public Ped[] GetAllPeds(params Model[] models)
		=> World.GetAllPeds(models);

	public Ped GetClosestPed(Vector3 position, float radius, params Model[] models)
		=> World.GetClosestPed(position, radius, models);

	public Ped[] GetNearbyPeds(Ped ped, float radius, params Model[] models)
		=> World.GetNearbyPeds(ped, radius, models);

	public Ped[] GetNearbyPeds(Vector3 position, float radius, params Model[] models)
		=> World.GetNearbyPeds(position, radius, models);
	
	public Vector3 GetNextPositionOnSidewalk(Vector3 position)
		=> World.GetSafeCoordForPed(position, true, 1);

	public Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied = false)
		=> World.GetNextPositionOnStreet(position, unoccupied);

	public string GetZoneDisplayName(Vector3 position)
		=> World.GetZoneDisplayName(position);

	public string GetZoneLocalizedName(Vector3 position)
		=> World.GetZoneLocalizedName(position);
}
