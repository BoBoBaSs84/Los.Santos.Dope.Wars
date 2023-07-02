using GTA;
using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Base.Tests.Helpers;
using Moq;

namespace LSDW.Domain.Tests.Factories;

[TestClass]
public partial class DomainFactoryTests
{
	private readonly Mock<IProviderManager> _providerManagerMock = MockHelper.GetProviderManager();
	private readonly Vector3 _zeroVector = Vector3.Zero;
	private readonly PedHash _pedHash = PedHash.Clown01SMY;
}
