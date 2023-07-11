using GTA;
using LemonUI;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Application.Constants;
using LSDW.Application.Factories;
using LSDW.Presentation.Factories;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
[ScriptAttributes(Author = ApplicationConstants.Author, SupportURL = ApplicationConstants.SupportURL)]
public sealed class Main : Script
{
	private readonly ObjectPool _processables = new();
	private readonly IProviderManager _providerManager;
	private readonly IServiceManager _serviceManager;
	private readonly ISettingsMenu _settingsMenu;
	private readonly ITrafficking _trafficking;

	/// <summary>
	/// Determines if the application is developer mode or not.
	/// </summary>
	public static bool IsDevelopment { get; private set; }

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
#if DEBUG
		IsDevelopment = true;
#else
		IsDevelopment = false;
#endif
		_providerManager = ApplicationFactory.GetProviderManager();
		_serviceManager = ApplicationFactory.GetServiceManager();

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
		while (!_providerManager.PlayerProvider.CanControlCharacter)
			Yield();

		if (args.KeyCode == Keys.F10)
		{
			_settingsMenu.Visible = true;
		}

		if (args.KeyCode == Keys.F9)
		{
			if (_trafficking.Status.Equals(MissionStatusType.STOPPED))
			{
				_serviceManager.StateService.Load(!IsDevelopment);
				_trafficking.StartMission();
				return;
			}

			if (_trafficking.Status.Equals(MissionStatusType.STARTED))
			{
				_serviceManager.StateService.Save(!IsDevelopment);
				_trafficking.StopMission();
				return;
			}
		}
	}
}
