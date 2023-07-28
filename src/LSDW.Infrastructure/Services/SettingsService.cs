using GTA;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Factories;
using static LSDW.Abstractions.Domain.Models.ISettings;

namespace LSDW.Infrastructure.Services;

internal sealed partial class SettingsService : ISettingsService
{
	private readonly static Lazy<SettingsService> _service = new(() => new());
	private readonly ISettings _settings;
	private readonly ScriptSettings _scriptSettings;

	private SettingsService()
	{
		_settings = DomainFactory.GetSettings();
		_scriptSettings = ScriptSettings.Load(Path.Combine(AppContext.BaseDirectory, _settings.IniFileName));
		Load();
		Save();

		_settings.Dealer.PropertyChanged += OnPropertyChanged;
		_settings.Market.PropertyChanged += OnPropertyChanged;
		_settings.Player.PropertyChanged += OnPropertyChanged;
		_settings.Trafficking.PropertyChanged += OnPropertyChanged;
	}

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
