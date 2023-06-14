using LSDW.Abstractions.Attributes;

namespace LSDW.Abstractions.Enumerators;

/// <summary>
/// The drug type enumerator.
/// </summary>
[Flags]
public enum DrugType
{
	/// <summary>
	/// The Cocaine drug type enumerator.
	/// </summary>
	[Drug("Cocaine", "Cocaine is a powerful stimulant and narcotic.", 100, Probability = 0.5f)]
	COKE = 1 << 0,

	/// <summary>
	/// The Heroin drug type enumerator.
	/// </summary>
	[Drug("Heroin", "Heroin is a semi-synthetic, strongly analgesic opioid.", 80, Probability = 0.6f)]
	SMACK = 1 << 1,

	/// <summary>
	/// The "Marijuana" drug type enumerator.
	/// </summary>
	[Drug("Marijuana", "Marijuana is a psychoactive drug from the Cannabis plant", 15, Probability = 0.9f)]
	CANA = 1 << 2,

	/// <summary>
	/// The "Hashish" drug type enumerator.
	/// </summary>
	[Drug("Hashish", "Hashish refers to the resin extracted from the cannabis plant.", 20, Probability = 0.95f)]
	HASH = 1 << 3,

	/// <summary>
	/// The "Mushrooms" drug type enumerator.
	/// </summary>
	[Drug("Mushrooms", "Psychoactive mushrooms, also known as magic mushrooms.", 25, Probability = 0.8f)]
	SHROOMS = 1 << 4,

	/// <summary>
	/// The "Amphetamine" drug type enumerator.
	/// </summary>
	[Drug("Amphetamine", "Illegally trafficked amphetamine is also known as 'speed' or 'pep'.", 35, Probability = 0.75f)]
	SPEED = 1 << 5,

	/// <summary>
	/// The "Angel Dust" drug type enumerator.
	/// </summary>
	[Drug("Angel Dust", "Also known as PCP or Peace Pill in the drug scene.", 30, Probability = 0.75f)]
	PCP = 1 << 6,

	/// <summary>
	/// The "Methamphetamine" drug type enumerator.
	/// </summary>
	[Drug("Methamphetamine", "Methamphetamine is also known as meth, crystal or ice.", 125, Probability = 0.6f)]
	METH = 1 << 7,

	/// <summary>
	/// The "Ketamine" drug type enumerator.
	/// </summary>
	[Drug("Ketamine", "Ketamine can greatly reduce the sensation of pain and cause unconsciousness.", 55, Probability = 0.8f)]
	KETA = 1 << 8,

	/// <summary>
	/// The "Mescaline" drug type enumerator.
	/// </summary>
	[Drug("Mescaline", "In terms of effect, mescaline is a typical hallucinogen, also called peyotl.", 45, Probability = 0.7f)]
	PEYO = 1 << 9,

	/// <summary>
	/// The "Ecstasy" drug type enumerator.
	/// </summary>
	[Drug("Ecstasy", "On the illegal market, Ecstasy, also XTC, is a term for so-called 'party pills'.", 15, Probability = 0.85f)]
	[Description()]
	XTC = 1 << 10,

	/// <summary>
	/// The "Acid" drug type enumerator.
	/// </summary>
	[Drug("Acid", "LSD, or 'acid', is sold in the form of small 'cardboards' printed with various designs.", 30, Probability = 0.8f)]
	LSD = 1 << 11,

	/// <summary>
	/// The "Molly" drug type enumerator.
	/// </summary>
	[Drug("Molly", "MDMA, also known as 'Molly', is a white or off-white powder or crystal, purer than XTC.", 65, Probability = 0.7f)]
	MDMA = 1 << 12,

	/// <summary>
	/// The "Crack" drug type enumerator.
	/// </summary>
	[Drug("Crack", "Crack and also known as 'rock', is a free base form of cocaine that can be smoked.", 75, Probability = 0.65f)]
	CRACK = 1 << 13,

	/// <summary>
	/// The "Oxycodone" drug type enumerator.
	/// </summary>
	[Drug("Oxycodone", "A semi-synthetic opioid, highly addictive and a common drug of abuse.", 20, Probability = 0.9f)]
	OXY = 1 << 14,
}
