using LSDW.Abstractions.Enumerators;

namespace LSDW.Abstractions.Presentation.Menus;

/// <summary>
/// The deal menu interface.
/// </summary>
public interface IDealMenu : IMenuBase
{
	/// <summary>
	/// The transaction type that defines the menu and it's function.
	/// </summary>
	public TransactionType Type { get; }

	/// <summary>
	/// The event that is invoked when the switch item was activated.
	/// </summary>
	public event EventHandler? SwitchItemActivated;
}
