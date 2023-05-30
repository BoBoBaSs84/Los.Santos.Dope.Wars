using LSDW.Core.Classes;
using LSDW.Interfaces.Services;
using System.Runtime.CompilerServices;
using System.Text;

namespace LSDW.Classes.Services;

/// <summary>
/// The logger service class.
/// </summary>
public class LoggerService : ILoggerService
{
	private readonly string _baseDirectory;
	private readonly string _logFileName;

	/// <summary>
	/// Initializes a instance of the logger service class.
	/// </summary>
	public LoggerService()
	{
		_baseDirectory = AppContext.BaseDirectory;
		_logFileName = Settings.LogFileName;
	}

	public void Critical(string message, [CallerMemberName] string callerName = "")
			=> LogToFile("ERROR", callerName, message);

	public void Information(string message, [CallerMemberName] string callerName = "")
			=> LogToFile("INFO", callerName, message);

	public void Warning(string message, [CallerMemberName] string callerName = "")
			=> LogToFile("WARN", callerName, message);

	/// <summary>
	/// Lofs the message content to the log file.
	/// </summary>
	/// <param name="type">The logger message type.</param>
	/// <param name="caller">The logger message caller.</param>
	/// <param name="message">The logger message itself.</param>
	private void LogToFile(string type, string caller, string message)
	{
		string path = Path.Combine(_baseDirectory, _logFileName);
		string content = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff}\t[{type}]\t<{caller}> - {message}{Environment.NewLine}";
		File.AppendAllText(path, content, Encoding.UTF8);
	}
}
