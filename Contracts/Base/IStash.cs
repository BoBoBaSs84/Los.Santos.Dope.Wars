using Los.Santos.Dope.Wars.Classes;

namespace Los.Santos.Dope.Wars.Contracts.Base
{
	/// <summary>
	/// The <see cref="IStash"/> interface, needed for all who want to do something with drugs
	/// </summary>
	public interface IStash
	{
		/// <summary>
		/// The <see cref="Drugs"/> property, well illegal goods, the main stock and trade of this mod
		/// </summary>
		List<Drug> Drugs { get; }

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
	}
}
