using LSDW.Helpers;
using LSDW.Services;

using GTA;

namespace LSDW.Dealers;

/// <summary>
/// A dealer's ped: putting one at his spot with the <c>[Dealers]</c> settings applied,
/// and scattering him while the player is wanted.
/// </summary>
internal static class DealerPeds
{
  /// <summary>
  /// How far a fleeing dealer has to get from his spot before his ped is deleted. Well
  /// inside the 100m spawn range, so the delete happens while he is still simulated
  /// rather than being left to the range check.
  /// </summary>
  private const float FleeDespawnDistance = 60f;

  /// <summary>
  /// Spawns a dealer's ped and applies the <c>[Dealers]</c> settings to it.
  /// </summary>
  internal static void SpawnDealerPed(DrugDealer dealer)
  {
    PedHash pedHash = DealerRoster.Models[RandomHelper.Random.Next(DealerRoster.Models.Count)];
    Model model = ScriptHelper.GetPedModel(pedHash);
    dealer.MainPed = Ped.Create(model, dealer.Position, dealer.Heading);
    dealer.MainPed.MarkAsNoLongerNeeded();
    dealer.MainPed.Task.StandStill(-1);

    WeaponHash weaponHash = DealerRoster.Weapons[RandomHelper.Random.Next(DealerRoster.Weapons.Count)];
    if (SettingsService.Dealers.HasWeapon)
    {
      _ = dealer.MainPed.Weapons.Give(weaponHash, 500, equipNow: false, isAmmoLoaded: true);
    }

    dealer.MainPed.DropsEquippedWeaponOnDeath = SettingsService.Dealers.DropsWeaponOnDeath;

    if (SettingsService.Dealers.HasArmor)
    {
      dealer.MainPed.Armor = SettingsService.Dealers.ArmorAmount;
    }

    // Upper bound made inclusive: Next()'s is exclusive, and a player writing
    // DeathMoneyMax=500 means 500 is payable.
    int deathMoney = RandomHelper.Random.Next(SettingsService.Dealers.DeathMoneyMin, SettingsService.Dealers.DeathMoneyMax + 1);
    if (SettingsService.Dealers.DropsMoneyOnDeath)
    {
      dealer.MainPed.Money = deathMoney;
    }

    dealer.MainPed.CanSwitchWeapons = true;
    dealer.MainPed.BlockPermanentEvents = true;

    // DrugDealer instances outlive their peds, so a fresh ped starts unprovoked and at
    // his post — whatever the one he replaced was doing.
    dealer.Hostile = false;
    dealer.Fleeing = false;

    // He holds his pitch instead of running. Behaviour fix: the original set attribute 17
    // under a comment claiming "always fight", but 17 is AlwaysFlee and AlwaysFight is 5.
    // The flee-attribute mask is a separate native and is cleared as before.
    dealer.MainPed.SetFleeAttributes(default, false);
    dealer.MainPed.SetCombatAttribute(CombatAttributes.AlwaysFlee, false);
    dealer.MainPed.SetCombatAttribute(CombatAttributes.AlwaysFight, true);
  }

  /// <summary>
  /// Scatters a dealer while the player is wanted, and clears the way for him to come
  /// back once they are clean.
  /// </summary>
  internal static void UpdateHeatReaction(DrugDealer dealer, Ped character, bool hot)
  {
    if (!hot)
    {
      if (!dealer.Fleeing)
      {
        return;
      }

      if (dealer.Hostile)
      {
        // Shot mid-flight, so he turned and fought. Left alive to finish it — Hostile
        // already stops him trading.
        dealer.Fleeing = false;
        return;
      }

      // Clean again: drop whoever is left mid-flight so the spawn path can put him back
      // at his spot on this same tick.
      dealer.MainPed?.Delete();
      dealer.MainPed = null;
      dealer.Fleeing = false;
      return;
    }

    if (dealer.Hostile || dealer.MainPed is not Ped ped || ped.IsInjured)
    {
      return;
    }

    if (!dealer.Fleeing)
    {
      dealer.Fleeing = true;
      ped.BlockPermanentEvents = false;
      ped.Task.ReactAndFlee(character);
      return;
    }

    // Far enough out. Deleting rather than letting him run keeps the roster's peds tied
    // to their spots.
    if (!ped.IsInRange(dealer.Position, FleeDespawnDistance))
    {
      ped.Delete();
      dealer.MainPed = null;
    }
  }
}
