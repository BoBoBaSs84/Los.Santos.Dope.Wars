using System.Globalization;

using GTA.UI;

using Font = GTA.UI.Font;
using Screen = GTA.UI.Screen;

namespace LSDW.UserInterface;

/// <summary>
/// The three-row box shown in the corner during a taco session: tacos sold, drugs left
/// and the running profit.
/// </summary>
/// <remarks>
/// Owned by <see cref="Activities.TacoMinigame"/> rather than static, so the backgrounds
/// below die with the session instead of outliving a script reload. The sprites themselves
/// are still never disposed — see <see cref="SpriteLifetime"/>; the point is ownership.
/// </remarks>
internal sealed class TacoHud
{
  /// <summary>
  /// Y offsets of the three HUD rows, from the bottom of the screen.
  /// </summary>
  private static readonly int[] HudRowOffsets = [133, 95, 56];

  /// <summary>
  /// The three row backgrounds, built on the first <see cref="Draw"/> and kept.
  /// </summary>
  private Sprite[]? _rowBackgrounds;

  /// <summary>
  /// Draws the session box.
  /// </summary>
  /// <param name="tacosSold">Sales completed this session.</param>
  /// <param name="drugsLeft">Units left in the player's stash.</param>
  /// <param name="profit">Takings so far this session.</param>
  public void Draw(int tacosSold, int drugsLeft, int profit)
  {
    string sold = tacosSold.ToString(CultureInfo.CurrentCulture);
    string left = drugsLeft.ToString(CultureInfo.CurrentCulture);

    new TextElement(sold, new PointF(Screen.Width - 45 - sold.Length * 5, Screen.Height - 130), 0.4f, Color.White, Font.ChaletLondon, Alignment.Center).Draw();
    new TextElement(left, new PointF(Screen.Width - 45 - left.Length * 5, Screen.Height - 92), 0.4f, Color.White, Font.ChaletLondon, Alignment.Center).Draw();
    new TextElement("~g~$" + profit, new PointF(Screen.Width - 50 - profit.ToString(CultureInfo.CurrentCulture).Length * 5, Screen.Height - 53), 0.4f, Color.White, Font.ChaletLondon, Alignment.Center).Draw();
    new TextElement("~b~Drugs Sold:\n\nDrugs Left:\n\nProfit:", new PointF(Screen.Width - 180, Screen.Height - 130), 0.4f, Color.White, Font.ChaletLondon, Alignment.Left).Draw();

    _rowBackgrounds ??= [.. HudRowOffsets.Select(y => new Sprite("shared", "bggradient_16x512", new SizeF(230, 30), new PointF(Screen.Width - 250, Screen.Height - y), Color.FromArgb(200, 255, 255, 255), 90f))];

    foreach (Sprite background in _rowBackgrounds)
    {
      background.Draw();
    }
  }
}
