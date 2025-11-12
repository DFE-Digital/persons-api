using AutoFixture;
using AutoMapper;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Application.MappingProfiles;
using Dfe.PersonsApi.Domain.Establishment;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Customizations;
using TrustGovernance = Dfe.PersonsApi.Domain.Trust.TrustGovernance;

namespace Dfe.PersonsApi.Application.Tests.MappingProfiles
{
    public class TrustWithGovernanceProfileTests
    {
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        public TrustWithGovernanceProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TrustWithGovernanceProfile>();
            });
            _mapper = config.CreateMapper();

            _fixture = new Fixture();
            _fixture.Customize(new OmitCircularReferenceCustomization());
            _fixture.Customize(new TrustGovernanceQueryModelCustomization());
        }

        [Theory]
        [CustomAutoData(typeof(OmitCircularReferenceCustomization), typeof(TrustGovernanceQueryModelCustomization))]
        public void Map_TrustGovernanceQueryModelToTrustGovernance_ShouldMapAllPropertiesCorrectly(
            TrustGovernanceQueryModel source,
            IFixture fixture)
        {
            // Arrange
            var expectedId = (int)source.TrustGovernance.SK;
            var expectedUKPRN = source.Trust.UKPRN;
            var expectedTRN = source.Trust.GroupID;
            var expectedFirstName = source.TrustGovernance.Forename1;
            var expectedLastName = source.TrustGovernance.Surname;
            var expectedEmail = source.TrustGovernance.Email;
            var expectedDisplayName = $"{source.TrustGovernance.Forename1} {source.TrustGovernance.Surname}";
            var expectedDisplayNameWithTitle = $"{source.TrustGovernance.Title} {source.TrustGovernance.Forename1} {source.TrustGovernance.Surname}";
            var expectedRoles = new List<string?> { source.GovernanceRoleType.Name };
            var expectedUpdatedAt = source.TrustGovernance.Modified;
            var expectedDateOfAppointment = source.TrustGovernance.DateOfAppointment;
            var expectedDateTermOfOfficeEndsEnded = source.TrustGovernance.DateTermOfOfficeEndsOrEnded;

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedId);
            result.UKPRN.Should().Be(expectedUKPRN);
            result.TRN.Should().Be(expectedTRN);
            result.FirstName.Should().Be(expectedFirstName);
            result.LastName.Should().Be(expectedLastName);
            result.Email.Should().Be(expectedEmail);
            result.DisplayName.Should().Be(expectedDisplayName);
            result.DisplayNameWithTitle.Should().Be(expectedDisplayNameWithTitle);
            result.Roles.Should().BeEquivalentTo(expectedRoles);
            result.UpdatedAt.Should().Be(expectedUpdatedAt);
            result.DateOfAppointment.Should().Be(expectedDateOfAppointment);
            result.DateTermOfOfficeEndsEnded.Should().Be(expectedDateTermOfOfficeEndsEnded);
        }

        [Fact]
        public void Map_WhenTrustGovernanceHasNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var trust = new Domain.Trust.Trust
            {
                SK = 456,
                UKPRN = "12345678",
                GroupID = "TR00123"
            };

            var trustGovernance = new TrustGovernance
            {
                SK = 789,
                Forename1 = "Jane",
                Surname = "Smith",
                Title = null,
                Email = null,
                Modified = null,
                DateOfAppointment = null,
                DateTermOfOfficeEndsOrEnded = null
            };

            var governanceRoleType = new GovernanceRoleType
            {
                SK = 2,
                Name = "Chief Executive Officer"
            };

            var source = new TrustGovernanceQueryModel(trust, governanceRoleType, trustGovernance);

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(789);
            result.UKPRN.Should().Be("12345678");
            result.TRN.Should().Be("TR00123");
            result.FirstName.Should().Be("Jane");
            result.LastName.Should().Be("Smith");
            result.Email.Should().BeNull();
            result.DisplayName.Should().Be("Jane Smith");
            result.DisplayNameWithTitle.Should().Be(" Jane Smith"); // Note: null title results in leading space
            result.Roles.Should().BeEquivalentTo(new List<string> { "Chief Executive Officer" });
            result.UpdatedAt.Should().BeNull();
            result.DateOfAppointment.Should().BeNull();
            result.DateTermOfOfficeEndsEnded.Should().BeNull();
        }

        [Fact]
        public void Map_WhenTrustHasNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var trust = new Domain.Trust.Trust
            {
                SK = 123,
                UKPRN = null,
                GroupID = null
            };

            var trustGovernance = _fixture.Create<TrustGovernance>();
            var governanceRoleType = _fixture.Create<GovernanceRoleType>();

            var source = new TrustGovernanceQueryModel(trust, governanceRoleType, trustGovernance);

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.UKPRN.Should().BeNull();
            result.TRN.Should().BeNull();
        }

        [Fact]
        public void Map_WhenGovernanceRoleTypeNameIsNull_ShouldMapRolesWithNullValue()
        {
            // Arrange
            var trust = _fixture.Create<Domain.Trust.Trust>();
            var trustGovernance = _fixture.Create<TrustGovernance>();
            var governanceRoleType = new GovernanceRoleType
            {
                SK = 3,
                Name = null
            };

            var source = new TrustGovernanceQueryModel(trust, governanceRoleType, trustGovernance);

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.Roles.Should().BeEquivalentTo(new List<string?> { null });
        }

        [Theory]
        [InlineData("Mrs", "Sarah", "Johnson")]
        [InlineData("Prof", "Michael", "Brown")]
        [InlineData("", "Emma", "Davis")]
        [InlineData(null, "David", "Wilson")]
        public void Map_DisplayNameFormats_ShouldFormatNamesCorrectly(string? title, string firstName, string lastName)
        {
            // Arrange
            var trust = _fixture.Create<Domain.Trust.Trust>();
            var trustGovernance = new TrustGovernance
            {
                SK = 1,
                Title = title,
                Forename1 = firstName,
                Surname = lastName
            };
            var governanceRoleType = _fixture.Create<GovernanceRoleType>();

            var source = new TrustGovernanceQueryModel(trust, governanceRoleType, trustGovernance);

            var expectedDisplayName = $"{firstName} {lastName}";
            var expectedDisplayNameWithTitle = $"{title} {firstName} {lastName}";

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.DisplayName.Should().Be(expectedDisplayName);
            result.DisplayNameWithTitle.Should().Be(expectedDisplayNameWithTitle);
        }

        [Fact]
        public void Profile_ShouldBeValidConfiguration()
        {
            // Arrange & Act & Assert
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TrustWithGovernanceProfile>();
            });

            // This will throw if the configuration is invalid
            config.AssertConfigurationIsValid();
        }

        [Theory]
        [CustomAutoData(typeof(OmitCircularReferenceCustomization), typeof(TrustGovernanceQueryModelCustomization))]
        public void Map_TrustSpecificProperties_ShouldMapCorrectly(
            TrustGovernanceQueryModel source,
            IFixture fixture)
        {
            // Arrange
            source.Trust.UKPRN = "87654321";
            source.Trust.GroupID = "TR98765";

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.UKPRN.Should().Be("87654321");
            result.TRN.Should().Be("TR98765");
        }

        [Theory]
        [CustomAutoData(typeof(OmitCircularReferenceCustomization), typeof(TrustGovernanceQueryModelCustomization))]
        public void Map_DateProperties_ShouldMapCorrectly(
            TrustGovernanceQueryModel source,
            IFixture fixture)
        {
            // Arrange
            var appointmentDate = "2023-01-15";
            var termEndDate = "2025-12-31";
            var modifiedDate = DateTime.Now.AddDays(-10);

            source.TrustGovernance.DateOfAppointment = appointmentDate;
            source.TrustGovernance.DateTermOfOfficeEndsOrEnded = termEndDate;
            source.TrustGovernance.Modified = modifiedDate;

            // Act
            var result = _mapper.Map<Application.Common.Models.TrustGovernance>(source);

            // Assert
            result.Should().NotBeNull();
            result.DateOfAppointment.Should().Be(appointmentDate);
            result.DateTermOfOfficeEndsEnded.Should().Be(termEndDate);
            result.UpdatedAt.Should().Be(modifiedDate);
        }
    }
}