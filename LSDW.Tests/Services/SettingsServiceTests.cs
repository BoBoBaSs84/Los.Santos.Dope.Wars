using LSDW.Persistence;
using LSDW.Services;

namespace LSDW.Tests.Services;

/// <summary>
/// The mapping from the on-disk settings to the live values: which keys are scaled, which are
/// clamped, and what a hand edit outside the range costs.
/// </summary>
/// <remarks>
/// The four mappers are pure functions of one section and are <see langword="internal"/> for
/// this: the only other way in is <c>Load</c>, which writes a file into the output directory and
/// notifies through the game on every failure path, so driving the clamps through it would test
/// the disk instead of the arithmetic. <b>No test here calls <c>Load</c>.</b>
/// <para>
/// The asymmetry the first tests pin is the documented trap: <c>TacoMaxMarkup</c>,
/// <c>XpMultiplier</c> and <c>MarketSupply</c> are divided by 100 and <c>SpecialOfferChance</c>
/// is not, because it meets a whole-number <c>Random.Next(0, 100)</c> roll. A dropped
/// <c>Percent</c> suffix does not imply scaling — the type does.
/// </para>
/// </remarks>
[TestClass]
public sealed class SettingsServiceTests
{
  private const double Tolerance = 1e-9;

  /// <summary>
  /// The lowest and highest a dealer's death money can be asked for — the ceiling SHVDN
  /// documents on <c>Ped.Money</c>.
  /// </summary>
  private const int MaxPedMoney = ushort.MaxValue;

  [TestMethod]
  public void MapEconomy_ScalesThreeKeysOutOfTheirPercent_AndLeavesTheFourth()
  {
    // One test, both halves, because the whole hazard is telling them apart.
    SettingsService.EconomySettings economy = SettingsService.MapEconomy(new EconomySection
    {
      TacoMaxMarkupPercent = 80,
      XpMultiplierPercent = 250,
      MarketSupplyPercent = 300,
      SpecialOfferChancePercent = 40
    });

    Assert.AreEqual(0.8, economy.TacoMaxMarkup, Tolerance);
    Assert.AreEqual(2.5, economy.XpMultiplier, Tolerance);
    Assert.AreEqual(3.0, economy.MarketSupply, Tolerance);
    Assert.AreEqual(40, economy.SpecialOfferChance, "the chance is compared against a whole-number roll");
  }

  [TestMethod]
  public void MapEconomy_ForTheDefaults_LeavesTheEconomyUnscaled()
  {
    // What the mod runs on with no file, and what the rest of the suite's XP arithmetic assumes.
    SettingsService.EconomySettings economy = SettingsService.MapEconomy(new EconomySection());

    Assert.AreEqual(1.0, economy.XpMultiplier, Tolerance);
    Assert.AreEqual(1.0, economy.MarketSupply, Tolerance);
    Assert.AreEqual(0.5, economy.TacoMaxMarkup, Tolerance);
  }

  [TestMethod]
  [DataRow(-5, 0, DisplayName = "Below the floor")]
  [DataRow(0, 0, DisplayName = "Off")]
  [DataRow(25, 25, DisplayName = "The default")]
  [DataRow(100, 100, DisplayName = "Certain")]
  [DataRow(500, 100, DisplayName = "Above the ceiling")]
  public void MapEconomy_ClampsTheOfferChanceToAPercentage(int written, int expected)
  {
    Assert.AreEqual(expected, SettingsService.MapEconomy(new EconomySection { SpecialOfferChancePercent = written }).SpecialOfferChance);
  }

  [TestMethod]
  [DataRow(0, 1, DisplayName = "Zero would re-roll every tick")]
  [DataRow(-24, 1, DisplayName = "Negative")]
  [DataRow(1, 1, DisplayName = "Hourly, the shortest allowed")]
  [DataRow(24, 24, DisplayName = "The default")]
  [DataRow(168, 168, DisplayName = "Weekly, the longest allowed")]
  [DataRow(1_000, 168, DisplayName = "Beyond a week")]
  public void MapEconomy_ClampsTheRestockIntervalToBetweenAnHourAndAWeek(int written, int expected)
  {
    // Not zero, deliberately: a market that re-rolls every tick is a notification per frame.
    Assert.AreEqual(expected, SettingsService.MapEconomy(new EconomySection { RestockIntervalHours = written }).RestockIntervalHours);
  }

