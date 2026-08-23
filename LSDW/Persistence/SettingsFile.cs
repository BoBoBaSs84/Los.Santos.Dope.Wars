using LSDW.Properties;
using BB84.SourceGenerators.Attributes;

namespace LSDW.Persistence;

/// <summary>
/// The shape of <c>scripts\LSDW.ini</c> — the mod's user-editable
/// configuration, and the single source of truth for every default.
/// <c>Read</c>/<c>TryRead</c>/<c>Write</c> are generated from these properties;
/// <see cref="SettingsService"/> loads the file and maps it to the live values.
/// </summary>
[GenerateIniFile(StringComparison.Ordinal)]
internal sealed partial class SettingsFile
{
  public const string FileName = $"{AssemblyInformation.Title}.ini";

  [GenerateIniFileSection]
  public KeysSection Keys { get; set; } = new();

  [GenerateIniFileSection]
  public EconomySection Economy { get; set; } = new();

  [GenerateIniFileSection]
  public DealersSection Dealers { get; set; } = new();

  [GenerateIniFileSection]
  public PoliceSection Police { get; set; } = new();
}

/// <summary>
/// The keys the mod listens for, stored as <see cref="System.Windows.Forms.Keys"/>
/// names. <see cref="SettingsService"/> parses each and falls back to the live value on
/// an unrecognised name, so a typo disables one bind rather than failing the file.
/// </summary>
internal sealed class KeysSection
{
  [GenerateIniFileValue]
  public string MenuUp { get; set; } = "NumPad8";

  [GenerateIniFileValue]
  public string MenuDown { get; set; } = "NumPad2";

  [GenerateIniFileValue]
  public string MenuLeft { get; set; } = "NumPad4";

  [GenerateIniFileValue]
  public string MenuRight { get; set; } = "NumPad6";

  [GenerateIniFileValue]
  public string Confirm { get; set; } = "NumPad5";

  [GenerateIniFileValue]
  public string TacoSession { get; set; } = "D2";

  [GenerateIniFileValue]
  public string BuyProperty { get; set; } = "E";
}

/// <summary>
/// Gameplay tunables. The XP curve and the per-level rewards stay code in
/// <see cref="Progression"/> / <see cref="RewardSystem"/>: the curve is a formula and
/// the rewards are index-coupled to unlock actions.
/// </summary>
internal sealed class EconomySection
{
  /// <summary>
  /// What the Taco Bomb drug front costs to buy.
  /// </summary>
  [GenerateIniFileValue]
  public int TacoPropertyPrice { get; set; } = 200000;

  /// <summary>
  /// The upper bound, as a percentage, on the random markup a taco-van sale adds over
  /// market price. Read into memory as a fraction (50 → 0.5).
  /// </summary>
  [GenerateIniFileValue]
  public int TacoMaxMarkupPercent { get; set; } = 50;

  /// <summary>
  /// Scales XP gained from a sale, as a percentage. 100 = ×1.0; 200 = double XP. Read
  /// into memory as a fraction.
  /// </summary>
  [GenerateIniFileValue]
  public int XpMultiplierPercent { get; set; } = 100;

  /// <summary>
  /// The chance, per market cycle, that one dealer runs a special offer. Clamped to
  /// 0..100 on load; <c>0</c> turns offers off.
  /// </summary>
  [GenerateIniFileValue]
  public int SpecialOfferChancePercent { get; set; } = 25;

  /// <summary>
  /// How many in-game hours between market cycles — each one deals every dealer a fresh
  /// share of the world's supply, and with it a fresh price. Clamped to 1..168 on load.
  /// </summary>
  [GenerateIniFileValue]
  public int RestockIntervalHours { get; set; } = 24;

  /// <summary>
  /// How much of a drug reaches the streets each cycle, against the built-in baseline.
  /// Clamped to 10..500 on load.
  /// </summary>
  /// <remarks>
  /// The one knob on the whole economy. Raising it floods the market: more stock everywhere,
  /// so prices sit lower and the gap between the cheapest and dearest dealer narrows. Lowering
  /// it starves the market and makes every run more profitable and harder to fill.
  /// </remarks>
  [GenerateIniFileValue]
  public int MarketSupplyPercent { get; set; } = 100;
}

