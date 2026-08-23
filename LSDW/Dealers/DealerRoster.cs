using LSDW.Services;

using GTA;
using GTA.Math;

namespace LSDW.Dealers;

/// <summary>
/// Where the street dealers stand, and which peds they can look like. Data only —
/// the behaviour that uses it lives in <see cref="DealerWorld"/>.
/// </summary>
internal static class DealerRoster
{
  /// <summary>
  /// The 50 fixed dealer spots.
  /// </summary>
  public static IReadOnlyList<DealerSpot> Spots { get; } =
  [
    new DealerSpot(1, new Vector3(287.011f, -991.685f, 33.108f), 141.753f),
    new DealerSpot(2, new Vector3(1095.781f, -2158.615f, 31.32f), 356.712f),
    new DealerSpot(3, new Vector3(1136.588f, -1356.314f, 34.582f), 184.119f),
    new DealerSpot(4, new Vector3(-624.057f, -1609.615f, 26.901f), 358.93f),
    new DealerSpot(5, new Vector3(-71.055f, -1205.16f, 27.823f), 333.899f),
    new DealerSpot(6, new Vector3(-1175.357f, -1571.212f, 4.357f), 155.562f),
    new DealerSpot(7, new Vector3(895.643f, 3612.164f, 32.824f), 229.007f),
    new DealerSpot(8, new Vector3(2442.651f, 4959.362f, 45.842f), 259.245f),
    new DealerSpot(9, new Vector3(-283.2f, 6176.041f, 31.496f), 131.388f),
    new DealerSpot(10, new Vector3(144.781f, -2407.616f, 6.001f), 261.199f),
    new DealerSpot(11, new Vector3(-1462.811f, -368.759f, 39.635f), 160.347f),
    new DealerSpot(12, new Vector3(-481.178f, -59.29f, 39.994f), 85.202f),
    new DealerSpot(13, new Vector3(254.855f, -302.236f, 49.646f), 210.328f),
    new DealerSpot(14, new Vector3(737.784f, 1195.371f, 326.265f), 335.606f),
    new DealerSpot(15, new Vector3(-1837.719f, 791.407f, 138.696f), 119.483f),
    new DealerSpot(16, new Vector3(551.738f, 2663.721f, 45.873f), 267.741f),
    new DealerSpot(17, new Vector3(2405.682f, 3128.587f, 48.154f), 306.136f),
    new DealerSpot(18, new Vector3(1469.889f, 6550.403f, 14.904f), 356.061f),
    new DealerSpot(19, new Vector3(-141.62f, -1653.69f, 32.61f), 51.45f),
    new DealerSpot(20, new Vector3(2544.97f, 364.42f, 108.61f), 88.92f),
    new DealerSpot(21, new Vector3(-2185.1f, 4247.77f, 48.52f), 202.22f),
    new DealerSpot(22, new Vector3(1491.33f, 1044.3f, 114.33f), 269.84f),
    new DealerSpot(23, new Vector3(-37.29f, 1893.6f, 195.36f), 310.6f),
    new DealerSpot(24, new Vector3(-1101.17f, 2723.73f, 18.8f), 28.03f),
    new DealerSpot(25, new Vector3(85.63f, -1959.49f, 21.12f), 328.65f),
    new DealerSpot(26, new Vector3(-1512.28f, 1520.81f, 115.29f), 80.55f),
    new DealerSpot(27, new Vector3(1466.09f, -1930.76f, 71.32f), 149.76f),
    new DealerSpot(28, new Vector3(342.3f, -2075.3f, 20.94f), 189.02f),
    new DealerSpot(29, new Vector3(-464.73f, 5345.44f, 80.72f), 98.61f),
    new DealerSpot(30, new Vector3(-3248.473f, 1010.151f, 12.468f), 22.366f),
    new DealerSpot(31, new Vector3(-2955.77f, 386.674f, 15.021f), 242.835f),
    new DealerSpot(32, new Vector3(-2060.458f, -311.486f, 13.315f), 36.606f),
    new DealerSpot(33, new Vector3(-1373.688f, -916.801f, 10.271f), 306.373f),
    new DealerSpot(34, new Vector3(-715.509f, -722.946f, 28.888f), 174.265f),
    new DealerSpot(35, new Vector3(-422.967f, -2169.216f, 11.338f), 349.362f),
    new DealerSpot(36, new Vector3(-307.140f, -2732.951f, 6.009f), 337.739f),
    new DealerSpot(37, new Vector3(599.548f, -2737.368f, 6.096f), 328.200f),
    new DealerSpot(38, new Vector3(493.186f, -1456.372f, 29.287f), 46.836f),
    new DealerSpot(39, new Vector3(-357.650f, -716.444f, 32.305f), 10.870f),
    new DealerSpot(40, new Vector3(726.209f, -748.178f, 25.738f), 142.967f),
    new DealerSpot(41, new Vector3(982.569f, -209.278f, 70.844f), 235.041f),
    new DealerSpot(42, new Vector3(116.621f, 279.124f, 109.973f), 31.523f),
    new DealerSpot(43, new Vector3(-824.234f, 807.585f, 202.587f), 22.052f),
    new DealerSpot(44, new Vector3(1560.007f, 2184.725f, 78.875f), 0.975f),
    new DealerSpot(45, new Vector3(2670.376f, 1612.862f, 24.502f), 275.615f),
    new DealerSpot(46, new Vector3(-1929.599f, 2037.797f, 140.831f), 243.984f),
    new DealerSpot(47, new Vector3(170.793f, 7028.269f, 2.449f), 343.355f),
    new DealerSpot(48, new Vector3(1319.878f, 4315.213f, 38.141f), 68.6454f),
    new DealerSpot(49, new Vector3(815.346f, -2983.114f, 6.021f), 308.222f),
    new DealerSpot(50, new Vector3(1240.95f, -3323.181f, 6.028f), 323.216f)
  ];

