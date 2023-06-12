﻿using GTA;
using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Missions;
using LSDW.Application.Providers;
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
		_settingsService = InfrastructureFactory.CreateSettingsService();
		_stateService = InfrastructureFactory.CreateGameStateService(_loggerService);
		_trafficking = new Trafficking(_timeProvider, _loggerService, _stateService);
		_settingsMenu = PresentationFactory.CreateSettingsMenu(_settingsService);

		Interval = 5;

		Aborted += _trafficking.OnAborted;

		KeyUp += OnKeyUp;
		KeyUp += _trafficking.OnKeyUp;

		Tick += _settingsMenu.OnTick;
		Tick += _trafficking.OnTick;
	}

	private void OnKeyUp(object sender, KeyEventArgs args)
	{
		if (Game.Player.CanControlCharacter && Game.Player.CanStartMission && args.KeyCode == Keys.F10)
			_settingsMenu.SetVisible(true);
	}
}
