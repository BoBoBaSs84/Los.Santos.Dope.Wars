using LSDW.Events;
using LSDW.Services;
using LSDW.UserInterface;

namespace LSDW.Core;

/// <summary>
/// The lifetime money figures, and whether the save loaded. Accumulated from the
/// transaction events (<see cref="DrugsSoldEvent"/>, <see cref="DrugsBoughtEvent"/>),
/// read by <see cref="StatsPanel"/>, and persisted by <see cref="SaveService"/>.
/// </summary>
internal sealed class PlayerStats
{
  /// <summary>
  /// Lifetime money spent buying drugs.
  /// </summary>
  public int Spent { get; set; }

  /// <summary>
  /// Lifetime money taken from selling them.
  /// </summary>
  public int Earned { get; set; }

  /// <summary>
  /// False when the save file could not be read. The stats panel and the XP bar
  /// stay hidden in that case rather than showing a player figures that are not
  /// theirs.
  /// </summary>
  public bool LoadSuccessful { get; set; }

  /// <summary>
  /// Subscribes to the transaction events. Constructed once at startup.
  /// </summary>
  /// <param name="events">The event bus to subscribe on.</param>
  public PlayerStats(EventService events)
  {
    events.Subscribe<DrugsSoldEvent>(sale => Earned += sale.Revenue);
    events.Subscribe<DrugsBoughtEvent>(buy => Spent += buy.Cost);
  }
}
