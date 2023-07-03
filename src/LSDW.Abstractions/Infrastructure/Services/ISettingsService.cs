namespace LSDW.Abstractions.Infrastructure.Services;

/// <summary>
/// The settings service interface.
/// </summary>
public partial interface ISettingsService
{
	/// <summary>
	/// Saves the current settings to file.
	/// </summary>
	void Save();
}
