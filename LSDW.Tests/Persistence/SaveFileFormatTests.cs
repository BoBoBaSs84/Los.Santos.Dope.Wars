using LSDW.Persistence;

using GTA.Chrono;

namespace LSDW.Tests.Persistence;

/// <summary>
/// The save document: what it writes, what it accepts, and what it refuses.
/// </summary>
/// <remarks>
/// The INI format these replaced could not express an empty list, a date or a nested record,
/// and the workarounds for that were the bulk of the persistence code. Several tests below
/// exist to hold the ground that was won — an empty collection round-tripping is a one-line
/// test now and was a sentinel constant before.
/// </remarks>
[TestClass]
public sealed class SaveFileFormatTests
{
  /// <summary>
  /// A date the test can rely on. Copied in <c>SaveDateTests</c> rather than shared: it is two
  /// lines, and an arrange-helper is worth more beside the assertions that use it than in a file
  /// of its own.
  /// </summary>
  private static GameClockDate Date(int year, int month, int day)
  {
    Assert.IsTrue(GameClockDate.TryFromYmd(year, month, day, out GameClockDate date),
      $"{year}-{month}-{day} is not a date the test can use");

    return date;
  }

  private static SaveFile Populated() => new()
  {
    Player = new PlayerSection { TacoOwned = true, TipsShown = [10, 20] },
    Stats = new StatsSection { Earned = 12_345, Spent = 6_789, XP = 42_000 },
    Drugs =
    [
      new DrugEntry { Name = "Cocaine", Amount = 4, BoughtPrice = 850 },
      new DrugEntry { Name = "Meth", Amount = 0, BoughtPrice = 200 }
    ],
    Market = new MarketSection
    {
      Next = Date(2013, 5, 18),
      NextHour = 6,
      Supply = [new SupplyEntry { Name = "Cocaine", Total = 250 }]
    },
    Dealers =
    [
      new DealerEntry
      {
        Id = 3,
        Discovered = true,
        Cooldown = Date(2013, 5, 17),
        Stock = [new StockEntry { Name = "Cocaine", Amount = 7, Price = 820, Demand = 1 }]
      },
      new DealerEntry { Id = 41, Discovered = true }
    ]
  };

  /// <summary>
  /// Writes <paramref name="file"/> and reads it back.
  /// </summary>
  private static SaveFile Reread(SaveFile file)
    => Read(SaveFile.Write(file));

  /// <summary>
  /// Reads a document that is expected to parse. The two assertions are kept apart on purpose: a
  /// combined <c>TryRead(…) &amp;&amp; read is not null</c> cannot say which half gave way.
  /// </summary>
  private static SaveFile Read(string xml)
  {
    Assert.IsTrue(SaveFile.TryRead(xml, out SaveFile? read), "the document did not parse");
    Assert.IsNotNull(read, "the document parsed but produced nothing");

    return read;
  }

  [TestMethod]
  public void Write_ProducesTheDocumentedShape()
  {
    string xml = SaveFile.Write(Populated());

    Assert.Contains("<SaveFile Version=\"2\">", xml);
    Assert.Contains("<Drug Name=\"Cocaine\" Amount=\"4\" BoughtPrice=\"850\" />", xml);
    Assert.Contains("<Market NextDate=\"2013-05-18\" NextHour=\"6\">", xml);
    Assert.Contains("<Supply Name=\"Cocaine\" Total=\"250\" />", xml);
    Assert.Contains("<Dealer Id=\"3\" Discovered=\"true\" CooldownUntil=\"2013-05-17\">", xml);
    Assert.Contains("<Stock Name=\"Cocaine\" Amount=\"7\" Price=\"820\" Demand=\"1\" />", xml);
  }

  [TestMethod]
  public void Write_OmitsTheCooldownAttribute_WhenNoCooldownStands()
  {
    string xml = SaveFile.Write(Populated());

    // Still self-closing, because a dealer with no stock entries has no child elements — which
    // is what a dealer looks like before the first market is dealt.
    Assert.Contains("<Dealer Id=\"41\" Discovered=\"true\" />", xml);
  }

  [TestMethod]
  public void Write_EmitsNoSchemaNamespaces()
  {
    string xml = SaveFile.Write(Populated());

    Assert.DoesNotContain("xmlns:xsi", xml);
    Assert.DoesNotContain("xmlns:xsd", xml);
  }

  [TestMethod]
  public void Write_DeclaresUtf8()
  {
    Assert.Contains("encoding=\"utf-8\"", SaveFile.Write(Populated()));
  }

  [TestMethod]
  public void RoundTrip_PreservesThePlayerSection()
  {
    PlayerSection player = Reread(Populated()).Player;

    Assert.IsTrue(player.TacoOwned);
    Assert.AreSequenceEqual([10, 20], player.TipsShown);
  }

  [TestMethod]
  public void RoundTrip_PreservesTheStats()
  {
    StatsSection stats = Reread(Populated()).Stats;

    Assert.AreEqual(12_345, stats.Earned);
    Assert.AreEqual(6_789, stats.Spent);
    Assert.AreEqual(42_000, stats.XP);
  }

  [TestMethod]
  public void RoundTrip_PreservesTheCarriedDrugs()
  {
    List<DrugEntry> drugs = Reread(Populated()).Drugs;

    Assert.HasCount(2, drugs);
    Assert.AreEqual("Cocaine", drugs[0].Name);
    Assert.AreEqual(4, drugs[0].Amount);
    Assert.AreEqual(850, drugs[0].BoughtPrice);
  }

