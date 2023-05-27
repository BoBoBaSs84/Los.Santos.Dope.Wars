using LSDW.Core.Classes;
using LSDW.Factories;
using LSDW.Interfaces.Services;

namespace LSDW.Tests.Services;

[TestClass]
public class LoggerServiceTests
{
	private readonly string logFileNamePath = Path.Combine(Environment.CurrentDirectory, Settings.LogFileName);
	private readonly ILoggerService logger = ServiceFactory.CreateLoggerService();

	[TestInitialize]
	public void TestInitialize()
	{
		if (File.Exists(logFileNamePath))
			File.Delete(logFileNamePath);
	}

	[TestCleanup]
	public void TestCleanup()
	{
		if (File.Exists(logFileNamePath))
			File.Delete(logFileNamePath);
	}

	[TestMethod]
	public void InformationTest()
	{
		string message = "This is a informational message";

		logger.Information(message);

		Assert.IsTrue(File.Exists(logFileNamePath));
	}

	[TestMethod]
	public void WarningTest()
	{
		string message = "This is a warning message";

		logger.Warning(message);

		Assert.IsTrue(File.Exists(logFileNamePath));
	}

	[TestMethod]
	public void CriticalTest()
	{
		string message = "This is a critical error message";

		logger.Critical(message);

		Assert.IsTrue(File.Exists(logFileNamePath));
	}
}