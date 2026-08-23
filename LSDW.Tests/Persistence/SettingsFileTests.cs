using LSDW.Persistence;

namespace LSDW.Tests.Persistence;

/// <summary>
/// The settings document: what it writes, what it tolerates from a hand edit, and what it
/// refuses.
/// </summary>
/// <remarks>
/// <c>Read</c>, <c>TryRead</c> and <c>Write</c> are generated from the <c>[GenerateIniFile]</c>
/// attributes by <c>BB84.SourceGenerators</c>, so nothing in this project was pinning them and a
/// package bump could change the on-disk format without a single failure. These tests are the
/// guard for that, and they are written against the behaviour rather than the emitted code.
/// <para>
/// The mapping to the live values — the clamps and the deliberate divide-by-100 asymmetry — is
/// <c>SettingsService</c>'s, not this type's, and is not covered here.
/// </para>
/// </remarks>
[TestClass]
public sealed class SettingsFileTests
{
  /// <summary>
  /// A file with every section edited away from its defaults, so a section quietly dropped on the
  /// way through cannot pass by coincidence.
  /// </summary>
  private static SettingsFile Edited() => new()
  {
    Keys = new KeysSection
    {
      MenuUp = "W",
      MenuDown = "S",
      MenuLeft = "A",
      MenuRight = "D",
      Confirm = "Enter",
      TacoSession = "F5",
      BuyProperty = "G"
    },
    Economy = new EconomySection
    {
      TacoPropertyPrice = 750_000,
      TacoMaxMarkupPercent = 80,
      XpMultiplierPercent = 250,
      SpecialOfferChancePercent = 40,
      RestockIntervalHours = 6,
      MarketSupplyPercent = 300
    },
    Dealers = new DealersSection
    {
      HasWeapon = false,
      HasArmor = false,
      ArmorAmount = 25,
      DropsWeaponOnDeath = true,
      DropsMoneyOnDeath = true,
      DeathMoneyMin = 500,
      DeathMoneyMax = 1_500,
      HasCooldown = false,
      CooldownDays = 7,
      FleeWantedLevel = 3,
      RequireDiscovery = false
    },
    Police = new PoliceSection
    {
      LoseDrugsOnDeath = false,
      LoseDrugsWhenBusted = false,
      BustChancePercent = 55,
      BustWantedLevel = 4
    }
  };

  private static SettingsFile Reread(SettingsFile file)
  {
    Assert.IsTrue(SettingsFile.TryRead(SettingsFile.Write(file), out SettingsFile? read));
    Assert.IsNotNull(read);

    return read;
  }

  [TestMethod]
  public void Write_EmitsEverySection()
  {
    // Key=Value under a bracketed heading is the whole reason this file is not XML: players
    // hand-edit their keybinds.
    string ini = SettingsFile.Write(new SettingsFile());

    Assert.Contains("[Keys]", ini);
    Assert.Contains("[Economy]", ini);
    Assert.Contains("[Dealers]", ini);
    Assert.Contains("[Police]", ini);
  }

  [TestMethod]
  public void Write_EmitsTheDefaultsAsPlainKeyValuePairs()
  {
    string ini = SettingsFile.Write(new SettingsFile());

    Assert.Contains("MenuUp=NumPad8", ini);
    Assert.Contains("XpMultiplierPercent=100", ini);
    Assert.Contains("MarketSupplyPercent=100", ini);
    Assert.Contains("HasWeapon=True", ini);
    Assert.Contains("BustWantedLevel=2", ini);
  }

  [TestMethod]
  public void RoundTrip_PreservesEveryKeybind()
  {
    KeysSection keys = Reread(Edited()).Keys;

    Assert.AreEqual("W", keys.MenuUp);
    Assert.AreEqual("S", keys.MenuDown);
    Assert.AreEqual("A", keys.MenuLeft);
    Assert.AreEqual("D", keys.MenuRight);
    Assert.AreEqual("Enter", keys.Confirm);
    Assert.AreEqual("F5", keys.TacoSession);
    Assert.AreEqual("G", keys.BuyProperty);
  }

  [TestMethod]
  public void RoundTrip_PreservesEveryEconomyValue()
  {
    EconomySection economy = Reread(Edited()).Economy;

    Assert.AreEqual(750_000, economy.TacoPropertyPrice);
    Assert.AreEqual(80, economy.TacoMaxMarkupPercent);
    Assert.AreEqual(250, economy.XpMultiplierPercent);
    Assert.AreEqual(40, economy.SpecialOfferChancePercent);
    Assert.AreEqual(6, economy.RestockIntervalHours);
    Assert.AreEqual(300, economy.MarketSupplyPercent);
  }

  [TestMethod]
  public void RoundTrip_PreservesEveryDealerValue()
  {
    DealersSection dealers = Reread(Edited()).Dealers;

    Assert.IsFalse(dealers.HasWeapon);
    Assert.IsFalse(dealers.HasArmor);
    Assert.AreEqual(25, dealers.ArmorAmount);
    Assert.IsTrue(dealers.DropsWeaponOnDeath);
    Assert.IsTrue(dealers.DropsMoneyOnDeath);
    Assert.AreEqual(500, dealers.DeathMoneyMin);
    Assert.AreEqual(1_500, dealers.DeathMoneyMax);
    Assert.IsFalse(dealers.HasCooldown);
    Assert.AreEqual(7, dealers.CooldownDays);
    Assert.AreEqual(3, dealers.FleeWantedLevel);
    Assert.IsFalse(dealers.RequireDiscovery);
  }

  [TestMethod]
  public void RoundTrip_PreservesEveryPoliceValue()
  {
    PoliceSection police = Reread(Edited()).Police;

    Assert.IsFalse(police.LoseDrugsOnDeath);
    Assert.IsFalse(police.LoseDrugsWhenBusted);
    Assert.AreEqual(55, police.BustChancePercent);
    Assert.AreEqual(4, police.BustWantedLevel);
  }