  [TestMethod]
  public void RoundTrip_PreservesTheDealers()
  {
    // Both of them, and the absent cooldown as well as the standing one — an attribute that is
    // simply missing is the case the reader has to be right about.
    List<DealerEntry> dealers = Reread(Populated()).Dealers;

    Assert.HasCount(2, dealers);
    Assert.AreEqual(3, dealers[0].Id);
    Assert.AreEqual(Date(2013, 5, 17), dealers[0].Cooldown);
    Assert.IsNull(dealers[1].Cooldown);
  }

  [TestMethod]
  public void RoundTrip_PreservesEachDealersStock()
  {
    List<DealerEntry> dealers = Reread(Populated()).Dealers;

    Assert.HasCount(1, dealers[0].Stock);
    Assert.AreEqual("Cocaine", dealers[0].Stock[0].Name);
    Assert.AreEqual(7, dealers[0].Stock[0].Amount);
    Assert.AreEqual(820, dealers[0].Stock[0].Price);
    Assert.AreEqual(1, dealers[0].Stock[0].Demand);
    Assert.IsEmpty(dealers[1].Stock);
  }

  [TestMethod]
  public void RoundTrip_PreservesTheMarket()
  {
    MarketSection market = Reread(Populated()).Market;

    Assert.AreEqual(Date(2013, 5, 18), market.Next);
    Assert.AreEqual(6, market.NextHour);
    Assert.HasCount(1, market.Supply);
    Assert.AreEqual("Cocaine", market.Supply[0].Name);
    Assert.AreEqual(250, market.Supply[0].Total);
  }

  [TestMethod]
  public void RoundTrip_AVersionOneDocument_LeavesTheMarketAbsent()
  {
    // The whole backward-compatibility story: a save from before the market was persisted has
    // no Market element and no Stock children, which SaveService reads as "deal a fresh one" —
    // the same thing a new game does.
    const string legacy = """
      <SaveFile Version="1">
        <Player TacoOwned="true" />
        <Stats Earned="1" Spent="2" XP="3" />
        <Dealers>
          <Dealer Id="3" Discovered="true" />
        </Dealers>
      </SaveFile>
      """;

    SaveFile read = Read(legacy);

    Assert.AreEqual(1, read.Version);
    Assert.IsNull(read.Market.Next);
    Assert.IsEmpty(read.Market.Supply);
    Assert.IsEmpty(read.Dealers[0].Stock);
  }

  [TestMethod]
  public void RoundTrip_EmptyCollections_Survive()
  {
    // The case the INI format could not express at all: it wrote a bare "Key=", which its
    // reader then parsed as one empty element and threw on, failing the entire load.
    SaveFile read = Reread(new SaveFile());

    Assert.IsEmpty(read.Player.TipsShown);
    Assert.IsEmpty(read.Drugs);
    Assert.IsEmpty(read.Dealers);
    Assert.IsEmpty(read.Market.Supply);
    Assert.IsNull(read.Market.Next);
  }

  [TestMethod]
  public void RoundTrip_IsStable()
  {
    string once = SaveFile.Write(Populated());

    Assert.AreEqual(once, SaveFile.Write(Read(once)));
  }

  [TestMethod]
  public void Version_DefaultsToCurrent()
  {
    Assert.AreEqual(SaveFile.CurrentVersion, new SaveFile().Version);
  }

  [TestMethod]
  public void Version_FromTheFile_IsPreserved()
  {
    // SaveService is what refuses a future version; the document just reports it.
    Assert.AreEqual(99, Read("<SaveFile Version=\"99\" />").Version);
  }

  [TestMethod]
  [DataRow("2013-13-45", DisplayName = "Month and day out of range")]
  [DataRow("not-a-date", DisplayName = "Not numeric")]
  [DataRow("2013-05", DisplayName = "Too few parts")]
  [DataRow("", DisplayName = "Empty")]
  public void Cooldown_Unparseable_ReadsAsNoCooldown(string raw)
  {
    // One bad date costs that dealer's cooldown and nothing else — the rest of the file
    // still loads, as it did under the INI reader.
    DealerEntry entry = new() { Id = 3, CooldownUntil = raw };

    Assert.IsNull(entry.Cooldown);
  }

  [TestMethod]
  public void Cooldown_SetToNull_RemovesTheRawValue()
  {
    DealerEntry entry = new() { Cooldown = Date(2013, 5, 17) };

    entry.Cooldown = null;

    Assert.IsNull(entry.CooldownUntil);
  }

  [TestMethod]
  public void Cooldown_PadsToFixedWidth()
  {
    Assert.AreEqual("0007-01-02", new DealerEntry { Cooldown = Date(7, 1, 2) }.CooldownUntil);
  }

  [TestMethod]
  [DataRow("", DisplayName = "Empty")]
  [DataRow("not xml at all", DisplayName = "Not markup")]
  [DataRow("<SaveFile Version=\"1\"", DisplayName = "Truncated mid-tag")]
  [DataRow("<Unexpected />", DisplayName = "Wrong root element")]
  public void TryRead_Malformed_ReturnsFalseRatherThanThrowing(string content)
  {
    Assert.IsFalse(SaveFile.TryRead(content, out SaveFile? read));
    Assert.IsNull(read);
  }

  [TestMethod]
  public void IsLegacyIniFormat_RecognisesTheOldSave()
  {
    // Same filename across the format change, so this is what stops an old file being
    // reported as corruption.
    Assert.IsTrue(SaveFile.IsLegacyIniFormat("[Player]\r\nTacoOwned=True\r\n"));
    Assert.IsTrue(SaveFile.IsLegacyIniFormat("\r\n  [Player]\r\n"), "leading whitespace is tolerated");
  }

  [TestMethod]
  public void IsLegacyIniFormat_DoesNotClaimXml()
  {
    Assert.IsFalse(SaveFile.IsLegacyIniFormat(SaveFile.Write(Populated())));
    Assert.IsFalse(SaveFile.IsLegacyIniFormat(""));
  }
}
