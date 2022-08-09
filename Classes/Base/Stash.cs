using Los.Santos.Dope.Wars.Interfaces.Base;
using System.Xml.Serialization;
using static Los.Santos.Dope.Wars.Enums;

namespace Los.Santos.Dope.Wars.Classes.Base;

/// <summary>
/// The <see cref="Stash"/> class is the base class for drug stashes.
/// Implements the members of the <see cref="IStash"/> interface
/// </summary>
[XmlRoot(ElementName = nameof(Stash), IsNullable = false)]
public abstract class Stash : IStash
{
	#region fields
	private static readonly List<Drug> AvailableDrugs = new()
	{
		new Drug(DrugType.Cocaine, "Cocaine is a powerful stimulant and narcotic.", 865),
		new Drug(DrugType.Heroin, "Heroin is a semi-synthetic, strongly analgesic opioid.", 895),
		new Drug(DrugType.Marijuana, "Marijuana is a psychoactive drug from the Cannabis plant", 165),
		new Drug(DrugType.Hashish, "Hashish refers to the resin extracted from the cannabis plant.", 125),
		new Drug(DrugType.Mushrooms, "Psychoactive mushrooms, also known as magic mushrooms.", 245),
		new Drug(DrugType.Amphetamine, "Amphetamine has a strong stimulating and uplifting effect.", 215),
		new Drug(DrugType.PCP, "Also known as Angel Dust or Peace Pill in the drug scene.", 255),
		new Drug(DrugType.Methamphetamine, "Methamphetamine is a powerful psychostimulant.", 785),
		new Drug(DrugType.Ketamine, "Ketamine is a dissociative anaesthetic used in human medicine.", 545),
		new Drug(DrugType.Mescaline, "Mescaline or mescaline is a psychedelic and hallucinogenic alkaloid.", 470),
		new Drug(DrugType.Ecstasy, "Ecstasy, also XTC, is a term for so-called 'party pills'.", 275),
		new Drug(DrugType.Acid, "Acid, also known as LSD, is one of the strongest known hallucinogens.", 265),
		new Drug(DrugType.MDMA, "MDMA is particularly known as a party drug that is widely used worldwide.", 315),
		new Drug(DrugType.Crack, "Crack is a drug made from cocaine salt and sodium bicarbonate.", 615),
		new Drug(DrugType.Oxycodone, "A semi-synthetic opioid, highly addictive and a common drug of abuse.", 185)
	};
	#endregion

	#region properties
	/// <inheritdoc/>
	[XmlArray(ElementName = nameof(Drugs), IsNullable = false)]
	[XmlArrayItem(ElementName = nameof(Drug), IsNullable = false)]
	public List<Drug> Drugs { get; set; }
	#endregion

	#region ctor
	/// <summary>
	/// The empty constructor for the <see cref="Stash"/> class
	/// </summary>
	public Stash() => Drugs = new();
	#endregion

	#region IStash members
	/// <inheritdoc/>
	public void Init()
	{
		Drugs.Clear();
		foreach (Drug? drug in AvailableDrugs)
			Drugs.Add(new Drug(drug.Name, drug.Description, drug.AveragePrice));
	}
	/// <inheritdoc/>
	public void AddToStash(string drugName, int drugQuantity)
	{
		Drug drug = Drugs.Where(x => x.Name.Equals(drugName, StringComparison.Ordinal)).SingleOrDefault();
		if (drug is not null)
			drug.Quantity += drugQuantity;
	}
	/// <inheritdoc/>
	public void RemoveFromStash(string drugName, int drugQuantity)
	{
		Drug drug = Drugs.Where(x => x.Name.Equals(drugName, StringComparison.Ordinal)).SingleOrDefault();
		if (drug is not null)
			drug.Quantity -= drugQuantity;
	}
	#endregion
}
