using LSDW.Abstractions.Application.Providers;
using LSDW.Abstractions.Domain.Services;
using Moq;

namespace LSDW.Base.Tests.Helpers;

[SuppressMessage("Style", "IDE0058", Justification = "UnitTest")]
public static class MockHelper
{
	public static Mock<INotificationService> GetNotificationServiceMock()
		=> new();

	public static Mock<ITimeProvider> GetTimeProviderMock()
	{
		Mock<ITimeProvider> mock = new();
		mock.Setup(x => x.Now).Returns(DateTime.MaxValue);
		mock.Setup(x => x.TimeOfDay).Returns(DateTime.MaxValue.TimeOfDay);
		return mock;
	}
}
