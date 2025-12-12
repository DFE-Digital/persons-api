using AutoFixture;
using AutoMapper;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Domain.Constituencies;
using Dfe.PersonsApi.Domain.ValueObjects;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using Dfe.PersonsApi.Tests.Common.Customizations.Entities;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
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
            _fixture.Customize(new OmitCircularReferenceCustomization());
            _fixture.Customize(new MemberOfParliamentCustomization());

        }
        
        [Theory]
        [CustomAutoData(typeof(OmitCircularReferenceCustomization), typeof(ConstituencyCustomization))]
        public void Should_Map_Constituency_To_MemberOfParliament(Constituency constituency)
        {   

            // Act
            var result = _mapper.Map<MemberOfParliament>(constituency);

            // Assert
            Assert.NotNull(result);          
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);            
            Assert.Equal("John Doe", result.DisplayName);
            Assert.Equal("Mr. John Doe MP", result.DisplayNameWithTitle);            
            Assert.Equal(constituency.LastRefresh, result.UpdatedAt);
            Assert.Equal(constituency.PartyName, result.ConstituencyPartyName);
            Assert.Equal(constituency.ConstituencyName, result.ConstituencyName);           
        }

        [Fact]
        public void Map_WhenConstituencyHasNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var constituency = new Constituency(            
               new ConstituencyId(1),
               new MemberId(789),
               "Some Constituency",
               null,
               new NameDetails("Smith, Jane", "Jane Smith", "Mr"),
               DateTime.UtcNow,
               null,
               null!
            );

            // Act
            var result = _mapper.Map<MemberOfParliament>(constituency);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(789);           
            result.FirstName.Should().Be("Jane");
            result.LastName.Should().Be("Smith");
            result.Email.Should().BeNull();
            result.DisplayName.Should().Be("Jane Smith");
            result.DisplayNameWithTitle.Should().Be("Mr");  
            result.ConstituencyName.Should().Be("Some Constituency");
            result.ConstituencyPartyName.Should().BeNull();
            result.Email.Should().BeNull();
            result.Phone.Should().BeNull();
        }

    }
}