/// <summary>
/// How the street dealers spawn and what killing one costs the world. Every default
/// here reproduces the behaviour it replaced, except <see cref="FleeWantedLevel"/>.
/// </summary>
internal sealed class DealersSection
{
  /// <summary>
  /// Whether dealers are armed. When set, each gets a random weapon from the roster
  /// with 500 rounds, unequipped.
  /// </summary>
  [GenerateIniFileValue]
  public bool HasWeapon { get; set; } = true;

  /// <summary>
  /// Whether dealers spawn wearing body armour.
  /// </summary>
  [GenerateIniFileValue]
  public bool HasArmor { get; set; } = true;

  /// <summary>
  /// How much armour a dealer spawns with. Clamped to 0..200 on load — the
  /// overcharged-armour ceiling, not the 100 bar cap.
  /// </summary>
  [GenerateIniFileValue]
  public int ArmorAmount { get; set; } = 125;

  /// <summary>
  /// Whether a killed dealer leaves his weapon behind to be looted.
  /// </summary>
  [GenerateIniFileValue]
  public bool DropsWeaponOnDeath { get; set; }

  /// <summary>
  /// Whether a killed dealer drops cash. The amount is rolled per spawn between
  /// <see cref="DeathMoneyMin"/> and <see cref="DeathMoneyMax"/>.
  /// </summary>
  [GenerateIniFileValue]
  public bool DropsMoneyOnDeath { get; set; }

  /// <summary>
  /// The least cash a killed dealer can drop. Clamped to 0..65535.
  /// </summary>
  [GenerateIniFileValue]
  public int DeathMoneyMin { get; set; } = 50;

  /// <summary>
  /// The most cash a killed dealer can drop, inclusive. Clamped to 0..65535.
  /// </summary>
  [GenerateIniFileValue]
  public int DeathMoneyMax { get; set; } = 250;

  /// <summary>
  /// Whether a killed dealer stays gone. The cooldown is persisted, keyed by
  /// <see cref="DrugDealer.Id"/>, so he stays gone across a reload.
  /// </summary>
  [GenerateIniFileValue]
  public bool HasCooldown { get; set; } = true;

  /// <summary>
  /// How many in-game days a killed dealer stays gone. Clamped to at least 0 on load;
  /// 0 means he returns on the next tick.
  /// </summary>
  [GenerateIniFileValue]
  public int CooldownDays { get; set; } = 3;

  /// <summary>
  /// The wanted level at which dealers scatter. Clamped to 0..5 on load; <c>0</c>
  /// restores the original stand-your-ground dealer.
  /// </summary>
  [GenerateIniFileValue]
  public int FleeWantedLevel { get; set; } = 1;

  /// <summary>
  /// Whether a dealer has to be found before he appears on the map.
  /// </summary>
  [GenerateIniFileValue]
  public bool RequireDiscovery { get; set; } = true;
}

/// <summary>
/// What the law costs the player: whether a death or an arrest empties the stash, and
/// how often a dealer trade turns out to be a police setup.
/// </summary>
internal sealed class PoliceSection
{
  /// <summary>Whether dying empties the stash.</summary>
  [GenerateIniFileValue]
  public bool LoseDrugsOnDeath { get; set; } = true;

  /// <summary>
  /// Whether being arrested empties the stash. Independent of
  /// <see cref="LoseDrugsOnDeath"/>, which is why the two are checked separately.
  /// </summary>
  [GenerateIniFileValue]
  public bool LoseDrugsWhenBusted { get; set; } = true;

  /// <summary>
  /// The chance, per completed dealer trade, that it was a police setup. Clamped to
  /// 0..100 on load; <c>0</c> turns busts off. Stays a whole number in memory.
  /// </summary>
  [GenerateIniFileValue]
  public int BustChancePercent { get; set; } = 10;

  /// <summary>
  /// How many stars a trafficking bust applies. Clamped to 0..5 on load.
  /// </summary>
  [GenerateIniFileValue]
  public int BustWantedLevel { get; set; } = 2;
}
