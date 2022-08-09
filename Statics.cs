using GTA;
using GTA.Math;
using System.Globalization;
using System.Reflection;

namespace Los.Santos.Dope.Wars;

/// <summary>
/// The <see cref="Statics"/> class holds all the needed statics
/// </summary>
public static class Statics
{
	private static readonly Assembly assembly;
	
	/// <summary>
	/// The <see cref="Statics"/> constructor
	/// </summary>
	static Statics()
	{
		assembly = Assembly.GetExecutingAssembly();
		Random = new Random();
		AssemblyName = assembly.GetName().Name;
		AssemblyVersion = assembly.GetName().Version.ToString();
		CultureInfo = CultureInfo.CurrentCulture;
		GameSettingsFileName = $"{Main.ScriptDirectory}\\{AssemblyName}.cfg";
		GameStateFileName = $"{Main.ScriptDirectory}\\{AssemblyName}.sav";
		LogFileName = $"{Main.ScriptDirectory}\\{AssemblyName}.log";
		ScreeSize = GTA.UI.Screen.Resolution;
	}

	/// <summary>
	/// The <see cref="AssemblyName"/> property
	/// </summary>
	public static string AssemblyName { get; private set; }

	/// <summary>
	/// The <see cref="AssemblyVersion"/> property
	/// </summary>
	public static string AssemblyVersion { get; private set; }

	/// <summary>
	/// The <see cref="CultureInfo"/> property
	/// </summary>
	public static CultureInfo CultureInfo { get; private set; }

	/// <summary>
	/// The <see cref="GameStateFileName"/> property
	/// </summary>
	public static string GameStateFileName { get; private set; }

	/// <summary>
	/// The <see cref="GameSettingsFileName"/> property
	/// </summary>
	public static string GameSettingsFileName { get; private set; }

	/// <summary>
	/// The <see cref="LogFileName"/> property
	/// </summary>
	public static string LogFileName { get; private set; }

	/// <summary>
	/// The <see cref="ScreeSize"/> property
	/// </summary>
	public static Size ScreeSize { get; private set; }

	/// <summary>
	/// The <see cref="Random"/> property
	/// </summary>
	public static Random Random { get; private set; }

	public static readonly Vector3 EntranceMarkerScale = new(1f, 1f, 1f);

	public static readonly Vector3 MissionMarkerScale = new(8f, 8f, 1.5f);

	public static readonly Vector3 WarehouseEntranceFranklin = new(191.66f, -2226.67f, 5.97f);

	public static readonly Vector3 WarehouseLocationFranklin = new(191.57f, -2223.44f, 4.95f);

	public static readonly Vector3 WarehouseMissionMarkerFranklin = new(180.73f, -2218.16f, 4.96f);

	public static readonly Vector3 WarehouseEntranceMichael = new(794.2f, -102.8f, 81f);

	public static readonly Vector3 WarehouseLocationMichael = new(799.5f, -94.6f, 80.6f);

	public static readonly Vector3 WarehouseMissionMarkerMichael = new(794.8f, -79.2f, 79.6f);

	public static readonly Vector3 WarehouseEntranceTrevor = new(996.6f, 3775.2f, 33.1f);

	public static readonly Vector3 WarehouseLocationTrevor = new(989.1f, 3574.8f, 35f);

	public static readonly Vector3 WarehouseMissionMarkerTrevor = new(977.75f, 3591.4f, 33.4f);

	/// <summary>
	/// The <see cref="DrugDealerPedHashes"/> list holds all the possible dealer models
	/// </summary>
	public static readonly List<PedHash> DrugDealerPedHashes = new()
	{
		PedHash.Famdd01,
		PedHash.Famca01GMY,
		PedHash.Famdnf01GMY,
		PedHash.Famfor01GMY,
		PedHash.Lost01GMY,
		PedHash.Lost02GMY,
		PedHash.Lost03GMY,
		PedHash.BallaEast01GMY,
		PedHash.BallaOrig01GMY,
		PedHash.Ballasog,
		PedHash.MexGang01GMY,
		PedHash.MexGoon01GMY,
		PedHash.MexThug01AMY,
		PedHash.Korean01GMY,
		PedHash.Korean02GMY,
		PedHash.PoloGoon01GMY,
		PedHash.PoloGoon02GMY,
		PedHash.SalvaGoon01GMY,
		PedHash.SalvaGoon02GMY,
		PedHash.SalvaGoon03GMY,
		PedHash.ChiGoon01GMM,
		PedHash.ChiGoon02GMM,
		PedHash.ArmGoon02GMY
	};

