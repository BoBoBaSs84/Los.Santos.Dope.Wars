using LSDW.Domain.Models;

namespace LSDW.Application.Abstractions.Infrastructure.Services;

/// <summary>
/// Represents a service abstraction responsible for managing application settings.
/// </summary>
public interface ISettingsService
{
	/// <summary>
	/// Gets the current settings instance. This property provides access to the settings
	/// that are currently loaded in memory.
	/// </summary>
	Settings Current { get; }

	/// <summary>
	/// Loads the settings from the storage. This method should be called before accessing
	/// any settings properties to ensure that the settings are up-to-date.
	/// </summary>
	void Load();

	/// <summary>
	/// Saves the current settings to the storage. This method should be called after
	/// modifying any settings properties to persist the changes.
	/// </summary>
	void Save();
}
