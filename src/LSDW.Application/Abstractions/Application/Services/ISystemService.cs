using LSDW.Application.Abstractions.Application.Providers;

namespace LSDW.Application.Abstractions.Application.Services;

/// <summary>
/// Represents a service that provides access to various system-related providers,
/// such as date and time, directory, file, and path providers.
/// </summary>
public interface ISystemService
{
	/// <summary>
	/// Gets the date and time provider.
	/// </summary>
	IDateTimeProvider DateTime { get; }

	/// <summary>
	/// Gets the directory provider.
	/// </summary>
	IDirectoryProvider Directory { get; }

	/// <summary>
	/// Gets the environment provider.
	/// </summary>
	IEnvironmentProvider Environment { get; }

	/// <summary>
	/// Gets the file provider.
	/// </summary>
	IFileProvider File { get; }

	/// <summary>
	/// Gets the path provider.
	/// </summary>
	IPathProvider Path { get; }
}
