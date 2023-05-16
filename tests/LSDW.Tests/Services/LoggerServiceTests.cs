using Logger = LSDW.Services.LoggerService;

namespace LSDW.Tests.Services;

[TestClass]
public class LoggerServiceTests
{
	[TestInitialize]
	public void TestInitialize()
	{
		if (File.Exists(Logger.LogFileNamePath))
			File.Delete(Logger.LogFileNamePath);
	}

	[TestCleanup]
	public void TestCleanup()
	{
		if (File.Exists(Logger.LogFileNamePath))
			File.Delete(Logger.LogFileNamePath);
	}

	[TestMethod]
	public void InformationTest()
	{
		string message = "This is a informational message";

		Logger.Information(message);

		Assert.IsTrue(File.Exists(Logger.LogFileNamePath));
	}

	[TestMethod]
	public void WarningTest()
	{
		string message = "This is a warning message";

		Logger.Warning(message);

		Assert.IsTrue(File.Exists(Logger.LogFileNamePath));
	}

	[TestMethod]
	public void ErrorTest()
	{
		string message = "This is a error message";

		Logger.Error(message);

		Assert.IsTrue(File.Exists(Logger.LogFileNamePath));
	}
}