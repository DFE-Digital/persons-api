using Dfe.PersonsApi.Domain.Establishment;

namespace Dfe.PersonsApi.Application.Common.Models
{
    public record TrustGovernanceQueryModel(
        Domain.Trust.Trust Trust,
        GovernanceRoleType GovernanceRoleType,
        Domain.Trust.TrustGovernance TrustGovernance
    );
}
