using LSDW.Classes.Persistence;
using LSDW.Core.Classes;
using LSDW.Core.Extensions;
using LSDW.Core.Interfaces.Classes;
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
	private readonly IPlayer _player;
	private readonly IEnumerable<IDealer> _dealers;
	private readonly string _baseDirectory;
	private readonly string _saveFileName;

	/// <summary>
	/// Initializes a instance of the game state service class.
	/// </summary>
	/// <param name="logger">The logger service instance to use.</param>
	/// <param name="player">The player instance to save.</param>
	/// <param name="dealers">The dealer instance colection to save.</param>
	internal GameStateService(ILoggerService logger, IPlayer player, IEnumerable<IDealer> dealers)
	{
		_logger = logger;
		_player = player;
		_dealers = dealers;

		_baseDirectory = AppContext.BaseDirectory;
		_saveFileName = Settings.SaveFileName;
	}

	public GameState Load()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException($"Could not load {filePath}");

			string fileContent =
				File.ReadAllText(filePath).Decompress();

			GameState gameState =
				new GameState().FromXmlString(fileContent);

			return gameState;
		}
		catch (Exception ex)
		{
			_logger.Critical(ex.Message);
			return new();
		}
	}

	public bool Save()
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			GameState gameState =
				PersistenceFactory.CreateGameState(_player, _dealers);

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
