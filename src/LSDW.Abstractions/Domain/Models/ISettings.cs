namespace LSDW.Abstractions.Domain.Models;

/// <summary>
/// The settings interface.
/// </summary>
public partial interface ISettings
{
	/// <summary>
	/// The settings file name.
	/// </summary>
	string IniFileName { get; }

	/// <summary>
	/// The log file name.
	/// </summary>
	string LogFileName { get; }

	/// <summary>
	/// The save file name.
	/// </summary>
	string SaveFileName { get; }

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
}