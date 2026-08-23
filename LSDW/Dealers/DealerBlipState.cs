namespace LSDW.Dealers;

/// <summary>
/// How a dealer's map blip should look, given everything currently true about him.
/// </summary>
/// <param name="Alpha">
/// The blip's opacity, <see cref="DealerBlipState.HiddenAlpha"/> or
/// <see cref="DealerBlipState.VisibleAlpha"/>.
/// </param>
/// <param name="Scale">
/// The blip's size, <see cref="DealerBlipState.RestingScale"/> or
/// <see cref="DealerBlipState.OfferScale"/>.
/// </param>
internal readonly record struct DealerBlipState(int Alpha, float Scale)
{
  /// <summary>
  /// A blip the player should not see at all.
  /// </summary>
  public const int HiddenAlpha = 0;

  /// <summary>
  /// A blip at full opacity.
  /// </summary>
  public const int VisibleAlpha = 255;

  /// <summary>
  /// The size of a dealer's blip when he is trading normally.
  /// </summary>
  public const float RestingScale = 0.75f;

  /// <summary>
  /// The size of a dealer's blip while he is running a special offer. Size is the whole
  /// signal — the blip keeps its ordinary colour and does not flash.
  /// </summary>
  public const float OfferScale = 1.0f;

  /// <summary>
  /// Works out how a dealer's blip should look. Total over its inputs — every
  /// combination yields a state, including the ones no caller can currently produce.
  /// </summary>
  /// <param name="discovered">
  /// Whether this dealer should be visible at all. Callers pass
  /// <see cref="DrugDealer.Revealed"/>, not <see cref="DrugDealer.Discovered"/> — the
  /// former folds in the <c>[Dealers] RequireDiscovery</c> setting.
  /// </param>
  /// <param name="onCooldown">Whether he is dead and waiting out his respawn cooldown.</param>
  /// <param name="hasOffer">Whether a special offer currently stands at his spot.</param>
  public static DealerBlipState For(bool discovered, bool onCooldown, bool hasOffer)
    => new(
      discovered && !onCooldown ? VisibleAlpha : HiddenAlpha,
      hasOffer ? OfferScale : RestingScale);
}
