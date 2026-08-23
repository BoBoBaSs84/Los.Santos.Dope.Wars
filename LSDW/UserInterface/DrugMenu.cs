using LSDW.Drugs;
using LSDW.Services;

namespace LSDW.UserInterface;

/// <summary>
/// The 2x3 grid of drugs shown at a dealer: where the selection sits and what each tile is
/// showing. Two of these exist side by side, one to buy from the dealer's stock and one to sell
/// from the player's bag.
/// </summary>
/// <remarks>
/// State only — <see cref="DrugMenuRenderer"/> owns every <c>GTA.UI</c> element and reads this
/// at draw time. It used to own both, which is what made it and everything holding it
/// impossible to construct outside the game.
/// </remarks>
internal sealed class DrugMenu
{
  /// <summary>
  /// Where the selection sits, and the nav keys that move it. The keys are read from the
  /// settings here, at construction, which is why <c>DrugDeal</c> loads them first — see
  /// <see cref="MenuCursor"/>.
  /// </summary>
  private readonly MenuCursor _cursor = new(
    SettingsService.Binds.MenuUp,
    SettingsService.Binds.MenuDown,
    SettingsService.Binds.MenuLeft,
    SettingsService.Binds.MenuRight);

  /// <summary>
  /// The six tiles, keyed by drug name, in catalog order.
  /// </summary>
  /// <remarks>
  /// Tiles are built empty. Every one of them is overwritten from the dealer's stock, or the
  /// player's bag, by <c>DealerTrading.Open</c> before the menu is ever drawn.
  /// </remarks>
  public Dictionary<string, TileState> DrugDictionary { get; } = [];

  /// <summary>Whether this menu is on screen.</summary>
  public bool Visible { get; set; }

  /// <inheritdoc cref="MenuCursor.Row"/>
  public int CurrentRow => _cursor.Row;

  /// <inheritdoc cref="MenuCursor.Column"/>
  public int CurrentColumn => _cursor.Column;

  /// <inheritdoc cref="MenuCursor.CurrentItem"/>
  public string CurrentItem => _cursor.CurrentItem;

  /// <summary>
  /// The tile the selection is sitting on.
  /// </summary>
  public TileState SelectedTile => DrugDictionary[CurrentItem];

  /// <summary>
  /// Builds one empty tile per catalog drug.
  /// </summary>
  public DrugMenu()
  {
    foreach (Drug drug in DrugCatalog.All)
    {
      DrugDictionary.Add(drug.Name, new TileState());
    }
  }

  /// <summary>
  /// Moves the selection and reports whether it went anywhere.
  /// </summary>
  /// <remarks>
  /// The click that used to happen here now belongs to the caller: this type no longer
  /// touches the game, and a sound is the game.
  /// </remarks>
  /// <param name="key">The key pressed.</param>
  /// <returns><see langword="true"/> if the selection moved.</returns>
  public bool ProcessKey(Keys key) => _cursor.TryMove(key);
}
