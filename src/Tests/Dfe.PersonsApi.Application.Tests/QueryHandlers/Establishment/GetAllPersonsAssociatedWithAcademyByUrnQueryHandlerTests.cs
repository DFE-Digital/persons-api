using AutoFixture;
using AutoFixture.Xunit2;
using Dfe.PersonsApi.Application.Common.Interfaces;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.Establishment.Queries.GetAllPersonsAssociatedWithAcademyByUrn;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Customizations;
using MockQueryable;
using NSubstitute;

namespace Dfe.PersonsApi.Application.Tests.QueryHandlers.Establishment
{
    public class GetAllPersonsAssociatedWithAcademyByUrnQueryHandlerTests
    {
        [Theory]
        [CustomAutoData(
            typeof(OmitCircularReferenceCustomization),
            typeof(AcademyGovernanceCustomization),
            typeof(AcademyGovernanceQueryModelCustomization),
            typeof(AutoMapperCustomization<AcademyWithGovernanceProfile>))]
        public async Task Handle_ShouldReturnPersonsAssociatedWithAcademy_WhenUrnExists(
            [Frozen] IEstablishmentQueryService mockEstablishmentQueryService,
            [Frozen] ICacheService<IMemoryCacheType> mockCacheService,
            GetAllPersonsAssociatedWithAcademyByUrnQueryHandler handler,
            GetAllPersonsAssociatedWithAcademyByUrnQuery query,
            List<AcademyGovernanceQueryModel> governanceQueryModels,
            IFixture fixture)

        {
            // Arrange
            var expectedGovernances = governanceQueryModels.Select(governance =>
                fixture.Customize(new AcademyGovernanceCustomization
                {
                    FirstName = governance?.EducationEstablishmentGovernance?.Forename1,
                    LastName = governance?.EducationEstablishmentGovernance?.Surname,
                }).Create<AcademyGovernance>()).ToList();

            var cacheKey = $"PersonsAssociatedWithAcademy_{CacheKeyHelper.GenerateHashedCacheKey(query.Urn.ToString())}";

            var mock = governanceQueryModels.BuildMock();

            mockEstablishmentQueryService.GetPersonsAssociatedWithAcademyByUrn(query.Urn)
                .Returns(mock);

            mockCacheService.GetOrAddAsync(cacheKey, Arg.Any<Func<Task<Result<List<AcademyGovernance>>>>>(), Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var callback = callInfo.ArgAt<Func<Task<Result<List<AcademyGovernance>>>>>(1);
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

            await mockCacheService.Received(1).GetOrAddAsync(cacheKey, Arg.Any<Func<Task<Result<List<AcademyGovernance>?>>>>(), nameof(GetAllPersonsAssociatedWithAcademyByUrnQueryHandler));
        }
    }
}
