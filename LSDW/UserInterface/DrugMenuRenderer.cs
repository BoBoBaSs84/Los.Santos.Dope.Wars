using System.Diagnostics.CodeAnalysis;

using LSDW.Drugs;

using GTA.UI;

using Font = GTA.UI.Font;
using Screen = GTA.UI.Screen;

namespace LSDW.UserInterface;

/// <summary>
/// Draws one <see cref="DrugMenu"/>: its frame, its background, and its six tiles.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = SpriteLifetime.Justification)]
internal sealed class DrugMenuRenderer
{
  private readonly ContainerElement _mainMenu;
  private readonly Sprite _bgGradient;
  private readonly Dictionary<string, DrugTileRenderer> _tiles = [];

  /// <summary>
  /// Builds the frame and one tile renderer per catalog drug.
  /// </summary>
  /// <param name="title">The header, e.g. <c>"BUY"</c>.</param>
  /// <param name="offset">Pixels to shift the pane left of the screen's right edge.</param>
  public DrugMenuRenderer(string title, int offset = 0)
  {
    _mainMenu = new ContainerElement(new PointF(Screen.Width - 500 - offset, 100), new SizeF(300, 500), Color.FromArgb(0, 0, 0, 0));
    _mainMenu.Items.Add(new ContainerElement(new PointF(0, 0), new SizeF(300, 40), Color.WhiteSmoke));
    _mainMenu.Items.Add(new TextElement(title, new PointF(150, -4), 1f, Color.Black, Font.RockstarTag, Alignment.Center));
    _bgGradient = new Sprite("shared", "bggradient_16x512", new SizeF(300, 400), new PointF(Screen.Width - 500 - offset, 138), Color.White);

    foreach (Drug drug in DrugCatalog.All)
    {
      _tiles.Add(drug.Name, new DrugTileRenderer(drug.Name, drug.SpriteDictionary, drug.SpriteName, drug.MenuRow, drug.MenuColumn, offset));
    }
  }

  /// <summary>
  /// Draws the menu, or nothing at all while it is hidden.
  /// </summary>
  /// <param name="menu">The menu to draw.</param>
  public void Draw(DrugMenu menu)
  {
    if (!menu.Visible)
    {
      return;
    }

    _mainMenu.Draw();
    _bgGradient.Draw();

    // Iterated over the state's dictionary, not this one, so the draw order stays catalog
    // order exactly as it was when the tiles owned their own state.
    foreach (KeyValuePair<string, TileState> tile in menu.DrugDictionary)
    {
      _tiles[tile.Key].Draw(tile.Value, selected: tile.Key == menu.CurrentItem);
    }
  }
}
