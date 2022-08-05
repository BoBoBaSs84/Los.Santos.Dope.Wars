using System.Text;
using static Los.Santos.Dope.Wars.Enums;
using static Los.Santos.Dope.Wars.Statics;

namespace Los.Santos.Dope.Wars.Extension
{
	/// <summary>
	/// The <see cref="Logger"/> class servers as ... a logger!
	/// </summary>
	public static class Logger
	{
		/// <summary>
		/// The <see cref="Trace(string)"/> method is for logging messages of type <see cref="LogLevels.Trace"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Trace(string message) => Log(nameof(Trace).ToUpper(CultureInfo), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="LogLevels.Status"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Status(string message) => Log(nameof(Status).ToUpper(CultureInfo), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="LogLevels.Warning"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Warning(string message) => Log(nameof(Warning).ToUpper(CultureInfo), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="LogLevels.Error"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Error(string message) => Log(nameof(Error).ToUpper(CultureInfo), message);

		/// <summary>
		/// Method is for logging messages of type <see cref="LogLevels.Panic"/>
		/// </summary>
		/// <param name="message"></param>
		public static void Panic(string message) => Log(nameof(Panic).ToUpper(CultureInfo), message);

		private static void Log(string type, string message)
		{
			File.AppendAllText(
					path: LogFileName,
					contents: $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | {type} | {message}{Environment.NewLine}",
					encoding: Encoding.UTF8
					);
		}
	}
}
