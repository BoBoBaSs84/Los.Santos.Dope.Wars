using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Classes.Models;
using static LSDW.Infrastructure.Factories.InfrastructureFactory;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
public class LoggerServiceTests
{
	private readonly string logFileNamePath = Path.Combine(Environment.CurrentDirectory, Settings.LogFileName);
	private readonly ILoggerService logger = CreateLoggerService();

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