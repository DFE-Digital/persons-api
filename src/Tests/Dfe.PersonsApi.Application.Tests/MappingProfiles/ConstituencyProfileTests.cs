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
            var memberId = new MemberId(123);
            var constituency = _fixture.Build<Constituency>()
                .With(c => c.MemberId, memberId)
                .With(c => c.NameDetails, new NameDetails(
                    NameListAs: "Doe, John",
                    NameDisplayAs: "John Doe MP",
                    NameFullTitle: "Rt Hon John Doe MP"
                ))
                .With(c => c.MemberContactDetails, new MemberContactDetails(
                    memberId: memberId,
                    typeId: 1,
                    email: "john.doe@test.com",
                    phone: "01234 567890"
                ))
                .With(c => c.LastRefresh, new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc))
                .With(c => c.ConstituencyName, "Bristol North")
                .With(c => c.PartyName, "Independent Party")
                .Create();

            // Act
            var result = _mapper.Map<MemberOfParliament>(constituency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Id);
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

            var memberId = new MemberId(123);
            var constituency = _fixture.Build<Constituency>()
                .With(c => c.MemberId, memberId)
                .With(c => c.NameDetails, new NameDetails(
                    NameListAs: "Doe, John",
                    NameDisplayAs: "John Doe MP",
                    NameFullTitle: "Rt Hon John Doe MP"
                ))                
                .With(c => c.LastRefresh, new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc))
                .With(c => c.ConstituencyName, "Bristol North")                
                .Create();


            constituency.PartyName = null;
            constituency.MemberContactDetails = null!;
            constituency.EndDate = null;

            // Act
            var result = _mapper.Map<MemberOfParliament>(constituency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Id);
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
