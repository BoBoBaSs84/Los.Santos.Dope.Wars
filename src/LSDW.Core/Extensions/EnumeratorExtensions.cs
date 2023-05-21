using LSDW.Core.Attributes;
using LSDW.Core.Enumerators;
using System.Reflection;

namespace LSDW.Core.Extensions;

/// <summary>
/// The enumerator extensions class.
/// </summary>
internal static class EnumeratorExtensions
{
	/// <summary>
	/// Should return the description of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	internal static string GetDescription<T>(this T value) where T : Enum
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
	/// Should return the display name of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	internal static string GetDisplayName<T>(this T value) where T : Enum
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
	/// Should return the market price of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	internal static int GetMarketValue<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.MarketValue;
		}

		return default;
	}

	/// <summary>
	/// Should return the rank of the <see cref="DrugType"/> enumerator.
	/// </summary>
	/// <typeparam name="T">The enmuerator type.</typeparam>
	/// <param name="value">The enumerator value.</param>
	internal static int GetRank<T>(this T value) where T : Enum
	{
		FieldInfo? fieldInfo = GetFieldInfo(value);

		if (fieldInfo is not null)
		{
			DrugAttribute? attribute = GetDrugTypeAttribute(fieldInfo);

			if (attribute is not null)
				return attribute.Rank;
		}

		return default;
	}

	/// <summary>
	/// Should return a list of all enumerators of the given type of enum.
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
