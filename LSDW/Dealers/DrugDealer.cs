using LSDW.Services;

using GTA;
using GTA.Chrono;
using GTA.Math;

namespace LSDW.Dealers;

/// <summary>
/// One street dealer: a fixed spot on the map, his ped and blip, and the book
/// <see cref="DealerMarket"/> deals him.
/// </summary>
internal sealed class DrugDealer
{
  /// <summary>
  /// The blip marking this dealer's spot, or <see langword="null"/> before
  /// <see cref="CreateBlip"/> has run. Never removed once made, except by
  /// <see cref="DealerWorld.Cleanup"/>.
  /// </summary>
  /// <remarks>
  /// <b>Not created by the constructor</b>, which is the only reason this type can be built
  /// outside the game: <c>Blip.Create</c> is a native. <see cref="DealerRoster.CreateDealers"/>
  /// makes the blip immediately after constructing each dealer, and is the only place that
  /// should — a dealer built anywhere else is a dealer with no marker on the map.
  /// </remarks>
  public Blip? DealBlip { get; private set; }

  /// <summary>
  /// Whether the player has ever come within spawn range of this dealer. Persisted
  /// under <see cref="Id"/>.
  /// </summary>
  public bool Discovered { get; set; }

  /// <summary>
  /// Whether this dealer should be visible at all. The only place
  /// <c>[Dealers] RequireDiscovery</c> is read.
  /// </summary>
  public bool Revealed => Discovered || !SettingsService.Dealers.RequireDiscovery;

  /// <summary>
  /// What this dealer holds, charges, wants and is offering — everything about him that is
  /// not a position in the world.
  /// </summary>
  /// <remarks>
  /// A property initialiser rather than a constructor assignment, so it is in place before
  /// anything — including <see cref="CreateBlip"/> — can read it.
  /// </remarks>
  public DealerBook Book { get; } = new();

  /// <summary>
  /// This spot's stable identity, from <see cref="DealerRoster.Spots"/>.
  /// </summary>
  public int Id { get; }

  /// <summary>
  /// Where the blip sits, and where the ped spawns.
  /// </summary>
  public Vector3 Position { get; }

  /// <summary>
  /// The heading the ped spawns with.
  /// </summary>
  public float Heading { get; }

  /// <summary>
  /// Null whenever the player is out of range and the ped is despawned.
  /// </summary>
  public Ped? MainPed { get; set; }

  /// <summary>
  /// Whether this dealer has been provoked and is fighting the player. Set once when
  /// first damaged, reset when the ped is respawned.
  /// </summary>
  public bool Hostile { get; set; }

  /// <summary>
  /// Whether this dealer has bolted because the player is wanted. Cleared when the
  /// player is clean again or when a fresh ped is spawned.
  /// </summary>
  public bool Fleeing { get; set; }

  /// <summary>
  /// The in-game date this dealer's death cooldown runs out, or <see langword="null"/>
  /// if he is not on one. Persisted, keyed by <see cref="Id"/>.
  /// </summary>
  public GameClockDate? CooldownUntil { get; set; }

  /// <summary>
  /// Initialises a new dealer at a spot, with no stock, no prices and no appetite. The first
  /// <see cref="DealerMarket.Tick"/> of a session overwrites all three.
  /// </summary>
  /// <remarks>
  /// Touches nothing in the game — see <see cref="DealBlip"/> for why, and for who makes the
  /// blip instead.
  /// </remarks>
  /// <param name="location">The position of the dealer's blip on the map.</param>
  /// <param name="heading">The heading of the dealer's ped when spawned.</param>
  /// <param name="id">This spot's stable id — see <see cref="Id"/>.</param>
  public DrugDealer(Vector3 location, float heading, int id)
  {
    Position = location;
    Heading = heading;
    Id = id;
  }

  /// <summary>
  /// Puts this dealer's blip on the map, and applies its first appearance. Called once, by
  /// <see cref="DealerRoster.CreateDealers"/>, immediately after construction.
  /// </summary>
  /// <remarks>
  /// Guarded against a second call the same way <c>TacoProperty.ShowBlip</c> is: a second blip
  /// would leave the first orphaned underneath it, invisible because the two match.
  /// </remarks>
  public void CreateBlip()
  {
    if (DealBlip != null)
    {
      return;
    }

    DealBlip = Blip.Create(Position);
    DealBlip.Sprite = BlipSprite.Drugs;
    DealBlip.IsShortRange = true;

    RefreshBlip();
  }

  /// <summary>
  /// Recomputes this dealer's blip appearance from his state and applies it. Call after
  /// changing <see cref="Discovered"/>, <see cref="CooldownUntil"/> or the book's offer.
  /// </summary>
  /// <remarks>
  /// A no-op before <see cref="CreateBlip"/> has run, which is what lets every site call it
  /// unconditionally — including the ones a test drives, where there is no blip at all.
  /// </remarks>
  public void RefreshBlip()
  {
    if (DealBlip is not Blip blip)
    {
      return;
    }

    DealerBlipState state = DealerBlipState.For(
      Revealed,
      CooldownUntil is not null,
      Book.HasOffer);

    blip.Alpha = state.Alpha;
    blip.Scale = state.Scale;
  }
}
