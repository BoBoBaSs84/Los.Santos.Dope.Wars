using GTA;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Infrastructure.Services;

internal sealed partial class SettingsService : ISettingsService
{
	private static readonly Lazy<SettingsService> _service = new(() => new());
	private readonly ISettings _settings;
	private readonly ScriptSettings _scriptSettings;

	/// <summary>
	/// The singleton instance of the settings service.
	/// </summary>
	internal static SettingsService Instance
		=> _service.Value;

	public IDealerSettings Dealer
		=> _settings.Dealer;

	public IMarketSettings Market
		=> _settings.Market;

	public IPlayerSettings Player
		=> _settings.Player;

	public ITraffickingSettings Trafficking
		=> _settings.Trafficking;

	public void Save()
		=> _scriptSettings.Save();
}
