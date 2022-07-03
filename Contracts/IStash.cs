using Los.Santos.Dope.Wars.Classes;
using System.Collections.Generic;

namespace Los.Santos.Dope.Wars.Contracts
{
	/// <summary>
	/// The <see cref="IStash"/> interface, needed for all who want to do something with drugs
	/// </summary>
	public interface IStash
	{
		/// <summary>
		/// The <see cref="Drugs"/> property, well illegal goods
		/// </summary>
		List<Drug> Drugs { get; }

		/// <summary>
		/// The <see cref="Money"/> property, first choice to trade with
		/// </summary>
		public int Money { get; }

		/// <summary>
		/// Initializes the <see cref="List{T}"/> of <see cref="Drug"/> named <see cref="Drugs"/>
		/// </summary>
		void Init();

		/// <summary>
		/// The <see cref="AddToStash(string, int)"/> method adds the quantity x for a drug
		/// </summary>
		/// <param name="drugName"></param>
		/// <param name="drugQuantity"></param>
		void AddToStash(string drugName, int drugQuantity);

		/// <summary>
		/// The <see cref="RemoveFromStash(string, int)"/> method removes the quantity x for a drug
		/// </summary>
		/// <param name="drugName"></param>
		/// <param name="drugQuantity"></param>
		void RemoveFromStash(string drugName, int drugQuantity);

		/// <summary>
		/// The <see cref="AddDrugMoney(int)"/> method adds the amount x to the <see cref="Money"/> property
		/// </summary>
		/// <param name="amount"></param>
		void AddDrugMoney(int amount);

		/// <summary>
		/// The <see cref="RemoveDrugMoney(int)"/> method removes the amount x from the <see cref="Money"/> property
		/// </summary>
		/// <param name="amount"></param>
		void RemoveDrugMoney(int amount);
	}
}
