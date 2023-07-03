using LSDW.Abstractions.Application.Managers;
using LSDW.Application.Managers;

namespace LSDW.Application.Factories;

internal static partial class ApplicationFactory
{
	/// <summary>
	/// Returns the provider manager singleton instance.
	/// </summary>
	public static IProviderManager GetProviderManager()
		=> ProviderManager.Instance;

	/// <summary>
	/// Returns the service manager singleton instance.
	/// </summary>
	public static IServiceManager GetServiceManager()
		=> ServiceManager.Instance;
}
