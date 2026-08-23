namespace LSDW.Drugs;

/// <summary>
/// One tradeable drug: its reference price, its icon, and where it sits in the
/// dealer menu. Everything the rest of the mod knows about a drug comes from
/// here — see <see cref="DrugCatalog"/>.
/// </summary>
/// <param name="Name">Also the key of every per-drug dictionary in the mod, and the key written to the save file.</param>
/// <param name="MarketValue">Reference price. A dealer's own price is derived from it and his share of the supply; the player's bought price is tracked separately.</param>
/// <param name="SupplyBase">
/// Units of this drug entering the world per dealer per market cycle at level 1 — so the
/// world total is this times the roster size, scaled by level. Runs <i>inverse</i> to
/// <paramref name="MarketValue"/>: the expensive drugs are the scarce ones, which is what
/// keeps them worth trafficking. See <see cref="Dealers.MarketSupply"/>.
/// </param>
/// <param name="SpriteDictionary">The texture dictionary the menu icon is streamed from.</param>
/// <param name="SpriteName">The texture within <paramref name="SpriteDictionary"/> used as the menu icon.</param>
/// <param name="MenuRow">Row in the 2x3 dealer menu, 0..2.</param>
/// <param name="MenuColumn">Column in the 2x3 dealer menu, 0..1.</param>
internal sealed record Drug(
  string Name,
  int MarketValue,
  int SupplyBase,
  string SpriteDictionary,
  string SpriteName,
  int MenuRow,
  int MenuColumn);
