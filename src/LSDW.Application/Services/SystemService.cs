using LSDW.Application.Abstractions.Application.Providers;
using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Providers;

namespace LSDW.Application.Services;

/// <summary>
/// Represents a service that provides access to various system-related providers,
/// such as date and time, directory, file, and path providers.
/// </summary>
internal sealed class SystemService : ISystemService
{
	private readonly Lazy<IDateTimeProvider> _dateTimeProvider = new(() => new DateTimeProvider());
	private readonly Lazy<IDirectoryProvider> _directoryProvider = new(() => new DirectoryProvider());
	private readonly Lazy<IEnvironmentProvider> _environmentProvider = new(() => new EnvironmentProvider());
	private readonly Lazy<IFileProvider> _fileProvider = new(() => new FileProvider());
	private readonly Lazy<IPathProvider> _pathProvider = new(() => new PathProvider());

	public IDateTimeProvider DateTime => _dateTimeProvider.Value;
	public IDirectoryProvider Directory => _directoryProvider.Value;
	public IEnvironmentProvider Environment => _environmentProvider.Value;
	public IFileProvider File => _fileProvider.Value;
	public IPathProvider Path => _pathProvider.Value;
}
