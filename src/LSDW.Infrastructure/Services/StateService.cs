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
/// The state service class.
/// </summary>
internal sealed class StateService : IStateService
{
	private readonly string _baseDirectory = AppContext.BaseDirectory;
	private readonly string _saveFileName = Settings.SaveFileName;
	private readonly ILoggerService _logger;

	/// <summary>
	/// Initializes a instance of the state service class.
	/// </summary>
	/// <param name="logger">The logger service instance to use.</param>
	internal StateService(ILoggerService logger)
	{
		_logger = logger;

		Dealers = DomainFactory.CreateDealers();
		Player = DomainFactory.CreatePlayer();
	}

	public ICollection<IDealer> Dealers { get; private set; }
	public IPlayer Player { get; private set; }

	public bool Load(bool decompress = true)
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			if (!File.Exists(filePath))
			{
				_logger.Information($"'{filePath}' was not found.");
				return Save(decompress);
			}
				

			string fileContent = File.ReadAllText(filePath);

			if (decompress)
				fileContent = fileContent.Decompress();

			GameState gameState = new GameState().FromXmlString(fileContent);
			_logger.Debug($"States of {nameof(gameState.Dealers)}: {gameState.Dealers.Count}.");

			Dealers = CreateDealers(gameState);
			_logger.Debug($"Instances of {nameof(Dealers)}: {Dealers.Count}.");
			Player = CreatePlayer(gameState);

			_logger.Information($"'{filePath}' was loaded.");
			return true;
		}
		catch (Exception ex)
		{
			_logger.Critical(ex.Message);
			return false;
		}
	}

	public bool Save(bool compress = true)
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			GameState gameState = CreateGameState(Dealers, Player);

			string fileContent = gameState.ToXmlString(XmlConstants.SerializerNamespaces);

			if (compress)
				fileContent = fileContent.Compress();

			File.WriteAllText(filePath, fileContent);

			_logger.Information($"'{filePath}' was saved.");
			return true;
		}
		catch (Exception ex)
		{
			_logger.Critical(ex.Message);
			return false;
		}
	}
}
