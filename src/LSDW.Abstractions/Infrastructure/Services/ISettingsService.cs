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

	/// <summary>
	/// Reads a value from the <see cref="GTA.ScriptSettings"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="section">The section where the value is.</param>
	/// <param name="name">The name of the key the value is saved at.</param>
	/// <param name="defaultvalue">The fall-back value if the key doesn't exist or casting to <typeparamref name="T"/> fails.</param>
	/// <returns>The value at name in section.</returns>
	T GetValue<T>(string section, string name, T defaultvalue);

	/// <summary>
	/// Sets a value in the <see cref="GTA.ScriptSettings"/>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="section">The section where the value is.</param>
	/// <param name="name">The name of the key the value is saved at.</param>
	/// <param name="value">The value to set the key to.</param>
	void SetValue<T>(string section, string name, T value);
}
