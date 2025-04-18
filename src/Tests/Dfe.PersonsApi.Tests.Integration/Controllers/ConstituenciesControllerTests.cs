﻿using System.Net;
using Dfe.PersonsApi.Infrastructure;
using Dfe.PersonsApi.Client.Contracts;
using Microsoft.EntityFrameworkCore;
using PersonsApi;
using System.Security.Claims;
using Dfe.PersonsApi.Tests.Common.Customizations;
using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using DfE.CoreLibs.Testing.Mocks.WebApplicationFactory;

namespace Dfe.PersonsApi.PersonsApi.Tests.Integration.Controllers
{
    public class ConstituenciesControllerTests
    {
        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization<Startup>))]
        public async Task GetMemberOfParliamentByConstituencyAsync_ShouldReturnMp_WhenConstituencyExists(
            CustomWebApplicationDbContextFactory<Startup> factory,
            IConstituenciesClient constituenciesClient)
        {
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            // Arrange
            var dbContext = factory.GetDbContext<MopContext>();

            await dbContext.Constituencies
                .Where(x => x.ConstituencyName == "Test Constituency 1")
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.ConstituencyName, "NewConstituencyName"));

            var constituencyName = Uri.EscapeDataString("NewConstituencyName");

            // Act
            var result = await constituenciesClient.GetMemberOfParliamentByConstituencyAsync(constituencyName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NewConstituencyName", result.ConstituencyName);
        }

        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization<Startup>))]
        public async Task GetMemberOfParliamentByConstituencyAsync_ShouldReturnNotFound_WhenConstituencyDoesNotExist(
            CustomWebApplicationDbContextFactory<Startup> factory,
            IConstituenciesClient constituenciesClient)
        {
            // Arrange
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            var constituencyName = Uri.EscapeDataString("NonExistentConstituency");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PersonsApiException>(async () =>
                await constituenciesClient.GetMemberOfParliamentByConstituencyAsync(constituencyName));

            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)exception.StatusCode);
        }

        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization<Startup>))]
        public async Task GetMemberOfParliamentByConstituenciesAsync_ShouldReturnMps_WhenConstituenciesExists(
            CustomWebApplicationDbContextFactory<Startup> factory,
            IConstituenciesClient constituenciesClient)
        {
            // Arrange
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            var dbcontext = factory.GetDbContext<MopContext>();

            await dbcontext.Constituencies.Where(x => x.ConstituencyName == "Test Constituency 1")
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.ConstituencyName, "NewConstituencyName"));

            var constituencyName = Uri.EscapeDataString("NewConstituencyName");

            // Act
            var result = await constituenciesClient.GetMembersOfParliamentByConstituenciesAsync(
                new GetMembersOfParliamentByConstituenciesQuery() { ConstituencyNames = [constituencyName, "Test Constituency 2"] });

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization<Startup>))]
        public async Task GetMemberOfParliamentByConstituenciesAsync_ShouldReturnEmpty_WhenConstituenciesDontExists(
            CustomWebApplicationDbContextFactory<Startup> factory,
            IConstituenciesClient constituenciesClient)
        {
            // Arrange
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            // Act
            var result = await constituenciesClient.GetMembersOfParliamentByConstituenciesAsync(
                new GetMembersOfParliamentByConstituenciesQuery() { ConstituencyNames = ["constituencyName"] });

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization<Startup>))]
        public async Task GetMemberOfParliamentByConstituenciesAsync_ShouldThrowAnException_WhenConstituenciesNotProvided(
            CustomWebApplicationDbContextFactory<Startup> factory,
            IConstituenciesClient constituenciesClient)
        {
            // Arrange
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PersonsApiException>(async () =>
                await constituenciesClient.GetMembersOfParliamentByConstituenciesAsync(
                    new GetMembersOfParliamentByConstituenciesQuery() { ConstituencyNames = [] }));

            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)exception.StatusCode);
        }
    }
}
