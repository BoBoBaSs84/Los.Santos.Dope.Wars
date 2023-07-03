using GTA;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Infrastructure.Services;

internal sealed partial class SettingsService : ISettingsService
{
	private readonly ScriptSettings _scriptSettings;

	/// <summary>
	/// The settings service singleton instance.
	/// </summary>
	public static SettingsService Instance => new();

	public void Save()
		=> _scriptSettings.Save();
}
