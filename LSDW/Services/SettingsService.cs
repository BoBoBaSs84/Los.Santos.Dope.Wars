using LSDW.Persistence;

namespace LSDW.Services;

/// <summary>
/// The live, parsed view of <c>scripts\LSDW.ini</c>: the keybinds the mod
/// listens for and the gameplay tunables it reads. <see cref="SettingsFile"/> is the
/// on-disk shape; this class owns the file I/O and the mapping to the typed values
/// every subsystem reads.
/// </summary>
internal static class SettingsService
{
  private static readonly string SettingsPath = Path.Combine(AppContext.BaseDirectory, SettingsFile.FileName);

  /// <summary>
  /// The ceiling SHVDN documents on <c>Ped.Money</c>.
  /// </summary>
  private const int MaxPedMoney = ushort.MaxValue;

  /// <summary>
  /// The shortest market interval a hand-edited file can ask for. Not <c>0</c>: that
  /// would re-roll every tick, which is a notification per frame.
  /// </summary>
  private const int MinIntervalHours = 1;

  /// <summary>The longest market interval — one in-game week.</summary>
  private const int MaxIntervalHours = 168;

  /// <summary>
  /// The thinnest market a hand-edited file can ask for, as a percent. Not <c>0</c>: a market
  /// with no supply prices every drug at the ceiling and lets nobody trade.
  /// </summary>
  private const int MinMarketSupplyPercent = 10;

  /// <summary>The fattest market — five times the baseline.</summary>
  private const int MaxMarketSupplyPercent = 500;

  /// <summary>
  /// The defaults every static below is seeded from. Declared first because static field
  /// initializers run top-to-bottom and they map it.
  /// </summary>
  private static readonly SettingsFile Defaults = new();

  /// <summary>
  /// The <c>[Keys]</c> binds. Named <c>Binds</c> rather than <c>Keys</c> because
  /// <see cref="System.Windows.Forms.Keys"/> is the type of every property inside it.
  /// </summary>
  public static KeySettings Binds { get; private set; } = MapKeys(Defaults.Keys, new KeySettings());

  /// <summary>The <c>[Economy]</c> tunables.</summary>
  public static EconomySettings Economy { get; private set; } = MapEconomy(Defaults.Economy);

  /// <summary>The <c>[Dealers]</c> tunables.</summary>
  public static DealerSettings Dealers { get; private set; } = MapDealers(Defaults.Dealers);

  /// <summary>The <c>[Police]</c> tunables.</summary>
  public static PoliceSettings Police { get; private set; } = MapPolice(Defaults.Police);

  /// <summary>
  /// Reads the settings file into the statics above, writing a default one first if there
  /// is none.
  /// </summary>
  /// <remarks>
  /// Takes the notification service rather than reaching for one, which is what keeps this
  /// type static without holding any seam of its own: the two failure paths below are the
  /// only things here that touch the game.
  /// </remarks>
  /// <param name="notifications">Where an unreadable or throwing file is reported.</param>
  public static void Load(INotificationService notifications)
  {
    try
    {
      SettingsFile settings;

      if (!File.Exists(SettingsPath))
      {
        // No file yet: drop one out so the player has something to edit, and run this
        // session on those same defaults.
        settings = new SettingsFile();
        File.WriteAllText(SettingsPath, SettingsFile.Write(settings));
      }
      else if (!SettingsFile.TryRead(File.ReadAllText(SettingsPath), out SettingsFile? parsed) || parsed is null)
      {
        // TryRead, not Read: a hand-edited settings file must not take the mod down. Fall
        // back to the defaults and map them, so the live values match what is on disk.
        notifications.Ticker("Error while loading settings: " + SettingsFile.FileName + " is not readable. Using defaults.");
        settings = new SettingsFile();
      }
      else
      {
        settings = parsed;
      }

      Binds = MapKeys(settings.Keys, Binds);
      Economy = MapEconomy(settings.Economy);
      Dealers = MapDealers(settings.Dealers);
      Police = MapPolice(settings.Police);
    }
    catch (Exception ex)
    {
      notifications.Ticker("Error while loading settings: " + ex.Message);
    }
  }

  /// <summary>
  /// Maps the <c>[Keys]</c> section, falling each bind back to <paramref name="fallback"/> on an
  /// unrecognised name rather than to any record default.
  /// </summary>
  internal static KeySettings MapKeys(KeysSection section, KeySettings fallback) => new(
    ParseKey(section.MenuUp, fallback.MenuUp),
    ParseKey(section.MenuDown, fallback.MenuDown),
    ParseKey(section.MenuLeft, fallback.MenuLeft),
    ParseKey(section.MenuRight, fallback.MenuRight),
    ParseKey(section.Confirm, fallback.Confirm),
    ParseKey(section.TacoSession, fallback.TacoSession),
    ParseKey(section.BuyProperty, fallback.BuyProperty));

