using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Los.Santos.Dope.Wars
{
	/// <summary>
	/// The <see cref="Constants"/> class holds statics and constant strings
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// The <see cref="AssemblyName"/> property
		/// </summary>
		public static readonly string AssemblyName = typeof(Constants).Assembly.GetName().Name;

		/// <summary>
		/// The <see cref="AssemblyVersion"/> property
		/// </summary>
		public static readonly string AssemblyVersion = typeof(Constants).Assembly.GetName().Version.ToString();

		/// <summary>
		/// The <see cref="GameStateFileName"/> property
		/// </summary>
		public static readonly string GameStateFileName = $"{Main.ScriptDirectory}\\{AssemblyName}.sav";

		/// <summary>
		/// The <see cref="GameSettingsFileName"/> property
		/// </summary>
		public static readonly string GameSettingsFileName = $"{Main.ScriptDirectory}\\{AssemblyName}.cfg";

		/// <summary>
		/// The "Log" file name
		/// </summary>
		public static readonly string LogFileName = $"{Main.ScriptDirectory}\\{AssemblyName}.log";

		/// <summary>
		/// The actaul screen size of the running game
		/// </summary>
		public static readonly Size ScreeSize = GTA.UI.Screen.Resolution;

		/// <summary>
		/// The xml namespace for save file and settings file
		/// </summary>
		public const string XmlNamespace = "http://www.los.santos.dope.wars.org";

		/// <summary>
		/// The volatility of market, so drug prices can reach from -10% to +10%
		/// </summary>
		public const double MarketVolatility = 0.10;

		/// <summary>
		/// For every player level the dealers get additional health and armor values.
		/// This would leed to lvl.1 -> 105 - lvl.50 -> 250
		/// </summary>
		public const float DealerArmorHealthPerLevelFactor = 5f;

		/// <summary>
		/// the additional discount per level factor level 1 means a plus of +0.5% and -0.5%
		/// </summary>
		public const double DiscountPerLevel = 0.005;

		/// <summary>
		/// The <see cref="TradePackOne"/> are the drugs the player can peddle from level 1 on
		/// </summary>
		public const Enums.DrugTypes TradePackOne = Enums.DrugTypes.Mushrooms | Enums.DrugTypes.Amphetamine | Enums.DrugTypes.Oxycodone | Enums.DrugTypes.Marijuana | Enums.DrugTypes.Hashish;

		/// <summary>
		/// The <see cref="TradePackTwo"/> are the drugs the player can additional peddle from level 10 on
		/// </summary>
		public const Enums.DrugTypes TradePackTwo = Enums.DrugTypes.Mescaline | Enums.DrugTypes.MDMA | Enums.DrugTypes.Ecstasy | Enums.DrugTypes.Acid | Enums.DrugTypes.PCP;

		/// <summary>
		/// The <see cref="TradePackThree"/> are the drugs the player can additional peddle from level 20 on
		/// </summary>
		public const Enums.DrugTypes TradePackThree = Enums.DrugTypes.Heroin | Enums.DrugTypes.Cocaine | Enums.DrugTypes.Methamphetamine | Enums.DrugTypes.Crack | Enums.DrugTypes.Ketamine;

		/// <summary>
		/// Random .. for random uses
		/// </summary>
		public static readonly Random random = new();

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

		public const float MarkerDrawDistance = 20f;

		public const float MarkerInteractionDistance = 1.25f;

		public static readonly Vector3 MarkerScale = new(1f, 1f, 1f);

		public static readonly Color MarkerColor = Color.FromArgb(255, 93, 182, 229);

		public static readonly Vector3 MissionMarkerScale = new(8f, 8f, 1.5f);

		public static readonly Color MissionMarkerColor = Color.FromArgb(255, 240, 200, 80);

		public static readonly Vector3 WarehouseEntranceFranklin = new(-320.3f, -1389.8f, 36.5f);

		public static readonly Vector3 WarehouseLocationFranklin = new(-307.3f, -1399.5f, 41.6f);

		public static readonly Vector3 WarehouseMissionStartFranklin = new(-323.7f, -1400.5f, 31.8f);

		public static readonly Vector3 WarehouseEntranceMichael = new(794.2f, -102.8f, 82f);

		public static readonly Vector3 WarehouseLocationMichael = new(799.5f, -94.6f, 80.6f);

		public static readonly Vector3 WarehouseMissionStartMichael = new(794.8f, -79.2f, 80.6f);
	}
}