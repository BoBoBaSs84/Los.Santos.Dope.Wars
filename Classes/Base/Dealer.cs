using GTA;
using GTA.Math;
using Los.Santos.Dope.Wars.Contracts.Base;
using Los.Santos.Dope.Wars.Extension;
using Los.Santos.Dope.Wars.Persistence.Settings;
using Los.Santos.Dope.Wars.Persistence.State;

namespace Los.Santos.Dope.Wars.Classes.Base
{
	/// <summary>
	/// The <see cref="Dealer"/> base class, implements the members of the <see cref="IDealer"/> interface
	/// </summary>
	public abstract class Dealer : Pedestrian, IDealer
	{
		#region properties
		/// <inheritdoc/>
		public bool IsDrugLord { get; private set; }
		/// <inheritdoc/>
		public bool ClosedforBusiness { get; set; }
		/// <inheritdoc/>
		public DateTime NextOpenBusinesTime { get; set; }
		/// <inheritdoc/>
		public DealerStash Stash { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The <see cref="Dealer"/> standard constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="heading"></param>
		/// <param name="isDrugLord"></param>
		public Dealer(Vector3 position, float heading, bool isDrugLord = false) : base(position, heading)
		{
			IsDrugLord = isDrugLord;
			Stash = new DealerStash();
		}
		#endregion

		#region IDealer members
		/// <inheritdoc/>
		public void FleeFromBust()
		{
			if (Ped is not null && PedCreated)
			{
				DeleteBlip();
				ClosedforBusiness = true;
				NextOpenBusinesTime = ScriptHookUtils.GetGameDateTime().AddHours(24);
				Ped.Task.FleeFrom(Position);
			}
		}
		/// <inheritdoc/>
		public void Refresh(GameSettings gameSettings, PlayerStats playerStats)
		{
			Stash.RefreshDrugMoney(playerStats, gameSettings, IsDrugLord);
			Stash.RefreshCurrentPrice(playerStats, gameSettings, IsDrugLord);
			(float health, float armor) = Utils.GetDealerHealthArmor(gameSettings.Dealer, playerStats.CurrentLevel);
			UpdatePed(
				health: health,
				armor: armor,
				money: Stash.DrugMoney,
				switchWeapons: gameSettings.Dealer.CanSwitchWeapons,
				blockEvents: gameSettings.Dealer.BlockPermanentEvents,
				dropWeapons: gameSettings.Dealer.DropsEquippedWeaponOnDeath
				);
		}
		/// <inheritdoc/>
		public void Restock(GameSettings gameSettings, PlayerStats playerStats)
		{
			Stash.RestockQuantity(playerStats, gameSettings, IsDrugLord);
			Refresh(gameSettings, playerStats);
		}
		#endregion
	}
}