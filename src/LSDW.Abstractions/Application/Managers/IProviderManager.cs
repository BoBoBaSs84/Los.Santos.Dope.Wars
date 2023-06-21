using LSDW.Abstractions.Domain.Providers;

namespace LSDW.Abstractions.Application.Managers;

/// <summary>
/// The provider manager interface.
/// </summary>
public interface IProviderManager
{
	/// <summary>
	/// The time provider.
	/// </summary>
	ITimeProvider TimeProvider { get; }

	/// <summary>
	/// The location provider.
	/// </summary>
	ILocationProvider LocationProvider { get; }
}
