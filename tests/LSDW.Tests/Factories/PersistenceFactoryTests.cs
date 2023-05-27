using GTA.Math;
using LSDW.Classes.Persistence;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;
using LSDW.Factories;
using LSDW.Interfaces.Actors;

namespace LSDW.Tests.Factories;

[TestClass]
public class PersistenceFactoryTests
{
	[TestMethod]
	public void CreateDrugStateTest()
	{
		IDrug drug = DrugFactory.CreateDrug();

		DrugState drugState = PersistenceFactory.CreateDrugState(drug);

		Assert.IsNotNull(drugState);
	}

	[TestMethod()]
	public void CreateDrugTest()
	{

	}

	[TestMethod]
	public void CreateDrugStatesTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();

		List<DrugState> drugStates = PersistenceFactory.CreateDrugStates(drugs);

		Assert.IsNotNull(drugStates);
		Assert.AreEqual(drugs.Count(), drugStates.Count);
	}

	[TestMethod]
	public void CreateDrugsTest()
	{

	}

	[TestMethod]
	public void CreateInventoryStateTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		int money = RandomHelper.GetInt();
		IInventory inventory = InventoryFactory.CreateInventory(drugs, money);

		InventoryState inventoryState = PersistenceFactory.CreateInventoryState(inventory);

		Assert.IsNotNull(inventoryState);
		Assert.AreEqual(money, inventoryState.Money);
		Assert.AreEqual(drugs.Count(), inventoryState.Drugs.Count);
	}

	[TestMethod]
	public void CreateInventoryTest()
	{

	}

	[TestMethod]
	public void CreatePlayerStateTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateAllDrugs();
		int money = RandomHelper.GetInt();
		IInventory inventory = InventoryFactory.CreateInventory(drugs, money);
		int experience = RandomHelper.GetInt();
		IPlayer player = PlayerFactory.CreatePlayer(inventory, experience);

		PlayerState playerState = PersistenceFactory.CreatePlayerState(player);

		Assert.IsNotNull(playerState);
		Assert.AreEqual(experience, playerState.Experience);
		Assert.AreEqual(money, playerState.Inventory.Money);
		Assert.AreEqual(drugs.Count(), playerState.Inventory.Drugs.Count);
	}

	[TestMethod]
	public void CreatePlayerTest()
	{

	}

	[TestMethod()]
	public void CreateDealerStateTest()
	{
		IDealer dealer = ActorFactory.CreateDealer(new Vector3(287.011f, -991.685f, 33.108f));

		DealerState dealerState = PersistenceFactory.CreateDealerState(dealer);

		Assert.IsNotNull(dealerState);
	}
}