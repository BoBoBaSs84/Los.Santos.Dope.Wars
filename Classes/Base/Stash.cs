using Los.Santos.Dope.Wars.Contracts;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Los.Santos.Dope.Wars.Classes.Base
{
	/// <summary>
	/// The <see cref="Stash"/> class is the base class for drug stashes.
	/// Implements the members of the <see cref="IStash"/> interface
	/// </summary>
	[XmlRoot(ElementName = nameof(Stash), IsNullable = false)]
	public abstract class Stash : IStash
	{
		private static readonly List<Drug> AvailableDrugs = new()
		{
			new Drug(Enums.DrugTypes.Cocaine, "Cocaine is a powerful stimulant and narcotic.", 865),
			new Drug(Enums.DrugTypes.Heroin, "Heroin is a semi-synthetic, strongly analgesic opioid.", 895),
			new Drug(Enums.DrugTypes.Marijuana, "Marijuana is a psychoactive drug from the Cannabis plant", 165),
			new Drug(Enums.DrugTypes.Hashish, "Hashish refers to the resin extracted from the cannabis plant.", 125),
			new Drug(Enums.DrugTypes.Mushrooms, "Psychoactive mushrooms, also known as magic mushrooms.", 245),
			new Drug(Enums.DrugTypes.Amphetamine, "Amphetamine has a strong stimulating and uplifting effect.", 215),
			new Drug(Enums.DrugTypes.PCP, "Also known as Angel Dust or Peace Pill in the drug scene.", 255),
			new Drug(Enums.DrugTypes.Methamphetamine, "Methamphetamine is a powerful psychostimulant.", 785),
			new Drug(Enums.DrugTypes.Ketamine, "Ketamine is a dissociative anaesthetic used in human medicine.", 545),
			new Drug(Enums.DrugTypes.Mescaline, "Mescaline or mescaline is a psychedelic and hallucinogenic alkaloid.", 470),
			new Drug(Enums.DrugTypes.Ecstasy, "Ecstasy, also XTC, is a term for so-called 'party pills'.", 275),
			new Drug(Enums.DrugTypes.Acid, "Acid, also known as LSD, is one of the strongest known hallucinogens.", 265),
			new Drug(Enums.DrugTypes.MDMA, "MDMA is particularly known as a party drug that is widely used worldwide.", 315),
			new Drug(Enums.DrugTypes.Crack, "Crack is a drug made from cocaine salt and sodium bicarbonate.", 615),
			new Drug(Enums.DrugTypes.Oxycodone, "A semi-synthetic opioid, highly addictive and a common drug of abuse.", 185)
		};

		#region properties
		/// <inheritdoc/>
		[XmlElement(ElementName = "Drug", IsNullable = false)]
		public List<Drug> Drugs { get; set; }
		/// <inheritdoc/>
		[XmlAttribute(AttributeName = nameof(Money))]
		public int Money { get; set; }
		#endregion

		#region ctor
		/// <summary>
		/// The empty constructor for the <see cref="Stash"/> class
		/// </summary>
		public Stash()
		{
			Drugs = new();			
			Money = default;
		}
		#endregion

		#region IStash members
		/// <inheritdoc/>
		public void Init()
		{
			Drugs.Clear();
			List<Drug>? drugList = AvailableDrugs;
			foreach (Drug? drug in drugList)
				Drugs.Add(new Drug(drug.Name, drug.Description, drug.AveragePrice));
		}
		/// <inheritdoc/>
		public void AddToStash(string drugName, int drugQuantity)
		{
			Drug drug = Drugs.Where(x => x.Name.Equals(drugName)).SingleOrDefault();
			if (drug is not null)
				drug.Quantity += drugQuantity;
		}
		/// <inheritdoc/>
		public void RemoveFromStash(string drugName, int drugQuantity)
		{
			Drug drug = Drugs.Where(x => x.Name.Equals(drugName)).SingleOrDefault();
			if (drug is not null)
				drug.Quantity -= drugQuantity;
		}
		/// <inheritdoc/>
		public void AddDrugMoney(int amount) => Money += amount;
		/// <inheritdoc/>
		public void RemoveDrugMoney(int amount) => Money -= amount;
		#endregion
	}
}
