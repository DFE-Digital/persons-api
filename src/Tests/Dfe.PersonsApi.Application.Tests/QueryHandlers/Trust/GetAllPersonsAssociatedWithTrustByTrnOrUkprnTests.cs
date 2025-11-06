using AutoFixture;
using AutoFixture.Xunit2;
using Dfe.PersonsApi.Application.Common.Interfaces;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Application.Trust.Queries.GetAllPersonsAssociatedWithTrustByTrnOrUkprn;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Customizations;
using MockQueryable;
using NSubstitute;

namespace Dfe.PersonsApi.Application.Tests.QueryHandlers.Trust
{
    public class GetAllPersonsAssociatedWithTrustByTrnOrUkprnQueryHandlerTests
    {
        [Theory]
        [CustomAutoData(
            typeof(OmitCircularReferenceCustomization),
            typeof(TrustGovernanceCustomization),
            typeof(TrustGovernanceQueryModelCustomization),
            typeof(AutoMapperCustomization<TrustWithGovernanceProfile>))]
        public async Task Handle_ShouldReturnPersonsAssociatedWithTrust_WhenTrustExists(
            [Frozen] ITrustQueryService mockTrustQueryService,
            [Frozen] ICacheService<IMemoryCacheType> mockCacheService,
            GetAllPersonsAssociatedWithTrustByTrnOrUkprnQueryHandler handler,
            List<TrustGovernanceQueryModel> governanceQueryModels,
            IFixture fixture)
        {
            // Arrange
            var expectedGovernances = governanceQueryModels.Select(governance =>
                fixture.Customize(new TrustGovernanceCustomization
                {
                    FirstName = governance?.TrustGovernance?.Forename1,
                    LastName = governance?.TrustGovernance?.Surname,
                }).Create<TrustGovernance>()).ToList();

            var query = new GetAllPersonsAssociatedWithTrustByTrnOrUkprnQuery("09532567");

            var cacheKey = $"PersonsAssociatedWithTrust_{CacheKeyHelper.GenerateHashedCacheKey(query.Id)}";

            var mock = governanceQueryModels.BuildMock();

            mockTrustQueryService.GetTrustGovernanceByGroupIdOrUkprn(Arg.Any<string>(), Arg.Any<string>())
                .Returns(mock);

            mockCacheService.GetOrAddAsync(cacheKey, Arg.Any<Func<Task<Result<List<TrustGovernance>>>>>(), Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var callback = callInfo.ArgAt<Func<Task<Result<List<TrustGovernance>>>>>(1);
                    return callback();
                });

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(expectedGovernances.Count, result.Value!.Count);
            for (int i = 0; i < result.Value!.Count; i++)
            {
                Assert.Equal(expectedGovernances[i].FirstName, result.Value![i].FirstName);
                Assert.Equal(expectedGovernances[i].LastName, result.Value![i].LastName);
            }

            await mockCacheService.Received(1).GetOrAddAsync(cacheKey, Arg.Any<Func<Task<Result<List<TrustGovernance>?>>>>(), nameof(GetAllPersonsAssociatedWithTrustByTrnOrUkprnQueryHandler));
        }
    }
}
