using LSDW.Classes.Persistence;
using LSDW.Core.Extensions;
using LSDW.Core.Factories;
using LSDW.Core.Interfaces.Models;
using LSDW.Core.Models;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using LSDW.Interfaces.Services;
using static LSDW.Constants.XmlConstants.NameSpaces;

namespace LSDW.Classes.Services;

/// <summary>
/// The game state service class.
/// </summary>
internal sealed class GameStateService : IGameStateService
{
	private readonly ILoggerService _logger;
	private readonly string _baseDirectory;
	private readonly string _saveFileName;

	/// <summary>
	/// Initializes a instance of the game state service class.
	/// </summary>
	/// <param name="logger">The logger service instance to use.</param>
	internal GameStateService(ILoggerService logger)
	{
		_logger = logger;

		_baseDirectory = AppContext.BaseDirectory;
		_saveFileName = Settings.SaveFileName;

		Dealers = ActorFactory.CreateDealers();
		Player = ModelFactory.CreatePlayer();

		_ = Load();
	}

	public IPlayer Player { get; private set; }

	public IEnumerable<IDealer> Dealers { get; private set; }

	public bool Load()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			if (!File.Exists(filePath))
				return Save();

			string fileContent =
				File.ReadAllText(filePath).Decompress();

			GameState gameState =
				new GameState().FromXmlString(fileContent);

			Dealers = PersistenceFactory.CreateDealers(gameState);
			Player = PersistenceFactory.CreatePlayer(gameState);

			return true;
		}
		catch (Exception ex)
		{
			_logger.Critical(ex.Message);
			return false;
		}
	}

	public bool Save()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			GameState gameState =
				PersistenceFactory.CreateGameState(Player, Dealers);

			string fileContent =
				gameState.ToXmlString(SerializerNamespaces).Compress();

			File.WriteAllText(filePath, fileContent);

			return true;
		}
		catch (Exception ex)
		{
			_logger.Critical(ex.Message);
			return false;
		}
	}
}
