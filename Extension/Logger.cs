using System;
using System.IO;
using System.Text;

namespace Los.Santos.Dope.Wars.Extension
{
	/// <summary>
	/// The <see cref="Logger"/> class servers as ... a logger!
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// Method is for logging messages of type <see cref="Enums.LogLevels.Trace"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Trace(string message) => Log(nameof(Trace).ToUpper(), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="Enums.LogLevels.Status"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Status(string message) => Log(nameof(Status).ToUpper(), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="Enums.LogLevels.Warning"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Warning(string message) => Log(nameof(Warning).ToUpper(), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="Enums.LogLevels.Error"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Error(string message) => Log(nameof(Error).ToUpper(), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="Enums.LogLevels.Panic"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Panic(string message) => Log(nameof(Panic).ToUpper(), message);

		private static void Log(string type, string message)
		{
			File.AppendAllText(
					path: Constants.LogFileName,
					contents: $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | {type} | {message}{Environment.NewLine}",
					encoding: Encoding.UTF8
					);
		}
	}
}
