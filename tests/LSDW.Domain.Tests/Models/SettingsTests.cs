using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Models;

namespace LSDW.Domain.Tests.Models;

[TestClass]
public partial class SettingsTests
{
	private readonly ISettings _settings;

	public SettingsTests() => _settings = Settings.Instance;
}
