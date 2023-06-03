using GTA;
using LSDW.Abstractions.Interfaces.Application;
using LSDW.Abstractions.Interfaces.Infrastructure;
using LSDW.Abstractions.Interfaces.Presentation;
using LSDW.Application.Missions;
using LSDW.Application.Services;
using LSDW.Infrastructure.Factories;
using LSDW.Presentation.Factories;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly IDateTimeService _timeService;
	private readonly ILoggerService _logger;
	private readonly ISettingsService _settings;
	private readonly IGameStateService _stateService;
	private readonly ISettingsMenu _settingsMenu;
	private readonly IMission _trafficking;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_timeService = new GameTimeService();
		_logger = InfrastructureFactory.CreateLoggerService();
		_settings = new SettingsService();
		_stateService = InfrastructureFactory.CreateGameStateService(_logger);
		_trafficking = new Trafficking(_timeService, _logger, _stateService);
		_settingsMenu = PresentationFactory.CreateSettingsMenu(_settings, _logger);

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
			_settingsMenu.SetVisible(true);
	}

	private void OnKeyDown(object sender, KeyEventArgs args)
	{ }

	private void OnTick(object sender, EventArgs args)
	{ }

	private void OnAborted(object sender, EventArgs args)
	{ }
}
