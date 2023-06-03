using System.Runtime.CompilerServices;

namespace LSDW.Abstractions.Interfaces.Infrastructure;

/// <summary>
/// The logger service interface.
/// </summary>
public interface ILoggerService
{
	/// <summary>
	/// Logs informational related things.
	/// </summary>
	/// <param name="message">The message to log.</param>
	/// <param name="callerName">The message caller.</param>
	void Information(string message, [CallerMemberName] string callerName = "");

	/// <summary>
	/// Logs warning related things.
	/// </summary>
	/// <param name="message">The message to log.</param>
	/// <param name="callerName">The message caller.</param>
	void Warning(string message, [CallerMemberName] string callerName = "");

	/// <summary>
	/// Logs error related things.
	/// </summary>
	/// <param name="message">The message to log.</param>
	/// <param name="callerName">The message caller.</param>
	void Critical(string message, [CallerMemberName] string callerName = "");
}
