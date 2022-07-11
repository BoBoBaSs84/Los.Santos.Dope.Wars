namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IPlayerStash"/> interface, provides methods specifically for the player
	/// </summary>
	public interface IPlayerStash
	{
		/// <summary>
		/// The <see cref="BuyDrug(string, int, int)"/> method is used to manipulate the player stash
		/// </summary>
		/// <param name="drugName"></param>
		/// <param name="drugQuantity"></param>
		/// <param name="drugPrice"></param>
		void BuyDrug(string drugName, int drugQuantity, int drugPrice);

		/// <summary>
		/// The <see cref="SellDrug(string, int, int)"/> method is used to manipulate the player stash
		/// </summary>
		/// <param name="drugName"></param>
		/// <param name="drugQuantity"></param>
		/// <param name="drugPrice"></param>
		void SellDrug(string drugName, int drugQuantity, int drugPrice);
	}
}
