using LSDW.Abstractions.Domain.Models;
using LSDW.Domain.Models;

namespace LSDW.Domain.Tests.Models;

[TestClass, ExcludeFromCodeCoverage]
public partial class SettingsTests
{
	private readonly ISettings _settings;

	public SettingsTests() => _settings = Settings.Instance;

	[TestMethod]
	public void InstanceTest()
	{
		Assert.IsNotNull(_settings);
		Assert.IsNotNull(_settings.Dealer);
		Assert.IsNotNull(_settings.Market);
		Assert.IsNotNull(_settings.Player);
		Assert.IsNotNull(_settings.Trafficking);
	}
}
