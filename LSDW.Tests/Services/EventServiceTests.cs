using LSDW.Events;
using LSDW.Services;

namespace LSDW.Tests.Services;

/// <summary>
/// The bus: who a published event reaches, and who it must not.
/// </summary>
/// <remarks>
/// Every subsystem in the mod is wired through this type, and until it stopped being a static
/// singleton none of it could be covered: a test could subscribe but never unsubscribe, so the
/// first test's handlers were still listening in the last one. The bus takes no GTA types and
/// holds nothing but a dictionary, so each test below builds its own and discards it.
/// <para>
/// The last test covers a handler that publishes from inside another publish — three deep, which
/// is what one sale actually does. Re-entrant <i>subscribing</i> is not covered because it is not
/// supported: <c>Publish</c> walks the live handler list, so a handler that subscribes to the type
/// it is handling throws an <see cref="InvalidOperationException"/>. Nothing can reach it — every
/// subscription happens in a constructor, and the first publish is
/// <c>Progression.ApplyLoadedState</c>, by which point every subscriber exists. Recorded rather
/// than asserted: a test would turn the limitation into a promise.
/// </para>
/// </remarks>
[TestClass]
public sealed class EventServiceTests
{
  [TestMethod]
  public void Publish_WithNoSubscriber_IsANoOp()
  {
    EventService events = new();

    events.Publish(new SaveRequestedEvent());

    // The TryGetValue miss is the whole path, so what is left to check is that nothing was
    // held onto: a handler subscribing afterwards must not be handed the event that already
    // went out.
    bool seen = false;
    events.Subscribe<SaveRequestedEvent>(_ => seen = true);

    Assert.IsFalse(seen);
  }

  [TestMethod]
  public void Publish_HandsTheHandlerThePublishedInstance()
  {
    // Not a copy and not a re-created event: PlayerStats and Progression both read fields off
    // the same DrugsSoldEvent, and DealerTradeEvent carries a live DrugDealer.
    EventService events = new();
    DrugsSoldEvent published = new(Revenue: 1200, Profit: 350);
    DrugsSoldEvent? received = null;

    events.Subscribe<DrugsSoldEvent>(sale => received = sale);
    events.Publish(published);

    Assert.AreSame(published, received);
  }

  [TestMethod]
  public void Publish_WithSeveralHandlers_InvokesEveryOneInSubscriptionOrder()
  {
    EventService events = new();
    List<int> order = [];

    events.Subscribe<SaveRequestedEvent>(_ => order.Add(1));
    events.Subscribe<SaveRequestedEvent>(_ => order.Add(2));
    events.Subscribe<SaveRequestedEvent>(_ => order.Add(3));

    events.Publish(new SaveRequestedEvent());

    Assert.AreSequenceEqual([1, 2, 3], order);
  }

  [TestMethod]
  public void Publish_IsKeyedByEventType_AndReachesNothingElse()
  {
    // The property the whole design rests on: RewardSystem hears about level-ups without
    // hearing about sales, so neither has to know the other exists.
    EventService events = new();
    bool sold = false;
    bool leveled = false;

    events.Subscribe<DrugsSoldEvent>(_ => sold = true);
    events.Subscribe<LeveledUpEvent>(_ => leveled = true);

    events.Publish(new DrugsSoldEvent(100, 50));

    Assert.IsTrue(sold);
    Assert.IsFalse(leveled);
  }

  [TestMethod]
  public void Subscribe_TheSameHandlerTwice_InvokesItTwice()
  {
    // Documents rather than endorses: the list does not dedupe, so a subsystem constructed
    // twice against one bus would double every reaction. Nothing does today, and a script
    // reload builds a new bus with it.
    EventService events = new();
    int calls = 0;
    Action<SaveRequestedEvent> handler = _ => calls++;

    events.Subscribe(handler);
    events.Subscribe(handler);

    events.Publish(new SaveRequestedEvent());

    Assert.AreEqual(2, calls);
  }

  [TestMethod]
  public void Publish_OnOneBus_IsNeverSeenByAnother()
  {
    // The reason the singleton went: the bus's lifetime decides every subsystem's, and an
    // ambient one outlives a script reload with the previous run's handlers still attached.
    EventService first = new();
    EventService second = new();
    bool firstSaw = false;
    bool secondSaw = false;

    first.Subscribe<SaveRequestedEvent>(_ => firstSaw = true);
    second.Subscribe<SaveRequestedEvent>(_ => secondSaw = true);

    second.Publish(new SaveRequestedEvent());

    Assert.IsFalse(firstSaw);
    Assert.IsTrue(secondSaw);
  }

  [TestMethod]
  public void Publish_FromInsideAHandler_ReachesTheOtherTypesSubscribers()
  {
    // The shape the mod actually uses: Progression handles a sale by publishing a level-up, and
    // RewardSystem handles that by publishing an unlock — three deep, all inside one sale.
    EventService events = new();
    List<string> seen = [];

    events.Subscribe<DrugsSoldEvent>(_ =>
    {
      seen.Add("sold");
      events.Publish(new LeveledUpEvent(2));
    });

    events.Subscribe<LeveledUpEvent>(_ =>
    {
      seen.Add("leveled");
      events.Publish(new VanStealingUnlockedEvent());
    });

    events.Subscribe<VanStealingUnlockedEvent>(_ => seen.Add("unlocked"));

    events.Publish(new DrugsSoldEvent(1_000, 1_000));

    Assert.AreSequenceEqual(["sold", "leveled", "unlocked"], seen);
  }
}
