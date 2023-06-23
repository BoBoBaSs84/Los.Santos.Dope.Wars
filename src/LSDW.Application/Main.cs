using GTA;
using LemonUI;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Managers;
using LSDW.Domain.Extensions;
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

	private bool isLoaded;
	private bool isDebug;

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
		isLoaded = _serviceManager.StateService.Load(!isDebug);
		_settingsMenu = PresentationFactory.CreateSettingsMenu(_serviceManager);		
		_trafficking = DomainFactory.CreateTraffickingMission(_serviceManager, _providerManager);
		_settingsMenu.Add(_processables);

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
		if (!(Game.Player.CanControlCharacter && Game.Player.CanStartMission))
			return;

		if (args.KeyCode == Keys.F10)
		{
			_settingsMenu.SetVisible(true);
		}

		if (args.KeyCode == Keys.F9)
		{
			if (_trafficking.Status.Equals(MissionStatusType.Stopped))
			{
				_trafficking.StartMission();
				return;
			}

			if (_trafficking.Status.Equals(MissionStatusType.Started))
			{
				_trafficking.StopMission();
				_ = _serviceManager.StateService.Save(!isDebug);
				return;
			}
		}

		if (args.KeyCode == Keys.F8)
		{
			var position = _providerManager.LocationProvider.PlayerPosition;
			var gameTime = _providerManager.TimeProvider.Now;
			_providerManager.NotificationProvider.ShowSubtitle($"{position} - {gameTime}");

			foreach(Abstractions.Domain.Models.IDealer dealer in _serviceManager.StateService.Dealers)
			{
				_ = dealer.Inventory.Restock();
			}
		}
	}
}
