using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace LSDW.Domain.Extensions;

/// <summary>
/// The string extensions class.
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Formats the string with parameters and <see cref="CultureInfo.InvariantCulture"/>.
	/// </summary>
	/// <param name="inputString">Original string with placeholders.</param>
	/// <param name="paramaters">Parameters to set in the placeholders.</param>
	/// <returns>The formated string.</returns>
	public static string FormatInvariant(this string inputString, params object[] paramaters)
		=> string.Format(CultureInfo.InvariantCulture, inputString, paramaters);

	/// <summary>
	/// Formats the string to <see cref="CultureInfo.InvariantCulture"/>.
	/// </summary>
	/// <param name="inputString">The input string to format.</param>
	/// <returns>The formated string.</returns>
	public static string ToInvariant(this string inputString)
		=> string.Format(CultureInfo.InvariantCulture, inputString);

	/// <summary>
	/// Compresses a string and returns a deflate compressed, Base64 encoded string.
	/// </summary>
	/// <param name="inputString">Input string to compress.</param>
	/// <param name="compressionLevel">The compression level to use.</param>
	/// <returns></returns>
	public static string Compress(this string inputString, CompressionLevel compressionLevel = CompressionLevel.Optimal)
	{
		byte[] compressedBytes;

		using (MemoryStream uncompressedStream = new(Encoding.UTF8.GetBytes(inputString)))
		{
			using MemoryStream compressedStream = new();
			using (DeflateStream compressorStream = new(compressedStream, compressionLevel, true))
				uncompressedStream.CopyTo(compressorStream);
			compressedBytes = compressedStream.ToArray();
		}

		return Convert.ToBase64String(compressedBytes);
	}

	/// <summary>
	/// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
	/// </summary>
	/// <param name="inputString">Input string to decompress.</param>
	public static string Decompress(this string inputString)
	{
		byte[] decompressedBytes;

		MemoryStream compressedStream = new(Convert.FromBase64String(inputString));
		using (DeflateStream decompressorStream = new(compressedStream, CompressionMode.Decompress))
		{
			using MemoryStream decompressedStream = new();
			decompressorStream.CopyTo(decompressedStream);
			decompressedBytes = decompressedStream.ToArray();
		}

		return Encoding.UTF8.GetString(decompressedBytes);
	}
}
