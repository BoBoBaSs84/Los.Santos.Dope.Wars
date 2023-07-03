using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Infrastructure.Services;

namespace LSDW.Infrastructure.Factories;

public static partial class InfrastructureFactory
{
	/// <summary>
	/// Returns the logger service singleton instance.
	/// </summary>
	public static ILoggerService GetLoggerService()
		=> LoggerService.Instance;

	/// <summary>
	/// Returns the state service singleton instance.
	/// </summary>
	public static IStateService GetStateService()
		=> StateService.Instance;

	/// <summary>
	/// Returns the settings service singleton instance.
	/// </summary>
	public static ISettingsService GetSettingsService()
		=> SettingsService.Instance;
}
