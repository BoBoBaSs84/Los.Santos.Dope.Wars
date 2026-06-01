using GTA;
using GTA.Math;

namespace LSDW.Application.Abstractions.Application.Providers;

/// <summary>
/// Represents an abstraction contract for the <see cref="World"/> class.
/// </summary>
public interface IWorldProvider
{
	/// <inheritdoc cref="World.IsClockPaused"/>
	bool IsClockPaused { get; set; }

	/// <inheritdoc cref="World.CurrentDate"/>
	DateTime CurrentDate { get; set; }

	/// <inheritdoc cref="World.CurrentTimeOfDay"/>
	TimeSpan CurrentTimeOfDay { get; set; }

	/// <inheritdoc cref="World.MillisecondsPerGameMinute"/>
	int MillisecondsPerGameMinute { get; set; }

	/// <inheritdoc cref="World.Blackout"/>
	bool Blackout { set; }

	/// <inheritdoc cref="World.Weather"/>
	Weather Weather { get; set; }

	/// <inheritdoc cref="World.NextWeather"/>
	Weather NextWeather { get; set; }

	/// <inheritdoc cref="World.GravityLevel"/>
	float GravityLevel { get; set; }

	/// <inheritdoc cref="World.WaypointBlip"/>
	Blip WaypointBlip { get; }

	/// <inheritdoc cref="World.WaypointPosition"/>
	Vector3 WaypointPosition { get; set; }

	/// <inheritdoc cref="World.VehicleCount"/>
	int VehicleCount { get; }

	/// <inheritdoc cref="World.PedCount"/>
	int PedCount { get; }

	/// <inheritdoc cref="World.PropCount"/>
	int PropCount { get; }

	/// <inheritdoc cref="World.PickupObjectCount"/>
	int PickupObjectCount { get; }

	/// <inheritdoc cref="World.BuildingCount"/>
	int BuildingCount { get; }

	/// <inheritdoc cref="World.AnimatedBuildingCount"/>
	int AnimatedBuildingCount { get; }

	/// <inheritdoc cref="World.InteriorInstanceCount"/>
	int InteriorInstanceCount { get; }

	/// <inheritdoc cref="World.InteriorProxyCount"/>
	int InteriorProxyCount { get; }

	/// <inheritdoc cref="World.ProjectileCount"/>
	int ProjectileCount { get; }

	/// <inheritdoc cref="World.EntityColliderCount"/>
	int EntityColliderCount { get; }

	/// <inheritdoc cref="World.VehicleCapacity"/>
	int VehicleCapacity { get; }

	/// <inheritdoc cref="World.PedCapacity"/>
	int PedCapacity { get; }

	/// <inheritdoc cref="World.PropCapacity"/>
	int PropCapacity { get; }

	/// <inheritdoc cref="World.PickupObjectCapacity"/>
	int PickupObjectCapacity { get; }

	/// <inheritdoc cref="World.ProjectileCapacity"/>
	int ProjectileCapacity { get; }

	/// <inheritdoc cref="World.BuildingCapacity"/>
	int BuildingCapacity { get; }

	/// <inheritdoc cref="World.AnimatedBuildingCapacity"/>
	int AnimatedBuildingCapacity { get; }

	/// <inheritdoc cref="World.InteriorInstanceCapacity"/>
	int InteriorInstanceCapacity { get; }

	/// <inheritdoc cref="World.InteriorProxyCapacity"/>
	int InteriorProxyCapacity { get; }

	/// <inheritdoc cref="World.EntityColliderCapacity"/>
	int EntityColliderCapacity { get; }

	/// <inheritdoc cref="World.RenderingCamera"/>
	Camera RenderingCamera { get; set; }

	/// <inheritdoc cref="World.Raycast(Vector3, Vector3, IntersectFlags, Entity)"/>
	RaycastResult Raycast(Vector3 source, Vector3 target, IntersectFlags options, Entity ignoreEntity);