	/// <summary>
	/// The <see cref="DrugLordPedHashes"/> list holds all the possible drug lord models
	/// </summary>
	public static readonly List<PedHash> DrugLordPedHashes = new()
	{
		PedHash.ChiBoss01GMM,
		PedHash.KorBoss01GMM,
		PedHash.MexBoss01GMM,
		PedHash.MexBoss02GMM,
		PedHash.OgBoss01AMM,
		PedHash.SalvaBoss01GMY
	};

	/// <summary>
	/// The <see cref="BodyguardPedHashes"/> list holds all the possible bodyguard models
	/// </summary>
	public static readonly List<PedHash> BodyguardPedHashes = new()
	{
		PedHash.Armoured01,
		PedHash.Armoured01SMM,
		PedHash.Armoured02SMM
	};

	/// <summary>
	/// The <see cref="DrugDealerWeaponHashes"/> list holds all the possible dealer weapons
	/// </summary>
	public static readonly List<WeaponHash> DrugDealerWeaponHashes = new()
	{
		WeaponHash.Pistol,
		WeaponHash.SMG,
		WeaponHash.AssaultRifle,
		WeaponHash.SawnOffShotgun,
		WeaponHash.MachinePistol
	};

	/// <summary>
	/// The <see cref="DrugLordWeaponHashes"/> list holds all the possible drug lord weapons
	/// </summary>
	public static readonly List<WeaponHash> DrugLordWeaponHashes = new()
	{
		WeaponHash.HeavyPistol,
		WeaponHash.AssaultSMG,
		WeaponHash.SpecialCarbine,
		WeaponHash.AssaultShotgun,
		WeaponHash.CombatMG
	};

	/// <summary>
	/// The <see cref="BodyguardWeaponHashes"/> list holds all the possible drug lord weapons
	/// </summary>
	public static readonly List<WeaponHash> BodyguardWeaponHashes = new()
	{
		WeaponHash.CombatMGMk2,
		WeaponHash.AssaultrifleMk2,
		WeaponHash.CarbineRifleMk2,
		WeaponHash.PistolMk2,
		WeaponHash.SpecialCarbineMk2,
		WeaponHash.PumpShotgunMk2,
		WeaponHash.SMGMk2
	};

