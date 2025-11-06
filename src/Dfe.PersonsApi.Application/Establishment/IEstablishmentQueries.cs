using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.PersonsApi.Application.Establishment
{
    public interface IEstablishmentQueries
    {
        Task<EstablishmentDto?> GetByUrn(string urn, CancellationToken cancellationToken);
    }
}
