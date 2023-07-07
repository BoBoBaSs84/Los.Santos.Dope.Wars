using LSDW.Abstractions.Attributes.Caches;
using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Extensions;

/// <summary>
/// The enumerator extensions class.
/// </summary>
public static class EnumeratorExtensions
{
	/// <summary>
	/// Returns the description of the <typeparamref name="T"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	/// <returns>The description or the enum name.</returns>
	public static string GetDescription<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible
		=> DescriptionAttributeCache<T>.GetDescription(value);

	/// <summary>
	/// Returns the description of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static string GetDrugDescription(this DrugType value)
		=> DrugAttributeCache<DrugType>.GetDescription(value);

	/// <summary>
	/// Returns the name of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static string GetName(this DrugType value)
		=> DrugAttributeCache<DrugType>.GetName(value);

	/// <summary>
	/// Returns the average price of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static int GetAveragePrice(this DrugType value)
		=> DrugAttributeCache<DrugType>.GetAveragePrice(value);

	/// <summary>
	/// Returns the probability property of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static float GetDrugProbability(this DrugType value)
		=> DrugAttributeCache<DrugType>.GetProbability(value);

	/// <summary>
	/// Returns a enumerable of all enumerators of the given type of enum.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static IEnumerable<T> GetList<T>(this T value) where T : Enum
		=> Enum.GetValues(value.GetType()).Cast<T>().ToList();
}