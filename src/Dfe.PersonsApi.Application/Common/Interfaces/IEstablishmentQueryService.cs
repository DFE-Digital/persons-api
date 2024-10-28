using Dfe.PersonsApi.Application.Common.Models;

namespace Dfe.PersonsApi.Application.Common.Interfaces
{
    public interface IEstablishmentQueryService
    {
        IQueryable<AcademyGovernanceQueryModel?>? GetPersonsAssociatedWithAcademyByUrn(int urn);
    }
}
