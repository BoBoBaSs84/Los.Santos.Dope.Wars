using GTA;
using LemonUI;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Managers;
using LSDW.Domain.Factories;
using LSDW.Presentation.Factories;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly ObjectPool _processables = new();
	private readonly IProviderManager _providerManager;
	private readonly IServiceManager _serviceManager;
	private readonly ISettingsMenu _settingsMenu;
	private readonly ITrafficking _trafficking;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_providerManager = new ProviderManager();
		_serviceManager = new ServiceManager();
		_settingsMenu = PresentationFactory.CreateSettingsMenu(_serviceManager);
		_settingsMenu.Add(_processables);
		_trafficking = DomainFactory.CreateTraffickingMission(_serviceManager, _providerManager);

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
