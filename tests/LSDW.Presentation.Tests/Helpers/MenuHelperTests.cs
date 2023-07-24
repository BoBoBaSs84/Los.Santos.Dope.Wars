using GTA.UI;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Presentation.Helpers;

namespace LSDW.Presentation.Tests.Helpers;

[TestClass]
public class MenuHelperTests
{
	private readonly IPlayer _player = DomainFactory.CreatePlayer();
	private readonly IInventory _inventory = DomainFactory.CreateInventory();

	[DataTestMethod]
	[DynamicData(nameof(GetRightMenuTypes), DynamicDataSourceType.Method)]
	public void GetRightAlignmentTest(TransactionType transactionType)
	{
		Alignment alignment = MenuHelper.GetAlignment(transactionType);

		Assert.AreEqual(Alignment.Right, alignment);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetLeftMenuTypes), DynamicDataSourceType.Method)]
	public void GetLeftAlignmentTest(TransactionType transactionType)
	{
		Alignment alignment = MenuHelper.GetAlignment(transactionType);

		Assert.AreEqual(Alignment.Left, alignment);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetRightMenuTypes), DynamicDataSourceType.Method)]
	public void GetInventoriesPlayerSourceTest(TransactionType transactionType)
	{
		(IInventory source, IInventory target) = MenuHelper.GetInventories(transactionType, _player, _inventory);

		Assert.AreSame(_player.Inventory, source);
		Assert.AreSame(_inventory, target);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetLeftMenuTypes), DynamicDataSourceType.Method)]
	public void GetInventoriesPlayerTargetTest(TransactionType transactionType)
	{
		(IInventory source, IInventory target) = MenuHelper.GetInventories(transactionType, _player, _inventory);

		Assert.AreSame(_player.Inventory, target);
		Assert.AreSame(_inventory, source);
	}

	private static IEnumerable<object[]> GetRightMenuTypes()
	{
		yield return new object[]
		{
			TransactionType.SELL
		};
		yield return new object[]
		{
			TransactionType.GIVE
		};
	}
	private static IEnumerable<object[]> GetLeftMenuTypes()
	{
		yield return new object[]
		{
			TransactionType.BUY
		};
		yield return new object[]
		{
			TransactionType.TAKE
		};
	}
}