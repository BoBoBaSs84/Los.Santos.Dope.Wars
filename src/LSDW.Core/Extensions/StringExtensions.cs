using System.Globalization;

namespace LSDW.Core.Extensions;

/// <summary>
/// The string extensions class.
/// </summary>
internal static class StringExtensions
{
	/// <summary>
	/// Formats the string with parameters and <see cref="CultureInfo.InvariantCulture"/>.
	/// </summary>
	/// <param name="source">Original string with placeholders.</param>
	/// <param name="paramaters">Parameters to set in the placeholders.</param>
	/// <returns>The formatted string.</returns>
	internal static string FormatInvariant(this string source, params object[] paramaters) =>
		string.Format(CultureInfo.InvariantCulture, source, paramaters);
}
