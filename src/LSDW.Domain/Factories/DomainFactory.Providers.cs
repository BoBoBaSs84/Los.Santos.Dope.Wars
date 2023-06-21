using LSDW.Abstractions.Domain.Providers;
using LSDW.Domain.Providers;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new time provider instance.
	/// </summary>
	public static ITimeProvider CreateTimeProvider()
		=> new TimeProvider();

	/// <summary>
	/// Creates a new location provider instance.
	/// </summary>
	public static ILocationProvider CreateLocationProvider()
		=> new LocationProvider();
}
