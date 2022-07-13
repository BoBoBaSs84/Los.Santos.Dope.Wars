using GTA;
using Los.Santos.Dope.Wars.Classes.Base;
using Los.Santos.Dope.Wars.Contracts;
using System.Linq;

namespace Los.Santos.Dope.Wars.Classes
{
	/// <summary>
	/// The <see cref="DealerStash"/> class for player types, inherits from <see cref="Stash"/>
	/// </summary>
	public class PlayerStash : Stash, IPlayerStash
	{
		#region ctor
		/// <summary>
		/// The standard constructor for the <see cref="PlayerStash"/> class
		/// </summary>
		public PlayerStash() { }
		#endregion

		#region IPlayerStash members
		/// <inheritdoc/>
		public void BuyDrug(string drugName, int drugQuantity, int drugPrice)
		{
			MoveIntoInventory(drugName, drugQuantity, drugPrice);
			Game.Player.Money -= drugPrice * drugQuantity;
		}
		/// <inheritdoc/>
		public void MoveIntoInventory(string drugName, int drugQuantity, int drugPrice)
		{
			Drug drug = Drugs.Where(x => x.Name.Equals(drugName)).SingleOrDefault();

			if (drug.Quantity.Equals(0))
				drug.PurchasePrice = drugPrice;
			else
				drug.PurchasePrice = ((drug.Quantity * drug.PurchasePrice) + (drugQuantity * drugPrice)) / (drug.Quantity + drugQuantity);
			
			AddToStash(drugName, drugQuantity);
		}
		/// <inheritdoc/>
		public void SellDrug(string drugName, int drugQuantity, int drugPrice)
		{
			TakeFromInventory(drugName, drugQuantity);
			Game.Player.Money += drugPrice * drugQuantity;
		}
		/// <inheritdoc/>
		public void TakeFromInventory(string drugName, int drugQuantity)
		{
			RemoveFromStash(drugName, drugQuantity);
		}
		#endregion
	}
}
