using LSDW.Drugs;

namespace LSDW.UserInterface;

/// <summary>
/// Where a <see cref="DrugMenu"/>'s selection sits in the catalog grid, and how the nav keys
/// move it.
/// </summary>
/// <remarks>
/// <para>
/// Split out of <see cref="DrugMenu"/>, which cannot be constructed without the game. The
/// bounds come from <see cref="DrugCatalog"/>'s grid rather than from the 2x3 the six current
/// drugs happen to produce, so adding a drug moves them on its own.
/// </para>
/// <para>
/// <b>The four keys are captured at construction, and that timing is load-bearing.</b> This is
/// where <c>DrugDeal</c>'s first ordering rule now lands: <c>SettingsService.Load</c> has to
/// have run before a cursor is built, or the player rebinds a key and nothing happens. The
/// cursor takes them as arguments rather than reading the settings itself, which is also what
/// makes it testable with binds no player has.
/// </para>
/// </remarks>
internal sealed class MenuCursor
{
  private readonly Keys _up;
  private readonly Keys _down;
  private readonly Keys _left;
  private readonly Keys _right;

  /// <summary>The row the selection sits on, from the top.</summary>
  public int Row { get; private set; }

  /// <summary>The column the selection sits in, from the left.</summary>
  public int Column { get; private set; }

  /// <summary>
  /// The drug under the selection.
  /// </summary>
  /// <remarks>
  /// Derived rather than stored. It used to be a field refreshed inside
  /// <c>DrugMenu.UpdateColors</c>, which meant every path that moved the cursor had to
  /// remember to call that method to keep the name honest.
  /// </remarks>
  public string CurrentItem => DrugCatalog.Grid[Column][Row];

  /// <summary>
  /// Binds the cursor to four nav keys, at the top-left of the grid.
  /// </summary>
  /// <param name="up">The key that moves the selection up a row.</param>
  /// <param name="down">The key that moves it down a row.</param>
  /// <param name="left">The key that moves it left a column.</param>
  /// <param name="right">The key that moves it right a column.</param>
  public MenuCursor(Keys up, Keys down, Keys left, Keys right)
  {
    _up = up;
    _down = down;
    _left = left;
    _right = right;
  }

  /// <summary>
  /// Moves the selection if <paramref name="key"/> is a nav key with somewhere to go.
  /// </summary>
  /// <remarks>
  /// The return value is what gates the menu's click: a key pressed against the edge of the
  /// grid moves nothing and must stay silent, or every wall in the menu answers back.
  /// </remarks>
  /// <param name="key">The key pressed.</param>
  /// <returns><see langword="true"/> if the selection moved.</returns>
  public bool TryMove(Keys key)
  {
    if (key == _up && Row != 0)
    {
      Row--;
    }
    else if (key == _down && Row != DrugCatalog.LastRow)
    {
      Row++;
    }
    else if (key == _left && Column != 0)
    {
      Column--;
    }
    else if (key == _right && Column != DrugCatalog.LastColumn)
    {
      Column++;
    }
    else
    {
      return false;
    }

    return true;
  }
}
