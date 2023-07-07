﻿namespace LSDW.Abstractions.Attributes.Caches;

/// <summary>
/// The description attribute cache class.
/// </summary>
/// <typeparam name="T">Should be a <see cref="Enum"/> value.</typeparam>
internal static class DescriptionAttributeCache<T> where T : struct, IComparable, IFormattable, IConvertible
{
	private static readonly Dictionary<T, string> _descriptions;

	/// <summary>
	/// Initializes a instance of the description attribute cache class.
	/// </summary>
	static DescriptionAttributeCache()
	{
		_descriptions = new Dictionary<T, string>();

		Type type = typeof(T);
		foreach (T value in Enum.GetValues(type).Cast<T>())
		{
			string valueName = value.ToString();
			_descriptions.Add(value, type.GetMember(valueName)[0].GetCustomAttribute<DescriptionAttribute>()?.Description ?? valueName);
		}
	}

	/// <summary>
	/// Returns the description of the <see cref="Enum"/> <paramref name="value"/> from the cache.
	/// </summary>
	/// <param name="value">The enum type value.</param>
	/// <returns>The enumerator description.</returns>
	internal static string GetDescription(T value)
		=> _descriptions[value];
}
