#pragma warning disable IDE0058 // Expression value is never used
using LSDW.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LSDW.Domain.Installers;

/// <summary>
/// The dependency injection installer for domain services.
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Registers domain services for dependency injection.
	/// </summary>
	/// <param name="services">The service collection to add the services to.</param>
	/// <returns>The same <see cref="IServiceCollection"/> collection for chaining.</returns>
	public static IServiceCollection AddDomainServices(this IServiceCollection services)
	{
		services.AddSingleton<Settings>();

		return services;
	}
}
