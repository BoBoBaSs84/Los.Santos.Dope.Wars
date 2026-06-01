using LSDW.Application.Abstractions.Application.Providers;
using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Abstractions.Infrastructure.Services;
using LSDW.Domain.Models;
using LSDW.Infrastructure.Services;

using Moq;

namespace LSDW.Infrastructure.Tests.Services;

[TestClass]
public sealed class SettingsServiceTests
{
	private const string CurrentDirectory = @"C:\Temp";
	private const string ExpectedPath = @"C:\Temp\LSDW.ini";

	private Mock<ILoggerService> _loggerServiceMock = null!;
	private Mock<IFileProvider> _fileProviderMock = null!;
	private Mock<IPathProvider> _pathProviderMock = null!;
	private Mock<IEnvironmentProvider> _environmentProviderMock = null!;
	private Mock<ISystemService> _systemServiceMock = null!;
	private Settings _settings = null!;
	private SettingsService _sut = null!;

	[TestInitialize]
	public void Setup()
	{
		_loggerServiceMock = new Mock<ILoggerService>();
		_fileProviderMock = new Mock<IFileProvider>();
		_pathProviderMock = new Mock<IPathProvider>();
		_environmentProviderMock = new Mock<IEnvironmentProvider>();
		_systemServiceMock = new Mock<ISystemService>();

		_systemServiceMock.Setup(s => s.File).Returns(_fileProviderMock.Object);
		_systemServiceMock.Setup(s => s.Path).Returns(_pathProviderMock.Object);
		_systemServiceMock.Setup(s => s.Environment).Returns(_environmentProviderMock.Object);

		_environmentProviderMock.Setup(e => e.CurrentDirectory).Returns(CurrentDirectory);

		_settings = new Settings();
		_pathProviderMock.Setup(p => p.Combine(CurrentDirectory, _settings.General.IniFileName)).Returns(ExpectedPath);

		_sut = new SettingsService(_loggerServiceMock.Object, _systemServiceMock.Object, _settings);
	}

	[TestMethod]
	public void ConstructorCombinesIniFilePathFromSystemProviders()
	{
		_ = new SettingsService(_loggerServiceMock.Object, _systemServiceMock.Object, _settings);

		_pathProviderMock.Verify(p => p.Combine(CurrentDirectory, _settings.General.IniFileName), Times.AtLeastOnce);
	}

	[TestMethod]
	public void CurrentReturnsInjectedSettingsInstance()
	{
		Settings actual = _sut.Current;

		Assert.AreSame(_settings, actual);
	}

	[TestMethod]
	public void LoadCreatesDefaultsWhenFileDoesNotExist()
	{
		_fileProviderMock.Setup(f => f.Exists(ExpectedPath)).Returns(false);

		_sut.Load();

		_loggerServiceMock.Verify(l => l.Warning(
			It.Is<string>(m => m.IndexOf(ExpectedPath, StringComparison.Ordinal) >= 0),
			It.IsAny<string>()), Times.Once);
		_fileProviderMock.Verify(f => f.WriteAllText(ExpectedPath, It.IsAny<string>()), Times.Once);
		_fileProviderMock.Verify(f => f.ReadAllText(It.IsAny<string>()), Times.Never);
	}

	[TestMethod]
	public void LoadReadsExistingFileAndAppliesSettings()
	{
		string fileContent = Settings.Write(new Settings());
		_fileProviderMock.Setup(f => f.Exists(ExpectedPath)).Returns(true);
		_fileProviderMock.Setup(f => f.ReadAllText(ExpectedPath)).Returns(fileContent);

		_sut.Load();

		_fileProviderMock.Verify(f => f.ReadAllText(ExpectedPath), Times.Once);
		_loggerServiceMock.Verify(l => l.Information(
			It.Is<string>(m => m.IndexOf("loaded successfully", StringComparison.OrdinalIgnoreCase) >= 0),
			It.IsAny<string>()), Times.AtLeastOnce);
		_fileProviderMock.Verify(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
	}

	[TestMethod]
	public void LoadLogsCriticalWhenReadFails()
	{
		InvalidOperationException exception = new("disk read error");
		_fileProviderMock.Setup(f => f.Exists(ExpectedPath)).Returns(true);
		_fileProviderMock.Setup(f => f.ReadAllText(ExpectedPath)).Throws(exception);

		_sut.Load();

		_loggerServiceMock.Verify(l => l.Critical(
			It.Is<string>(m => m.IndexOf(ExpectedPath, StringComparison.Ordinal) >= 0),
			exception,
			It.IsAny<string>()), Times.Once);
	}

	[TestMethod]
	public void SaveWritesSerializedSettingsToFile()
	{
		_sut.Save();

		string expectedContent = Settings.Write(_settings);
		_fileProviderMock.Verify(f => f.WriteAllText(ExpectedPath, expectedContent), Times.Once);
		_loggerServiceMock.Verify(l => l.Information(
			It.Is<string>(m => m.IndexOf("saved successfully", StringComparison.OrdinalIgnoreCase) >= 0),
			It.IsAny<string>()), Times.AtLeastOnce);
	}

	[TestMethod]
	public void SaveLogsCriticalWhenWriteFails()
	{
		InvalidOperationException exception = new("disk write error");
		_fileProviderMock.Setup(f => f.WriteAllText(ExpectedPath, It.IsAny<string>())).Throws(exception);

		_sut.Save();

		_loggerServiceMock.Verify(l => l.Critical(
			It.Is<string>(m => m.IndexOf(ExpectedPath, StringComparison.Ordinal) >= 0),
			exception,
			It.IsAny<string>()), Times.Once);
	}
}