	/// <inheritdoc cref="World.Raycast(Vector3, Vector3, float, IntersectFlags, Entity)"/>
	RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectFlags options, Entity ignoreEntity);

	/// <inheritdoc cref="World.GetCrosshairCoordinates()"/>
	RaycastResult GetCrosshairCoordinates();

	/// <inheritdoc cref="World.GetCrosshairCoordinates(IntersectFlags, Entity)"/>
	RaycastResult GetCrosshairCoordinates(IntersectFlags intersectOptions, Entity ignoreEntity);

	/// <inheritdoc cref="World.GetDistance(Vector3, Vector3)"/>
	float GetDistance(Vector3 origin, Vector3 destination);

	/// <inheritdoc cref="World.CalculateTravelDistance(Vector3, Vector3)"/>
	float CalculateTravelDistance(Vector3 origin, Vector3 destination);

	/// <inheritdoc cref="World.GetGroundHeight(Vector2)"/>
	float GetGroundHeight(Vector2 position);

	/// <inheritdoc cref="World.GetGroundHeight(Vector3)"/>
	float GetGroundHeight(Vector3 position);

	/// <inheritdoc cref="World.GetSafeCoordForPed(Vector3, bool, int)"/>
	Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk, int flags);

	/// <inheritdoc cref="World.GetNextPositionOnStreet(Vector2, bool)"/>
	Vector3 GetNextPositionOnStreet(Vector2 position, bool unoccupied);

	/// <inheritdoc cref="World.GetNextPositionOnStreet(Vector3, bool)"/>
	Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied);

	/// <inheritdoc cref="World.GetNextPositionOnSidewalk(Vector2)"/>
	Vector3 GetNextPositionOnSidewalk(Vector2 position);

	/// <inheritdoc cref="World.GetNextPositionOnSidewalk(Vector3)"/>
	Vector3 GetNextPositionOnSidewalk(Vector3 position);

	/// <inheritdoc cref="World.GetStreetName(Vector2)"/>
	string GetStreetName(Vector2 position);

	/// <inheritdoc cref="World.GetStreetName(Vector3)"/>
	string GetStreetName(Vector3 position);

	/// <inheritdoc cref="World.GetZoneDisplayName(Vector2)"/>
	string GetZoneDisplayName(Vector2 position);

	/// <inheritdoc cref="World.GetZoneDisplayName(Vector3)"/>
	string GetZoneDisplayName(Vector3 position);

	/// <inheritdoc cref="World.GetZoneLocalizedName(Vector2)"/>
	string GetZoneLocalizedName(Vector2 position);

	/// <inheritdoc cref="World.GetZoneLocalizedName(Vector3)"/>
	string GetZoneLocalizedName(Vector3 position);

	/// <inheritdoc cref="World.IsPointInAngledArea(Vector3, Vector3, Vector3, float, bool)"/>
	bool IsPointInAngledArea(Vector3 point, Vector3 originEdge, Vector3 extentEdge, float width, bool includeZAxis);

	/// <inheritdoc cref="World.TransitionToWeather(Weather, float)"/>
	void TransitionToWeather(Weather weather, float duration);

	/// <inheritdoc cref="World.RemoveWaypoint()"/>
	void RemoveWaypoint();

	/// <inheritdoc cref="World.GetAllBlips(BlipSprite[])"/>
	Blip[] GetAllBlips(BlipSprite[] blipTypes);

	/// <inheritdoc cref="World.GetNearbyBlips(Vector3, float, BlipSprite[])"/>
	Blip[] GetNearbyBlips(Vector3 position, float radius, BlipSprite[] blipTypes);

	/// <inheritdoc cref="World.CreateBlip(Vector3)"/>
	Blip CreateBlip(Vector3 position);

	/// <inheritdoc cref="World.CreateBlip(Vector3, float)"/>
	Blip CreateBlip(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosestPed(Vector3, float, Model[])"/>
	Ped GetClosestPed(Vector3 position, float radius, Model[] models);

	/// <inheritdoc cref="World.GetAllPeds(Model[])"/>
	Ped[] GetAllPeds(Model[] models);

	/// <inheritdoc cref="World.GetNearbyPeds(Ped, float, Model[])"/>
	Ped[] GetNearbyPeds(Ped ped, float radius, Model[] models);

	/// <inheritdoc cref="World.GetNearbyPeds(Vector3, float, Model[])"/>
	Ped[] GetNearbyPeds(Vector3 position, float radius, Model[] models);

	/// <inheritdoc cref="World.GetClosestVehicle(Vector3, float, Model[])"/>
	Vehicle GetClosestVehicle(Vector3 position, float radius, Model[] models);

	/// <inheritdoc cref="World.GetAllVehicles(Model[])"/>
	Vehicle[] GetAllVehicles(Model[] models);

	/// <inheritdoc cref="World.GetNearbyVehicles(Ped, float, Model[])"/>
	Vehicle[] GetNearbyVehicles(Ped ped, float radius, Model[] models);

	/// <inheritdoc cref="World.GetNearbyVehicles(Vector3, float, Model[])"/>
	Vehicle[] GetNearbyVehicles(Vector3 position, float radius, Model[] models);

	/// <inheritdoc cref="World.GetClosestProp(Vector3, float, Model[])"/>
	Prop GetClosestProp(Vector3 position, float radius, Model[] models);

	/// <inheritdoc cref="World.GetAllProps(Model[])"/>
	Prop[] GetAllProps(Model[] models);

	/// <inheritdoc cref="World.GetNearbyProps(Vector3, float, Model[])"/>
	Prop[] GetNearbyProps(Vector3 position, float radius, Model[] models);

	/// <inheritdoc cref="World.GetClosestPickupObject(Vector3, float)"/>
	Prop GetClosestPickupObject(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllPickupObjects()"/>
	Prop[] GetAllPickupObjects();

	/// <inheritdoc cref="World.GetNearbyPickupObjects(Vector3, float)"/>
	Prop[] GetNearbyPickupObjects(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosestProjectile(Vector3, float)"/>
	Projectile GetClosestProjectile(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllProjectiles()"/>
	Projectile[] GetAllProjectiles();

	/// <inheritdoc cref="World.GetNearbyProjectiles(Vector3, float)"/>
	Projectile[] GetNearbyProjectiles(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllEntities()"/>
	Entity[] GetAllEntities();

	/// <inheritdoc cref="World.GetNearbyEntities(Vector3, float)"/>
	Entity[] GetNearbyEntities(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllBuildings()"/>
	Building[] GetAllBuildings();

	/// <inheritdoc cref="World.GetNearbyBuildings(Vector3, float)"/>
	Building[] GetNearbyBuildings(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosestBuilding(Vector3, float)"/>
	Building GetClosestBuilding(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllAnimatedBuildings()"/>
	AnimatedBuilding[] GetAllAnimatedBuildings();

	/// <inheritdoc cref="World.GetNearbyAnimatedBuildings(Vector3, float)"/>
	AnimatedBuilding[] GetNearbyAnimatedBuildings(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosestAnimatedBuilding(Vector3, float)"/>
	AnimatedBuilding GetClosestAnimatedBuilding(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllInteriorInstances()"/>
	InteriorInstance[] GetAllInteriorInstances();

	/// <inheritdoc cref="World.GetNearbyInteriorInstances(Vector3, float)"/>
	InteriorInstance[] GetNearbyInteriorInstances(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosestInteriorInstance(Vector3, float)"/>
	InteriorInstance GetClosestInteriorInstance(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetAllInteriorProxies()"/>
	InteriorProxy[] GetAllInteriorProxies();

	/// <inheritdoc cref="World.GetNearbyInteriorProxies(Vector3, float)"/>
	InteriorProxy[] GetNearbyInteriorProxies(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosestInteriorProxy(Vector3, float)"/>
	InteriorProxy GetClosestInteriorProxy(Vector3 position, float radius);

	/// <inheritdoc cref="World.GetClosest(Vector3, Building[])"/>
	Building GetClosest(Vector3 position, Building[] buildings);

	/// <inheritdoc cref="World.GetClosest(Vector2, Building[])"/>
	Building GetClosest(Vector2 position, Building[] buildings);

	/// <inheritdoc cref="World.GetClosest(Vector3, AnimatedBuilding[])"/>
	AnimatedBuilding GetClosest(Vector3 position, AnimatedBuilding[] animatedBuildings);

	/// <inheritdoc cref="World.GetClosest(Vector2, AnimatedBuilding[])"/>
	AnimatedBuilding GetClosest(Vector2 position, AnimatedBuilding[] animatedBuildings);

	/// <inheritdoc cref="World.GetClosest(Vector3, InteriorInstance[])"/>
	InteriorInstance GetClosest(Vector3 position, InteriorInstance[] interiorInstances);

	/// <inheritdoc cref="World.GetClosest(Vector2, InteriorInstance[])"/>
	InteriorInstance GetClosest(Vector2 position, InteriorInstance[] interiorInstances);

	/// <inheritdoc cref="World.GetClosest(Vector3, InteriorProxy[])"/>
	InteriorProxy GetClosest(Vector3 position, InteriorProxy[] interiorProxies);

	/// <inheritdoc cref="World.GetClosest(Vector2, InteriorProxy[])"/>
	InteriorProxy GetClosest(Vector2 position, InteriorProxy[] interiorProxies);

	/// <inheritdoc cref="World.CreatePed(Model, Vector3, float)"/>
	Ped CreatePed(Model model, Vector3 position, float heading);

	/// <inheritdoc cref="World.CreateRandomPed(Vector3)"/>
	Ped CreateRandomPed(Vector3 position);

	/// <inheritdoc cref="World.CreateVehicle(Model, Vector3, float)"/>
	Vehicle CreateVehicle(Model model, Vector3 position, float heading);

	/// <inheritdoc cref="World.CreateProp(Model, Vector3, bool, bool)"/>
	Prop CreateProp(Model model, Vector3 position, bool dynamic, bool placeOnGround);

	/// <inheritdoc cref="World.CreateProp(Model, Vector3, Vector3, bool, bool)"/>
	Prop CreateProp(Model model, Vector3 position, Vector3 rotation, bool dynamic, bool placeOnGround);

	/// <inheritdoc cref="World.CreatePropNoOffset(Model, Vector3, bool)"/>
	Prop CreatePropNoOffset(Model model, Vector3 position, bool dynamic);

	/// <inheritdoc cref="World.CreatePropNoOffset(Model, Vector3, Vector3, bool)"/>
	Prop CreatePropNoOffset(Model model, Vector3 position, Vector3 rotation, bool dynamic);

	/// <inheritdoc cref="World.CreateAmbientPickup(PickupType, Vector3, Model, int)"/>
	Prop CreateAmbientPickup(PickupType type, Vector3 position, Model model, int value);

	/// <inheritdoc cref="World.CreatePickup(PickupType, Vector3, Model, int)"/>
	Pickup CreatePickup(PickupType type, Vector3 position, Model model, int value);

	/// <inheritdoc cref="World.CreatePickup(PickupType, Vector3, Vector3, Model, int)"/>
	Pickup CreatePickup(PickupType type, Vector3 position, Vector3 rotation, Model model, int value);

	/// <inheritdoc cref="World.GetAllCheckpoints()"/>
	Checkpoint[] GetAllCheckpoints();

	/// <inheritdoc cref="World.CreateCheckpoint(CheckpointIcon, Vector3, Vector3, float, Color)"/>
	Checkpoint CreateCheckpoint(CheckpointIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color);

	/// <inheritdoc cref="World.CreateCheckpoint(CheckpointCustomIcon, Vector3, Vector3, float, Color)"/>
	Checkpoint CreateCheckpoint(CheckpointCustomIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color);

	/// <inheritdoc cref="World.DestroyAllCameras()"/>
	void DestroyAllCameras();

	/// <inheritdoc cref="World.CreateCamera(Vector3, Vector3, float)"/>
	Camera CreateCamera(Vector3 position, Vector3 rotation, float fov);

	/// <inheritdoc cref="World.CreateParticleEffectNonLooped(ParticleEffectAsset, string, Vector3, Vector3, float, InvertAxisFlags)"/>
	bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, Vector3 pos, Vector3 rot, float scale, InvertAxisFlags invertAxis);

	/// <inheritdoc cref="World.CreateParticleEffectNonLooped(ParticleEffectAsset, string, Entity, Vector3, Vector3, float, InvertAxisFlags)"/>
	bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, Entity entity, Vector3 off, Vector3 rot, float scale, InvertAxisFlags invertAxis);

	/// <inheritdoc cref="World.CreateParticleEffectNonLooped(ParticleEffectAsset, string, EntityBone, Vector3, Vector3, float, InvertAxisFlags)"/>
	bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, EntityBone entityBone, Vector3 off, Vector3 rot, float scale, InvertAxisFlags invertAxis);

	/// <inheritdoc cref="World.CreateParticleEffect(ParticleEffectAsset, string, Entity, Vector3, Vector3, float, InvertAxisFlags)"/>
	ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, Entity entity, Vector3 offset, Vector3 rotation, float scale, InvertAxisFlags invertAxis);

	/// <inheritdoc cref="World.CreateParticleEffect(ParticleEffectAsset, string, EntityBone, Vector3, Vector3, float, InvertAxisFlags)"/>
	ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, EntityBone entityBone, Vector3 offset, Vector3 rotation, float scale, InvertAxisFlags invertAxis);

	/// <inheritdoc cref="World.CreateParticleEffect(ParticleEffectAsset, string, Vector3, Vector3, float, InvertAxisFlags)"/>
	ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, Vector3 position, Vector3 rotation, float scale, InvertAxisFlags invertAxis);

	/// <inheritdoc cref="World.RemoveAllParticleEffectsInRange(Vector3, float)"/>
	void RemoveAllParticleEffectsInRange(Vector3 pos, float range);

	/// <inheritdoc cref="World.AddRope(RopeType, Vector3, Vector3, float, float, bool)"/>
	Rope AddRope(RopeType type, Vector3 position, Vector3 rotation, float length, float minLength, bool breakable);

	/// <inheritdoc cref="World.ShootBullet(Vector3, Vector3, Ped, WeaponAsset, int, float)"/>
	void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, WeaponAsset weaponAsset, int damage, float speed);

	/// <inheritdoc cref="World.AddExplosion(Vector3, ExplosionType, float, float, Ped, bool, bool)"/>
	void AddExplosion(Vector3 position, ExplosionType type, float radius, float cameraShake, Ped owner, bool aubidble, bool invisible);

	/// <inheritdoc cref="World.AddRelationshipGroup(string)"/>
	RelationshipGroup AddRelationshipGroup(string name);

	/// <inheritdoc cref="World.DrawMarker(MarkerType, Vector3, Vector3, Vector3, Vector3, Color, bool, bool, bool, string, string, bool)"/>
	void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color, bool bobUpAndDown, bool faceCamera, bool rotateY, string textueDict, string textureName, bool drawOnEntity);

	/// <inheritdoc cref="World.DrawLightWithRange(Vector3, Color, float, float)"/>
	void DrawLightWithRange(Vector3 position, Color color, float range, float intensity);

	/// <inheritdoc cref="World.DrawSpotLight(Vector3, Vector3, Color, float, float, float, float, float)"/>
	void DrawSpotLight(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout);

	/// <inheritdoc cref="World.DrawSpotLightWithShadow(Vector3, Vector3, Color, float, float, float, float, float)"/>
	void DrawSpotLightWithShadow(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout);

	/// <inheritdoc cref="World.DrawLine(Vector3, Vector3, Color)"/>
	void DrawLine(Vector3 start, Vector3 end, Color color);

	/// <inheritdoc cref="World.DrawPolygon(Vector3, Vector3, Vector3, Color)"/>
	void DrawPolygon(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, Color color);

	/// <inheritdoc cref="World.DrawBoxForAngledArea(Vector3, Vector3, float, Color, DrawBoxFlags)"/>
	void DrawBoxForAngledArea(Vector3 originEdge, Vector3 extentEdge, float width, Color color, DrawBoxFlags drawFlags);
}
