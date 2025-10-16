using GTA.UI;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Presentation.Helpers;

namespace LSDW.Presentation.Tests.Helpers;

[TestClass, ExcludeFromCodeCoverage]
public class MenuHelperTests
{
	private readonly IPlayer _player = DomainFactory.CreatePlayer();
	private readonly IInventory _inventory = DomainFactory.CreateInventory();

	[TestMethod]
	[DynamicData(nameof(GetRightMenuTypes))]
	public void GetRightAlignmentTest(TransactionType transactionType)
	{
		Alignment alignment = MenuHelper.GetAlignment(transactionType);

		Assert.AreEqual(Alignment.Right, alignment);
	}

	[TestMethod]
	[DynamicData(nameof(GetLeftMenuTypes))]
	public void GetLeftAlignmentTest(TransactionType transactionType)
	{
		Alignment alignment = MenuHelper.GetAlignment(transactionType);

		Assert.AreEqual(Alignment.Left, alignment);
	}

	[TestMethod]
	[DynamicData(nameof(GetRightMenuTypes))]
	public void GetInventoriesPlayerSourceTest(TransactionType transactionType)
	{
		(IInventory source, IInventory target) = MenuHelper.GetInventories(transactionType, _player, _inventory);

		Assert.AreSame(_player.Inventory, source);
		Assert.AreSame(_inventory, target);
	}

	[TestMethod]
	[DynamicData(nameof(GetLeftMenuTypes))]
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