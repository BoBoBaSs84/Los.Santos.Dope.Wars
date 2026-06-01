using LSDW.Application.Abstractions.Application.Providers;

using GTA;
using GTA.Math;

namespace LSDW.Application.Providers;

/// <summary>
/// Represents the implementation of the <see cref="IWorldProvider"/> interface.
/// </summary>
internal sealed class WorldProvider : IWorldProvider
{
	public bool IsClockPaused
	{
		get => World.IsClockPaused;
		set => World.IsClockPaused = value;
	}
	public DateTime CurrentDate
	{
		get => World.CurrentDate;
		set => World.CurrentDate = value;
	}
	public TimeSpan CurrentTimeOfDay
	{
		get => World.CurrentTimeOfDay;
		set => World.CurrentTimeOfDay = value;
	}
	public int MillisecondsPerGameMinute
	{
		get => World.MillisecondsPerGameMinute;
		set => World.MillisecondsPerGameMinute = value;
	}
	public bool Blackout
	{
		set => World.Blackout = value;
	}
	public Weather Weather
	{
		get => World.Weather;
		set => World.Weather = value;
	}
	public Weather NextWeather
	{
		get => World.NextWeather;
		set => World.NextWeather = value;
	}
	public float GravityLevel
	{
		get => World.GravityLevel;
		set => World.GravityLevel = value;
	}
	public Blip WaypointBlip
	{
		get => World.WaypointBlip;
	}
	public Vector3 WaypointPosition
	{
		get => World.WaypointPosition;
		set => World.WaypointPosition = value;
	}
	public int VehicleCount
	{
		get => World.VehicleCount;
	}
	public int PedCount
	{
		get => World.PedCount;
	}
	public int PropCount
	{
		get => World.PropCount;
	}
	public int PickupObjectCount
	{
		get => World.PickupObjectCount;
	}
	public int BuildingCount
	{
		get => World.BuildingCount;
	}
	public int AnimatedBuildingCount
	{
		get => World.AnimatedBuildingCount;
	}
	public int InteriorInstanceCount
	{
		get => World.InteriorInstanceCount;
	}
	public int InteriorProxyCount
	{
		get => World.InteriorProxyCount;
	}
	public int ProjectileCount
	{
		get => World.ProjectileCount;
	}
	public int EntityColliderCount
	{
		get => World.EntityColliderCount;
	}
	public int VehicleCapacity
	{
		get => World.VehicleCapacity;
	}
	public int PedCapacity
	{
		get => World.PedCapacity;
	}
	public int PropCapacity
	{
		get => World.PropCapacity;
	}
	public int PickupObjectCapacity
	{
		get => World.PickupObjectCapacity;
	}
	public int ProjectileCapacity
	{
		get => World.ProjectileCapacity;
	}
	public int BuildingCapacity
	{
		get => World.BuildingCapacity;
	}
	public int AnimatedBuildingCapacity
	{
		get => World.AnimatedBuildingCapacity;
	}
	public int InteriorInstanceCapacity
	{
		get => World.InteriorInstanceCapacity;
	}
	public int InteriorProxyCapacity
	{
		get => World.InteriorProxyCapacity;
	}
	public int EntityColliderCapacity
	{
		get => World.EntityColliderCapacity;
	}
	public Camera RenderingCamera
	{
		get => World.RenderingCamera;
		set => World.RenderingCamera = value;
	}
	public RaycastResult Raycast(Vector3 source, Vector3 target, IntersectFlags options, Entity ignoreEntity) => World.Raycast(source, target, options, ignoreEntity);

