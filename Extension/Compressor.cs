using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Los.Santos.Dope.Wars.Extension
{
	/// <summary>
	/// The <see cref="Compressor"/> class for compression and decompression of stuff
	/// </summary>
	public static class Compressor
	{
		/// <summary>
		/// Takes a string and returns a compressesd string
		/// </summary>
		/// <param name="uncompressedString"></param>
		/// <returns><see cref="string"/></returns>
		public static string CompressString(string uncompressedString)
		{
			try
			{
				byte[] dataToCompress = Encoding.UTF8.GetBytes(uncompressedString);
				byte[] compressedData = Compress(dataToCompress);
				string compressedString = Encoding.UTF8.GetString(compressedData);
				return compressedString;
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
			return string.Empty;
		}

		/// <summary>
		/// Takes a compressed string and returns a decompressed string
		/// </summary>
		/// <param name="compressedString"></param>
		/// <returns><see cref="string"/></returns>
		public static string DecompressString(string compressedString)
		{
			try
			{
				byte[] dataToDecompress = Encoding.UTF8.GetBytes(compressedString);
				byte[] decompressedData = Decompress(dataToDecompress);
				string decompressedString = Encoding.UTF8.GetString(decompressedData);
				return decompressedString;
			}
			catch (Exception ex)
			{
				Logger.Error($"{ex.Message} - {ex.InnerException} - {ex.StackTrace}");
			}
			return string.Empty;
		}

		/// <summary>
		/// The compress method
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns><see cref="byte"/></returns>
		private static byte[] Compress(byte[] bytes)
		{
			using MemoryStream? memoryStream = new();
			using (DeflateStream? deflateStream = new(memoryStream, CompressionLevel.Optimal))
			{
				deflateStream.Write(bytes, 0, bytes.Length);
			}
			return memoryStream.ToArray();
		}

		/// <summary>
		/// The decompress method
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns><see cref="byte"/></returns>
		private static byte[] Decompress(byte[] bytes)
		{
			using MemoryStream? memoryStream = new(bytes);
			using MemoryStream? outputStream = new();
			using (DeflateStream? inflateStream = new(memoryStream, CompressionMode.Decompress))
			{
				inflateStream.CopyTo(outputStream);
			}
			return outputStream.ToArray();
		}
	}
}
