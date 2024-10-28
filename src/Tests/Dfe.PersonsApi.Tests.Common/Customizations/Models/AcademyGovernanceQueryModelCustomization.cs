using AutoFixture;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Establishment;

namespace Dfe.PersonsApi.Testing.Common.Customizations.Models
{
    public class AcademyGovernanceQueryModelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<AcademyGovernanceQueryModel>(composer => composer
                .FromFactory(() =>
                {
                    var establishmentGovernance = fixture.Create<EducationEstablishmentGovernance>();
                    var governanceRoleType = fixture.Create<GovernanceRoleType>();
                    var establishment = fixture.Create<Establishment>();

                    return new AcademyGovernanceQueryModel(establishmentGovernance, governanceRoleType, establishment);
                }));
        }
    }
}
