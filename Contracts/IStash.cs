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
	}
}
