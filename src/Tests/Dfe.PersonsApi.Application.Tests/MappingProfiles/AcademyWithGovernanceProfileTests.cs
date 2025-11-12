using AutoFixture;
using AutoMapper;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Domain.Establishment;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Customizations;

namespace Dfe.PersonsApi.Application.Tests.MappingProfiles
{
    public class AcademyWithGovernanceProfileTests
    {
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        public AcademyWithGovernanceProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AcademyWithGovernanceProfile>();
            });
            _mapper = config.CreateMapper();

            _fixture = new Fixture();
            _fixture.Customize(new OmitCircularReferenceCustomization());
            _fixture.Customize(new AcademyGovernanceQueryModelCustomization());
        }

        [Theory]
        [CustomAutoData(typeof(OmitCircularReferenceCustomization), typeof(AcademyGovernanceQueryModelCustomization))]
        public void Map_AcademyGovernanceQueryModelToAcademyGovernance_ShouldMapAllPropertiesCorrectly(
            AcademyGovernanceQueryModel source,
            IFixture fixture)
        {
            // Arrange
            var expectedId = (int)source.EducationEstablishmentGovernance.SK;
            var expectedFirstName = source.EducationEstablishmentGovernance.Forename1;
            var expectedLastName = source.EducationEstablishmentGovernance.Surname;
            var expectedEmail = source.EducationEstablishmentGovernance.Email;
            var expectedDisplayName = $"{source.EducationEstablishmentGovernance.Forename1} {source.EducationEstablishmentGovernance.Surname}";
            var expectedDisplayNameWithTitle = $"{source.EducationEstablishmentGovernance.Title} {source.EducationEstablishmentGovernance.Forename1} {source.EducationEstablishmentGovernance.Surname}";
            var expectedRoles = new List<string?> { source.GovernanceRoleType.Name };
            var expectedUpdatedAt = source.EducationEstablishmentGovernance.Modified;
            var expectedDateOfAppointment = source.EducationEstablishmentGovernance.DateOfAppointment;
            var expectedDateTermOfOfficeEndsEnded = source.EducationEstablishmentGovernance.DateTermOfOfficeEndsEnded;
            var expectedURN = source.Establishment.URN;
            var expectedUKPRN = source.Establishment.UKPRN;

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedId);
            result.FirstName.Should().Be(expectedFirstName);
            result.LastName.Should().Be(expectedLastName);
            result.Email.Should().Be(expectedEmail);
            result.DisplayName.Should().Be(expectedDisplayName);
            result.DisplayNameWithTitle.Should().Be(expectedDisplayNameWithTitle);
            result.Roles.Should().BeEquivalentTo(expectedRoles);
            result.UpdatedAt.Should().Be(expectedUpdatedAt);
            result.DateOfAppointment.Should().Be(expectedDateOfAppointment);
            result.DateTermOfOfficeEndsEnded.Should().Be(expectedDateTermOfOfficeEndsEnded);

            // Test Academy-specific properties that were missing
            result.URN.Should().Be(expectedURN);
            result.UKPRN.Should().Be(expectedUKPRN);

            // Test that Phone is not mapped (should be null since it's ignored)
            result.Phone.Should().BeNull();
        }

        [Fact]
        public void Map_WhenEducationEstablishmentGovernanceHasNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var establishmentGovernance = new EducationEstablishmentGovernance
            {
                SK = 123,
                Forename1 = "John",
                Surname = "Doe",
                Title = null,
                Email = null,
                Modified = null,
                DateOfAppointment = null,
                DateTermOfOfficeEndsEnded = null
            };

            var governanceRoleType = new GovernanceRoleType
            {
                SK = 1,
                Name = "Chair of Governors"
            };

            var establishment = new Domain.Establishment.Establishment
            {
                SK = 456,
                URN = 12345,
                UKPRN = "10012345"
            };

            var source = new AcademyGovernanceQueryModel(establishmentGovernance, governanceRoleType, establishment);

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(123);
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
            result.Email.Should().BeNull();
            result.DisplayName.Should().Be("John Doe");
            result.DisplayNameWithTitle.Should().Be(" John Doe"); // Note: null title results in leading space
            result.Roles.Should().BeEquivalentTo(new List<string> { "Chair of Governors" });
            result.UpdatedAt.Should().BeNull();
            result.DateOfAppointment.Should().BeNull();
            result.DateTermOfOfficeEndsEnded.Should().BeNull();

            // Test establishment mappings work even with null governance values
            result.URN.Should().Be(12345);
            result.UKPRN.Should().Be("10012345");
            result.Phone.Should().BeNull(); // Ignored field
        }

        [Fact]
        public void Map_WhenEstablishmentHasNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var establishmentGovernance = _fixture.Create<EducationEstablishmentGovernance>();
            var governanceRoleType = _fixture.Create<GovernanceRoleType>();
            var establishment = new Domain.Establishment.Establishment
            {
                SK = 789,
                URN = null,
                UKPRN = null
            };

            var source = new AcademyGovernanceQueryModel(establishmentGovernance, governanceRoleType, establishment);

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.URN.Should().BeNull();
            result.UKPRN.Should().BeNull();
        }

        [Fact]
        public void Map_WhenGovernanceRoleTypeNameIsNull_ShouldMapRolesWithNullValue()
        {
            // Arrange
            var establishmentGovernance = _fixture.Create<EducationEstablishmentGovernance>();
            var governanceRoleType = new GovernanceRoleType
            {
                SK = 1,
                Name = null
            };
            var establishment = _fixture.Create<Domain.Establishment.Establishment>();

            var source = new AcademyGovernanceQueryModel(establishmentGovernance, governanceRoleType, establishment);

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.Roles.Should().BeEquivalentTo(new List<string?> { null });
        }

        [Theory]
        [InlineData("Mr", "John", "Smith")]
        [InlineData("Dr", "Jane", "Doe")]
        [InlineData("", "Bob", "Johnson")]
        [InlineData(null, "Alice", "Brown")]
        public void Map_DisplayNameFormats_ShouldFormatNamesCorrectly(string? title, string firstName, string lastName)
        {
            // Arrange
            var establishmentGovernance = new EducationEstablishmentGovernance
            {
                SK = 1,
                Title = title,
                Forename1 = firstName,
                Surname = lastName
            };

            var governanceRoleType = _fixture.Create<GovernanceRoleType>();
            var establishment = _fixture.Create<Domain.Establishment.Establishment>();

            var source = new AcademyGovernanceQueryModel(establishmentGovernance, governanceRoleType, establishment);

            var expectedDisplayName = $"{firstName} {lastName}";
            var expectedDisplayNameWithTitle = $"{title} {firstName} {lastName}";

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert
            result.DisplayName.Should().Be(expectedDisplayName);
            result.DisplayNameWithTitle.Should().Be(expectedDisplayNameWithTitle);
        }

        [Fact]
        public void Map_EstablishmentProperties_ShouldMapCorrectly()
        {
            // Arrange
            var establishmentGovernance = _fixture.Create<EducationEstablishmentGovernance>();
            var governanceRoleType = _fixture.Create<GovernanceRoleType>();
            var establishment = new Domain.Establishment.Establishment
            {
                SK = 999,
                URN = 54321,
                UKPRN = "10054321"
            };

            var source = new AcademyGovernanceQueryModel(establishmentGovernance, governanceRoleType, establishment);

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.URN.Should().Be(54321);
            result.UKPRN.Should().Be("10054321");
        }

        [Fact]
        public void Map_PhoneProperty_ShouldBeIgnored()
        {
            // Arrange
            var source = _fixture.Create<AcademyGovernanceQueryModel>();

            // Act
            var result = _mapper.Map<AcademyGovernance>(source);

            // Assert - Phone should always be null since it's ignored in the mapping
            result.Should().NotBeNull();
            result.Phone.Should().BeNull();
        }

        [Fact]
        public void Profile_ShouldBeValidConfiguration()
        {
            // Arrange & Act & Assert
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AcademyWithGovernanceProfile>();
            });

            // This will throw if the configuration is invalid
            config.AssertConfigurationIsValid();
        }
    }
}