using GTA;
using LSDW.Application.Abstractions.Application.Services;
using LSDW.Application.Abstractions.Infrastructure.Services;
using LSDW.Application.Installers;
using LSDW.Domain.Events.System;
using LSDW.Domain.Installers;
using LSDW.Infrastructure.Installers;
using LSDW.Presentation.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace LSDW;

/// <summary>
/// The main entry point of the modification.
/// </summary>
[ScriptAttributes(Author = "BoBoBaSs84", SupportURL = "https://github.com/BoBoBaSs84")]
public sealed class StartUp : Script
{
	private readonly IEventService _eventService;
	private readonly ILoggerService _loggerService;
	private readonly ISettingsService _settingsService;

	/// <summary>
	/// Initializes a new instance of the <see cref="StartUp"/> class.
	/// </summary>
	public StartUp()
	{
		ServiceProvider serviceProvider = CreateServiceProvider();
		_eventService = serviceProvider.GetRequiredService<IEventService>();
		_loggerService = serviceProvider.GetRequiredService<ILoggerService>();

		_settingsService = serviceProvider.GetRequiredService<ISettingsService>();
		_settingsService.Load();

		AppDomain.CurrentDomain.UnhandledException += (s, e)
			=> OnUnhandledException(e.ExceptionObject as Exception);

		Interval = 10;

		Tick += (s, e) => _eventService.Publish(new TickTriggered());
		Aborted += (s, e) => _eventService.Publish(new AbortTriggered());
		KeyDown += (s, e) => _eventService.Publish(new KeyPressed(e.KeyData));
		KeyUp += (s, e) => _eventService.Publish(new KeyReleased(e.KeyData));
	}

	private void OnUnhandledException(Exception? exception)
		=> _loggerService.Critical("An unhandled exception occurred.", exception);

	/// <summary>
	/// Creates the service provider.
	/// </summary>
	/// <returns>The service provider.</returns>
	private static ServiceProvider CreateServiceProvider()
	{
		IServiceCollection services = new ServiceCollection()
			.AddApplicationServices()
			.AddDomainServices()
			.AddInfrastructureServices()
			.AddPresentationServices();

		return services.BuildServiceProvider();
	}
}
