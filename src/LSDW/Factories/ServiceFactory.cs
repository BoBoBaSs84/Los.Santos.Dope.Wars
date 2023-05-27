using LSDW.Interfaces.Services;
using LSDW.Services;

namespace LSDW.Factories;

/// <summary>
/// The service factory class.
/// </summary>
public static class ServiceFactory
{
	/// <summary>
	/// Creates a new settings service instance.
	/// </summary>
	public static ISettingsService CreateSettingsService()
		=> new SettingsService();

	/// <summary>
	/// Creates a new logger service instance.
	/// </summary>
	public static ILoggerService CreateLoggerService()
		=> new LoggerService();
}
