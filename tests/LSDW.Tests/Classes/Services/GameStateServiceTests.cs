using LSDW.Classes.Persistence;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Helpers;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Models;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using LSDW.Interfaces.Services;

namespace LSDW.Tests.Classes.Services;

[TestClass]
[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public class GameStateServiceTests
{
	private static readonly string _cString = @"pVZdb4IwFP0rhPcJhaJIlIQhmYsymJBMH5k2SoJAKG7j3w9mVXTdbGh4IPdyOed+nPR2lODNp/EU7ZHwtU9SbDT2Q5yOxV1Z5tiQpG1c7g7vvXW2lx6z+okCrENpnuFeEKVl/ZpkOeq9RQWWntMPVLuKSmyDbVAnsAmKElRcISXdkObZ1knLm6zypBOWn0TVTVabolt9xWHbxukCIpqjn/kdsxJIYYbzlaMiRum67tdAA32oyppCYpvv5zkJZNqGm6WoGosAKNrwFFi7mxzxtSmQmo2wymt425s5px9q5+uhzjAuK1MeSb985yi/iNfI1LVLzNFDbML0L2/gWvasI3F/yEFsWy8WjVcBg/vMQOFgnlrBlMYMVXCfWVF5mj1deJ4b0MhVmYV8wEPuO86E2nFteJ8aAg5q3/Y7SkyBHLSuE1IHrUKNRWI6B/XMCanqBlqfodc86vadlUctmkXdKs9RsgxturIVhm7zEM8Dqq4h0BkqlnkENnGpU2YQtsYjbHvxx6ENVYaSdZ6SveWKfoZAhiHf3RdXJj7Z7U17Wb7kNhIjLEgkkuxvssyPF5/W1+Z2Zn4D";
	private static readonly string _baseDirectory = AppContext.BaseDirectory;
	private static readonly string _saveFileName = Settings.SaveFileName;

	[ClassInitialize]
	public static void ClassInitialize(TestContext context)
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);
		File.WriteAllText(filePath, _cString);
	}

	[ClassCleanup]
	public static void ClassCleanup()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		if (File.Exists(filePath))
			File.Delete(filePath);
	}

	[TestMethod]
	public void LoadTest()
	{
		IGameStateService? stateService;
		IPlayer player = ModelFactory.CreatePlayer(RandomHelper.GetInt(123456789, 987654321));
		player.Inventory.Randomize(player.Level);
		IEnumerable<IDealer> dealers = new List<IDealer>();
		stateService = LSDW.Factories.ServiceFactory.CreateGameStateService(player, dealers);
		
		GameState state = stateService.Load();
		
		Assert.IsNotNull(state);
	}

	[TestMethod]
	public void LoadExceptionTest()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		if (File.Exists(filePath))
			File.Delete(filePath);

		IGameStateService stateService = LSDW.Factories.ServiceFactory.CreateGameStateService(null!, null!);

		GameState state = stateService.Load();

		Assert.AreEqual(0, state.Player.Experience);
	}

	[TestMethod]
	public void SaveTest()
	{
		IPlayer player = ModelFactory.CreatePlayer(RandomHelper.GetInt(123456789, 987654321));
		player.Inventory.Randomize(player.Level);
		IEnumerable<IDealer> dealers = new List<IDealer>();

		IGameStateService stateService = LSDW.Factories.ServiceFactory.CreateGameStateService(player, dealers);

		bool success = stateService.Save();

		Assert.IsTrue(success);
	}

	[TestMethod]
	public void SaveExceptionTest()
	{
		IGameStateService stateService = LSDW.Factories.ServiceFactory.CreateGameStateService(null!, null!);

		bool success = stateService.Save();

		Assert.IsFalse(success);
	}
}