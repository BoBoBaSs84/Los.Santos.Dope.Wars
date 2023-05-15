using LSDW.Core.Classes;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Classes;
using LSDW.Core.Properties;

namespace LSDW.Core.Tests.Extensions;

[TestClass]
public class ClassExtensionsTests
{
	private readonly string SaveFileNamePath = Path.Combine(AppContext.BaseDirectory, Settings.Default.SaveFileName);

	[TestCleanup]
	public void TestCleanup()
	{
		//if (File.Exists(SaveFileNamePath))
		//	File.Delete(SaveFileNamePath);
	}

	[TestMethod]
	public void ToXmlStringTest()
	{
		IEnumerable<IDrug> drugs = DrugFactory.CreateRandomDrugs();
		IInventory inventory = InventoryFactory.CreateInventory(drugs);
		inventory.Add(RandomHelper.GetInt(25000, 75000));
		int experience = RandomHelper.GetInt(75000, 275000);
		IPlayer player = PlayerFactory.CreatePlayer(inventory, experience);

		PlayerState playerState = StateFactory.CreatePlayerState(player);
		string xmlString = playerState.ToXmlString();

		File.WriteAllText(SaveFileNamePath, xmlString, System.Text.Encoding.UTF8);

		Assert.IsNotNull(xmlString);
	}

	[TestMethod()]
	public void FromXmlStringTest()
	{
		string xmlString = File.ReadAllText(SaveFileNamePath);
		PlayerState playerState = new PlayerState().FromXmlString(xmlString);

		IPlayer player = PlayerFactory.CreatePlayer(playerState);

		Assert.IsNotNull(player);
	}
}