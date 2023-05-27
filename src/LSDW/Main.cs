using GTA;
using LSDW.Factories;
using LSDW.Interfaces.Services;
using LSDW.UI;

namespace LSDW;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	private readonly ILoggerService _logger;
	private readonly ISettingsService _settingsService;
	private readonly SettingsMenu _settingsMenu;

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		_logger = ServiceFactory.CreateLoggerService();
		_settingsService = ServiceFactory.CreateSettingsService();
		_settingsMenu = new(_settingsService, _logger);

		Interval = 10;

		Aborted += OnAborted;
		KeyDown += OnKeyDown;
		KeyUp += OnKeyUp;
		Tick += OnTick;
		Tick += _settingsMenu.OnTick;
	}

	private void OnKeyUp(object sender, KeyEventArgs args)
	{
		if (args.KeyCode == Keys.F10)
			_settingsMenu.Visible = true;
	}

	private void OnKeyDown(object sender, KeyEventArgs args)
	{ }

	private void OnTick(object sender, EventArgs args)
	{ }

	private void OnAborted(object sender, EventArgs args)
	{ }
}
