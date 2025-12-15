using AutoFixture;
using AutoMapper;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Domain.Constituencies;
using Dfe.PersonsApi.Domain.ValueObjects;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Customizations;

namespace Dfe.PersonsApi.Application.Tests.MappingProfiles
{
    public class ConstituencyProfileTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public ConstituencyProfileTests()
        {  
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ConstituencyProfile>();
            });

            _fixture = new Fixture();
            _mapper = config.CreateMapper();
            _fixture.Customize(new DateOnlyCustomization());           
        }

        [Fact]
        public void Should_Map_Constituency_To_MemberOfParliament()
        {
            // Arrange
            var memberId = _fixture.Create<MemberId>();
            var constituency = _fixture.Build<Constituency>()
            .Without(c => c.ConstituencyName)   // Stops AutoFixture overriding
            .Without(c => c.PartyName)
            .With(c => c.MemberId, memberId)
            .With(c => c.NameDetails, new NameDetails("Doe, John", "John Doe MP", "Rt Hon John Doe MP"))
            .With(c => c.MemberContactDetails, new MemberContactDetails(memberId, 1, "john.doe@test.com", "01234 567890"))
            .With(c => c.LastRefresh, new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc))
            .With(c => c.ConstituencyName, "Bristol North")
            .With(c => c.PartyName, "Independent Party")
            .Create();

            // Act
            var result = _mapper.Map<MemberOfParliament>(constituency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memberId.Value, result.Id);
            Assert.Equal("John", result.FirstName);            
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john.doe@test.com", result.Email);
            Assert.Equal("John Doe MP", result.DisplayName);
            Assert.Equal("Rt Hon John Doe MP", result.DisplayNameWithTitle);
            Assert.Equal("01234 567890", result.Phone);
            Assert.Equal("Bristol North", result.ConstituencyName);
            Assert.Equal("Independent Party", result.ConstituencyPartyName);
        }

        [Fact]        
        public void Map_WhenConstituencyHasNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var memberId = _fixture.Create<MemberId>();
            var constituency = _fixture.Build<Constituency>()
            .Without(c => c.MemberContactDetails)
            .Without(c => c.PartyName)
            .With(c => c.MemberId, memberId)
            .With(c => c.NameDetails, new NameDetails("Doe, John", "John Doe MP", "Rt Hon John Doe MP"))
            .With(c => c.LastRefresh, new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc))
            .With(c => c.ConstituencyName, "Bristol North")
            .Create();           

            // Act
            var result = _mapper.Map<MemberOfParliament>(constituency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memberId.Value, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Null(result.Email);
            Assert.Equal("John Doe MP", result.DisplayName);
            Assert.Equal("Rt Hon John Doe MP", result.DisplayNameWithTitle);
            Assert.Null(result.Phone);
            Assert.Equal("Bristol North", result.ConstituencyName);
            Assert.Null(result.ConstituencyPartyName);          
        }

    }
}
