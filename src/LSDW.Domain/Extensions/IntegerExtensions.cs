﻿namespace LSDW.Domain.Extensions;

/// <summary>
/// The integer extensions class.
/// </summary>
public static class IntegerExtensions
{
	/// <summary>
	/// Returns an array of integers starting from zero.
	/// </summary>
	/// <param name="value">The maximum integer value.</param>
	public static int[] GetArray(this int value)
	{
		int[] array = new int[value + 1];
		for (int i = 0; i <= value; i++)
			array[i] = i;
		return array;
	}

	/// <summary>
	/// Returns an list of integers starting from zero.
	/// </summary>
	/// <param name="value">The maximum integer value.</param>
	public static List<int> GetList(this int value)
		=> GetArray(value).ToList();
}
