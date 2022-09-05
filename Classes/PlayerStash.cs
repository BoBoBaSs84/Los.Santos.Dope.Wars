using GTA;
using Los.Santos.Dope.Wars.Classes.BaseTypes;
using Los.Santos.Dope.Wars.Interfaces;

namespace Los.Santos.Dope.Wars.Classes;

/// <summary>
/// The <see cref="PlayerStash"/> class.
/// </summary>
/// <remarks>
/// Inherits from the <see cref="Stash"/> base class.
/// Implements the members of the <see cref="IPlayerStash"/> interface.
/// </remarks>
public class PlayerStash : Stash, IPlayerStash
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PlayerStash"/> class.
	/// </summary>
	public PlayerStash() => Drugs = new();

	#region IPlayerStash members
	/// <inheritdoc cref="IPlayerStash.BuyDrug(string, int, int)"/>
	public void BuyDrug(string drugName, int drugQuantity, int drugPrice)
	{
		CalculateNewPurchasePrice(drugName, drugQuantity, drugPrice);
		AddToStash(drugName, drugQuantity);
		Game.Player.Money -= drugPrice * drugQuantity;
	}

	/// <inheritdoc cref="IPlayerStash.MoveIntoInventory(string, int, int)"/>
	public void MoveIntoInventory(string drugName, int drugQuantity, int drugPrice)
	{
		CalculateNewPurchasePrice(drugName, drugQuantity, drugPrice);
		AddToStash(drugName, drugQuantity);
	}
	/// <inheritdoc cref="IPlayerStash.SellDrug(string, int, int)"/>
	public void SellDrug(string drugName, int drugQuantity, int drugPrice)
	{
		RemoveFromStash(drugName, drugQuantity);
		Game.Player.Money += drugPrice * drugQuantity;
	}
	/// <inheritdoc cref="IPlayerStash.TakeFromInventory(string, int)"/>
	public void TakeFromInventory(string drugName, int drugQuantity) =>
		RemoveFromStash(drugName, drugQuantity);
	#endregion

	/// <summary>
	/// The method calculaties the new purchase price, when adding drug into the player stash. 
	/// </summary>
	/// <param name="drugName">The name of the drug.</param>
	/// <param name="drugQuantity">The quantity / amount of the drug.</param>
	/// <param name="drugPrice">The current price of the drug.</param>
	private void CalculateNewPurchasePrice(string drugName, int drugQuantity, int drugPrice)
	{
		Drug drug = Drugs.Where(x => x.Name.Equals(drugName, StringComparison.Ordinal)).Single();
		drug.PurchasePrice = drug.Quantity.Equals(0) ? drugPrice
			: ((drug.Quantity * drug.PurchasePrice) + (drugQuantity * drugPrice)) / (drug.Quantity + drugQuantity);
	}
}
