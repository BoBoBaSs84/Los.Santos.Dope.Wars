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
		public PlayerStash() => Init();
		#endregion

		#region IPlayerStash members
		/// <inheritdoc/>
		public void BuyDrug(string name, int quantity, int price)
		{
			Drug drug = Drugs.Where(x => x.Name.Equals(name)).SingleOrDefault();

			if (drug.Quantity.Equals(0))
				drug.PurchasePrice = price;
			else
				drug.PurchasePrice = ((drug.Quantity * drug.PurchasePrice) + (quantity * price)) / (drug.Quantity + quantity);

			drug.Quantity += quantity;
		}
		/// <inheritdoc/>
		public void SellDrug(string name, int quantity, int price)
		{
			Drug drug = Drugs.Where(x => x.Name.Equals(name)).SingleOrDefault();
			int transactionValue = quantity * price;
			drug.Quantity -= quantity;
		}
		#endregion
	}
}
