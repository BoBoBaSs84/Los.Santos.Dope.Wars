namespace LSDW.UserInterface;

/// <summary>
/// What a key press did to an <see cref="AmountSelector"/>, and with it which part of the
/// window has to be redrawn.
/// </summary>
internal enum AmountChange
{
  /// <summary>Nothing moved: an unbound key, or a bound one against a limit.</summary>
  None,

  /// <summary>The quantity moved.</summary>
  Amount,

  /// <summary>The accept/cancel choice flipped.</summary>
  Ok
}

/// <summary>
/// The quantity a transaction window is sitting on, the ceiling it may not pass, and whether
/// the player is currently over accept or over cancel.
/// </summary>
/// <remarks>
/// <para>
/// Split out of <see cref="TransactionWindow"/>, whose <c>Amount</c> and <c>OkBool</c> setters
/// write captions and sprite colours. The window keeps that; what moves here is the decision
/// about whether a press changes anything at all.
/// </para>
/// <para>
/// The four nav binds are captured at construction, for the same load-bearing reason as
/// <see cref="MenuCursor"/>: <c>SettingsService.Load</c> has to have run first.
/// </para>
/// </remarks>
internal sealed class AmountSelector
{
  private readonly Keys _up;
  private readonly Keys _down;
  private readonly Keys _left;
  private readonly Keys _right;

  /// <summary>How many units the window is offering to trade.</summary>
  public int Amount { get; set; }

  /// <summary>
  /// The most it may offer: the dealer's stock or the player's bag space when buying,
  /// whichever of what they carry and what he still wants is smaller when selling.
  /// </summary>
  /// <remarks>
  /// <see cref="TryProcessKey"/> compares against this with <c>!=</c> rather than <c>&lt;</c>,
  /// which is how it has always read. A caller that sets <see cref="Amount"/> above this would
  /// therefore keep incrementing past it; nothing does, because every window sets the ceiling
  /// and then zeroes the amount.
  /// </remarks>
  public int MaxAmount { get; set; }

  /// <summary>
  /// Whether accept is the highlighted choice. Windows open on accept.
  /// </summary>
  public bool Ok { get; set; }

  /// <summary>
  /// Binds the selector to the four nav keys.
  /// </summary>
  /// <param name="up">The key that raises the quantity.</param>
  /// <param name="down">The key that lowers it.</param>
  /// <param name="left">One of the two keys that flip accept and cancel.</param>
  /// <param name="right">The other.</param>
  public AmountSelector(Keys up, Keys down, Keys left, Keys right)
  {
    _up = up;
    _down = down;
    _left = left;
    _right = right;
  }

  /// <summary>
  /// Applies a key press and reports what it moved.
  /// </summary>
  /// <remarks>
  /// Note the asymmetry, which is deliberate and predates this type: the quantity keys report
  /// nothing when they are against a limit, so a full bag stays silent, while the two toggle
  /// keys always have something to flip and so always report. Left and right do the same
  /// thing — the choice is a pair, and either direction crosses it.
  /// </remarks>
  /// <param name="key">The key pressed.</param>
  /// <returns>Which part of the window changed, if any.</returns>
  public AmountChange TryProcessKey(Keys key)
  {
    if (key == _up)
    {
      if (Amount != MaxAmount)
      {
        Amount++;
        return AmountChange.Amount;
      }
    }
    else if (key == _down)
    {
      if (Amount != 0)
      {
        Amount--;
        return AmountChange.Amount;
      }
    }
    else if (key == _left || key == _right)
    {
      Ok = !Ok;
      return AmountChange.Ok;
    }

    return AmountChange.None;
  }
}
