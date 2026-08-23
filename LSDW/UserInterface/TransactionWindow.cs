using LSDW.Services;

namespace LSDW.UserInterface;

/// <summary>
/// The confirm screen's state: which drug, at what price against what reference, how many, and
/// whether the player is over accept or over cancel.
/// </summary>
/// <remarks>
/// <para>
/// State only — <see cref="TransactionWindowRenderer"/> owns every <c>GTA.UI</c> element and
/// reads this at draw time.
/// </para>
/// <para>
/// The properties are plain, and that is the point of the change: they used to be computed
/// setters that wrote captions and read each other, which is why
/// <c>DealerTrading.OpenPurchaseWindow</c> and <c>OpenSaleWindow</c> each carried a comment
/// warning that their assignment order was load-bearing. It no longer is.
/// </para>
/// </remarks>
internal sealed class TransactionWindow
{
  /// <summary>
  /// The quantity, its ceiling, the accept/cancel choice, and the nav binds that move them.
  /// The keys are read from the settings at construction — see <see cref="AmountSelector"/>.
  /// </summary>
  private readonly AmountSelector _selector = new(
    SettingsService.Binds.MenuUp,
    SettingsService.Binds.MenuDown,
    SettingsService.Binds.MenuLeft,
    SettingsService.Binds.MenuRight);

  /// <summary>
  /// Which side of the trade this window is for. Decides the sign of every comparison it
  /// shows.
  /// </summary>
  public bool SellingDrugs { get; set; }

  /// <summary>Whether this window is on screen.</summary>
  public bool Visible { get; set; }

  /// <summary>The drug being traded.</summary>
  public string DrugName { get; set; } = string.Empty;

  /// <summary>What this dealer charges or pays, per unit.</summary>
  public int Value { get; set; }

  /// <summary>
  /// The reference price: the market value when buying, what the player paid when selling.
  /// </summary>
  public int NormalValue { get; set; }

  /// <summary>
  /// The market value, set only when selling — where there are three prices to show rather
  /// than two. Assigning it is what switches this window to the four-line layout.
  /// </summary>
  public int GlobalValue
  {
    get;
    set
    {
      field = value;
      ShowsMarketLine = true;
    }
  }

  /// <summary>
  /// Whether the four-line sell layout is in force. Set by assigning
  /// <see cref="GlobalValue"/>, never directly, so the flag and the figure cannot disagree.
  /// </summary>
  public bool ShowsMarketLine { get; private set; }

  /// <inheritdoc cref="AmountSelector.Amount"/>
  public int Amount
  {
    get => _selector.Amount;
    set => _selector.Amount = value;
  }

  /// <inheritdoc cref="AmountSelector.MaxAmount"/>
  public int MaxAmount
  {
    get => _selector.MaxAmount;
    set => _selector.MaxAmount = value;
  }

  /// <inheritdoc cref="AmountSelector.Ok"/>
  public bool OkBool
  {
    get => _selector.Ok;
    set => _selector.Ok = value;
  }

  /// <summary>
  /// Applies a key press and reports what it moved, so the caller can click.
  /// </summary>
  /// <param name="key">The key pressed.</param>
  public AmountChange ProcessKey(Keys key) => _selector.TryProcessKey(key);
}
