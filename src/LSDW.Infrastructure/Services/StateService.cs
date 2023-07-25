using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Extensions;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Constants;
using LSDW.Infrastructure.Models;
using LSDW.Infrastructure.Properties;
using System.Xml.Serialization;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Services;

/// <summary>
/// The state service class.
/// </summary>
internal sealed class StateService : IStateService
{
	private readonly static Lazy<StateService> _service = new(() => new());
	private readonly ILoggerService _loggerService;
	private readonly XmlSerializerNamespaces _namespaces;
	private readonly string _baseDirectory;
	private readonly string _saveFileName;

	/// <summary>
	/// Initializes a instance of the state service class.
	/// </summary>
	private StateService()
	{
		_loggerService = LoggerService.Instance;
		_namespaces = XmlConstants.SerializerNamespaces;
		_baseDirectory = AppContext.BaseDirectory;
		_saveFileName = DomainFactory.GetSettings().SaveFileName;

		Dealers = DomainFactory.CreateDealers();
		Player = DomainFactory.CreatePlayer();
	}

	/// <summary>
	/// The singleton instance of the state service.
	/// </summary>
	internal static StateService Instance
		=> _service.Value;

	public ICollection<IDealer> Dealers { get; private set; }
	public IPlayer Player { get; private set; }

	public bool Load(bool decompress = true)
	{
		string filePath = Path.Combine(_baseDirectory, _saveFileName);

		try
		{
			if (!File.Exists(filePath))
			{
				_loggerService.Warning(Resources.StateService_Load_NotFound.FormatInvariant(filePath));
				return Save(decompress);
			}

			string fileContent = File.ReadAllText(filePath);

			if (decompress)
				fileContent = fileContent.Decompress();

			State state = new State().FromXmlString(fileContent);

			Dealers = CreateDealers(state);
			Player = CreatePlayer(state);

			_loggerService.Information(Resources.StateService_Load_Loaded.FormatInvariant(filePath));
			return true;
		}
		catch (Exception ex)
		{
			_loggerService.Critical(Resources.StateService_Load_Critical, ex);
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

			_loggerService.Information(Resources.StateService_Save_Saved.FormatInvariant(filePath));
			return true;
		}
		catch (Exception ex)
		{
			_loggerService.Critical(Resources.StateService_Save_Critical, ex);
			return false;
		}
	}
}
