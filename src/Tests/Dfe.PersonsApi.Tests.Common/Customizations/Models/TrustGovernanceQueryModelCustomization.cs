using AutoFixture;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Establishment;
using Dfe.PersonsApi.Domain.Trust;
using TrustGovernance = Dfe.PersonsApi.Domain.Trust.TrustGovernance;

namespace Dfe.PersonsApi.Testing.Common.Customizations.Models
{
    public class TrustGovernanceQueryModelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<TrustGovernanceQueryModel>(composer => composer
                .FromFactory(() =>
                {
                    var trust = fixture.Create<Trust>();
                    var governanceRoleType = fixture.Create<GovernanceRoleType>();
                    var trustGovernance = fixture.Create<TrustGovernance>();

                    return new TrustGovernanceQueryModel(trust, governanceRoleType, trustGovernance);
                }));
        }
    }
}
