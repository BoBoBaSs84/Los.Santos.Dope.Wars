using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Domain.Models;
using LSDW.Infrastructure.Constants;
using LSDW.Infrastructure.Models;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Services;

/// <summary>
/// The game state service class.
/// </summary>
internal sealed class GameStateService : IGameStateService
{
	private readonly string _baseDirectory = AppContext.BaseDirectory;
	private readonly string _saveFileName = Settings.SaveFileName;
	private readonly ILoggerService _logger;

	/// <summary>
	/// Initializes a instance of the game state service class.
	/// </summary>
	/// <param name="logger">The logger service instance to use.</param>
	internal GameStateService(ILoggerService logger)
	{
		_logger = logger;
		
		Dealers = DomainFactory.CreateDealers();
		Player = DomainFactory.CreatePlayer();
		
		_ = Load();
	}

	public ICollection<IDealer> Dealers { get; private set; }
	public IPlayer Player { get; private set; }

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
			GameState gameState = CreateGameState(Dealers, Player);

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
