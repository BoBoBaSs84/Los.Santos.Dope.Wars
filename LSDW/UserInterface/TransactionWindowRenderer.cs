using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using LSDW.Drugs;

using GTA.UI;

using Font = GTA.UI.Font;
using Screen = GTA.UI.Screen;

namespace LSDW.UserInterface;

/// <summary>
/// Draws one <see cref="TransactionWindow"/>: the frame, the drug, the amount, the price
/// comparison, the running total, and the accept/cancel pair.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = SpriteLifetime.Justification)]
internal sealed class TransactionWindowRenderer
{
  private readonly int _offset;
  private readonly ContainerElement _mainMenu;
  private readonly TextElement _drugNameText;
  private readonly TextElement _amountText;
  private readonly TextElement _descriptionText;
  private readonly TextElement _moneyText;
  private readonly TextElement _summary;
  private readonly Sprite _bgGradient;
  private readonly Sprite _arrowUpandDown;
  private readonly Sprite _ok;
  private readonly Sprite _cancel;

  /// <summary>
  /// The icon for the drug currently being traded, and the name it was built for.
  /// </summary>
  /// <remarks>
  /// <b>Swapped when the drug changes, never once per frame.</b> This is the one sprite here
  /// that is replaced rather than kept, so the old one is disposed first — see
  /// <see cref="SpriteLifetime"/>. Rebuilding it on every <see cref="Draw"/> would leak a
  /// texture at frame rate instead of once per window opening.
  /// </remarks>
  private Sprite? _drugSprite;

  /// <inheritdoc cref="_drugSprite"/>
  private string? _drugSpriteFor;

  /// <summary>
  /// Builds the frame and every fixed element.
  /// </summary>
  /// <param name="action">The header, e.g. <c>"BUY"</c>.</param>
  /// <param name="sellingDrugs">
  /// Which side this window is for, which decides the three-line label it opens with.
  /// </param>
  /// <param name="offset">Pixels to shift the pane left of the screen's right edge.</param>
  public TransactionWindowRenderer(string action, bool sellingDrugs, int offset = 0)
  {
    _offset = offset;

    _mainMenu = new ContainerElement(new PointF(Screen.Width - 500 - offset, 100), new SizeF(300, 500), Color.FromArgb(0, 0, 0, 0));
    _mainMenu.Items.Add(new ContainerElement(new PointF(0, 0), new SizeF(300, 40), Color.WhiteSmoke));
    _mainMenu.Items.Add(new TextElement(action, new PointF(150, -4), 1f, Color.Black, Font.RockstarTag, Alignment.Center));

    _bgGradient = new Sprite("shared", "bggradient_16x512", new SizeF(300, 400), new PointF(Screen.Width - 500 - offset, 138), Color.White);
    _drugNameText = new TextElement("DefaultDrug", new PointF(Screen.Width - 415 - offset, 165), 0.7f, Color.White, Font.HouseScript, Alignment.Left);
    _amountText = new TextElement("0", new PointF(Screen.Width - 270 - offset, 165), 0.8f, Color.White, Font.HouseScript, Alignment.Center);

    _descriptionText = sellingDrugs
      ? new TextElement("Bought Price\nSale Price\nProfit", new PointF(Screen.Width - 480 - offset, 230), 0.5f)
      : new TextElement("Market Value\nPrice\nProfit", new PointF(Screen.Width - 480 - offset, 230), 0.5f);

    _moneyText = new TextElement("", new PointF(Screen.Width - 230 - offset, 230), 0.5f, Color.White, Font.ChaletLondon, Alignment.Center);
    _summary = new TextElement("", new PointF(Screen.Width - 350 - offset, 340), 0.5f, Color.White, Font.ChaletLondon, Alignment.Center);

    _arrowUpandDown = new Sprite("commonmenu", "shop_arrows_upanddown", new SizeF(50, 50), new PointF(Screen.Width - 255 - offset, 155), Color.White);
    _ok = new Sprite("commonmenu", "shop_box_tick", new SizeF(80, 80), new PointF(Screen.Width - 440 - offset, 410), Color.White);
    _cancel = new Sprite("commonmenu", "shop_box_cross", new SizeF(80, 80), new PointF(Screen.Width - 330 - offset, 410), Color.FromArgb(50, 255, 255, 255));
  }

  /// <summary>
  /// Draws the window, or nothing at all while it is hidden.
  /// </summary>
  /// <param name="window">The window to draw.</param>
  public void Draw(TransactionWindow window)
  {
    if (!window.Visible)
    {
      return;
    }

    Refresh(window);

    _mainMenu.Draw();
    _bgGradient.Draw();
    _drugSprite?.Draw();
    _drugNameText.Draw();
    _amountText.Draw();
    _arrowUpandDown.Draw();
    _descriptionText.Draw();
    _moneyText.Draw();
    _summary.Draw();
    _ok.Draw();
    _cancel.Draw();
  }

  /// <summary>
  /// Brings every caption, colour and the drug icon up to date with
  /// <paramref name="window"/>, before any of them is drawn.
  /// </summary>
  private void Refresh(TransactionWindow window)
  {
    SwapDrugSprite(window.DrugName);

    _drugNameText.Caption = window.DrugName;
    _amountText.Caption = window.Amount.ToString(CultureInfo.CurrentCulture);

    if (window.ShowsMarketLine)
    {
      _moneyText.Caption = TransactionSummary.MoneyTextWithMarket(window.GlobalValue, window.NormalValue, window.Value);
      _descriptionText.Caption = "Market Value\nBought Price\nSale Price\nProfit";
    }
    else
    {
      _moneyText.Caption = TransactionSummary.MoneyText(window.SellingDrugs, window.Value, window.NormalValue);
    }

    _summary.Caption = TransactionSummary.SummaryText(window.SellingDrugs, window.Amount, window.Value, window.NormalValue);

    _ok.Color = window.OkBool ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(50, 255, 255, 255);
    _cancel.Color = window.OkBool ? Color.FromArgb(50, 255, 255, 255) : Color.FromArgb(255, 255, 255, 255);
  }

  /// <inheritdoc cref="_drugSprite"/>
  private void SwapDrugSprite(string drugName)
  {
    if (drugName.Length == 0 || drugName == _drugSpriteFor)
    {
      return;
    }

    Drug drug = DrugCatalog.Get(drugName);

    _drugSprite?.Dispose();
    _drugSprite = new Sprite(drug.SpriteDictionary, drug.SpriteName, new SizeF(60, 60), new PointF(Screen.Width - 480 - _offset, 150), Color.White);
    _drugSpriteFor = drugName;
  }
}
