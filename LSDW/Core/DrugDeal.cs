using LSDW.Abstractions;
using LSDW.Activities;
using LSDW.Dealers;
using LSDW.Drugs;
using LSDW.Events;
using LSDW.Helpers;
using LSDW.Leveling;
using LSDW.Services;
using LSDW.UserInterface;

using GTA;
using GTA.Native;

namespace LSDW.Core;

/// <summary>
/// The ScriptHookVDotNet entry point. SHVDN constructs this once and drives
/// everything through <see cref="OnTick"/> and <see cref="OnKeyDown"/>.
/// </summary>
/// <remarks>
/// It builds the mod's whole object graph in its constructor and holds it in the fields
/// below, so every subsystem's lifetime is this script's. What is left <see langword="static"/>
/// is either pure (<c>Perks</c>, the three market types, the catalog and rosters) or the two
/// file services, <c>SettingsService</c> and <c>SaveService</c> — and those two are the only
/// state a script reload does not clear.
/// </remarks>
public class DrugDeal : Script
{
  private readonly INotificationService _notifications;
  private readonly IAudioProvider _audio;
  private readonly IPlayerMoney _money;
  private readonly EventService _events;
  private readonly PlayerStash _stash;
  private readonly PlayerStats _stats;
  private readonly Progression _progression;
  private readonly RewardSystem _rewards;
  private readonly PlayerWallet _wallet;
  private readonly DealerPolice _police;
  private readonly TacoMinigame _taco;
  private readonly VanStealing _vans;
  private readonly DealerWorld _world;
  private readonly DealerTrading _trading;
  private readonly SaveService _save;

  public DrugDeal()
  {
    // The object graph below is built in dependency order, and mostly says so itself: an
    // argument cannot be passed before it exists. The two comments left in this constructor
    // mark the orderings that are *not* expressed that way, and each says what breaking it
    // costs. Do not add a third without the same.

    // The one thing built before the settings, and the only reason that is safe: the
    // providers are generated pass-throughs to GTA statics and the service over them holds
    // nothing, so there is no state here for a wrong order to catch out. Everything that
    // notifies takes it as an argument, including the Load below.
    _notifications = new NotificationService(new NotificationProvider(), new ScreenProvider());
    _audio = new AudioProvider();
    _money = new PlayerMoney();

    // STILL LOAD-BEARING. Settings first: TradePanes.CreateLive below builds the TradeSides,
    // whose DrugMenus and TransactionWindows capture the configured nav keys into readonly
    // fields at construction. Loading the file afterwards gives the panes the default binds
    // and no error — the player rebinds a key and nothing happens.
    SettingsService.Load(_notifications);

    _events = new EventService();
    _stash = new PlayerStash();

    _stats = new PlayerStats(_events);
    _progression = new Progression(_events);
    _taco = new TacoMinigame(_events, _notifications, _audio, _money, _progression, _stash);
    _vans = new VanStealing(_events, _notifications, _stash);

    // Held, not discarded, although neither is ever called again: both do their whole job
    // from the bus, and the field is what says this script owns that subscription.
    _wallet = new PlayerWallet(_events, _money);
    _police = new DealerPolice(_events, _notifications, _stash);

    _rewards = new RewardSystem(_events, _notifications, _progression, _stash);

    _world = new DealerWorld(_events, _notifications, _progression);
    _trading = new DealerTrading(_events, _notifications, _audio, _money, _progression, _stash, TradePanes.CreateLive(_stash, _stats, _progression));

    Tick += OnTick;
    KeyDown += OnKeyDown;
    Aborted += OnAborted;

    // Last, because it takes the dealer world: that argument is what replaced the pair of
    // Func<> hooks this constructor used to wire by hand, and with them the ordering rule
    // that wiring them after Load silently dropped every persisted cooldown, every
    // discovered dealer and the whole saved market. Subscribing this late is safe because
    // construction publishes nothing — ApplyLoadedState below is the first publisher, and
    // every subscriber is registered by then.
    _save = new SaveService(_events, _notifications, _world, _taco.Property, _stats, _progression, _stash);

    _save.Load();

    // STILL LOAD-BEARING, and genuinely sequential: it reads what Load just wrote. Rebuilds
    // the level from the loaded XP and replays its level-ups so bag size and unlocks
    // re-apply; TipsShown keeps the notifications from repeating.
    _progression.ApplyLoadedState();
  }

  private void OnTick(object sender, EventArgs e)
  {
    Ped character = Game.Player.Character;

    DetectConfiscation(character);

    // The panes are drawn before the dealer world ticks and the HUD after it, as
    // they always have been: draw order is native call order.
    _trading.DrawPanes();

    UpdateDealerMenus(_world.OnTick(character));

    if (_trading.IsOpen)
    {
      _trading.DrawHud();
    }

    if (_vans.VanUnlocked)
    {
      _vans.OnTick();
    }

    if (_taco.MinigameUnlocked)
    {
      _taco.OnTick(character);
    }
  }

  /// <summary>
  /// Removes everything the mod put into the world when SHVDN unloads or reloads the
  /// script, so a reload does not leave the previous run's blips, peds and vehicles
  /// behind.
  /// </summary>
  private void OnAborted(object sender, EventArgs e)
  {
    _world.Cleanup();
    _taco.Cleanup();
    _vans.Cleanup();
  }

  /// <summary>
  /// Detects the two states that cost the player their carried stash — dying, or being
  /// arrested and publishes the matching event.
  /// </summary>
  private void DetectConfiscation(Ped character)
  {
    if (character.IsDead)
    {
      _events.Publish(new PlayerDiedEvent());
    }
    else if (WasJustArrested())
    {
      _events.Publish(new PlayerArrestedEvent());
    }
  }

  /// <summary>
  /// Whether the player was arrested within the last 10 ticks. <c>-1</c> is the
  /// native's "never arrested this session" and is excluded, so a fresh save does not
  /// read as a bust.
  /// </summary>
  private static bool WasJustArrested()
    => Function.Call<int>(Hash.GET_TIME_SINCE_LAST_ARREST) is < 10 and not -1;

  /// <summary>
  /// Opens the dealer's menus when the player walks up to one, and closes them
  /// again when they walk off or pick up a wanted level.
  /// </summary>
  private void UpdateDealerMenus(DrugDealer? atDealer)
  {
    if (ScriptHelper.WantedLevel != 0)
    {
      _trading.Close();
    }
    else if (atDealer == null)
    {
      if (_trading.IsOpen)
      {
        _trading.Close();
      }
    }
    else if (!_trading.IsOpen)
    {
      _trading.Open(atDealer);
    }
    else if (_trading.CurrentDealer != atDealer)
    {
      // Only closes; the `!IsOpen` branch above reopens on the next tick.
      _trading.Close();
    }
  }

  private void OnKeyDown(object sender, KeyEventArgs e)
  {
    // Both sets are rebindable and default to disjoint values (NumPad vs 2/E), so out of
    // the box they never contend.
    _trading.ProcessKey(e.KeyCode);
    _taco.ProcessKey(e.KeyCode);
  }
}
