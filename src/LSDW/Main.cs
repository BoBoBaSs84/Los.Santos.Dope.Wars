using GTA;

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
		Interval = 10;

		Aborted += OnAborted;
		KeyDown += OnKeyDown;
		KeyUp += OnKeyUp;
		Tick += OnTick;
	}

	private void OnKeyUp(object sender, KeyEventArgs args) => throw new NotImplementedException();
	private void OnKeyDown(object sender, KeyEventArgs args) => throw new NotImplementedException();
	private void OnTick(object sender, EventArgs args) => throw new NotImplementedException();
	private void OnAborted(object sender, EventArgs args) => throw new NotImplementedException();
}
