namespace LSDW.Drugs;

/// <summary>
/// The single source of truth for which drugs exist. Every per-drug collection in
/// the mod — the player's stash, the market prices, each dealer's stock, the menu
/// tiles, the transaction window's icon — is derived from this list, so adding a
/// drug means adding one entry here and nothing else.
/// </summary>
internal static class DrugCatalog
{
  /// <remarks>
  /// <see cref="Drug.SupplyBase"/> runs inverse to <see cref="Drug.MarketValue"/> — cocaine
  /// is dear because it is scarce. The two together are the whole economy: they set how much
  /// of a drug reaches the world each cycle, and therefore what dealers charge for it.
  /// </remarks>
  public static IReadOnlyList<Drug> All { get; } =
  [
    new Drug("Cocaine", 850, 5, "commonmenu", "mp_specitem_coke", 0, 0),
    new Drug("Meth", 200, 14, "commonmenu", "mp_specitem_meth", 1, 0),
    new Drug("Acid", 250, 12, "mpinventory", "mp_specitem_safe", 2, 0),
    new Drug("Weed", 10, 40, "commonmenu", "mp_specitem_weed", 2, 1),
    new Drug("Heroin", 700, 6, "commonmenu", "mp_specitem_heroin", 0, 1),
    new Drug("Ketamine", 80, 24, "mpinventory", "drug_trafficking", 1, 1)
  ];

  /// <summary>
  /// The menu layout, indexed as <c>[column][row]</c>, built from each drug's
  /// <see cref="Drug.MenuRow"/> and <see cref="Drug.MenuColumn"/>. Note that Weed
  /// and Heroin land swapped relative to reading order — that is the original
  /// layout, preserved.
  /// </summary>
  public static string[][] Grid { get; } = BuildGrid();

  /// <summary>
  /// The highest valid column index in <see cref="Grid"/>. Read this rather than writing
  /// the bound out: the menus are a 2x3 grid today only because the catalog has six drugs
  /// laid out that way.
  /// </summary>
  public static int LastColumn => Grid.Length - 1;

  /// <summary>
  /// The highest valid row index in <see cref="Grid"/>. Every column is allocated the same
  /// length by <see cref="BuildGrid"/>, so column 0 speaks for all of them.
  /// </summary>
  public static int LastRow => Grid[0].Length - 1;

  public static Drug Get(string name)
  {
    return Lookup[name];
  }

  private static Dictionary<string, Drug> Lookup { get; } = All.ToDictionary(drug => drug.Name);

  /// <summary>
  /// A fresh <c>name -&gt; 0</c> map, in catalog order.
  /// </summary>
  public static Dictionary<string, int> NewAmountMap()
    => All.ToDictionary(drug => drug.Name, drug => 0);

  /// <summary>
  /// A fresh <c>name -&gt; market price</c> map, in catalog order.
  /// </summary>
  public static Dictionary<string, int> NewPriceMap()
    => All.ToDictionary(drug => drug.Name, drug => drug.MarketValue);

  private static string[][] BuildGrid()
  {
    int columns = All.Max(drug => drug.MenuColumn) + 1;
    int rows = All.Max(drug => drug.MenuRow) + 1;

    string[][] grid = new string[columns][];
    for (int column = 0; column < columns; column++)
    {
      grid[column] = new string[rows];
    }

    foreach (Drug drug in All)
    {
      grid[drug.MenuColumn][drug.MenuRow] = drug.Name;
    }

    return grid;
  }
}
