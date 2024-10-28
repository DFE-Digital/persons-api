using Dfe.PersonsApi.Domain.Establishment;

namespace Dfe.PersonsApi.Application.Common.Models
{
    public record AcademyGovernanceQueryModel(
        EducationEstablishmentGovernance EducationEstablishmentGovernance,
        GovernanceRoleType GovernanceRoleType,
        Domain.Establishment.Establishment Establishment
    );
}
