using GTA;
using LSDW.Abstractions.Infrastructure.Services;

namespace LSDW.Infrastructure.Services;

internal sealed partial class SettingsService : ISettingsService
{
	private readonly ScriptSettings _scriptSettings;

	public void Save()
		=> _scriptSettings.Save();
}