  /// <summary>
  /// Maps the <c>[Economy]</c> section. <b>Three of these divide by 100 and one does not</b>
  /// — see <see cref="EconomySettings"/>. <c>TacoPropertyPrice</c> is deliberately
  /// unclamped: there is no wrong price for a building.
  /// </summary>
  internal static EconomySettings MapEconomy(EconomySection section)
  {
    // The three scaled keys become fractions; the chance stays the whole number it is compared
    // against. Names follow the target property, not the on-disk key.
    double tacoMaxMarkup = section.TacoMaxMarkupPercent / 100.0;
    double xpMultiplier = section.XpMultiplierPercent / 100.0;
    int specialOfferChance = Clamp(section.SpecialOfferChancePercent, 0, 100);
    int restockIntervalHours = Clamp(section.RestockIntervalHours, MinIntervalHours, MaxIntervalHours);

    // Clamped before scaling, and never to 0: an empty market has no prices to speak of, and
    // the ceiling stops a hand-edited file from flooding the streets past the point of a game.
    double marketSupply = Clamp(section.MarketSupplyPercent, MinMarketSupplyPercent, MaxMarketSupplyPercent) / 100.0;

    return new(
      section.TacoPropertyPrice, tacoMaxMarkup, xpMultiplier, specialOfferChance,
      restockIntervalHours, marketSupply);
  }

  /// <summary>
  /// Maps the <c>[Dealers]</c> section.
  /// </summary>
  internal static DealerSettings MapDealers(DealersSection section)
  {
    int armorAmount = Clamp(section.ArmorAmount, 0, 200);
    int deathMoneyMin = Clamp(section.DeathMoneyMin, 0, MaxPedMoney);
    int deathMoneyMax = Math.Max(deathMoneyMin, Clamp(section.DeathMoneyMax, 0, MaxPedMoney));
    int cooldownDays = Math.Max(0, section.CooldownDays);
    int fleeWantedLevel = Clamp(section.FleeWantedLevel, 0, 5);

    return new(
      section.HasWeapon, section.HasArmor, armorAmount, section.DropsWeaponOnDeath, section.DropsMoneyOnDeath,
      deathMoneyMin, deathMoneyMax, section.HasCooldown, cooldownDays, fleeWantedLevel, section.RequireDiscovery);
  }

  /// <summary>Maps the <c>[Police]</c> section.</summary>
  internal static PoliceSettings MapPolice(PoliceSection section)
  {
    int bustChancePercent = Clamp(section.BustChancePercent, 0, 100);
    int bustWantedLevel = Clamp(section.BustWantedLevel, 0, 5);

    return new(section.LoseDrugsOnDeath, section.LoseDrugsWhenBusted, bustChancePercent, bustWantedLevel);
  }

  /// <summary>
  /// Parses a <see cref="Keys"/> name, keeping <paramref name="fallback"/> on an
  /// unrecognised value so one typo disables a single bind rather than the file.
  /// </summary>
  private static Keys ParseKey(string name, Keys fallback)
    => Enum.TryParse(name, ignoreCase: true, out Keys key) ? key : fallback;

  /// <summary>
  /// Keeps a hand-edited value inside the range its consumer can take. Hand-rolled
  /// because <c>net48</c> predates <c>Math.Clamp</c>.
  /// </summary>
  private static int Clamp(int value, int min, int max)
    => value < min ? min : value > max ? max : value;

  /// <summary>
  /// The parsed <c>[Keys]</c> binds.
  /// </summary>
  internal sealed record KeySettings(
    Keys MenuUp = Keys.NumPad8,
    Keys MenuDown = Keys.NumPad2,
    Keys MenuLeft = Keys.NumPad4,
    Keys MenuRight = Keys.NumPad6,
    Keys Confirm = Keys.NumPad5,
    Keys TacoSession = Keys.D2,
    Keys BuyProperty = Keys.E);

  /// <summary>
  /// The parsed <c>[Economy]</c> tunables.
  /// </summary>
  /// <remarks>
  /// <b>Three of these are scaled out of their on-disk percent and one is not.</b>
  /// <paramref name="TacoMaxMarkup"/>, <paramref name="XpMultiplier"/> and
  /// <paramref name="MarketSupply"/> are fractions; <paramref name="SpecialOfferChance"/> stays
  /// a whole number 0..100 because it meets a <c>Random.Next(0, 100)</c> roll. A dropped
  /// <c>Percent</c> suffix does not imply scaling — check the type, not the name.
  /// </remarks>
  /// <param name="TacoPropertyPrice">What the Taco Bomb front costs. Unclamped.</param>
  /// <param name="TacoMaxMarkup">The taco-van markup ceiling as a fraction (0.5 = a 0–50% range).</param>
  /// <param name="XpMultiplier">The XP multiplier as a factor (1.0 = unscaled).</param>
  /// <param name="SpecialOfferChance">A whole number 0..100, deliberately unscaled.</param>
  /// <param name="RestockIntervalHours">In-game hours between market cycles, 1..168.</param>
  /// <param name="MarketSupply">The supply multiplier as a factor (1.0 = the built-in baseline).</param>
  internal sealed record EconomySettings(
    int TacoPropertyPrice,
    double TacoMaxMarkup,
    double XpMultiplier,
    int SpecialOfferChance,
    int RestockIntervalHours,
    double MarketSupply);

  /// <summary>
  /// The parsed <c>[Dealers]</c> tunables.
  /// </summary>
  internal sealed record DealerSettings(
    bool HasWeapon,
    bool HasArmor,
    int ArmorAmount,
    bool DropsWeaponOnDeath,
    bool DropsMoneyOnDeath,
    int DeathMoneyMin,
    int DeathMoneyMax,
    bool HasCooldown,
    int CooldownDays,
    int FleeWantedLevel,
    bool RequireDiscovery);

  /// <summary>
  /// The parsed <c>[Police]</c> tunables.
  /// </summary>
  internal sealed record PoliceSettings(
    bool LoseDrugsOnDeath,
    bool LoseDrugsWhenBusted,
    int BustChance,
    int BustWantedLevel);
}
