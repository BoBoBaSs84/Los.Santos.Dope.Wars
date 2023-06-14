using LSDW.Abstractions.Attributes;
using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Extensions;

/// <summary>
/// The enumerator extensions class.
/// </summary>
public static class EnumeratorExtensions
{
	/// <summary>
	/// Returns the description of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static string GetDescription<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DescriptionAttribute? attribute = GetDescriptionAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.Description;
		}

		return value.ToString();
	}

	/// <summary>
	/// Returns the display name of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static string GetDisplayName<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.DisplayName;
		}

		return value.ToString();
	}

	/// <summary>
	/// Returns the average price of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static int GetAveragePrice<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.AveragePrice;
		}

		return default;
	}

	/// <summary>
	/// Returns the probability property of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static float GetProbability<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.Probability;
		}

		return default;
	}

	/// <summary>
	/// Returns a list of all enumerators of the given type of enum.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static List<T> GetList<T>(this T value) where T : Enum
		=> Enum.GetValues(value.GetType()).Cast<T>().ToList();

	private static FieldInfo GetFieldInfo<T>(T value) where T : Enum
		=> value.GetType().GetField(value.ToString());

	private static DrugAttribute? GetDrugTypeAttribute(FieldInfo? fieldInfo)
		=> fieldInfo.GetCustomAttribute(typeof(DrugAttribute), false) as DrugAttribute;

	private static DescriptionAttribute? GetDescriptionAttribute(FieldInfo? fieldInfo)
		=> fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
}
