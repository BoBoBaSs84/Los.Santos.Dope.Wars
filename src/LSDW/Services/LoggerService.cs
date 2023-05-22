using LSDW.Properties;
using System.Runtime.CompilerServices;
using System.Text;

namespace LSDW.Services;

/// <summary>
/// The logger service class.
/// </summary>
public static class LoggerService
{
	private static readonly string BaseDirectory = AppContext.BaseDirectory;
	private static readonly string FileName = Settings.Default.LogFileName;

	/// <summary>
	/// The log file name and path.
	/// </summary>
	public static readonly string LogFileNamePath = Path.Combine(BaseDirectory, FileName);

	/// <summary>
	/// Should log informational related things.
	/// </summary>
	/// <param name="message">The message to log.</param>
	/// <param name="callerName">The message caller.</param>
	public static void Information(string message, [CallerMemberName] string callerName = "")
		=> LogToFile("INFO", callerName, message);

	/// <summary>
	/// Should log warning related things.
	/// </summary>
	/// <param name="message">The message to log.</param>
	/// <param name="callerName">The message caller.</param>
	public static void Warning(string message, [CallerMemberName] string callerName = "")
		=> LogToFile("WARN", callerName, message);

	/// <summary>
	/// Should log error related things.
	/// </summary>
	/// <param name="message">The message to log.</param>
	/// <param name="callerName">The message caller.</param>
	public static void Error(string message, [CallerMemberName] string callerName = "")
		=> LogToFile("ERROR", callerName, message);

	/// <summary>
	/// Should log the message content to the log file.
	/// </summary>
	/// <param name="type">The logger message type.</param>
	/// <param name="caller">The logger message caller.</param>
	/// <param name="message">The logger message itself.</param>
	private static void LogToFile(string type, string caller, string message)
	{
		string content = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff}\t[{type}]\t<{caller}> - {message}{Environment.NewLine}";
		File.AppendAllText(LogFileNamePath, content, Encoding.UTF8);
	}
}
