using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Abstractions.Models;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Constants;
using LSDW.Infrastructure.Models;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;
using RESX = LSDW.Infrastructure.Properties.Resources;

namespace LSDW.Infrastructure.Services;

/// <summary>
/// The state service class.
/// </summary>
internal sealed class StateService : IStateService
{
	private readonly XmlSerializerNamespaces _namespaces = XmlConstants.SerializerNamespaces;
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
				_logger.Warning(RESX.StateService_Load_NotFound.FormatInvariant(filePath));
				return Save(decompress);
			}

			string fileContent = File.ReadAllText(filePath);

			if (decompress)
				fileContent = fileContent.Decompress();

			State state = new State().FromXmlString(fileContent);

			Dealers = CreateDealers(state);
			Player = CreatePlayer(state);

			_logger.Information(RESX.StateService_Load_Loaded.FormatInvariant(filePath));
			return true;
		}
		catch (Exception ex)
		{
			_logger.Critical(RESX.StateService_Load_Critical, ex);
			return false;
		}
	}

	public bool Save(bool compress = true)
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			State state = CreateGameState(Dealers, Player);

			string fileContent = state.ToXmlString(_namespaces);

			if (compress)
				fileContent = fileContent.Compress();

			File.WriteAllText(filePath, fileContent);

			_logger.Information(RESX.StateService_Save_Saved.FormatInvariant(filePath));
			return true;
		}
		catch (Exception ex)
		{
			_logger.Critical(RESX.StateService_Save_Critical, ex);
			return false;
		}
	}
}