  [TestMethod]
  [DataRow(0, 0.1, DisplayName = "An empty market is refused")]
  [DataRow(5, 0.1, DisplayName = "Below the floor")]
  [DataRow(10, 0.1, DisplayName = "The thinnest allowed")]
  [DataRow(100, 1.0, DisplayName = "The baseline")]
  [DataRow(500, 5.0, DisplayName = "The fattest allowed")]
  [DataRow(900, 5.0, DisplayName = "Above the ceiling")]
  public void MapEconomy_ClampsTheMarketSupplyBeforeScalingIt(int written, double expected)
  {
    // Order matters: clamped as a percent and then divided, so the live value can never be zero
    // — a market with no supply prices every drug at the ceiling and lets nobody trade.
    Assert.AreEqual(expected, SettingsService.MapEconomy(new EconomySection { MarketSupplyPercent = written }).MarketSupply, Tolerance);
  }

  [TestMethod]
  public void MapEconomy_LeavesThePropertyPriceAlone()
  {
    // Documented as unclamped: there is no wrong price for a building, including a free one.
    Assert.AreEqual(0, SettingsService.MapEconomy(new EconomySection { TacoPropertyPrice = 0 }).TacoPropertyPrice);
    Assert.AreEqual(50_000_000, SettingsService.MapEconomy(new EconomySection { TacoPropertyPrice = 50_000_000 }).TacoPropertyPrice);
  }

  [TestMethod]
  public void MapEconomy_LeavesTheTwoScaledMultipliersUnclamped()
  {
    // The asymmetry runs deeper than the divide: these two are scaled but not bounded, so a
    // player who wants tenfold XP gets it. Recorded because it is easy to read the clamps above
    // as applying to the whole section.
    SettingsService.EconomySettings economy = SettingsService.MapEconomy(new EconomySection
    {
      TacoMaxMarkupPercent = 5_000,
      XpMultiplierPercent = 1_000
    });

    Assert.AreEqual(50.0, economy.TacoMaxMarkup, Tolerance);
    Assert.AreEqual(10.0, economy.XpMultiplier, Tolerance);
  }

  [TestMethod]
  [DataRow(-10, 0, DisplayName = "Negative")]
  [DataRow(125, 125, DisplayName = "The default, overcharged past the bar cap")]
  [DataRow(200, 200, DisplayName = "The overcharged ceiling")]
  [DataRow(500, 200, DisplayName = "Above it")]
  public void MapDealers_ClampsTheArmourToTheOverchargedCeiling(int written, int expected)
  {
    // 200, not the 100 the bar shows: overcharged armour is a real state in the game.
    Assert.AreEqual(expected, SettingsService.MapDealers(new DealersSection { ArmorAmount = written }).ArmorAmount);
  }

  [TestMethod]
  public void MapDealers_ClampsTheDeathMoneyToWhatAPedCanCarry()
  {
    SettingsService.DealerSettings dealers = SettingsService.MapDealers(new DealersSection
    {
      DeathMoneyMin = -100,
      DeathMoneyMax = 1_000_000
    });

    Assert.AreEqual(0, dealers.DeathMoneyMin);
    Assert.AreEqual(MaxPedMoney, dealers.DeathMoneyMax);
  }

  [TestMethod]
  public void MapDealers_WithAnInvertedMoneyRange_RaisesTheMaximumToTheMinimum()
  {
    // The roll between the two would throw on an inverted range, so the file is corrected rather
    // than refused: a player who writes min above max gets a fixed drop, not a crash.
    SettingsService.DealerSettings dealers = SettingsService.MapDealers(new DealersSection
    {
      DeathMoneyMin = 900,
      DeathMoneyMax = 100
    });

    Assert.AreEqual(900, dealers.DeathMoneyMin);
    Assert.AreEqual(900, dealers.DeathMoneyMax);
  }

