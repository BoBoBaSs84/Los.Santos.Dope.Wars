using LSDW.Abstractions.Application.Managers;
using LSDW.Base.Tests.Helpers;
using Moq;

namespace LSDW.Application.Tests.Factories;

[TestClass]
public partial class ApplicationFactoryTests
{
	private readonly Mock<IServiceManager> _serviceManagerMock = MockHelper.GetServiceManager();
	private readonly Mock<IProviderManager> _providerManagerMock = MockHelper.GetProviderManager();
}