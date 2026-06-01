using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Abstractions.Infrastructure.Services;
using LSDW.Domain.Models;

namespace LSDW.Infrastructure.Services;

/// <summary>
/// Represents the settings service implementation. This service is responsible for loading
/// and saving modification settings to a file.
/// </summary>
internal sealed class SettingsService : ISettingsService
{
	private readonly ILoggerService _loggerService;
	private readonly ISystemService _systemService;
	private readonly Settings _settings;
	private readonly string _iniFilePath;

	/// <summary>
	/// Initializes a new instance of the <see cref="SettingsService"/> class.
	/// </summary>
	/// <param name="loggerService">The logger service instance to be used by the settings service.</param>
	/// <param name="systemService">The system service instance to be used by the settings service.</param>
	/// <param name="settings">The modification settings instance to be used by the settings service.</param>
	public SettingsService(ILoggerService loggerService, ISystemService systemService, Settings settings)
	{
		_loggerService = loggerService;
		_systemService = systemService;
		_settings = settings;

		_iniFilePath = _systemService.Path.Combine(_systemService.Environment.CurrentDirectory, _settings.General.IniFileName);
	}

	public Settings Current => _settings;

	public void Load()
	{
		_loggerService.Information($"Loading settings from {_iniFilePath}.");
		try
		{
			if (!_systemService.File.Exists(_iniFilePath))
			{
				_loggerService.Warning($"Settings file {_iniFilePath} does not exist. Creating default settings.");
				Save();
				return;
			}
			string fileContent = _systemService.File.ReadAllText(_iniFilePath);
			Settings loadedSettings = Settings.Read(fileContent);
			_settings.Load(loadedSettings);
			_loggerService.Information($"Settings loaded successfully from {_iniFilePath}.");
		}
		catch (Exception ex)
		{
			_loggerService.Critical($"Failed to load settings from {_iniFilePath}.", ex);
			return;
		}
	}

	public void Save()
	{
		_loggerService.Information($"Saving settings to {_iniFilePath}.");
		try
		{
			string fileContent = Settings.Write(_settings);
			_systemService.File.WriteAllText(_iniFilePath, fileContent);
			_loggerService.Information($"Settings saved successfully to {_iniFilePath}.");
		}
		catch (Exception ex)
		{
			_loggerService.Critical($"Failed to save settings to {_iniFilePath}.", ex);
			return;
		}
	}
}
