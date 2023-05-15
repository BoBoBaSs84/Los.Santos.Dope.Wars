using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Classes;

namespace LSDW.Core.Tests.Factories;

[TestClass()]
public class CharacterFactoryTests
{
	[TestMethod()]
	public void CreatePlayerTest()
	{
		IPlayerCharacter? character;

		character = CharacterFactory.CreateNewPlayer();

		Assert.IsNotNull(character);
	}

	[TestMethod()]
	public void CreateExistingPlayerTest()
	{
		IPlayerCharacter? character;
		IInventoryCollection inventory = InventoryFactory.CreateInventory();
		int money = 12000;
		inventory.Add(money);
		int experience = money;

		character = CharacterFactory.CreateExistingPlayer(inventory, experience);

		Assert.IsNotNull(character);
		Assert.AreEqual(money, character.Inventory.Money);
		Assert.AreEqual(experience, character.CurrentExperience);
	}
}