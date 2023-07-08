using GTA;
using LSDW.Presentation.Menus.Base;

namespace LSDW.Presentation;

/// <summary>
/// The Main class.
/// </summary>
[ScriptAttributes(NoDefaultInstance = true)]
public sealed class Main : Script
{
	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
		=> Tick += OnTick;

	private void OnTick(object sender, EventArgs e)
		=> MenuBase.Processables.Process();
}