	public RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectFlags options, Entity ignoreEntity) => World.Raycast(source, direction, maxDistance, options, ignoreEntity);

	public RaycastResult GetCrosshairCoordinates() => World.GetCrosshairCoordinates();

	public RaycastResult GetCrosshairCoordinates(IntersectFlags intersectOptions, Entity ignoreEntity) => World.GetCrosshairCoordinates(intersectOptions, ignoreEntity);

	public float GetDistance(Vector3 origin, Vector3 destination) => World.GetDistance(origin, destination);

	public float CalculateTravelDistance(Vector3 origin, Vector3 destination) => World.CalculateTravelDistance(origin, destination);

	public float GetGroundHeight(Vector2 position) => World.GetGroundHeight(position);

	public float GetGroundHeight(Vector3 position) => World.GetGroundHeight(position);

	public Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk, int flags) => World.GetSafeCoordForPed(position, sidewalk, flags);

	public Vector3 GetNextPositionOnStreet(Vector2 position, bool unoccupied) => World.GetNextPositionOnStreet(position, unoccupied);

	public Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied) => World.GetNextPositionOnStreet(position, unoccupied);

	public Vector3 GetNextPositionOnSidewalk(Vector2 position) => World.GetNextPositionOnSidewalk(position);

	public Vector3 GetNextPositionOnSidewalk(Vector3 position) => World.GetNextPositionOnSidewalk(position);

	public string GetStreetName(Vector2 position) => World.GetStreetName(position);

	public string GetStreetName(Vector3 position) => World.GetStreetName(position);

	public string GetZoneDisplayName(Vector2 position) => World.GetZoneDisplayName(position);

	public string GetZoneDisplayName(Vector3 position) => World.GetZoneDisplayName(position);

	public string GetZoneLocalizedName(Vector2 position) => World.GetZoneLocalizedName(position);

	public string GetZoneLocalizedName(Vector3 position) => World.GetZoneLocalizedName(position);

	public bool IsPointInAngledArea(Vector3 point, Vector3 originEdge, Vector3 extentEdge, float width, bool includeZAxis) => World.IsPointInAngledArea(point, originEdge, extentEdge, width, includeZAxis);

	public void TransitionToWeather(Weather weather, float duration) => World.TransitionToWeather(weather, duration);

	public void RemoveWaypoint() => World.RemoveWaypoint();

	public Blip[] GetAllBlips(BlipSprite[] blipTypes) => World.GetAllBlips(blipTypes);

	public Blip[] GetNearbyBlips(Vector3 position, float radius, BlipSprite[] blipTypes) => World.GetNearbyBlips(position, radius, blipTypes);

	public Blip CreateBlip(Vector3 position) => World.CreateBlip(position);

	public Blip CreateBlip(Vector3 position, float radius) => World.CreateBlip(position, radius);

	public Ped GetClosestPed(Vector3 position, float radius, Model[] models) => World.GetClosestPed(position, radius, models);

	public Ped[] GetAllPeds(Model[] models) => World.GetAllPeds(models);

	public Ped[] GetNearbyPeds(Ped ped, float radius, Model[] models) => World.GetNearbyPeds(ped, radius, models);

	public Ped[] GetNearbyPeds(Vector3 position, float radius, Model[] models) => World.GetNearbyPeds(position, radius, models);

	public Vehicle GetClosestVehicle(Vector3 position, float radius, Model[] models) => World.GetClosestVehicle(position, radius, models);

	public Vehicle[] GetAllVehicles(Model[] models) => World.GetAllVehicles(models);

	public Vehicle[] GetNearbyVehicles(Ped ped, float radius, Model[] models) => World.GetNearbyVehicles(ped, radius, models);

	public Vehicle[] GetNearbyVehicles(Vector3 position, float radius, Model[] models) => World.GetNearbyVehicles(position, radius, models);

	public Prop GetClosestProp(Vector3 position, float radius, Model[] models) => World.GetClosestProp(position, radius, models);

	public Prop[] GetAllProps(Model[] models) => World.GetAllProps(models);

	public Prop[] GetNearbyProps(Vector3 position, float radius, Model[] models) => World.GetNearbyProps(position, radius, models);

	public Prop GetClosestPickupObject(Vector3 position, float radius) => World.GetClosestPickupObject(position, radius);

	public Prop[] GetAllPickupObjects() => World.GetAllPickupObjects();

	public Prop[] GetNearbyPickupObjects(Vector3 position, float radius) => World.GetNearbyPickupObjects(position, radius);

	public Projectile GetClosestProjectile(Vector3 position, float radius) => World.GetClosestProjectile(position, radius);

	public Projectile[] GetAllProjectiles() => World.GetAllProjectiles();

	public Projectile[] GetNearbyProjectiles(Vector3 position, float radius) => World.GetNearbyProjectiles(position, radius);

	public Entity[] GetAllEntities() => World.GetAllEntities();

	public Entity[] GetNearbyEntities(Vector3 position, float radius) => World.GetNearbyEntities(position, radius);

	public Building[] GetAllBuildings() => World.GetAllBuildings();

	public Building[] GetNearbyBuildings(Vector3 position, float radius) => World.GetNearbyBuildings(position, radius);

	public Building GetClosestBuilding(Vector3 position, float radius) => World.GetClosestBuilding(position, radius);

	public AnimatedBuilding[] GetAllAnimatedBuildings() => World.GetAllAnimatedBuildings();

	public AnimatedBuilding[] GetNearbyAnimatedBuildings(Vector3 position, float radius) => World.GetNearbyAnimatedBuildings(position, radius);

	public AnimatedBuilding GetClosestAnimatedBuilding(Vector3 position, float radius) => World.GetClosestAnimatedBuilding(position, radius);

	public InteriorInstance[] GetAllInteriorInstances() => World.GetAllInteriorInstances();

	public InteriorInstance[] GetNearbyInteriorInstances(Vector3 position, float radius) => World.GetNearbyInteriorInstances(position, radius);

	public InteriorInstance GetClosestInteriorInstance(Vector3 position, float radius) => World.GetClosestInteriorInstance(position, radius);

	public InteriorProxy[] GetAllInteriorProxies() => World.GetAllInteriorProxies();

	public InteriorProxy[] GetNearbyInteriorProxies(Vector3 position, float radius) => World.GetNearbyInteriorProxies(position, radius);

	public InteriorProxy GetClosestInteriorProxy(Vector3 position, float radius) => World.GetClosestInteriorProxy(position, radius);

	public Building GetClosest(Vector3 position, Building[] buildings) => World.GetClosest(position, buildings);

	public Building GetClosest(Vector2 position, Building[] buildings) => World.GetClosest(position, buildings);

	public AnimatedBuilding GetClosest(Vector3 position, AnimatedBuilding[] animatedBuildings) => World.GetClosest(position, animatedBuildings);

	public AnimatedBuilding GetClosest(Vector2 position, AnimatedBuilding[] animatedBuildings) => World.GetClosest(position, animatedBuildings);

	public InteriorInstance GetClosest(Vector3 position, InteriorInstance[] interiorInstances) => World.GetClosest(position, interiorInstances);

	public InteriorInstance GetClosest(Vector2 position, InteriorInstance[] interiorInstances) => World.GetClosest(position, interiorInstances);

	public InteriorProxy GetClosest(Vector3 position, InteriorProxy[] interiorProxies) => World.GetClosest(position, interiorProxies);

	public InteriorProxy GetClosest(Vector2 position, InteriorProxy[] interiorProxies) => World.GetClosest(position, interiorProxies);

	public Ped CreatePed(Model model, Vector3 position, float heading) => World.CreatePed(model, position, heading);

	public Ped CreateRandomPed(Vector3 position) => World.CreateRandomPed(position);

	public Vehicle CreateVehicle(Model model, Vector3 position, float heading) => World.CreateVehicle(model, position, heading);

	public Prop CreateProp(Model model, Vector3 position, bool dynamic, bool placeOnGround) => World.CreateProp(model, position, dynamic, placeOnGround);

	public Prop CreateProp(Model model, Vector3 position, Vector3 rotation, bool dynamic, bool placeOnGround) => World.CreateProp(model, position, rotation, dynamic, placeOnGround);

	public Prop CreatePropNoOffset(Model model, Vector3 position, bool dynamic) => World.CreatePropNoOffset(model, position, dynamic);

	public Prop CreatePropNoOffset(Model model, Vector3 position, Vector3 rotation, bool dynamic) => World.CreatePropNoOffset(model, position, rotation, dynamic);

	public Prop CreateAmbientPickup(PickupType type, Vector3 position, Model model, int value) => World.CreateAmbientPickup(type, position, model, value);

	public Pickup CreatePickup(PickupType type, Vector3 position, Model model, int value) => World.CreatePickup(type, position, model, value);

	public Pickup CreatePickup(PickupType type, Vector3 position, Vector3 rotation, Model model, int value) => World.CreatePickup(type, position, rotation, model, value);

	public Checkpoint[] GetAllCheckpoints() => World.GetAllCheckpoints();

	public Checkpoint CreateCheckpoint(CheckpointIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color) => World.CreateCheckpoint(icon, position, pointTo, radius, color);

	public Checkpoint CreateCheckpoint(CheckpointCustomIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color) => World.CreateCheckpoint(icon, position, pointTo, radius, color);

	public void DestroyAllCameras() => World.DestroyAllCameras();

	public Camera CreateCamera(Vector3 position, Vector3 rotation, float fov) => World.CreateCamera(position, rotation, fov);

	public bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, Vector3 pos, Vector3 rot, float scale, InvertAxisFlags invertAxis) => World.CreateParticleEffectNonLooped(asset, effectName, pos, rot, scale, invertAxis);

	public bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, Entity entity, Vector3 off, Vector3 rot, float scale, InvertAxisFlags invertAxis) => World.CreateParticleEffectNonLooped(asset, effectName, entity, off, rot, scale, invertAxis);

	public bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, EntityBone entityBone, Vector3 off, Vector3 rot, float scale, InvertAxisFlags invertAxis) => World.CreateParticleEffectNonLooped(asset, effectName, entityBone, off, rot, scale, invertAxis);

	public ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, Entity entity, Vector3 offset, Vector3 rotation, float scale, InvertAxisFlags invertAxis) => World.CreateParticleEffect(asset, effectName, entity, offset, rotation, scale, invertAxis);

	public ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, EntityBone entityBone, Vector3 offset, Vector3 rotation, float scale, InvertAxisFlags invertAxis) => World.CreateParticleEffect(asset, effectName, entityBone, offset, rotation, scale, invertAxis);

	public ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, Vector3 position, Vector3 rotation, float scale, InvertAxisFlags invertAxis) => World.CreateParticleEffect(asset, effectName, position, rotation, scale, invertAxis);

	public void RemoveAllParticleEffectsInRange(Vector3 pos, float range) => World.RemoveAllParticleEffectsInRange(pos, range);

	public Rope AddRope(RopeType type, Vector3 position, Vector3 rotation, float length, float minLength, bool breakable) => World.AddRope(type, position, rotation, length, minLength, breakable);

	public void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, WeaponAsset weaponAsset, int damage, float speed) => World.ShootBullet(sourcePosition, targetPosition, owner, weaponAsset, damage, speed);

	public void AddExplosion(Vector3 position, ExplosionType type, float radius, float cameraShake, Ped owner, bool aubidble, bool invisible) => World.AddExplosion(position, type, radius, cameraShake, owner, aubidble, invisible);

	public RelationshipGroup AddRelationshipGroup(string name) => World.AddRelationshipGroup(name);

	public void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color, bool bobUpAndDown, bool faceCamera, bool rotateY, string textueDict, string textureName, bool drawOnEntity) => World.DrawMarker(type, pos, dir, rot, scale, color, bobUpAndDown, faceCamera, rotateY, textueDict, textureName, drawOnEntity);

	public void DrawLightWithRange(Vector3 position, Color color, float range, float intensity) => World.DrawLightWithRange(position, color, range, intensity);

	public void DrawSpotLight(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout) => World.DrawSpotLight(pos, dir, color, distance, brightness, roundness, radius, fadeout);

	public void DrawSpotLightWithShadow(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout) => World.DrawSpotLightWithShadow(pos, dir, color, distance, brightness, roundness, radius, fadeout);

	public void DrawLine(Vector3 start, Vector3 end, Color color) => World.DrawLine(start, end, color);

	public void DrawPolygon(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, Color color) => World.DrawPolygon(vertexA, vertexB, vertexC, color);

	public void DrawBoxForAngledArea(Vector3 originEdge, Vector3 extentEdge, float width, Color color, DrawBoxFlags drawFlags) => World.DrawBoxForAngledArea(originEdge, extentEdge, width, color, drawFlags);

}
