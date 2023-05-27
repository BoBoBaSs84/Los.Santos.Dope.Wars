using LSDW.Services;

namespace LSDW.Tests.Services;

[TestClass]
public class SettingsServiceTests
{
	[TestMethod]
	public void InitTest()
	{
		SettingsService settingsService = new();

		Assert.IsNotNull(settingsService);
	}
}