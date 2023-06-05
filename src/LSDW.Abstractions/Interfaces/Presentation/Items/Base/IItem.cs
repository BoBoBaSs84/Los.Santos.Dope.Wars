namespace LSDW.Abstractions.Interfaces.Presentation.Items.Base;

/// <summary>
/// The item base interface.
/// </summary>
public interface IItem
{
	/// <summary>
	/// Event triggered when the item is activated.
	/// </summary>
	event EventHandler Activated;
}
