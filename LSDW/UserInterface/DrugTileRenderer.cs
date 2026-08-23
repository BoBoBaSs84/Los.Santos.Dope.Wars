using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using GTA.UI;

using Font = GTA.UI.Font;
using Screen = GTA.UI.Screen;

namespace LSDW.UserInterface;

/// <summary>
/// Draws one drug tile: icon, name, amount, and the arrow showing whether this dealer's price
/// beats the tile's reference price.
/// </summary>
/// <remarks>
/// The appearance half of what <c>DrugMenuItem</c> used to be; <see cref="TileState"/> is the
/// other half. Everything here is a colour, a position or a rotation — the decisions those
/// express belong to the state.
/// </remarks>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = SpriteLifetime.Justification)]
internal sealed class DrugTileRenderer
{
  /// <summary>
  /// The alpha a tile is drawn at, which is how availability and selection are shown.
  /// </summary>
  private enum ItemState
  {
    StateUnavailable = 20,
    StateAvailable = 120,
    StateSelected = 255
  }

  private static readonly Color DimmedColor = Color.FromArgb(255, 30, 30, 30);

  private readonly Sprite _iconSprite;
  private readonly Sprite _statusSprite;
  private readonly TextElement _drugName;
  private readonly TextElement _amountText;

  /// <summary>
  /// Places one tile in its grid slot.
  /// </summary>
  /// <param name="name">The drug's name, shown as the label.</param>
  /// <param name="spriteDict">The texture dictionary the icon comes from.</param>
  /// <param name="spriteName">The icon texture.</param>
  /// <param name="row">The tile's row in the grid.</param>
  /// <param name="column">The tile's column in the grid.</param>
  /// <param name="offset">The owning side's layout offset.</param>
  public DrugTileRenderer(string name, string spriteDict, string spriteName, int row, int column, int offset = 0)
  {
    // Screen.Width is a float in v3, so the layout maths is float too. Every value
    // here is a whole number that a float represents exactly, so the pixels land
    // where they always did.
    float iconX = (column == 0) ? (Screen.Width - 450 - offset) : (Screen.Width - 330 - offset);
    float labelX = (column == 0) ? (Screen.Width - 420 - offset) : (Screen.Width - 300 - offset);
    float amountX = (column == 0) ? (Screen.Width - 460 - offset) : (Screen.Width - 340 - offset);
    int rowY = 100 * row;

    // Color.White is passed explicitly: GTA.UI.Sprite defaults to WhiteSmoke, which
    // would dim every icon by ten levels against the gradient behind it.
    _iconSprite = new Sprite(spriteDict, spriteName, new SizeF(60, 60), new PointF(iconX, 180 + rowY), Color.White);
    _drugName = new TextElement(name, new PointF(labelX, 160 + rowY), 0.5f, Color.White, Font.HouseScript, Alignment.Center);
    _amountText = new TextElement("0", new PointF(amountX, 230 + rowY), 0.7f, Color.White, Font.HouseScript, Alignment.Left);
    _statusSprite = new Sprite("commonmenu", "arrowleft", new SizeF(40, 40), new PointF(labelX, 210 + rowY), Color.ForestGreen, 90f);
  }

  /// <summary>
  /// Draws the tile for the state it is in.
  /// </summary>
  /// <remarks>
  /// The colours are applied here rather than pushed in by whoever last changed the state,
  /// which is what <c>DrugMenu.UpdateColors</c> existed to do and what every caller of it had
  /// to remember. Only the <see cref="Draw"/> calls reach the game, so the native call order
  /// is what it always was.
  /// </remarks>
  /// <param name="state">What this tile is showing.</param>
  /// <param name="selected">Whether the cursor is sitting on it.</param>
  public void Draw(TileState state, bool selected)
  {
    // Selection wins over availability: the tile under the cursor is drawn at full alpha even
    // when there is nothing to trade on it, so the cursor never disappears.
    SetColor(selected
      ? ItemState.StateSelected
      : state.Tradeable ? ItemState.StateAvailable : ItemState.StateUnavailable);

    _iconSprite.Draw();
    _drugName.Draw();
    _amountText.Caption = state.Amount.ToString(CultureInfo.CurrentCulture);
    _amountText.Draw();

    if (!state.Tradeable)
    {
      return;
    }

    // Green arrow up for a good deal, red arrow down for a bad one, nothing at all when the
    // two prices match. Which way round that runs is the state's decision, not this method's.
    switch (state.Deal)
    {
      case TileDeal.Good:
        _statusSprite.Color = Color.DarkSeaGreen;
        _statusSprite.Rotation = 90f;
        _statusSprite.Draw();
        break;

      case TileDeal.Bad:
        _statusSprite.Color = Color.IndianRed;
        _statusSprite.Rotation = -90f;
        _statusSprite.Draw();
        break;

      case TileDeal.Neutral:
      default:
        break;
    }
  }

  private void SetColor(ItemState state)
  {
    Color color = (state == ItemState.StateUnavailable)
      ? DimmedColor
      : Color.FromArgb((int)state, 255, 255, 255);

    _iconSprite.Color = color;
    _drugName.Color = color;
    _amountText.Color = color;
  }
}