  [TestMethod]
  public void MapDealers_WithAnInvertedRangeOutsideTheCeiling_CorrectsBoth()
  {
    // The maximum is clamped first and only then raised to the minimum, so both land inside what
    // a ped can hold.
    SettingsService.DealerSettings dealers = SettingsService.MapDealers(new DealersSection
    {
      DeathMoneyMin = MaxPedMoney,
      DeathMoneyMax = -50
    });

    Assert.AreEqual(MaxPedMoney, dealers.DeathMoneyMin);
    Assert.AreEqual(MaxPedMoney, dealers.DeathMoneyMax);
  }

  [TestMethod]
  [DataRow(-3, 0, DisplayName = "Negative becomes an immediate return")]
  [DataRow(0, 0, DisplayName = "He returns on the next tick")]
  [DataRow(3, 3, DisplayName = "The default")]
  [DataRow(3_650, 3_650, DisplayName = "No upper bound — a decade is allowed")]
  public void MapDealers_FloorsTheCooldownAtZeroDays(int written, int expected)
  {
    Assert.AreEqual(expected, SettingsService.MapDealers(new DealersSection { CooldownDays = written }).CooldownDays);
  }

  [TestMethod]
  [DataRow(-1, 0, DisplayName = "Negative restores the stand-your-ground dealer")]
  [DataRow(0, 0, DisplayName = "Never flees")]
  [DataRow(1, 1, DisplayName = "The default")]
  [DataRow(5, 5, DisplayName = "Only at five stars")]
  [DataRow(9, 5, DisplayName = "Above what the game has")]
  public void MapDealers_ClampsTheFleeLevelToTheStarsTheGameHas(int written, int expected)
  {
    Assert.AreEqual(expected, SettingsService.MapDealers(new DealersSection { FleeWantedLevel = written }).FleeWantedLevel);
  }

  [TestMethod]
  public void MapDealers_CarriesEveryFlagStraightThrough()
  {
    // Nothing to clamp on a bool, so what is checked is that none of them is dropped or inverted
    // on the way across — they are mapped positionally into a record.
    SettingsService.DealerSettings dealers = SettingsService.MapDealers(new DealersSection
    {
      HasWeapon = false,
      HasArmor = false,
      DropsWeaponOnDeath = true,
      DropsMoneyOnDeath = true,
      HasCooldown = false,
      RequireDiscovery = false
    });

    Assert.IsFalse(dealers.HasWeapon);
    Assert.IsFalse(dealers.HasArmor);
    Assert.IsTrue(dealers.DropsWeaponOnDeath);
    Assert.IsTrue(dealers.DropsMoneyOnDeath);
    Assert.IsFalse(dealers.HasCooldown);
    Assert.IsFalse(dealers.RequireDiscovery);
  }

  [TestMethod]
  [DataRow(-20, 0, DisplayName = "Negative turns busts off")]
  [DataRow(0, 0, DisplayName = "Off")]
  [DataRow(10, 10, DisplayName = "The default")]
  [DataRow(100, 100, DisplayName = "Every trade")]
  [DataRow(250, 100, DisplayName = "Above a percentage")]
  public void MapPolice_ClampsTheBustChanceToAPercentage(int written, int expected)
  {
    // Stays a whole number, like the offer chance and unlike the three scaled economy keys.
    Assert.AreEqual(expected, SettingsService.MapPolice(new PoliceSection { BustChancePercent = written }).BustChance);
  }

  [TestMethod]
  [DataRow(-2, 0, DisplayName = "Negative")]
  [DataRow(2, 2, DisplayName = "The default")]
  [DataRow(5, 5, DisplayName = "The most the game has")]
  [DataRow(11, 5, DisplayName = "Above it")]
  public void MapPolice_ClampsTheBustLevelToTheStarsTheGameHas(int written, int expected)
  {
    Assert.AreEqual(expected, SettingsService.MapPolice(new PoliceSection { BustWantedLevel = written }).BustWantedLevel);
  }

  [TestMethod]
  public void MapPolice_CarriesBothFlagsStraightThrough()
  {
    // Independent of each other on purpose — dying and being arrested are checked separately.
    SettingsService.PoliceSettings police = SettingsService.MapPolice(new PoliceSection
    {
      LoseDrugsOnDeath = true,
      LoseDrugsWhenBusted = false
    });

    Assert.IsTrue(police.LoseDrugsOnDeath);
    Assert.IsFalse(police.LoseDrugsWhenBusted);
  }

