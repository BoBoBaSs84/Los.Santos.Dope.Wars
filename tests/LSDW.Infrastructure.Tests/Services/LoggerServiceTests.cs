using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
[SuppressMessage("Usage", "CA2201", Justification = "Unit testing.")]
public class LoggerServiceTests
{
	private readonly string logFileNamePath = Path.Combine(Environment.CurrentDirectory, DomainFactory.GetSettings().LogFileName);
	private readonly ILoggerService logger = InfrastructureFactory.GetLoggerService();

	[TestCleanup]
	public void TestCleanup()
	{
		if (File.Exists(logFileNamePath))
			File.Delete(logFileNamePath);
	}

	[TestMethod]
	public void DebugTest()
	{
		string message = "This is a debug message";

		logger.Debug(message);

		Assert.IsTrue(File.Exists(logFileNamePath));
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

	[TestMethod]

	public void CriticalWithExceptionTest()
	{
		string message = "This is a critical error message";
		Exception exception = new("This is a test exception message");

		logger.Critical(message, exception);

		Assert.IsTrue(File.Exists(logFileNamePath));
	}
}