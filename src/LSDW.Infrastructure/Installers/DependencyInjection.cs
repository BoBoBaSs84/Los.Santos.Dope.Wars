#pragma warning disable IDE0058 // Expression value is never used
using LSDW.Application.Abstractions.Infrastructure.Services;
using LSDW.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;

namespace LSDW.Infrastructure.Installers;

/// <summary>
/// The dependency injection installer for infrastructure services.
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Registers infrastructure services for dependency injection.
	/// </summary>
	/// <param name="services">The service collection to add the services to.</param>
	/// <returns>The same <see cref="IServiceCollection"/> collection for chaining.</returns>
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
	{
		services.AddSingleton<ILoggerService, LoggerService>();
		services.AddSingleton<ISettingsService, SettingsService>();

		return services;
	}
}
