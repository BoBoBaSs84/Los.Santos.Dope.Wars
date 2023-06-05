using GTA;
using LSDW.Abstractions.Interfaces.Application.Missions;
using LSDW.Abstractions.Interfaces.Application.Providers;
using LSDW.Abstractions.Interfaces.Infrastructure.Services;
using LSDW.Abstractions.Interfaces.Presentation.Menus;
using LSDW.Application.Missions;
using LSDW.Application.Providers;
using LSDW.Application.Services;
using LSDW.Domain.Interfaces.Services;
using LSDW.Infrastructure.Factories;
using LSDW.Presentation.Factories;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly ITimeProvider _timeProvider;
	private readonly ILoggerService _loggerService;
	private readonly ISettingsService _settingsService;
	private readonly IGameStateService _stateService;
	private readonly ISettingsMenu _settingsMenu;
	private readonly ITrafficking _trafficking;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_timeProvider = new GameTimeProvider();
		_loggerService = InfrastructureFactory.CreateLoggerService();
		_settingsService = new SettingsService();
		_stateService = InfrastructureFactory.CreateGameStateService(_loggerService);
		_trafficking = new Trafficking(_timeProvider, _loggerService, _stateService);
		_settingsMenu = PresentationFactory.CreateSettingsMenu(_settingsService, _loggerService);

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
