using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using LSDW.Properties;

using GTA.Chrono;

namespace LSDW.Persistence;

/// <summary>
/// The shape of <c>scripts\LSDW.sav</c> — the player's persisted progress,
/// written as XML. The mapping to and from live game state lives in
/// <see cref="Services.SaveService"/>; this is the document and nothing else.
/// </summary>
/// <remarks>
/// <b>Public on purpose, against the internal-by-default convention followed everywhere else
/// in this assembly.</b> <see cref="XmlSerializer"/> generates a serialization assembly that
/// can only reach public types — an internal DTO fails at runtime with "inaccessible due to
/// its protection level", not at compile time. Do not narrow these back to <c>internal</c>.
/// </remarks>
[XmlRoot("SaveFile")]
public sealed class SaveFile
{
  public const string FileName = $"{AssemblyInformation.Title}.sav";

  /// <summary>
  /// The format this build writes. <see cref="Services.SaveService"/> refuses anything
  /// higher rather than misreading a file a newer build produced.
  /// </summary>
  /// <remarks>
  /// <c>2</c> added <see cref="MarketSection"/> and <see cref="DealerEntry.Stock"/>. A version
  /// 1 file still loads: both are simply absent, and an absent market is dealt on the first
  /// tick exactly as a new game's is.
  /// </remarks>
  public const int CurrentVersion = 2;

  /// <summary>
  /// Built once and cached. Only the <see cref="XmlSerializer(Type)"/> overload is cached by
  /// the framework; the others emit a fresh assembly per call and leak it for the life of
  /// the process, which for a script that reloads in-session would accumulate.
  /// </summary>
  private static readonly XmlSerializer Serializer = new(typeof(SaveFile));

  /// <summary>
  /// One empty entry, which suppresses the <c>xsi</c> and <c>xsd</c> declarations the
  /// serializer would otherwise put on the root element.
  /// </summary>
  private static readonly XmlSerializerNamespaces NoNamespaces = new([XmlQualifiedName.Empty]);

  private static readonly XmlWriterSettings WriterSettings = new()
  {
    Indent = true,
    IndentChars = "  "
  };

  /// <summary>
  /// DTDs are refused and no resolver is attached. The save is a local file, but it is also
  /// hand-editable, and an entity-expansion payload in one would hang the game rather than
  /// fail a parse.
  /// </summary>
  private static readonly XmlReaderSettings ReaderSettings = new()
  {
    DtdProcessing = DtdProcessing.Prohibit,
    XmlResolver = null
  };

  [XmlAttribute("Version")]
  public int Version { get; set; } = CurrentVersion;

  [XmlElement("Player")]
  public PlayerSection Player { get; set; } = new();

  [XmlElement("Stats")]
  public StatsSection Stats { get; set; } = new();

  /// <summary>
  /// One entry per drug the player carries, keyed by <see cref="Drugs.Drug.Name"/>. Keyed in
  /// the file rather than by a property per drug, so adding a drug to
  /// <see cref="Drugs.DrugCatalog"/> needs no change here.
  /// </summary>
  [XmlArray("Drugs")]
  [XmlArrayItem("Drug")]
  public List<DrugEntry> Drugs { get; set; } = [];

  /// <summary>
  /// The state of the market: this cycle's supply and when the next one is due.
  /// </summary>
  [XmlElement("Market")]
  public MarketSection Market { get; set; } = new();

  /// <summary>
  /// One entry per dealer in the roster.
  /// </summary>
  /// <remarks>
  /// Every dealer is written, not only the found ones. The supply is dealt across the whole
  /// roster at once, so restoring part of it would leave the totals in
  /// <see cref="MarketSection.Supply"/> describing a market that no longer exists.
  /// </remarks>
  [XmlArray("Dealers")]
  [XmlArrayItem("Dealer")]
  public List<DealerEntry> Dealers { get; set; } = [];

  /// <summary>
  /// Serialises to indented UTF-8 XML.
  /// </summary>
  public static string Write(SaveFile file)
  {
    using Utf8StringWriter output = new();

    using (XmlWriter writer = XmlWriter.Create(output, WriterSettings))
    {
      Serializer.Serialize(writer, file, NoNamespaces);
    }

    return output.ToString();
  }

  /// <summary>
  /// Parses a save document, returning <see langword="false"/> rather than throwing on
  /// anything malformed — a hand-edited or truncated file must not take the script down.
  /// </summary>
  public static bool TryRead(string content, out SaveFile? file)
  {
    try
    {
      using StringReader input = new(content);
      using XmlReader reader = XmlReader.Create(input, ReaderSettings);

      file = Serializer.Deserialize(reader) as SaveFile;
      return file is not null;
    }
    catch (Exception exception) when (exception is InvalidOperationException or XmlException)
    {
      // The serializer wraps parse failures in InvalidOperationException; a few paths let
      // the XmlException through directly.
      file = null;
      return false;
    }
  }

  /// <summary>
  /// Whether <paramref name="content"/> is a save from before the move to XML.
  /// </summary>
  /// <remarks>
  /// The filename did not change across that move, so an old file lands on the XML parser
  /// and would otherwise be reported as unreadable — which reads as corruption rather than
  /// as the format change it is. An INI save opens with its first <c>[Section]</c> header.
  /// </remarks>
  public static bool IsLegacyIniFormat(string content)
    => content.TrimStart().StartsWith("[", StringComparison.Ordinal);

