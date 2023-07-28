using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Abstractions.Infrastructure.Services;

/// <summary>
/// The settings service interface.
/// </summary>
public interface ISettingsService
{
	/// <summary>
	/// The dealer settings.
	/// </summary>
	IDealerSettings Dealer { get; }

	/// <summary>
	/// The market settings.
	/// </summary>
	IMarketSettings Market { get; }

	/// <summary>
	/// The player settings.
	/// </summary>
	IPlayerSettings Player { get; }

	/// <summary>
	/// The trafficking settings.
	/// </summary>
	ITraffickingSettings Trafficking { get; }

	/// <summary>
	/// Saves the current settings to file.
	/// </summary>
	void Save();

	/// <summary>
	/// Saves the current settings from file.
	/// </summary>
	void Load();
}
