using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Providers;
using LSDW.Abstractions.Infrastructure.Services;
using Moq;

namespace LSDW.Base.Tests.Helpers;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public static class MockHelper
{
	public static Mock<INotificationProvider> GetNotificationServiceMock()
		=> new();

	public static Mock<ITimeProvider> GetTimeProviderMock()
	{
		Mock<ITimeProvider> mock = new();
		mock.Setup(x => x.Now).Returns(DateTime.MinValue.AddYears(100));
		mock.Setup(x => x.TimeOfDay).Returns(DateTime.MinValue.AddYears(100).TimeOfDay);
		return mock;
	}

	public static Mock<ILoggerService> GetLoggerServiceMock()
		=> new();

	public static Mock<IGameStateService> GetGameStateServiceMock()
		=> new();

	public static Mock<ITrafficking> GetTraffickingMock()
		=> new();

	public static Mock<IInventory> GetInventoryMock()
		=> new();

	public static Mock<IPlayer> GetPlayerMock()
		=> new();
}
