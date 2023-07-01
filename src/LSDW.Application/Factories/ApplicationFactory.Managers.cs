using LSDW.Abstractions.Application.Managers;
using LSDW.Application.Managers;

namespace LSDW.Application.Factories;

internal static partial class ApplicationFactory
{
	/// <summary>
	/// Creates a new provider manager instance.
	/// </summary>
	/// <returns></returns>
	public static IProviderManager CreateProviderManager()
		=> new ProviderManager();

	/// <summary>
	/// Creates a new service manager instance.
	/// </summary>
	/// <returns></returns>
	public static IServiceManager CreateServiceManager()
		=> new ServiceManager();
}
