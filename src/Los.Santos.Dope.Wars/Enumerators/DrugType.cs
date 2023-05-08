using LSDW.Attributes;

namespace LSDW.Enumerators;

public enum DrugType
{
	/// <summary>
	/// The cocaine drug type enumerator.
	/// </summary>
	[DrugType("Cocaine", 2050, "Cocaine is a powerful stimulant and narcotic.")]
	COKE = 1,

	/// <summary>
	/// The methamphetamine drug type enumerator.
	/// </summary>
	[DrugType("Methamphetamine", 2200, "Methamphetamine is also known as meth or crystal.")]
	METH = 2
}
