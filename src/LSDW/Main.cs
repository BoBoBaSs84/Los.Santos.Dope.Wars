using GTA;
using LSDW.Classes.UI;
using LSDW.Core.Enumerators;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Interfaces.Services;

namespace LSDW;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly ILoggerService _logger;
	private readonly ISettingsService _settings;
	private readonly SettingsMenu _settingsMenu;
	private readonly SideMenu _leftSideMenu;
	private readonly IPlayer _player;
	private readonly IInventory _dealerInventory;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_logger = Factories.ServiceFactory.CreateLoggerService();
		_settings = Factories.ServiceFactory.CreateSettingsService();
		_settingsMenu = Factories.MenuFactory.CreateSettingsMenu(_settings, _logger);
		_player = ModelFactory.CreatePlayer();
		_player.Inventory.Randomize(_player.Level);
		_dealerInventory = ModelFactory.CreateInventory();
		_dealerInventory.Randomize(_player.Level);
		_leftSideMenu = Factories.MenuFactory.CreateSideMenu(MenuType.SELL, _player, _dealerInventory);

		Interval = 5;

		Aborted += OnAborted;
		KeyDown += OnKeyDown;
		KeyUp += OnKeyUp;
		Tick += OnTick;
		Tick += _settingsMenu.OnTick;
		Tick += _leftSideMenu.OnTick;
	}

	private void OnKeyUp(object sender, KeyEventArgs args)
	{
		if (args.KeyCode == Keys.F10 && !_leftSideMenu.Visible)
			_settingsMenu.Visible = true;

		if (args.KeyCode == Keys.F9 && !_settingsMenu.Visible)
			_leftSideMenu.Visible = true;
	}

	private void OnKeyDown(object sender, KeyEventArgs args)
	{ }

	private void OnTick(object sender, EventArgs args)
	{ }

	private void OnAborted(object sender, EventArgs args)
	{ }
}
