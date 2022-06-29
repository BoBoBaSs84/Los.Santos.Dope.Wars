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
		/// Takes a string, compresses it and returns it
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
		/// Takes a compressed string, decompresses it and returns it
		/// </summary>
		/// <param name="compressedString"></param>
		/// <returns></returns>
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
		/// <returns></returns>
		private static byte[] Compress(byte[] bytes)
		{
			using MemoryStream? memoryStream = new();
			using (GZipStream? gzipStream = new(memoryStream, CompressionLevel.Optimal))
			{
				gzipStream.Write(bytes, 0, bytes.Length);
			}
			return memoryStream.ToArray();
		}

		/// <summary>
		/// The decompress method
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private static byte[] Decompress(byte[] bytes)
		{
			using MemoryStream? memoryStream = new(bytes);
			using MemoryStream? outputStream = new();
			using (GZipStream? decompressStream = new(memoryStream, CompressionMode.Decompress))
			{
				decompressStream.CopyTo(outputStream);
			}
			return outputStream.ToArray();
		}

		private static void CompressFile(string OriginalFileName, string CompressedFileName)
		{
			using FileStream originalFileStream = File.Open(OriginalFileName, FileMode.Open);
			using FileStream compressedFileStream = File.Create(CompressedFileName);
			using DeflateStream? compressor = new(compressedFileStream, CompressionMode.Compress);
			originalFileStream.CopyTo(compressor);
		}

		private static void DecompressFile(string CompressedFileName, string DecompressedFileName)
		{
			using FileStream compressedFileStream = File.Open(CompressedFileName, FileMode.Open);
			using FileStream outputFileStream = File.Create(DecompressedFileName);
			using DeflateStream? decompressor = new(compressedFileStream, CompressionMode.Decompress);
			decompressor.CopyTo(outputFileStream);
		}
	}
}
