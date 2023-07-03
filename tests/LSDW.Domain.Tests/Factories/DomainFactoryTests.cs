using GTA;
using GTA.Math;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Base.Tests.Helpers;
using Moq;

namespace LSDW.Domain.Tests.Factories;

[TestClass]
public partial class DomainFactoryTests
{
	private readonly Mock<IProviderManager> _providerManagerMock = MockHelper.GetProviderManager();
	private readonly Mock<IPlayer> _playerMock = MockHelper.GetPlayer();
	private readonly Mock<IInventory> _inventoryMock = MockHelper.GetInventory();
	private readonly Vector3 _zeroVector = Vector3.Zero;
	private readonly PedHash _pedHash = PedHash.Clown01SMY;
}
