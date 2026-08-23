using LSDW.Helpers;
using LSDW.Leveling;

using GTA;
using GTA.Math;

namespace LSDW.Activities;

/// <summary>
/// The pedestrians around the van: which of them want to buy, walking them to the
/// counter, and running the serving timer that completes a sale. Sells nothing itself —
/// <see cref="ServeCustomers"/> takes the sale as a callback, so this type never names
/// <see cref="TacoSales"/>.
/// </summary>
internal sealed class TacoCustomers
{
  /// <summary>
  /// How far a customer has progressed, keyed by ped handle:
  /// 0 = not interested, 1 = interested, 2..49 = walking over / being served
  /// (the count doubles as the serving timer), 50 = sale completes, -1 = done.
  /// </summary>
  private readonly Dictionary<int, int> _customers = [];
  private readonly Progression _progression;

  /// <param name="progression">The level the interest odds are read from.</param>
  public TacoCustomers(Progression progression)
    => _progression = progression;

  /// <summary>
  /// Gets the number of customers served, for the session HUD to show. Counts every
  /// session, not just the current one — <see cref="Clear"/> forgets the peds, not the
  /// tally.
  /// </summary>
  public int TacosSold { get; private set; }

  /// <summary>
  /// Forgets every tracked customer. Called as a session starts and as one ends, so
  /// interest is not carried between sessions.
  /// </summary>
  public void Clear()
    => _customers.Clear();

  /// <summary>
  /// Rolls once per newly-seen pedestrian: a third of them want to buy.
  /// </summary>
  public void TrackNearbyCustomers(Ped character)
  {
    foreach (Ped ped in World.GetNearbyPeds(character, 20f))
    {
      if (!_customers.ContainsKey(ped.Handle) && !ped.IsInVehicle() && ped.IsHuman)
        _customers.Add(ped.Handle, (RandomHelper.Random.Next(0, Perks.TacoInterestOneIn(_progression.Level)) == 0) ? 1 : 0);
    }
  }

  /// <summary>
  /// Advances every tracked customer one step, and invokes <paramref name="sell"/> for
  /// each one whose serving timer has run out.
  /// </summary>
  /// <param name="character">The player.</param>
  /// <param name="sell">What a completed sale does.</param>
  public void ServeCustomers(Ped character, Action sell)
  {
    // Counts down so writes back into _customers cannot disturb the loop. Bug fix: the
    // bound was `i > 0`, which skipped index 0 entirely. The body only updates values,
    // never adds or removes keys, so 0 is safe to serve.
    for (int i = _customers.Count - 1; i >= 0; i--)
    {
      KeyValuePair<int, int> customer = _customers.ElementAt(i);

      // Null once the ped is gone, hence the guards below. The sale at 50 fires either
      // way — it is the timer that completes it, not the customer.
      Ped? ped = Entity.FromHandle(customer.Key) as Ped;
      Vector3 counter = character.Position + character.RightVector;

      // Marker over anyone interested or on their way.
      if ((customer.Value == 1 || customer.Value == 2) && ped != null && ped.IsInRange(character.Position, 50f) && ped.IsAlive)
      {
        World.DrawMarker(MarkerType.Cone, ped.Position + new Vector3(0f, 0f, 2f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.FromArgb(100, 58, 189, 106), true, true, false);
      }

      // At the counter: start the chat animation, then tick the serving timer.
      if (customer.Value >= 2 && customer.Value < 50 && ped != null && ped.IsInRange(counter, 2f))
      {
        if (customer.Value == 2)
        {
          ped.Task.ClearAll();
          ped.Task.PlayAnimation("random@street_race", "_car_b_chatting_female", 8f, -8f, 2000, AnimationFlags.Loop, 8f);
        }
        _customers[customer.Key]++;
      }

      // Timer full: the sale goes through and they wander off.
      if (customer.Value == 50)
      {
        TacosSold++;

        if (ped != null)
        {
          ped.IsPersistent = false;
          ped.Task.Wander();
        }

        _customers[customer.Key] = -1;
        sell();
      }

      // Interested and the van has stopped: walk them over. A ped that no longer exists
      // counts as "not at the counter", so the state still advances.
      if (customer.Value == 1 && character.CurrentVehicle.Speed < 1f && (ped == null || !ped.IsInRange(counter, 2f)))
      {
        if (ped != null)
        {
          ped.Task.ClearAll();
          ped.IsPersistent = true;

          // Every argument is passed explicitly: the defaults are Run, a 0.25 radius and
          // a 40000 heading, which makes the customer jog over rather than walk.
          ped.Task.FollowNavMeshTo(counter, PedMoveBlendRatio.Walk, timeBeforeWarp: -1, radius: 0f, navigationFlags: FollowNavMeshFlags.Default, finalHeading: 0f);
        }

        _customers[customer.Key] = 2;
      }

      // Drove off mid-approach: send them back to merely interested.
      if (customer.Value == 2 && character.CurrentVehicle.Speed > 1f)
      {
        ped?.Task.ClearAll();
        _customers[customer.Key] = 1;
      }
    }
  }
}
