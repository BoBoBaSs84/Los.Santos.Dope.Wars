using LSDW.Abstractions.Application.Missions;
using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Application.Missions;
using LSDW.Application.Tests.Helpers;
using LSDW.Domain.Factories;
using LSDW.Infrastructure.Factories;

namespace LSDW.Application.Tests.Missions;

[TestClass]
public class TraffickingTests
{
	[TestMethod]
	public void ConstructorTest()
	{
		INotificationService notificationService = DomainFactory.CreateNotificationService();
		ITimeProvider timeProvider = new RealTimeProvider();
		ILoggerService loggerService = InfrastructureFactory.CreateLoggerService();
		IGameStateService stateService = InfrastructureFactory.CreateGameStateService();

		ITrafficking trafficking = new Trafficking(timeProvider, loggerService, stateService, notificationService);

		Assert.AreEqual(MissionStatusType.Stopped, trafficking.Status);
	}
}