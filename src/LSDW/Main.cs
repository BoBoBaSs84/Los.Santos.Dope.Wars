using GTA;
using Logger = LSDW.Core.Services.LoggerService;

namespace LSDW;

/// <summary>
/// The Main class.
/// </summary>
public sealed class Main : Script
{
	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

		Interval = 10;

		Aborted += OnAborted;
		KeyDown += OnKeyDown;
		KeyUp += OnKeyUp;
		Tick += OnTick;
	}

	private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
	{
		Exception? ex = args.ExceptionObject as Exception;
		if (ex is not null)
			Logger.Error($"Exception: {ex.Message}");
		Logger.Error($"Terminating: {args.IsTerminating}");
	}

	private void OnKeyUp(object sender, KeyEventArgs args) => throw new NotImplementedException();
	private void OnKeyDown(object sender, KeyEventArgs args) => throw new NotImplementedException();
	private void OnTick(object sender, EventArgs args) => throw new NotImplementedException();
	private void OnAborted(object sender, EventArgs args) => throw new NotImplementedException();
}
