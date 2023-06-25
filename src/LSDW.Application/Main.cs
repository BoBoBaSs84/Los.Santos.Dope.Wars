using GTA;
using LemonUI;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Factories;
using LSDW.Application.Managers;
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

	private readonly bool isDebug;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
#if DEBUG
		isDebug = true;
#else
		isDebug = false;
#endif
		_providerManager = new ProviderManager();
		_serviceManager = new ServiceManager();
		_serviceManager.StateService.Load(!isDebug);

		_settingsMenu = PresentationFactory.CreateSettingsMenu(_serviceManager);
		_settingsMenu.Add(_processables);

		_trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManager, _providerManager);
		_trafficking.LeftSideMenu.Add(_processables);
		_trafficking.RightSideMenu.Add(_processables);

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
		while (!Game.Player.CanControlCharacter)
			Yield();

		if (args.KeyCode == Keys.F10)
		{
			_settingsMenu.SetVisible(true);
		}

		if (args.KeyCode == Keys.F9)
		{
			if (_trafficking.Status.Equals(MissionStatusType.STOPPED))
			{
				_trafficking.StartMission();
				return;
			}

			if (_trafficking.Status.Equals(MissionStatusType.STARTED))
			{
				_ = _serviceManager.StateService.Save(!isDebug);
				_trafficking.StopMission();
				return;
			}
		}
	}
}