	/// <summary>
	/// The <see cref="DrugDealerSpawnLocations"/> list holds all the possible dealer spawn locations
	/// </summary>
	public static readonly Tuple<Vector3, float>[] DrugDealerSpawnLocations =
	{
		Tuple.Create(new Vector3(287.011f, -991.685f, 33.108f), 141.753f),
		Tuple.Create(new Vector3(1095.781f, -2158.615f, 31.32f), 356.712f),
		Tuple.Create(new Vector3(1136.588f, -1356.314f, 34.582f), 184.119f),
		Tuple.Create(new Vector3(-624.057f, -1609.615f, 26.901f), 358.93f),
		Tuple.Create(new Vector3(-71.055f, -1205.16f, 27.823f), 333.899f),
		Tuple.Create(new Vector3(-1175.357f, -1571.212f, 4.357f), 155.562f),
		Tuple.Create(new Vector3(895.643f, 3612.164f, 32.824f), 229.007f),
		Tuple.Create(new Vector3(2442.651f, 4959.362f, 45.842f), 259.245f),
		Tuple.Create(new Vector3(-283.2f, 6176.041f, 31.496f), 131.388f),
		Tuple.Create(new Vector3(144.781f, -2407.616f, 6.001f), 261.199f),
		Tuple.Create(new Vector3(-1462.811f, -368.759f, 39.635f), 160.347f),
		Tuple.Create(new Vector3(-481.178f, -59.29f, 39.994f), 85.202f),
		Tuple.Create(new Vector3(254.855f, -302.236f, 49.646f), 210.328f),
		Tuple.Create(new Vector3(737.784f, 1195.371f, 326.265f), 335.606f),
		Tuple.Create(new Vector3(-1837.719f, 791.407f, 138.696f), 119.483f),
		Tuple.Create(new Vector3(551.738f, 2663.721f, 45.873f), 267.741f),
		Tuple.Create(new Vector3(2405.682f, 3128.587f, 48.154f), 306.136f),
		Tuple.Create(new Vector3(1469.889f, 6550.403f, 14.904f), 356.061f),
		Tuple.Create(new Vector3(-141.62f, -1653.69f, 32.61f), 51.45f),
		Tuple.Create(new Vector3(2544.97f, 364.42f, 108.61f), 88.92f),
		Tuple.Create(new Vector3(-2185.1f, 4247.77f, 48.52f), 202.22f),
		Tuple.Create(new Vector3(1491.33f, 1044.3f, 114.33f), 269.84f),
		Tuple.Create(new Vector3(-37.29f, 1893.6f, 195.36f), 310.6f),
		Tuple.Create(new Vector3(-1101.17f, 2723.73f, 18.8f), 28.03f),
		Tuple.Create(new Vector3(85.63f, -1959.49f, 21.12f), 328.65f),
		Tuple.Create(new Vector3(-1512.28f, 1520.81f, 115.29f), 80.55f),
		Tuple.Create(new Vector3(1466.09f, -1930.76f, 71.32f), 149.76f),
		Tuple.Create(new Vector3(342.3f, -2075.3f, 20.94f), 189.02f),
		Tuple.Create(new Vector3(-464.73f, 5345.44f, 80.72f), 98.61f),
		Tuple.Create(new Vector3(-693.68f, -751.71f, 25.04f), -166.03f)
		};

	/// <summary>
	/// The <see cref="DrugLordSpawnLocations"/> list holds all the possible drug lord spawn locations
	/// </summary>
	//todo just dummy!		
	public static readonly Tuple<Vector3, float>[] DrugLordSpawnLocations =
	{
		Tuple.Create(new Vector3(287.011f, -991.685f, 33.108f), 141.753f),
		Tuple.Create(new Vector3(1095.781f, -2158.615f, 31.32f), 356.712f),
		Tuple.Create(new Vector3(1136.588f, -1356.314f, 34.582f), 184.119f),
		Tuple.Create(new Vector3(-624.057f, -1609.615f, 26.901f), 358.93f),
		Tuple.Create(new Vector3(-71.055f, -1205.16f, 27.823f), 333.899f),
	};

	/// <summary>
	/// The <see cref="WarehouseMissionVehicles"/> list holds all the possile stealable vehicle for the warehouse mission
	/// </summary>
	public static readonly List<VehicleHash> WarehouseMissionVehicles = new()
	{
		VehicleHash.Boxville,
		VehicleHash.Boxville2,
		VehicleHash.Boxville3,
		VehicleHash.Burrito,
		VehicleHash.Burrito2,
		VehicleHash.Burrito3,
		VehicleHash.Burrito4,
		VehicleHash.Mule,
		VehicleHash.Mule2,
		VehicleHash.Paradise,
		VehicleHash.Pony,
		VehicleHash.Pony2,
		VehicleHash.Rumpo,
		VehicleHash.Rumpo2,
		VehicleHash.Speedo,
		VehicleHash.Speedo4,
		VehicleHash.Surfer,
		VehicleHash.Surfer2,
		VehicleHash.Youga,
		VehicleHash.Youga2
	};

	/// <summary>
	/// The <see cref="VanSafePlaces"/> list holds all the possible van safe places
	/// </summary>
	public static readonly List<Vector3> VanSafePlaces = new()
	{
		new Vector3(692.636f, -1011.985f, 22.464f),
		new Vector3(1978.605f, 5171.296f, 47.542f),
		new Vector3(1039.071f, 2435.874f, 44.842f),
		new Vector3(-1608.686f, -1000.225f, 7.512f),
		new Vector3(-619.311f, 346.452f, 85.017f),
		new Vector3(212.366f, -3328.39f, 5.704f)
	};
}
