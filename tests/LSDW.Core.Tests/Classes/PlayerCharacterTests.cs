using LSDW.Core.Interfaces.Classes;
using CF = LSDW.Core.Factories.CharacterFactory;
using IF = LSDW.Core.Factories.InventoryFactory;

namespace LSDW.Core.Tests.Classes;

[TestClass]
public class PlayerCharacterTests
{
	[TestMethod]
	public void AddExperienceTest()
	{
		IPlayerCharacter character = CF.CreateNewPlayer();
		int pointsToAdd = 500;

		character.AddExperience(pointsToAdd);

		Assert.AreEqual(pointsToAdd, character.CurrentExperience);
	}

	[TestMethod]
	public void CurrentLevelTest()
	{
		IPlayerCharacter character = CF.CreateNewPlayer();
		int pointsToAdd = 1500;

		character.AddExperience(pointsToAdd);

		Assert.AreEqual(1, character.CurrentLevel);
	}

	[TestMethod]
	public void MaximumInventoryQuantityTest()
	{
		IPlayerCharacter character = CF.CreateNewPlayer();
		int pointsToAdd = 10000;

		character.AddExperience(pointsToAdd);

		Assert.AreEqual(120, character.MaximumInventoryQuantity);
	}

	[TestMethod]
	public void NextLevelExperienceTest()
	{
		IInventoryCollection drugs = IF.CreateInventory();
		int experience = 1000;

		IPlayerCharacter player = CF.CreateExistingPlayer(drugs, experience);

		Assert.AreEqual(8000, player.NextLevelExperience);
	}
}