using LSDW.Attributes;
using LSDW.Enumerators;
using System.Reflection;

namespace LSDW.Extensions;

/// <summary>
/// The enumerator extensions class.
/// </summary>
public static class EnumeratorExtensions
{
	/// <summary>
	/// Should return the display name of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The enumerator value.</param>
	public static string GetDisplayName<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugTypeAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.DisplayName;
		}

		return value.ToString();
	}

	/// <summary>
	/// Should return the market price of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The enumerator value.</param>
	public static int GetMarketPrice<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugTypeAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.MarketPrice;
		}

		return default;
	}

	/// <summary>
	/// Should return the rank of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value">The enumerator value.</param>
	public static int GetRank<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugTypeAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.Rank;
		}

		return default;
	}

	private static FieldInfo GetFieldInfo<T>(T value) where T : Enum
		=> value.GetType().GetField(value.ToString());

	private static DrugTypeAttribute? GetDrugTypeAttribute(FieldInfo? fieldInfo)
		=> fieldInfo.GetCustomAttribute(typeof(DrugTypeAttribute), false) as DrugTypeAttribute;
}