  /// <summary>
  /// The ped models a dealer is randomly given when it spawns.
  /// </summary>
  public static IReadOnlyList<PedHash> Models { get; } =
  [
    PedHash.ArmGoon01GMM,
    PedHash.ArmGoon02GMY,
    PedHash.BallaEast01GMY,
    PedHash.BallaOrig01GMY,
    PedHash.Ballasog,
    PedHash.ChiGoon01GMM,
    PedHash.ChiGoon02GMM,
    PedHash.Famca01GMY,
    PedHash.Famdd01,
    PedHash.Famdnf01GMY,
    PedHash.Famfor01GMY,
    PedHash.Lost01GMY,
    PedHash.Lost02GMY,
    PedHash.Lost03GMY,
    PedHash.MexGang01GMY,
    PedHash.MexGoon01GMY,
    PedHash.MexGoon03GMY,
    PedHash.MexThug01AMY,
    PedHash.PoloGoon01GMY,
    PedHash.PoloGoon02GMY,
    PedHash.SalvaGoon01GMY,
    PedHash.SalvaGoon02GMY,
    PedHash.SalvaGoon03GMY
  ];

  /// <summary>
  /// Generic weapons that a dealer can carry. <see cref="DealerWorld"/> draws exactly one
  /// of these per dealer when it spawns the ped, and gives it with 500 ammo, unequipped.
  /// </summary>
  public static IReadOnlyList<WeaponHash> Weapons { get; } =
  [
    WeaponHash.Pistol,
    WeaponHash.PistolMk2,
    WeaponHash.Revolver,
    WeaponHash.RevolverMk2,
    WeaponHash.SMG,
    WeaponHash.SMGMk2,
    WeaponHash.AssaultRifle,
    WeaponHash.AssaultrifleMk2,
    WeaponHash.CarbineRifle,
    WeaponHash.CarbineRifleMk2,
    WeaponHash.PumpShotgun,
    WeaponHash.PumpShotgunMk2,
  ];

  /// <summary>
  /// Builds one <see cref="DrugDealer"/> per spot, in roster order.
  /// </summary>
  /// <param name="notifications">Where a duplicated spot id is reported.</param>
  public static List<DrugDealer> CreateDealers(INotificationService notifications)
  {
    List<DrugDealer> dealers = [.. Spots.Select(spot => new DrugDealer(spot.Position, spot.Heading, spot.Id))];

    // The blip is not made by the constructor, so that a dealer can be built without the
    // game. This is the one place that makes it — see DrugDealer.DealBlip.
    dealers.ForEach(dealer => dealer.CreateBlip());

    foreach (int id in dealers.GroupBy(dealer => dealer.Id).Where(group => group.Count() > 1).Select(group => group.Key))
    {
      notifications.Ticker("~r~Dealer roster error:~w~ spot id " + id + " is used more than once. Their saved state will be shared.");
    }

    return dealers;
  }
}