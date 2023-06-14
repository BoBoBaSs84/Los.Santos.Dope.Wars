using LSDW.Abstractions.Domain.Actors;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Classes.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Constants;
using LSDW.Infrastructure.Models;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Services;

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

		Dealers = DomainFactory.CreateDealers();
		Player = DomainFactory.CreatePlayer();

		_ = Load();
	}

	public IPlayer Player { get; private set; }

	public ICollection<IDealer> Dealers { get; private set; }

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

			Dealers = CreateDealers(gameState);
			Player = CreatePlayer(gameState);

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
			GameState gameState = CreateGameState(Player, Dealers);

			string fileContent =
				gameState.ToXmlString(XmlConstants.SerializerNamespaces).Compress();

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
