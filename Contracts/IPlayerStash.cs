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
		/// <param name="name"></param>
		/// <param name="quantity"></param>
		/// <param name="price"></param>
		void BuyDrug(string name, int quantity, int price);

		/// <summary>
		/// The <see cref="SellDrug(string, int, int)"/> method is used to manipulate the player stash
		/// </summary>
		/// <param name="name"></param>
		/// <param name="quantity"></param>
		/// <param name="price"></param>
		void SellDrug(string name, int quantity, int price);
	}
}