  [TestMethod]
  public void MapKeys_ParsesEveryBind()
  {
    SettingsService.KeySettings keys = SettingsService.MapKeys(
      new KeysSection
      {
        MenuUp = "W",
        MenuDown = "S",
        MenuLeft = "A",
        MenuRight = "D",
        Confirm = "Enter",
        TacoSession = "F5",
        BuyProperty = "G"
      },
      new SettingsService.KeySettings());

    Assert.AreEqual(Keys.W, keys.MenuUp);
    Assert.AreEqual(Keys.S, keys.MenuDown);
    Assert.AreEqual(Keys.A, keys.MenuLeft);
    Assert.AreEqual(Keys.D, keys.MenuRight);
    Assert.AreEqual(Keys.Enter, keys.Confirm);
    Assert.AreEqual(Keys.F5, keys.TacoSession);
    Assert.AreEqual(Keys.G, keys.BuyProperty);
  }

  [TestMethod]
  public void MapKeys_IsCaseInsensitive()
  {
    // Players type these by hand, so "numpad8" is the same bind as "NumPad8".
    SettingsService.KeySettings keys = SettingsService.MapKeys(
      new KeysSection { MenuUp = "numpad8", Confirm = "ENTER" },
      new SettingsService.KeySettings());

    Assert.AreEqual(Keys.NumPad8, keys.MenuUp);
    Assert.AreEqual(Keys.Enter, keys.Confirm);
  }

  [TestMethod]
  public void MapKeys_AnUnrecognisedName_CostsThatBindAlone()
  {
    // The point of the per-bind fallback: one typo disables one key rather than failing the file
    // and leaving the player with no menu at all.
    SettingsService.KeySettings fallback = new();

    SettingsService.KeySettings keys = SettingsService.MapKeys(
      new KeysSection { MenuUp = "NotAKeyAtAll", MenuDown = "S" },
      fallback);

    Assert.AreEqual(fallback.MenuUp, keys.MenuUp, "the typo should have fallen back");
    Assert.AreEqual(Keys.S, keys.MenuDown, "the bind beside it should still have parsed");
  }

  [TestMethod]
  public void MapKeys_FallsBackToTheLiveValue_NotTheRecordDefault()
  {
    // Documented distinction: the fallback is whatever is currently bound, so re-reading a file
    // after a bad edit keeps what the player had rather than resetting them to the shipped
    // layout. Load passes the live Binds in for exactly this.
    SettingsService.KeySettings live = new(MenuUp: Keys.Up, MenuDown: Keys.Down);

    SettingsService.KeySettings keys = SettingsService.MapKeys(new KeysSection { MenuUp = "rubbish" }, live);

    Assert.AreEqual(Keys.Up, keys.MenuUp);
    Assert.AreNotEqual(new SettingsService.KeySettings().MenuUp, keys.MenuUp);
  }

  [TestMethod]
  public void MapKeys_ANumericName_IsTakenAsAKeyCode()
  {
    // Enum.TryParse accepts a number as well as a name, so a hand-edited "999" binds an unnamed
    // key code rather than falling back. Harmless — no keyboard produces it, so the bind simply
    // never fires — and recorded here because it is not obvious from the fallback above.
    SettingsService.KeySettings keys = SettingsService.MapKeys(
      new KeysSection { MenuUp = "999" },
      new SettingsService.KeySettings());

    Assert.AreEqual((Keys)999, keys.MenuUp);
  }

  [TestMethod]
  public void MapKeys_ForTheDefaults_ProducesTheShippedLayout()
  {
    SettingsService.KeySettings keys = SettingsService.MapKeys(new KeysSection(), new SettingsService.KeySettings());

    Assert.AreEqual(Keys.NumPad8, keys.MenuUp);
    Assert.AreEqual(Keys.NumPad2, keys.MenuDown);
    Assert.AreEqual(Keys.NumPad4, keys.MenuLeft);
    Assert.AreEqual(Keys.NumPad6, keys.MenuRight);
    Assert.AreEqual(Keys.NumPad5, keys.Confirm);
    Assert.AreEqual(Keys.D2, keys.TacoSession);
    Assert.AreEqual(Keys.E, keys.BuyProperty);
  }
}
