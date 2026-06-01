using LSDW.Application.Abstractions.Application.Providers;
using LSDW.Application.Abstractions.Application.Services;
using LSDW.Domain.Models;
using LSDW.Infrastructure.Services;
using Moq;
using System.Text;
using System.Text.RegularExpressions;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
public sealed class LoggerServiceTests
{
	private const string CurrentDirectory = @"C:\Temp";
	private const string EnvNewLine = "\r\n";
	private const string ExpectedPath = @"C:\Temp\LSDW.log";

	private Mock<IFileProvider> _fileProviderMock = null!;
	private Mock<IPathProvider> _pathProviderMock = null!;
	private Mock<IEnvironmentProvider> _environmentProviderMock = null!;
	private Mock<ISystemService> _systemServiceMock = null!;
	private Settings _settings = null!;
	private LoggerService _sut = null!;

	[TestInitialize]
	public void Setup()
	{
		_fileProviderMock = new Mock<IFileProvider>();
		_pathProviderMock = new Mock<IPathProvider>();
		_environmentProviderMock = new Mock<IEnvironmentProvider>();
		_systemServiceMock = new Mock<ISystemService>();

		_systemServiceMock.Setup(s => s.File).Returns(_fileProviderMock.Object);
		_systemServiceMock.Setup(s => s.Path).Returns(_pathProviderMock.Object);
		_systemServiceMock.Setup(s => s.Environment).Returns(_environmentProviderMock.Object);

		_environmentProviderMock.Setup(e => e.CurrentDirectory).Returns(CurrentDirectory);
		_environmentProviderMock.Setup(e => e.NewLine).Returns(EnvNewLine);

		_settings = new Settings();
		_pathProviderMock.Setup(p => p.Combine(CurrentDirectory, _settings.General.LogFileName)).Returns(ExpectedPath);

		_sut = new LoggerService(_systemServiceMock.Object, _settings);
	}

	[TestMethod]
	public void ConstructorCombinesLogFilePathFromSystemProviders()
	{
		_ = new LoggerService(_systemServiceMock.Object, _settings);

		_pathProviderMock.Verify(p => p.Combine(CurrentDirectory, _settings.General.LogFileName), Times.AtLeastOnce);
	}

	[TestMethod]
	[DataRow("DBG", nameof(LoggerService.Debug))]
	[DataRow("INF", nameof(LoggerService.Information))]
	[DataRow("WRN", nameof(LoggerService.Warning))]
	[DataRow("ERR", nameof(LoggerService.Error))]
	public void LogMethodsAppendFormattedEntryToLogFile(string expectedType, string method)
	{
		const string message = "the message";
		const string callerName = "TheCaller";

		switch (method)
		{
			case nameof(LoggerService.Debug):
				_sut.Debug(message, callerName);
				break;
			case nameof(LoggerService.Information):
				_sut.Information(message, callerName);
				break;
			case nameof(LoggerService.Warning):
				_sut.Warning(message, callerName);
				break;
			case nameof(LoggerService.Error):
				_sut.Error(message, callerName);
				break;
		}

		_fileProviderMock.Verify(f => f.AppendAllText(
			ExpectedPath,
			It.Is<string>(content => MatchesLogEntry(content, expectedType, callerName, message)),
			It.IsAny<Encoding>()), Times.Once);
	}

	[TestMethod]
	public void LogMethodsUseUtf8Encoding()
	{
		_sut.Information("anything", "caller");

		_fileProviderMock.Verify(f => f.AppendAllText(
			ExpectedPath,
			It.IsAny<string>(),
			It.Is<Encoding>(e => e.Equals(Encoding.UTF8))), Times.Once);
	}

	[TestMethod]
	public void CriticalAppendsMessageAndExceptionToLogFile()
	{
		const string message = "boom";
		const string callerName = "TheCaller";
		InvalidOperationException exception = new("explosion");

		_sut.Critical(message, exception, callerName);

		_fileProviderMock.Verify(f => f.AppendAllText(
			ExpectedPath,
			It.Is<string>(content =>
				MatchesLogEntry(content, "FTL", callerName, $"{message} - {exception}")),
			It.IsAny<Encoding>()), Times.Once);
	}

	[TestMethod]
	public void CriticalAllowsNullException()
	{
		const string message = "boom";
		const string callerName = "TheCaller";

		_sut.Critical(message, exception: null, callerName);

		_fileProviderMock.Verify(f => f.AppendAllText(
			ExpectedPath,
			It.Is<string>(content =>
				MatchesLogEntry(content, "FTL", callerName, $"{message} - ")),
			It.IsAny<Encoding>()), Times.Once);
	}

	[TestMethod]
	public void LogMethodsUseEnvironmentNewLineAsTerminator()
	{
		_sut.Information("payload", "Caller");

		_fileProviderMock.Verify(f => f.AppendAllText(
			ExpectedPath,
			It.Is<string>(content => content.EndsWith(EnvNewLine, StringComparison.Ordinal)),
			It.IsAny<Encoding>()), Times.Once);
	}

	[TestMethod]
	public void LogMethodsUseCallerMemberNameWhenCallerNotProvided()
	{
		LogFromHelper();

		_fileProviderMock.Verify(f => f.AppendAllText(
			ExpectedPath,
			It.Is<string>(content => content.IndexOf($"<{nameof(LogFromHelper)}>", StringComparison.Ordinal) >= 0),
			It.IsAny<Encoding>()), Times.Once);
	}

	private void LogFromHelper()
		=> _sut.Information("payload");

	// Validates the log entry layout: "{timestamp}\t[{type}]\t<{caller}> - {message}{newline}".
	private static bool MatchesLogEntry(string content, string type, string caller, string message)
	{
		string pattern = @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}\t\[" + Regex.Escape(type) + @"\]\t<" + Regex.Escape(caller) + "> - " + Regex.Escape(message) + Regex.Escape(EnvNewLine) + "$";
		return Regex.IsMatch(content, pattern, RegexOptions.Singleline);
	}
}
