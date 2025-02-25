using LSDW.Abstractions.Enumerators;
using System.Reflection;

namespace LSDW.Abstractions.Attributes.Caches;

/// <summary>
/// The drug attribute cache class.
/// </summary>
/// <typeparam name="T">The drug type value.</typeparam>
internal static class DrugAttributeCache<T> where T : struct, IComparable, IFormattable, IConvertible
{
	private static readonly Dictionary<T, int> _averagePrices;
	private static readonly Dictionary<T, string> _descriptions;
	private static readonly Dictionary<T, float> _probabilities;
	private static readonly Dictionary<T, string> _names;

	/// <summary>
	/// Initializes a instance of the drug attribute cache class.
	/// </summary>
	static DrugAttributeCache()
	{
		_averagePrices = new Dictionary<T, int>();
		_descriptions = new Dictionary<T, string>();
		_probabilities = new Dictionary<T, float>();
		_names = new Dictionary<T, string>();

		Type type = typeof(T);
		foreach (T value in Enum.GetValues(type).Cast<T>())
		{
			string valueName = value.ToString();
			_averagePrices.Add(value, type.GetMember(valueName)[0].GetCustomAttribute<DrugAttribute>()?.AveragePrice ?? default);
			_descriptions.Add(value, type.GetMember(valueName)[0].GetCustomAttribute<DrugAttribute>()?.Description ?? valueName);
			_probabilities.Add(value, type.GetMember(valueName)[0].GetCustomAttribute<DrugAttribute>()?.Probability ?? default);
			_names.Add(value, type.GetMember(valueName)[0].GetCustomAttribute<DrugAttribute>()?.Name ?? valueName);
		}
	}

	/// <summary>
	/// Returns the average drug price of the <see cref="DrugType"/>
	/// enumerator <paramref name="value"/> from the cache.
	/// </summary>
	/// <param name="value">The drug type value.</param>
	/// <returns>The average price.</returns>
	internal static int GetAveragePrice(T value)
		=> _averagePrices[value];

	/// <summary>
	/// Returns the drug description of the <see cref="DrugType"/>
	/// enumerator <paramref name="value"/> from the cache.
	/// </summary>
	/// <param name="value">The drug type value.</param>
	/// <returns>The drug description.</returns>
	internal static string GetDescription(T value)
		=> _descriptions[value];

	/// <summary>
	/// Returns the drug probability of the <see cref="DrugType"/>
	/// enumerator <paramref name="value"/> from the cache.
	/// </summary>
	/// <param name="value">The drug type value.</param>
	/// <returns>The drug probability.</returns>
	internal static float GetProbability(T value)
		=> _probabilities[value];

	/// <summary>
	/// Returns the drug name of the <see cref="DrugType"/>
	/// enumerator <paramref name="value"/> from the cache.
	/// </summary>
	/// <param name="value">The drug type value.</param>
	/// <returns>The drug name.</returns>
	internal static string GetName(T value)
		=> _names[value];
}
