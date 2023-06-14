using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Models;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
public class GameStateServiceTests
{
	private static readonly string _baseDirectory = AppContext.BaseDirectory;
	private static readonly string _saveFileName = Settings.SaveFileName;

	[TestCleanup]
	public void TestCleanup()
		=> DeleteSaveFile();

	[TestMethod]
	public void LoadTest()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);
		IGameStateService stateService = CreateGameStateService();

		bool success = stateService.Load();

		Assert.IsTrue(success);
		Assert.IsTrue(File.Exists(filePath));
	}

	[TestMethod]
	public void LoadExceptionTest()
	{
		DeleteSaveFile();
		string filePath = Path.Combine(_baseDirectory, _saveFileName);
		File.AppendAllText(filePath, "");
		IGameStateService stateService = CreateGameStateService();

		bool success = stateService.Load();

		Assert.IsFalse(success);
	}

	[TestMethod]
	public void SaveTest()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);
		IGameStateService stateService = CreateGameStateService();

		bool success = stateService.Save();

		Assert.IsTrue(success);
		Assert.IsTrue(File.Exists(filePath));
	}

	private static void DeleteSaveFile()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		if (File.Exists(filePath))
			File.Delete(filePath);
	}
}