  [TestMethod]
  public void RoundTrip_IsStable()
  {
    // The file is rewritten whenever it is absent, so a write that did not reproduce itself would
    // drift a player's settings a little on every launch.
    string once = SettingsFile.Write(Edited());

    Assert.AreEqual(once, SettingsFile.Write(Reread(Edited())));
  }

  [TestMethod]
  public void Read_SkipsCommentsAndBlankLines()
  {
    // Both comment markers, because a player writing one of them into their own file must not
    // lose the section it sits above.
    const string ini = """
      ; a semicolon comment

      [Economy]
      # a hash comment
      RestockIntervalHours=12
      """;

    Assert.IsTrue(SettingsFile.TryRead(ini, out SettingsFile? read));
    Assert.IsNotNull(read);
    Assert.AreEqual(12, read.Economy.RestockIntervalHours);
  }

  [TestMethod]
  public void Read_TrimsAroundSectionsKeysAndValues()
  {
    // What a hand-edited file actually looks like.
    const string ini = """
        [ Economy ]
          RestockIntervalHours =  12
      """;

    Assert.IsTrue(SettingsFile.TryRead(ini, out SettingsFile? read));
    Assert.IsNotNull(read);
    Assert.AreEqual(12, read.Economy.RestockIntervalHours);
  }

  [TestMethod]
  public void Read_IgnoresAnUnknownSectionAndAnUnknownKey()
  {
    // A key this build dropped, or one a newer build added: skipped, and everything around it
    // still loads.
    const string ini = """
      [Nonsense]
      Whatever=1

      [Economy]
      RestockIntervalHours=12
      NoSuchKey=nonsense
      """;

    Assert.IsTrue(SettingsFile.TryRead(ini, out SettingsFile? read));
    Assert.IsNotNull(read);
    Assert.AreEqual(12, read.Economy.RestockIntervalHours);
  }

  [TestMethod]
  public void Read_IgnoresALineWithNoAssignment()
  {
    const string ini = """
      [Economy]
      this line has no equals sign
      RestockIntervalHours=12
      """;

    Assert.IsTrue(SettingsFile.TryRead(ini, out SettingsFile? read));
    Assert.IsNotNull(read);
    Assert.AreEqual(12, read.Economy.RestockIntervalHours);
  }

  [TestMethod]
  public void Read_AnAbsentSection_KeepsItsDefaults()
  {
    // A player who deletes a section they do not care about gets the defaults for it rather than
    // zeroes, which is what makes the file safe to prune.
    const string ini = """
      [Economy]
      RestockIntervalHours=12
      """;

    Assert.IsTrue(SettingsFile.TryRead(ini, out SettingsFile? read));
    Assert.IsNotNull(read);
    Assert.AreEqual("NumPad8", read.Keys.MenuUp);
    Assert.AreEqual(2, read.Police.BustWantedLevel);
    Assert.IsTrue(read.Dealers.HasWeapon);
  }

  [TestMethod]
  [DataRow("", DisplayName = "Empty")]
  [DataRow("not an ini file at all", DisplayName = "Prose")]
  [DataRow("[Economy]", DisplayName = "A heading and nothing under it")]
  [DataRow("RestockIntervalHours=12", DisplayName = "A value with no section")]
  public void TryRead_ContentWithNothingToApply_IsTheDefaults(string content)
  {
    // Deliberately *not* a failure: TryRead returning false is what makes SettingsService report
    // the file as unreadable, and an empty or sectionless file is not corrupt, it just says
    // nothing. Every value stays at its default.
    Assert.IsTrue(SettingsFile.TryRead(content, out SettingsFile? read));
    Assert.IsNotNull(read);
    Assert.AreEqual(new SettingsFile().Economy.RestockIntervalHours, read.Economy.RestockIntervalHours);
    Assert.AreEqual(new SettingsFile().Keys.MenuUp, read.Keys.MenuUp);
  }

  [TestMethod]
  [DataRow("[Economy]\r\nRestockIntervalHours=twelve", DisplayName = "Words where a number goes")]
  [DataRow("[Economy]\r\nRestockIntervalHours=12.5", DisplayName = "A fraction where an integer goes")]
  [DataRow("[Economy]\r\nTacoPropertyPrice=99999999999999", DisplayName = "Past what an int holds")]
  [DataRow("[Dealers]\r\nHasWeapon=maybe", DisplayName = "Not a boolean")]
  public void TryRead_AValueThatWillNotParse_ReturnsFalseRatherThanThrowing(string content)
  {
    // The rule both files follow: a hand edit must degrade to a notification, never take the
    // script down. SettingsService falls back to the defaults from here.
    Assert.IsFalse(SettingsFile.TryRead(content, out SettingsFile? read));
    Assert.IsNull(read);
  }

  [TestMethod]
  public void Defaults_AreTheOnesTheSettingsAreDocumentedAgainst()
  {
    // This class is the single source of truth for every default, and the live values are seeded
    // from a fresh instance of it before any file is read — so these are what the mod runs on
    // when there is no file at all.
    SettingsFile defaults = new();

    Assert.AreEqual(100, defaults.Economy.XpMultiplierPercent);
    Assert.AreEqual(100, defaults.Economy.MarketSupplyPercent);
    Assert.AreEqual(50, defaults.Economy.TacoMaxMarkupPercent);
    Assert.AreEqual(25, defaults.Economy.SpecialOfferChancePercent);
    Assert.AreEqual(24, defaults.Economy.RestockIntervalHours);
    Assert.AreEqual(200_000, defaults.Economy.TacoPropertyPrice);
  }
}
