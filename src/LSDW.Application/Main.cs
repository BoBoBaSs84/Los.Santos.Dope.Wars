using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Application.Constants;
using LSDW.Application.Factories;
using LSDW.Application.Managers;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
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
	private readonly IInventory _drugs = DomainFactory.CreateInventory();

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
		_serviceManager.StateService.Player.Inventory.Restock(100);
		_drugs.Restock(100);
		_dealMenu = new(_providerManager, TransactionType.SELL, _serviceManager.StateService.Player, _drugs);

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
