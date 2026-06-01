#pragma warning disable IDE0058 // Expression value is never used
using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace LSDW.Application.Installers;

/// <summary>
/// The dependency injection installer for application services.
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Registers application services for dependency injection.
	/// </summary>
	/// <param name="services">The service collection to add the services to.</param>
	/// <returns>The same <see cref="IServiceCollection"/> collection for chaining.</returns>
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddSingleton<IEventService, EventService>()
			.AddSingleton<IPedestrianService, PedestrianService>()
			.AddSingleton<IScriptHookService, ScriptHookService>()
			.AddSingleton<ISystemService, SystemService>();

		return services;
	}
}
