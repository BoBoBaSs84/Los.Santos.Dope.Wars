using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Application.Models.Missions;

namespace LSDW.Application.Factories;

internal static partial class ApplicationFactory
{
	/// <summary>
	/// Creates a new trafficking mission instance.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	public static ITrafficking CreateTraffickingMission(IServiceManager serviceManager, IProviderManager providerManager)
		=> new Trafficking(serviceManager, providerManager);
}
