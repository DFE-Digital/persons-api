using AutoFixture;
using Dfe.PersonsApi.Application.Common.Models;

namespace Dfe.PersonsApi.Testing.Common.Customizations.Models
{
    public class TrustGovernanceCustomization : ICustomization
    {
        public string? FirstName { get; set; } = "John";
        public string? LastName { get; set; } = "Doe";
        public string? Email { get; set; } = "john.doe@example.com";
        public string? DisplayName { get; set; } = "John Doe";
        public string? DisplayNameWithTitle { get; set; } = "Mr. John Doe";
        public List<string> Roles { get; set; } = ["MP"];
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string? DateOfAppointment { get; set; } = "10/01/2025";
        public string? DateTermOfOfficeEndsEnded { get; set; } = "11/02/2025";

        public void Customize(IFixture fixture)
        {
            fixture.Customize<TrustGovernance>(composer => composer
                .With(x => x.FirstName, FirstName)
                .With(x => x.LastName, LastName)
                .With(x => x.Email, Email)
                .With(x => x.DisplayName, DisplayName)
                .With(x => x.DisplayNameWithTitle, DisplayNameWithTitle)
                .With(x => x.Roles, Roles)
                .With(x => x.UpdatedAt, UpdatedAt)
                .With(x => x.DateOfAppointment, DateOfAppointment)
                .With(x => x.DateTermOfOfficeEndsEnded, DateTermOfOfficeEndsEnded)
             );
        }
    }
}
