using GTA;
using GTA.Math;
using LSDW.Abstractions.Domain.Missions;
using LSDW.Abstractions.Infrastructure.Services;
using LSDW.Base.Tests.Helpers;
using Moq;

namespace LSDW.Infrastructure.Tests.Factories;

[TestClass]
public partial class InfrastructureFactoryTests
{
	private static readonly Mock<ILoggerService> _loggerMock = MockHelper.GetLoggerServiceMock();
	private static readonly Mock<ITrafficking> _traffickingMock = MockHelper.GetTraffickingMock();
	private static readonly Vector3 _zeroVector = new(0, 0, 0);
	private static readonly PedHash _pedHash = PedHash.Clown01SMY;
}
