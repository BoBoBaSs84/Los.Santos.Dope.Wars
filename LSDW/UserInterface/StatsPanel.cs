using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using LSDW.Core;
using LSDW.Leveling;

using GTA.UI;

using Font = GTA.UI.Font;
using Screen = GTA.UI.Screen;

namespace LSDW.UserInterface;

/// <summary>
/// The earned/spent/profit box and the level badge and XP bar, both shown while
/// the player is standing at a dealer. The figures themselves live in
/// <see cref="PlayerStats"/>; levelling lives in <see cref="Progression"/>.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = SpriteLifetime.Justification)]
internal sealed class StatsPanel
{
  private readonly Sprite _bgGradient;
  private readonly Sprite _levelBadge;
  private readonly Sprite _xpBarBackground;
  private readonly Sprite _xpBarFill;
  private readonly ContainerElement _mainMenu;
  private readonly TextElement _mainText;
  private readonly PlayerStats _stats;
  private readonly Progression _progression;

  /// <param name="stats">The lifetime figures this panel shows.</param>
  /// <param name="progression">The level and XP the badge and bar show.</param>
  public StatsPanel(PlayerStats stats, Progression progression)
  {
    _stats = stats;
    _progression = progression;

    _mainMenu = new ContainerElement(new PointF(430, 450), new SizeF(300, 100), Color.FromArgb(0, 0, 0, 0));
    _mainMenu.Items.Add(new ContainerElement(new PointF(0, 0), new SizeF(300, 20), Color.WhiteSmoke));
    _mainMenu.Items.Add(new TextElement("Statistics", new PointF(150, -4), 0.6f, Color.Black, Font.RockstarTag, Alignment.Center));
    _mainMenu.Items.Add(new TextElement("Total Money Spent:\nTotal Money Earned:\n\nProfit:", new PointF(6, 30), 0.4f, Color.WhiteSmoke, Font.ChaletLondon, Alignment.Left));
    _mainMenu.Items.Add(_mainText = new TextElement("", new PointF(250, 30), 0.4f, Color.WhiteSmoke, Font.ChaletLondon, Alignment.Left));

    _bgGradient = new Sprite("shared", "bggradient_16x512", new SizeF(300, 200), new PointF(430, 470), Color.White);
    _levelBadge = new Sprite("mprankbadge", "globe", new SizeF(64, 64), new PointF(Screen.Width / 2 - 200, Screen.Height - 150), Color.FromArgb(100, 200, 200, 200));
    _xpBarBackground = new Sprite("timerbars", "damagebarfill_128", new SizeF(200, 20), new PointF(Screen.Width / 2 - 130, Screen.Height - 130), Color.FromArgb(100, 255, 255, 255));
    _xpBarFill = new Sprite("timerbars", "damagebarfill_128", new SizeF(200, 20), new PointF(Screen.Width / 2 - 130, Screen.Height - 130), Color.White);
  }

  /// <summary>
  /// The spent/earned/profit box.
  /// </summary>
  public void DrawPanel()
  {
    int profit = _stats.Earned - _stats.Spent;

    string format = "${0}\n${1}\n\n";
    if (profit < 0)
    {
      format += "~r~${2}";
    }
    else if (profit == 0)
    {
      format += "${2}";
    }
    else
    {
      format += "~g~${2}";
    }

    // Right-align the figures by nudging the column left as they get wider.
    int widest = new[] { _stats.Earned, _stats.Spent, profit }.Max(value => value.ToString(CultureInfo.CurrentCulture).Length);
    _mainText.Position = new PointF(300 - widest * 10 - 15, 30);
    _mainText.Caption = string.Format(CultureInfo.CurrentCulture, format, _stats.Spent, _stats.Earned, profit);

    _mainMenu.Draw();
    _bgGradient.Draw();
  }

  /// <summary>
  /// The level badge and the XP bar. Reads the level and XP <see cref="Progression"/>
  /// maintains; both advance on sales through the event system, not here.
  /// </summary>
  public void DrawLevelBar()
  {
    int goal = Progression.GoalForLevel(_progression.Level);
    int into = _progression.XpIntoCurrentLevel;

    // At the cap there is no next level: XP stops accruing (see Progression.OnDrugsSold),
    // so show a full bar labelled "Max Level" rather than a goal that can never resolve.
    bool maxed = _progression.Level >= Progression.MaxLevel;

    // Below the cap the fill tracks progress; clamp so it never overruns the bar.
    int fillWidth = maxed ? 200 : Convert.ToInt32(Math.Min(1.0, into / (double)goal) * 200.0);

    // Drawn in the original order, interleaved with the text, to keep the native
    // call sequence what it always was.
    _levelBadge.Draw();

    new TextElement(_progression.Level.ToString(CultureInfo.CurrentCulture), new PointF(Screen.Width / 2 - 169, Screen.Height - 140), 0.9f, Color.White, Font.RockstarTag, Alignment.Center).Draw();
    new TextElement(maxed ? "Max Level" : "Next Level ~g~" + into + "~w~/~y~" + goal, new PointF(Screen.Width / 2 - 30, Screen.Height - 110), 0.33f, Color.White, Font.ChaletLondon, Alignment.Center).Draw();

    _xpBarFill.Size = new SizeF(fillWidth, 20);
    _xpBarBackground.Draw();
    _xpBarFill.Draw();
  }
}
