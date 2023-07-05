using LSDW.Abstractions.Attributes;
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
	public static string GetDescription<T>(this T value) where T : Enum
	{
		FieldInfo fieldInfo = GetFieldInfo(value);

		DescriptionAttribute? attribute = GetDescriptionAttribute(fieldInfo);

		return attribute is not null ? attribute.Description : value.ToString();
	}

	/// <summary>
	/// Returns the description of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static string GetDrugDescription(this DrugType value)
	{
		FieldInfo fieldInfo = GetFieldInfo(value);

		DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

		return attribute is not null ? attribute.Description : value.ToString();
	}

	/// <summary>
	/// Returns the name of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static string GetDrugName(this DrugType value)
	{
		FieldInfo fieldInfo = GetFieldInfo(value);

		DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

		return attribute is not null ? attribute.Name : value.ToString();
	}

	/// <summary>
	/// Returns the average price of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static int GetAverageDrugPrice(this DrugType value)
	{
		FieldInfo fieldInfo = GetFieldInfo(value);

		DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

		return attribute is not null ? attribute.AveragePrice : default;
	}

	/// <summary>
	/// Returns the probability property of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <param name="value">The enumerator value.</param>
	public static float GetDrugProbability(this DrugType value)
	{
		FieldInfo fieldInfo = GetFieldInfo(value);

		DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

		return attribute is not null ? attribute.Probability : (float)default;
	}

	/// <summary>
	/// Returns a enumerable of all enumerators of the given type of enum.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	public static IEnumerable<T> GetList<T>(this T value) where T : Enum
		=> Enum.GetValues(value.GetType()).Cast<T>().ToList();

	private static FieldInfo GetFieldInfo<T>(T value) where T : Enum
		=> value.GetType().GetField(value.ToString());

	private static DrugAttribute? GetDrugTypeAttribute(FieldInfo? fieldInfo)
		=> fieldInfo.GetCustomAttribute(typeof(DrugAttribute), false) as DrugAttribute;

	private static DescriptionAttribute? GetDescriptionAttribute(FieldInfo? fieldInfo)
		=> fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
}