  /// <summary>
  /// A <see cref="StringWriter"/> that reports UTF-8, so the XML declaration says
  /// <c>utf-8</c> rather than the <c>utf-16</c> a string buffer would otherwise imply.
  /// </summary>
  private sealed class Utf8StringWriter() : StringWriter(CultureInfo.InvariantCulture)
  {
    public override Encoding Encoding => Encoding.UTF8;
  }
}

public sealed class PlayerSection
{
  [XmlAttribute("TacoOwned")]
  public bool TacoOwned { get; set; }

  /// <summary>
  /// The levels whose one-time unlock notification has already been shown.
  /// </summary>
  [XmlArray("TipsShown")]
  [XmlArrayItem("Level")]
  public List<int> TipsShown { get; set; } = [];
}

public sealed class StatsSection
{
  [XmlAttribute("Earned")]
  public int Earned { get; set; }

  [XmlAttribute("Spent")]
  public int Spent { get; set; }

  [XmlAttribute("XP")]
  public int XP { get; set; }
}

/// <summary>
/// One drug's carried amount and cost basis.
/// </summary>
public sealed class DrugEntry
{
  [XmlAttribute("Name")]
  public string Name { get; set; } = string.Empty;

  [XmlAttribute("Amount")]
  public int Amount { get; set; }

  [XmlAttribute("BoughtPrice")]
  public int BoughtPrice { get; set; }
}

/// <summary>
/// One dealer's persisted state, keyed by <see cref="Dealers.DrugDealer.Id"/>.
/// </summary>
public sealed class DealerEntry
{
  [XmlAttribute("Id")]
  public int Id { get; set; }

  [XmlAttribute("Discovered")]
  public bool Discovered { get; set; }

  /// <summary>
  /// The death cooldown as <c>yyyy-MM-dd</c>, or <see langword="null"/> when none stands, in
  /// which case the attribute is left out of the file entirely.
  /// </summary>
  /// <remarks>
  /// A string rather than the date itself: <see cref="GameClockDate"/> is a struct with
  /// static factories and read-only properties, which <see cref="XmlSerializer"/> cannot
  /// round-trip. <see cref="Cooldown"/> is the typed way in and out.
  /// </remarks>
  [XmlAttribute("CooldownUntil")]
  public string? CooldownUntil { get; set; }

  /// <summary>
  /// <see cref="CooldownUntil"/> as a date, or <see langword="null"/> when it is absent or
  /// unparseable. An unreadable date costs this dealer's cooldown and nothing else.
  /// </summary>
  [XmlIgnore]
  public GameClockDate? Cooldown
  {
    get => SaveDate.Parse(CooldownUntil);
    set => CooldownUntil = value is GameClockDate date ? SaveDate.Format(date) : null;
  }

  /// <summary>
  /// What this dealer holds, charges and wants, one entry per drug. Empty in a save from
  /// before the market was persisted, which reads as "deal him a fresh one".
  /// </summary>
  [XmlElement("Stock")]
  public List<StockEntry> Stock { get; set; } = [];
}

/// <summary>
/// One dealer's position in one drug: what he has, what he charges for it, and how much more
/// of it he will take.
/// </summary>
/// <remarks>
/// All three are persisted rather than recomputed, because the price is only derived from the
/// share <i>at the moment of distribution</i> — by the time the player saves, they may have
/// traded the stock away, and re-deriving would move a price that is meant to stand for the
/// whole cycle.
/// </remarks>
public sealed class StockEntry
{
  [XmlAttribute("Name")]
  public string Name { get; set; } = string.Empty;

  [XmlAttribute("Amount")]
  public int Amount { get; set; }

  [XmlAttribute("Price")]
  public int Price { get; set; }

  [XmlAttribute("Demand")]
  public int Demand { get; set; }
}

/// <summary>
/// The market itself: how much of each drug this cycle put on the streets, and when the next
/// cycle is due.
/// </summary>
/// <remarks>
/// The totals are persisted because they are not recoverable from the dealers — trading moves
/// stock around, so summing what everyone holds no longer gives what was dealt. They are what
/// every special offer sizes itself against.
/// </remarks>
public sealed class MarketSection
{
  /// <summary>
  /// The date the next distribution falls on as <c>yyyy-MM-dd</c>, or <see langword="null"/>
  /// when no market has been dealt yet.
  /// </summary>
  [XmlAttribute("NextDate")]
  public string? NextDate { get; set; }

  /// <summary>
  /// The hour of <see cref="NextDate"/> the next distribution falls on, 0..23. Whole hours are
  /// enough: cycles are a whole number of hours anchored to midnight, so a distribution never
  /// lands part way through one.
  /// </summary>
  [XmlAttribute("NextHour")]
  public int NextHour { get; set; }

  /// <summary>
  /// <see cref="NextDate"/> as a date, or <see langword="null"/> when it is absent or
  /// unparseable — in which case the next tick deals a fresh market, which is the same thing a
  /// new game does.
  /// </summary>
  [XmlIgnore]
  public GameClockDate? Next
  {
    get => SaveDate.Parse(NextDate);
    set => NextDate = value is GameClockDate date ? SaveDate.Format(date) : null;
  }

  /// <summary>
  /// One entry per drug that reached the streets this cycle.
  /// </summary>
  [XmlElement("Supply")]
  public List<SupplyEntry> Supply { get; set; } = [];
}

/// <summary>
/// How many units of one drug the current cycle dealt across the whole roster.
/// </summary>
public sealed class SupplyEntry
{
  [XmlAttribute("Name")]
  public string Name { get; set; } = string.Empty;

  [XmlAttribute("Total")]
  public int Total { get; set; }
}
