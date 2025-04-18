﻿using AutoFixture;
using AutoFixture.Xunit2;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.Constituencies.Queries.GetMemberOfParliamentByConstituency;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using Dfe.PersonsApi.Tests.Common.Customizations.Entities;
using DfE.CoreLibs.Caching.Helpers;
using DfE.CoreLibs.Caching.Interfaces;
using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using DfE.CoreLibs.Testing.AutoFixture.Customizations;
using NSubstitute;

namespace Dfe.PersonsApi.Application.Tests.QueryHandlers.Constituency
{
    public class GetMemberOfParliamentByConstituencyQueryHandlerTests
    {
        [Theory]
        [CustomAutoData(
            typeof(MemberOfParliamentCustomization),
            typeof(ConstituencyCustomization),
            typeof(AutoMapperCustomization<ConstituencyProfile>))]
        public async Task Handle_ShouldReturnMemberOfParliament_WhenConstituencyExists(
            [Frozen] IConstituencyRepository mockConstituencyRepository,
            [Frozen] ICacheService<IMemoryCacheType> mockCacheService,
            GetMemberOfParliamentByConstituencyQueryHandler handler,
            GetMemberOfParliamentByConstituencyQuery query,
            Domain.Constituencies.Constituency constituency,
            IFixture fixture)
        {
            // Arrange
            var expectedMp = fixture.Customize(new MemberOfParliamentCustomization()
                {
                    FirstName = constituency.NameDetails.NameListAs.Split(",")[1].Trim(),
                    LastName = constituency.NameDetails.NameListAs.Split(",")[0].Trim(),
                    ConstituencyName = constituency.ConstituencyName,
            }).Create<MemberOfParliament>();

            var cacheKey = $"MemberOfParliament_{CacheKeyHelper.GenerateHashedCacheKey(query.ConstituencyName)}";

            mockConstituencyRepository.GetMemberOfParliamentByConstituencyAsync(query.ConstituencyName, default)
                .Returns(constituency);

            mockCacheService.GetOrAddAsync(
                    cacheKey,
                    Arg.Any<Func<Task<Result<MemberOfParliament>>>>(),
                    Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var callback = callInfo.ArgAt<Func<Task<Result<MemberOfParliament>>>>(1);
                    return callback();
                });

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMp.FirstName, result.Value!.FirstName);
            Assert.Equal(expectedMp.LastName, result.Value!.LastName);
            Assert.Equal(expectedMp.ConstituencyName, result.Value!.ConstituencyName);

            await mockConstituencyRepository.Received(1).GetMemberOfParliamentByConstituencyAsync(query.ConstituencyName, default);
        }
    }
}
