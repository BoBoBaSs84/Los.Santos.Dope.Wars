using GTA;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Infrastructure.Services;

internal sealed partial class SettingsService : ISettingsService
{
	private readonly ScriptSettings _scriptSettings;

	/// <summary>
	/// The singleton instance of the settings service.
	/// </summary>
	internal static readonly SettingsService Instance = new();

	public void Save()
		=> _scriptSettings.Save();
}
