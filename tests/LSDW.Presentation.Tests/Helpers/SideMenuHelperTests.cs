using GTA.UI;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Enumerators;
using LSDW.Domain.Factories;
using LSDW.Presentation.Helpers;

namespace LSDW.Presentation.Tests.Helpers;

[TestClass]
public class SideMenuHelperTests
{
	[DataTestMethod]
	[DynamicData(nameof(GetRightMenuTypes), DynamicDataSourceType.Method)]
	public void GetRightAlignmentTest(TransactionType transactionType)
	{
		Alignment alignment = SideMenuHelper.GetAlignment(transactionType);

		Assert.AreEqual(Alignment.Right, alignment);
	}

	[DataTestMethod]
	[DynamicData(nameof(GetLeftMenuTypes), DynamicDataSourceType.Method)]
	public void GetLeftAlignmentTest(TransactionType transactionType)
	{
		Alignment alignment = SideMenuHelper.GetAlignment(transactionType);

		Assert.AreEqual(Alignment.Left, alignment);
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

	private static readonly IPlayer _player = DomainFactory.CreatePlayer();
	private static readonly IInventory _inventory = DomainFactory.CreateInventory();

	private static IEnumerable<object[]> GetPlayerSourceInventory()
	{
		yield return new object[]
		{
			TransactionType.SELL,
			_player,
			_inventory
		};
		yield return new object[]
		{
			TransactionType.GIVE,
			_player,
			_inventory
		};
	}

	private static IEnumerable<object[]> GetPlayerTargetInventory()
	{
		yield return new object[]
		{
			TransactionType.BUY,
			_player,
			_inventory
		};
		yield return new object[]
		{
			TransactionType.TAKE,
			_player,
			_inventory
		};
	}
}