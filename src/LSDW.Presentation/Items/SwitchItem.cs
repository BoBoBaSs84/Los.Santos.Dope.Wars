using LemonUI.Menus;
using LSDW.Abstractions.Presentation.Items;
using LSDW.Domain.Enumerators;
using LSDW.Presentation.Helpers;

namespace LSDW.Presentation.Items;

/// <summary>
/// The switch item class.
/// </summary>
internal sealed class SwitchItem : NativeItem, ISwitchItem
{
	/// <summary>
	/// Initializes a instance of the switch item class.
	/// </summary>
	/// <param name="type">The transaction type for the switch item.</param>
	internal SwitchItem(TransactionType type) : base(string.Empty)
	{
		Description = SwitchItemHelper.GetDescription(type);
		Enabled = true;
		Title = SwitchItemHelper.GetTitle(type);
	}
}
