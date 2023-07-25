using LSDW.Abstractions.Domain.Models;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Domain.Models;

/// <summary>
/// The application settings class.
/// </summary>
internal sealed partial class Settings : ISettings
{
	private static readonly Lazy<Settings> _settings = new(() => new());

	/// <summary>
	/// Initializes a instance of the settings class.
	/// </summary>
	private Settings()
	{
		IniFileName = "LSDW.ini";
		LogFileName = "LSDW.log";
		SaveFileName = "LSDW.sav";
	}

	/// <summary>
	/// The singleton instance of the settings.
	/// </summary>
	public static Settings Instance
		=> _settings.Value;

	public string IniFileName { get; }
	public string LogFileName { get; }
	public string SaveFileName { get; }

	public IDealerSettings Dealer
		=> DealerSettings.Instance;

	public IMarketSettings Market
		=> MarketSettings.Instance;

	public IPlayerSettings Player
		=> PlayerSettings.Instance;

	public ITraffickingSettings Trafficking
		=> TraffickingSettings.Instance;
}
