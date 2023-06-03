namespace LSDW.Abstractions.Interfaces.Presentation.Base;

/// <summary>
/// The menu base interface.
/// </summary>
public interface IMenu
{
	/// <summary>
	/// Sets the menu visibility.
	/// </summary>
	/// <param name="value"><see langword="true"/> or <see langword="false"/></param>
	void SetVisible(bool value);

	/// <summary>
	/// Put code that needs to be looped each frame in here.
	/// </summary>
	void OnTick(object sender, EventArgs args);
}
