using LSDW.Helpers;

namespace LSDW.Drugs;

/// <summary>
/// Everything the player is carrying, plus the market they trade against. Reading is open;
/// the two carried maps are read-only and change through <see cref="Add"/>,
/// <see cref="AddPurchased"/>, <see cref="Remove"/>, <see cref="ClearCarried"/> and
/// <see cref="Restore"/>.
/// </summary>
/// <remarks>
/// This used to carry a hard constraint — <i>must depend on <see cref="DrugCatalog"/> and
/// nothing else</i> — because the state below was static and therefore read while other
/// types were still initialising, so a reference back into the mod could deadlock type
/// initialisation: a hang at script load with no exception to read. That is gone. The state
/// is now an instance built by <see cref="Core.DrugDeal"/> before anything that reads it, so
/// there is no type initialiser left to deadlock. The constraint is recorded rather than
/// deleted because the hazard it guarded against is real for any static that runs code.
/// <para>
/// <see cref="MarketValue"/> and <see cref="DrugNames"/> stay static: they are projections
/// of the catalog, identical for every player and never written. Threading them as instance
/// state would have pulled this type into <see cref="Dealers.DealerMarket"/>, which wants
/// nothing else from it.
/// </para>
/// </remarks>
internal sealed class PlayerStash
{
  private readonly Dictionary<string, int> _carriedAmounts = DrugCatalog.NewAmountMap();
  private readonly Dictionary<string, int> _paidPrices = DrugCatalog.NewPriceMap();

  /// <summary>
  /// Reference prices. Dealers randomise around these; what the player paid is <see cref="BoughtPrice"/>.
  /// </summary>
  public static IReadOnlyDictionary<string, int> MarketValue { get; } = DrugCatalog.NewPriceMap();

  /// <summary>
  /// Catalog order, and it must stay that way: the market passes take consecutive draws
  /// off the shared <see cref="RandomHelper.Random"/> as they walk this list.
  /// </summary>
  public static IReadOnlyList<string> DrugNames { get; } = [.. DrugCatalog.All.Select(drug => drug.Name)];

  /// <summary>
  /// What the player is carrying, keyed by drug name.
  /// </summary>
  public IReadOnlyDictionary<string, int> Drugs => _carriedAmounts;

  /// <summary>
  /// What the player actually paid, per drug — the basis for the sell-side profit figure.
  /// </summary>
  public IReadOnlyDictionary<string, int> BoughtPrice => _paidPrices;

  /// <summary>
  /// How many items the player is carrying, across all drugs. Derived, not counted —
  /// don't reintroduce a stored count for a sum over six entries.
  /// </summary>
  public int Bag => _carriedAmounts.Values.Sum();

  /// <summary>
  /// Raised by levelling up — see <see cref="Leveling.Progression"/>.
  /// </summary>
  public int BagSize { get; set; } = 100;

  /// <summary>
  /// Which one-time hint notifications have already fired. Persisted, so they never repeat.
  /// </summary>
  public List<int> TipsShown { get; } = [];

  /// <summary>
  /// Adds drugs the player did not pay for, leaving <see cref="BoughtPrice"/> alone.
  /// </summary>
  public void Add(string drug, int amount)
    => _carriedAmounts[drug] += amount;

  /// <summary>
  /// Adds drugs the player bought, recording what they paid as the new cost basis.
  /// </summary>
  public void AddPurchased(string drug, int amount, int pricePaid)
  {
    _carriedAmounts[drug] += amount;
    _paidPrices[drug] = pricePaid;
  }

  /// <summary>
  /// Takes drugs off the player, leaving <see cref="BoughtPrice"/> alone so the sale
  /// that prompted it can still read the cost basis afterwards.
  /// </summary>
  public void Remove(string drug, int amount)
    => _carriedAmounts[drug] -= amount;

  /// <summary>
  /// Empties the bag, as a death or an arrest does. Prices are left untouched.
  /// </summary>
  public void ClearCarried()
  {
    foreach (string drug in DrugNames)
    {
      _carriedAmounts[drug] = 0;
    }
  }

  /// <summary>
  /// Sets one drug's carried amount and cost basis outright, for the save loader.
  /// </summary>
  public void Restore(string drug, int amount, int boughtPrice)
  {
    _carriedAmounts[drug] = amount;
    _paidPrices[drug] = boughtPrice;
  }
}
