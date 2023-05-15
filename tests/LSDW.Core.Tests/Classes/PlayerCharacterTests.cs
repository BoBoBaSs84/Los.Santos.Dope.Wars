using LSDW.Core.Interfaces.Classes;
using CF = LSDW.Core.Factories.PlayerFactory;
using IF = LSDW.Core.Factories.InventoryFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class PlayerCharacterTests
{
	[TestMethod]
	public void AddExperienceTest()
	{
		IPlayer character = CF.CreatePlayer();
		int pointsToAdd = 500;

		character.AddExperience(pointsToAdd);

		Assert.AreEqual(pointsToAdd, character.Experience);
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayer character = CF.CreatePlayer();
		int pointsToAdd = 1500;

		character.AddExperience(pointsToAdd);

		Assert.AreEqual(1, character.Level);
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{
		IPlayer character = CF.CreatePlayer();
		int pointsToAdd = 10000;

		character.AddExperience(pointsToAdd);

		Assert.AreEqual(120, character.MaximumInventoryQuantity);
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		IInventory drugs = IF.CreateInventory();
		int experience = 1000;

		IPlayer player = CF.CreatePlayer(drugs, experience);

		Assert.AreEqual(8000, player.ExperienceNextLevel);
	}
}