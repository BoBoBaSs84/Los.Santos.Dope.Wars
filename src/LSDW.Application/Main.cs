using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Application.Constants;
using LSDW.Application.Factories;
using LSDW.Domain.Extensions;
using LSDW.Presentation.Menus.Base;
using LSDW.Presentation.Menus.DealMenu;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
[ScriptAttributes(Author = ApplicationConstants.Author, SupportURL = ApplicationConstants.SupportURL)]
public sealed class Main : Script
{
	private readonly DealMenu _dealMenu;
	private readonly IProviderManager _providerManager;
	private readonly IServiceManager _serviceManager;

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
		_dealMenu = new(_serviceManager.StateService.Player.Inventory.Restock(100));

		Interval = 10;

		KeyUp += OnKeyUp;
		Tick += OnTick;
	}

	private void OnTick(object sender, EventArgs e)
		=> MenuBase.Processables.Process();

	private void OnKeyUp(object sender, KeyEventArgs args)
	{
		while (!Game.Player.CanControlCharacter)
			Yield();

		if (args.KeyCode == Keys.F10)
		{
			if (_dealMenu.LatestMenu is not null)
				_dealMenu.LatestMenu.Toggle();
			else
				_dealMenu.Toggle();
		}
	}
}
