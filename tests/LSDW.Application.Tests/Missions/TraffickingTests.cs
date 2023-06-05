﻿using LSDW.Abstractions.Interfaces.Application.Missions;
using LSDW.Abstractions.Interfaces.Application.Providers;
using LSDW.Abstractions.Interfaces.Infrastructure.Services;
using LSDW.Application.Missions;
using LSDW.Application.Tests.Helpers;
using LSDW.Domain.Enumerators;
using LSDW.Infrastructure.Factories;

namespace LSDW.Application.Tests.Missions;

[TestClass]
public class TraffickingTests
{
	[TestMethod]
	public void ConstructorTest()
	{
		ITimeProvider timeProvider = new RealTimeProvider();
		ILoggerService loggerService = InfrastructureFactory.CreateLoggerService();
		IGameStateService stateService = InfrastructureFactory.CreateGameStateService();

		ITrafficking trafficking = new Trafficking(timeProvider, loggerService, stateService);

		Assert.AreEqual(MissionStatusType.Stopped, trafficking.Status);
	}
}