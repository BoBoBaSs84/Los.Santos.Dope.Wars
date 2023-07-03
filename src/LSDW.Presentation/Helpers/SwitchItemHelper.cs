using LSDW.Abstractions.Enumerators;
using LSDW.Presentation.Properties;

namespace LSDW.Presentation.Helpers;

/// <summary>
/// The switch item Helper class.
/// </summary>
internal static class SwitchItemHelper
{
	/// <summary>
	/// Gets the title for the switch item depending on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the switch item.</param>
	/// <returns>The title for the switch item.</returns>
	internal static string GetTitle(TransactionType type)
	{
		string title = type switch
		{
			TransactionType.BUY => Resources.UI_Switch_Item_Title_Buy,
			TransactionType.SELL => Resources.UI_Switch_Item_Title_Sell,
			TransactionType.TAKE => Resources.UI_Switch_Item_Title_Take,
			TransactionType.GIVE => Resources.UI_Switch_Item_Title_Give,
			_ => string.Empty
		};
		return title;
	}

	/// <summary>
	/// Gets the description for the switch item depending on the transaction type.
	/// </summary>
	/// <param name="type">The transaction type for the switch item.</param>
	/// <returns>The description for the switch item.</returns>
	internal static string GetDescription(TransactionType type)
	{
		string description = type switch
		{
			TransactionType.BUY => Resources.UI_Switch_Item_Description_Buy,
			TransactionType.SELL => Resources.UI_Switch_Item_Description_Sell,
			TransactionType.TAKE => Resources.UI_Switch_Item_Description_Take,
			TransactionType.GIVE => Resources.UI_Switch_Item_Description_Give,
			_ => string.Empty
		};
		return description;
	}
}
