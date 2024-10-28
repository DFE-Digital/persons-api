namespace Dfe.PersonsApi.Domain.Interfaces.Repositories
{
    public interface IEstablishmentRepository
    {
        Task<Domain.Establishment.Establishment?> GetEstablishmentByUrn(string urn, CancellationToken cancellationToken);

    }
}
