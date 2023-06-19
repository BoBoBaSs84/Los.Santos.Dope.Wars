using GTA;
using LemonUI;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Providers;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;
using LSDW.Presentation.Factories;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly ObjectPool _processables = new();
	private readonly INotificationService _notificationService = DomainFactory.CreateNotificationService();
	private readonly ITimeProvider _timeProvider = new GameTimeProvider();
	private readonly ILoggerService _loggerService = InfrastructureFactory.CreateLoggerService();
	private readonly ISettingsService _settingsService = InfrastructureFactory.CreateSettingsService();
	private readonly IGameStateService _stateService;
	private readonly ISettingsMenu _settingsMenu;
	private readonly ITrafficking _trafficking;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_stateService = InfrastructureFactory.CreateGameStateService(_loggerService);
		_settingsMenu = PresentationFactory.CreateSettingsMenu(_settingsService);
		_settingsMenu.Add(_processables);
		_trafficking = DomainFactory.CreateTraffickingMission(_stateService.Player, _stateService.Dealers, _timeProvider, _loggerService, _notificationService);

		Interval = 10;

		Aborted += _trafficking.OnAborted;

		KeyUp += OnKeyUp;

		Tick += _trafficking.OnTick;
		Tick += OnTick;
	}

	private void OnTick(object sender, EventArgs e)
		=> _processables.Process();

	private void OnKeyUp(object sender, KeyEventArgs args)
	{
		if (args.KeyCode == Keys.F10)
		{
			if (Game.Player.CanControlCharacter && Game.Player.CanStartMission)
				_settingsMenu.SetVisible(true);
		}

		if (args.KeyCode == Keys.F9)
		{
			if (Game.Player.CanControlCharacter && Game.Player.CanStartMission)
				_trafficking.StartMission();
		}
	}
}
