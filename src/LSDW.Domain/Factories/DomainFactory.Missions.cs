using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Domain.Missions;

namespace LSDW.Domain.Factories;

public static partial class DomainFactory
{
	/// <summary>
	/// Creates a new trafficking mission instance.
	/// </summary>
	/// <param name="serviceManager">The service manager instance to use.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	public static ITrafficking CreateTraffickingMission(IServiceManager serviceManager, IProviderManager providerManager)
		=> new Trafficking(serviceManager, providerManager);
}
