using GTA;
using LSDW.Classes.UI;
using LSDW.Factories;
using LSDW.Interfaces.Missions;
using LSDW.Interfaces.Services;

namespace LSDW;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly IDateTimeService _timeService;
	private readonly ILoggerService _logger;
	private readonly ISettingsService _settings;
	private readonly IGameStateService _stateService;
	private readonly SettingsMenu _settingsMenu;
	private readonly IMission _trafficking;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_timeService = ServiceFactory.CreateDateTimeService();
		_logger = ServiceFactory.CreateLoggerService();
		_settings = ServiceFactory.CreateSettingsService();
		_stateService = ServiceFactory.CreateGameStateService(_logger);
		_trafficking = MissionFactory.CreateTraffickingMission(_timeService, _logger, _stateService);
		_settingsMenu = MenuFactory.CreateSettingsMenu(_settings, _logger);

		Interval = 5;

		Aborted += OnAborted;
		Aborted += _trafficking.OnAborted;

		KeyDown += OnKeyDown;
		KeyUp += OnKeyUp;

		Tick += OnTick;
		Tick += _settingsMenu.OnTick;
		Tick += _trafficking.OnTick;
	}

	private void OnKeyUp(object sender, KeyEventArgs args)
	{
		if (args.KeyCode == Keys.F10)
			_settingsMenu.Visible = true;
	}

	private void OnKeyDown(object sender, KeyEventArgs args)
	{ }

	private void OnTick(object sender, EventArgs args)
	{ }

	private void OnAborted(object sender, EventArgs args)
	{ }
}
