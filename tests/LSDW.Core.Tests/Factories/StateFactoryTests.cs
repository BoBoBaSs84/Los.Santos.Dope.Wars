using LSDW.Core.Classes;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass]
public class StateFactoryTests
{
	[TestMethod]
	public void CreateDrugStateTest()
	{
		IDrug drug = DrugFactory.CreateRandomDrug();

		DrugState drugState = StateFactory.CreateDrugState(drug);

		Assert.IsNotNull(drugState);
	}

	[TestMethod]
	public void CreateDrugStatesTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateRandomDrugs();

		List<DrugState> drugStates = StateFactory.CreateDrugStates(drugs);

		Assert.IsNotNull(drugStates);
		Assert.AreEqual(drugs.Count(), drugStates.Count);
	}

	[TestMethod]
	public void CreateInventoryStateTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateRandomDrugs();
		IInventory inventory = InventoryFactory.CreateInventory(drugs);

		InventoryState inventoryState = StateFactory.CreateInventoryState(inventory);

		Assert.IsNotNull(inventoryState);
		Assert.AreEqual(drugs.Count(), inventoryState.Drugs.Count);
	}

	[TestMethod]
	public void CreatePlayerStateTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateRandomDrugs();
		int money = RandomHelper.GetInt();
		IInventory inventory = InventoryFactory.CreateInventory(drugs, money);
		int experience = RandomHelper.GetInt();
		IPlayer player = PlayerFactory.CreatePlayer(inventory, experience);

		PlayerState playerState = StateFactory.CreatePlayerState(player);

		Assert.IsNotNull(playerState);
		Assert.AreEqual(experience, playerState.Experience);
		Assert.AreEqual(money, playerState.Inventory.Money);
		Assert.AreEqual(drugs.Count(), playerState.Inventory.Drugs.Count);
	}
}