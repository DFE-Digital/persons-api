using AutoFixture.Xunit2;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.Constituencies.Queries.SearchMembersOfParliament;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using Dfe.PersonsApi.Tests.Common.Customizations.Entities;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Customizations;
using MockQueryable;
using NSubstitute;

namespace Dfe.PersonsApi.Application.Tests.QueryHandlers.Constituency
{
    public class SearchMembersOfParliamentQueryHandlerTests
    {
        [Theory]
        [CustomAutoData(
            typeof(ConstituencyCustomization),
            typeof(DateOnlyCustomization),
            typeof(AutoMapperCustomization<ConstituencyProfile>))]
        public async Task Handle_ShouldReturnMembersOfParliament_WhenTheSearchTermMatches(
            [Frozen] IConstituencyRepository mockConstituencyRepository,
            [Frozen] ICacheService<IMemoryCacheType> mockCacheService,
            SearchMembersOfParliamentQueryHandler handler,
            List<PersonsApi.Domain.Constituencies.Constituency> constituencies)
        {
            // Arrange
            const string searchTerm = "john doe";

            mockConstituencyRepository.SearchMembersOfParliamentQueryable(searchTerm)
                .Returns(constituencies.BuildMock());

            SetupCachePassThrough(mockCacheService, searchTerm);

            // Act
            var result = await handler.Handle(new SearchMembersOfParliamentQuery(searchTerm), default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(constituencies.Count, result.Value!.Count);
            Assert.All(result.Value!, memberOfParliament =>
            {
                Assert.Equal("John", memberOfParliament.FirstName);
                Assert.Equal("Doe", memberOfParliament.LastName);
            });
            Assert.Equal(
                constituencies.Select(c => c.ConstituencyName),
                result.Value!.Select(memberOfParliament => memberOfParliament.ConstituencyName));

            mockConstituencyRepository.Received(1).SearchMembersOfParliamentQueryable(searchTerm);
        }

        [Theory]
        [CustomAutoData(
            typeof(ConstituencyCustomization),
            typeof(DateOnlyCustomization),
            typeof(AutoMapperCustomization<ConstituencyProfile>))]
        public async Task Handle_ShouldTrimAndLowercaseTheSearchTerm_BeforeSearching(
            [Frozen] IConstituencyRepository mockConstituencyRepository,
            [Frozen] ICacheService<IMemoryCacheType> mockCacheService,
            SearchMembersOfParliamentQueryHandler handler,
            List<PersonsApi.Domain.Constituencies.Constituency> constituencies)
        {
            // Arrange
            const string normalisedSearchTerm = "london";

            mockConstituencyRepository.SearchMembersOfParliamentQueryable(normalisedSearchTerm)
                .Returns(constituencies.BuildMock());

            SetupCachePassThrough(mockCacheService, normalisedSearchTerm);

            // Act
            var result = await handler.Handle(new SearchMembersOfParliamentQuery("  LoNdOn  "), default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(constituencies.Count, result.Value!.Count);

            mockConstituencyRepository.Received(1).SearchMembersOfParliamentQueryable(normalisedSearchTerm);
        }

        [Theory]
        [CustomAutoData(
            typeof(ConstituencyCustomization),
            typeof(DateOnlyCustomization),
            typeof(AutoMapperCustomization<ConstituencyProfile>))]
        public async Task Handle_ShouldReturnAnEmptyCollection_WhenNothingMatches(
            [Frozen] IConstituencyRepository mockConstituencyRepository,
            [Frozen] ICacheService<IMemoryCacheType> mockCacheService,
            SearchMembersOfParliamentQueryHandler handler)
        {
            // Arrange
            const string searchTerm = "no such member or constituency";

            mockConstituencyRepository.SearchMembersOfParliamentQueryable(searchTerm)
                .Returns(new List<PersonsApi.Domain.Constituencies.Constituency>().BuildMock());

            SetupCachePassThrough(mockCacheService, searchTerm);

            // Act
            var result = await handler.Handle(new SearchMembersOfParliamentQuery(searchTerm), default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value!);
        }

        private static void SetupCachePassThrough(ICacheService<IMemoryCacheType> mockCacheService, string searchTerm)
        {
            var cacheKey = $"MemberOfParliamentSearch_{CacheKeyHelper.GenerateHashedCacheKey(searchTerm)}";

            mockCacheService.GetOrAddAsync(cacheKey, Arg.Any<Func<Task<Result<List<MemberOfParliament>>>>>(), Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var callback = callInfo.ArgAt<Func<Task<Result<List<MemberOfParliament>>>>>(1);
                    return callback();
                });
        }
    }
}
