using Dfe.PersonsApi.Utils.Enums;
using Dfe.PersonsApi.Utils.Helpers;

namespace Dfe.PersonsApi.Application.Tests.Helpers
{
    public class IdentifierHelperTests
    {
        [Theory]
        [InlineData("TR12345", TrustIdType.Trn)]   // Valid TRN
        [InlineData("12345678", TrustIdType.UkPrn)] // Valid UKPRN
        [InlineData("INVALID", TrustIdType.Invalid)] // Invalid ID
        public void DetermineIdType_ShouldReturnCorrectType_WhenIdIsProvided(string id, TrustIdType expectedType)
        {
            // Arrange
            var validators = TrustIdValidator.GetTrustIdValidators();

            // Act
            var result = IdentifierHelper<string, TrustIdType>.DetermineIdType(id, validators);

            // Assert
            Assert.Equal(expectedType, result);
        }
    }
}
