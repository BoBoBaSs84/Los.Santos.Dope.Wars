using System.Runtime.CompilerServices;
using System.Text;
using static Los.Santos.Dope.Wars.Enums;
using static Los.Santos.Dope.Wars.Statics;

namespace Los.Santos.Dope.Wars.Extension;

/// <summary>
/// The static <see cref="Logger"/> class servers as, wait for it ... logger!
/// </summary>
public static class Logger
{
	/// <summary>
	/// The <see cref="Trace(string, string)"/> method for logging messages of type <see cref="LogLevels.Trace"/>
	/// </summary>
	/// <param name="message"></param>
	/// <param name="callerName"></param>
	public static void Trace(string message, [CallerMemberName] string callerName = "")
	{
		if (Main.LogLevels.HasFlag(LogLevels.Trace))
			LogToFile(nameof(Trace).ToUpper(CultureInfo), callerName, message);
	}

	/// <summary>
	/// Method is for logging messages of type <see cref="LogLevels.Information"/>
	/// </summary>
	/// <param name="message"></param>
	/// <param name="callerName"></param>
	public static void Information(string message, [CallerMemberName] string callerName = "")
	{
		if (Main.LogLevels.HasFlag(LogLevels.Information))
			LogToFile(nameof(Information).ToUpper(CultureInfo), callerName, message);
	}

	/// <summary>
	/// The <see cref="Debug(string, string)"/> method for logging messages of type <see cref="LogLevels.Debug"/>
	/// </summary>
	/// <param name="message"></param>
	/// <param name="callerName"></param>
	public static void Debug(string message, [CallerMemberName] string callerName = "")
	{
		if (Main.LogLevels.HasFlag(LogLevels.Debug))
			LogToFile(nameof(Debug).ToUpper(CultureInfo), callerName, message);
	}

	/// <summary>
	/// The <see cref="Warning(string, string)"/> method for logging messages of type <see cref="LogLevels.Warning"/>
	/// </summary>
	/// <param name="message"></param>
	/// <param name="callerName"></param>
	public static void Warning(string message, [CallerMemberName] string callerName = "")
	{
		if (Main.LogLevels.HasFlag(LogLevels.Warning))
			LogToFile(nameof(Warning).ToUpper(CultureInfo), callerName, message);
	}

	/// <summary>
	/// The <see cref="Error(string, string)"/> method for logging messages of type <see cref="LogLevels.Error"/>
	/// </summary>
	/// <param name="message"></param>
	/// <param name="callerName"></param>
	public static void Error(string message, [CallerMemberName] string callerName = "")
	{
		if (Main.LogLevels.HasFlag(LogLevels.Error))
			LogToFile(nameof(Error).ToUpper(CultureInfo), callerName, message);
	}

	/// <summary>
	/// The <see cref="Critical(string, string)"/> method for logging messages of type <see cref="LogLevels.Critical"/>
	/// </summary>
	/// <param name="message"></param>
	/// <param name="callerName"></param>
	public static void Critical(string message, [CallerMemberName] string callerName = "")
	{
		if (Main.LogLevels.HasFlag(LogLevels.Critical))
			LogToFile(nameof(Critical).ToUpper(CultureInfo), callerName, message);
	}

	/// <summary>
	/// The <see cref="LogToFile(string, string, string)"/> method logs the message content to the log file.
	/// </summary>
	/// <param name="type">The logger message type.</param>
	/// <param name="caller">The logger message caller.</param>
	/// <param name="message">The logger message itself.</param>
	private static void LogToFile(string type, string caller, string message)
	{
		string content = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} [{type}] <{caller}> {message}{Environment.NewLine}";
		File.AppendAllText(path: LogFileName, contents: content, encoding: Encoding.UTF8);
	}
}