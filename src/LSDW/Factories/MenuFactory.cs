using LSDW.Core.Enumerators;
using LSDW.Core.Interfaces.Classes;
using LSDW.Enumerators;
using LSDW.UserInterfaces.Trafficking;

namespace LSDW.Factories;

/// <summary>
/// The menu factory class.
/// </summary>
public static class MenuFactory
{
	/// <summary>
	/// Creates a new left side menu instance.
	/// </summary>
	/// <param name="transactionType">The transaction type.</param>
	/// <param name="inventory">The inventory to use.</param>
	public static SideMenu GetLeftSideMenu(TransactionType transactionType, IInventory inventory)
		=> new(GetLeftMenuType(transactionType), Color.Blue, inventory);

	/// <summary>
	/// Creates a new right side menu instance.
	/// </summary>
	/// <param name="transactionType">The transaction type.</param>
	/// <param name="inventory">The inventory to use.</param>
	public static SideMenu GetRightSideMenu(TransactionType transactionType, IInventory inventory)
		=> new(GetRightMenuType(transactionType), Color.Blue, inventory);

	private static MenuType GetLeftMenuType(TransactionType transactionType)
		=> transactionType switch
		{
			TransactionType.TRAFFIC => MenuType.BUY,
			TransactionType.DEPOSIT => MenuType.RETRIEVE,
			_ => MenuType.TAKE,
		};

	private static MenuType GetRightMenuType(TransactionType transactionType)
		=> transactionType switch
		{
			TransactionType.TRAFFIC => MenuType.SELL,
			TransactionType.DEPOSIT => MenuType.STORE,
			_ => MenuType.GIVE,
		};
}
