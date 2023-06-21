using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Missions.Base;

namespace LSDW.Abstractions.Domain.Missions;

/// <summary>
/// The trafficking mission interface.
/// </summary>
public interface ITrafficking : IMission
{
	/// <summary>
	/// The service manager instance to use.
	/// </summary>
	IServiceManager ServiceManager { get; }

	/// <summary>
	/// The provider manager instance to use.
	/// </summary>
	IProviderManager ProviderManager { get; }
}
