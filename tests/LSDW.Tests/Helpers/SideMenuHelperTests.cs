using GTA.UI;
using LSDW.Core.Enumerators;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;
using LSDW.Helpers;

namespace LSDW.Tests.Helpers;

[TestClass]
public class SideMenuHelperTests
{
	[DataTestMethod]
	[DynamicData(nameof(GetRightMenuTypes), DynamicDataSourceType.Method)]
	public void GetMaximumPlayerQuantityTest(MenuType menuType)
	{
		IPlayer player = PlayerFactory.CreatePlayer();

		int maximumQuantity = SideMenuHelper.GetMaximumQuantity(menuType, player);

		Assert.AreEqual(int.MaxValue, maximumQuantity);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetLeftMenuTypes), DynamicDataSourceType.Method)]
	public void GetMaximumIntQuantityTest(MenuType menuType)
	{
		IPlayer player = PlayerFactory.CreatePlayer();

		int maximumQuantity = SideMenuHelper.GetMaximumQuantity(menuType, player);

		Assert.AreEqual(player.MaximumInventoryQuantity, maximumQuantity);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetPlayerSourceInventory), DynamicDataSourceType.Method)]
	public void GetInventoriesSourcePlayerTest(MenuType menuType, IPlayer player, IInventory drugs)
	{
		(IInventory source, IInventory target) = SideMenuHelper.GetInventories(menuType, player, drugs);

		Assert.AreEqual(player.Inventory, source);
		Assert.AreEqual(drugs, target);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetPlayerTargetInventory), DynamicDataSourceType.Method)]
	public void GetInventoriesTargetPlayerTest(MenuType menuType, IPlayer player, IInventory drugs)
	{
		(IInventory source, IInventory target) = SideMenuHelper.GetInventories(menuType, player, drugs);

		Assert.AreEqual(player.Inventory, target);
		Assert.AreEqual(drugs, source);
	}

	[TestMethod]
	public void GetTitleTest()
	{
	}

	[DataTestMethod]
	[DynamicData(nameof(GetRightMenuTypes), DynamicDataSourceType.Method)]
	public void GetRightAlignmentTest(MenuType menuType)
	{
		Alignment alignment = SideMenuHelper.GetAlignment(menuType);

		Assert.AreEqual(Alignment.Right, alignment);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetLeftMenuTypes), DynamicDataSourceType.Method)]
	public void GetLeftAlignmentTest(MenuType menuType)
	{
		Alignment alignment = SideMenuHelper.GetAlignment(menuType);

		Assert.AreEqual(Alignment.Left, alignment);
	}

	[TestMethod]
	public void GetSubtitleTest()
	{
	}

	private static IEnumerable<object[]> GetRightMenuTypes()
	{
		yield return new object[]
		{
			MenuType.SELL
		};
		yield return new object[]
		{
			MenuType.STORE
		};
		yield return new object[]
		{
			MenuType.GIVE
		};
	}

	private static IEnumerable<object[]> GetLeftMenuTypes()
	{
		yield return new object[]
		{
			MenuType.BUY
		};
		yield return new object[]
		{
			MenuType.RETRIEVE
		};
		yield return new object[]
		{
			MenuType.TAKE
		};
	}

	private static readonly IPlayer _player = PlayerFactory.CreatePlayer();
	private static readonly IInventory _inventory = InventoryFactory.CreateInventory();

	private static IEnumerable<object[]> GetPlayerSourceInventory()
	{
		yield return new object[]
		{
			MenuType.SELL,
			_player,
			_inventory
		};
		yield return new object[]
		{
			MenuType.STORE,
			_player,
			_inventory
		};
		yield return new object[]
		{
			MenuType.GIVE,
			_player,
			_inventory
		};
	}

	private static IEnumerable<object[]> GetPlayerTargetInventory()
	{
		yield return new object[]
		{
			MenuType.BUY,
			_player,
			_inventory
		};
		yield return new object[]
		{
			MenuType.RETRIEVE,
			_player,
			_inventory
		};
		yield return new object[]
		{
			MenuType.TAKE,
			_player,
			_inventory
		};
	}
}