using LSDW.Core.Attributes;
using System.ComponentModel;

namespace LSDW.Core.Enumerators;

/// <summary>
/// The drug type enumerator.
/// </summary>
[Flags]
public enum DrugType
{
	/// <summary>
	/// The Cocaine drug type enumerator.
	/// </summary>
	[Drug("Cocaine", 100, 3)]
	[Description("Cocaine is a powerful stimulant and narcotic.")]
	COKE = 1 << 0,

	/// <summary>
	/// The Heroin drug type enumerator.
	/// </summary>
	[Drug("Heroin", 80, 3)]
	[Description("Heroin is a semi-synthetic, strongly analgesic opioid.")]
	SMACK = 1 << 1,

	/// <summary>
	/// The "Marijuana" drug type enumerator.
	/// </summary>
	[Drug("Marijuana", 15, 1)]
	[Description("Marijuana is a psychoactive drug from the Cannabis plant")]
	CANA = 1 << 2,

	/// <summary>
	/// The "Hashish" drug type enumerator.
	/// </summary>
	[Drug("Hashish", 20, 1)]
	[Description("Hashish refers to the resin extracted from the cannabis plant.")]
	HASH = 1 << 3,

	/// <summary>
	/// The "Mushrooms" drug type enumerator.
	/// </summary>
	[Drug("Mushrooms", 25, 1)]
	[Description("Psychoactive mushrooms, also known as magic mushrooms.")]
	SHROOMS = 1 << 4,

	/// <summary>
	/// The "Amphetamine" drug type enumerator.
	/// </summary>
	[Drug("Amphetamine", 35, 2)]
	[Description("Illegally trafficked amphetamine is also known as 'speed' or 'pep'.")]
	SPEED = 1 << 5,

	/// <summary>
	/// The "Angel Dust" drug type enumerator.
	/// </summary>
	[Drug("Angel Dust", 30, 1)]
	[Description("Also known as PCP or Peace Pill in the drug scene.")]
	PCP = 1 << 6,

	/// <summary>
	/// The "Methamphetamine" drug type enumerator.
	/// </summary>
	[Drug("Methamphetamine", 125, 3)]
	[Description("On the black market, methamphetamine is also known as meth or crystal.")]
	METH = 1 << 7,

	/// <summary>
	/// The "Ketamine" drug type enumerator.
	/// </summary>
	[Drug("Ketamine", 55, 2)]
	[Description("Ketamine can greatly reduce the sensation of pain and cause unconsciousness.")]
	KETA = 1 << 8,

	/// <summary>
	/// The "Mescaline" drug type enumerator.
	/// </summary>
	[Drug("Mescaline", 45, 2)]
	[Description("In terms of effect, mescaline is a typical hallucinogen, also called peyotl.")]
	PEYO = 1 << 9,

	/// <summary>
	/// The "Ecstasy" drug type enumerator.
	/// </summary>
	[Drug("Ecstasy", 15, 1)]
	[Description("On the illegal market, Ecstasy, also XTC, is a term for so-called 'party pills'.")]
	XTC = 1 << 10,

	/// <summary>
	/// The "Acid" drug type enumerator.
	/// </summary>
	[Drug("Acid", 30, 1)]
	[Description("LSD, or 'acid', is sold in the form of small 'cardboards' printed with various designs.")]
	LSD = 1 << 11,

	/// <summary>
	/// The "Molly" drug type enumerator.
	/// </summary>
	[Drug("Molly", 65, 2)]
	[Description("MDMA, also known as 'Molly', is a white or off-white powder or crystal, purer than XTC.")]
	MDMA = 1 << 12,

	/// <summary>
	/// The "Crack" drug type enumerator.
	/// </summary>
	[Drug("Crack", 75, 3)]
	[Description("Crack and also known as 'rock', is a free base form of cocaine that can be smoked.")]
	CRACK = 1 << 13,

	/// <summary>
	/// The "Oxycodone" drug type enumerator.
	/// </summary>
	[Drug("Oxycodone", 20, 1)]
	[Description("A semi-synthetic opioid, highly addictive and a common drug of abuse.")]
	OXY = 1 << 14,
}
