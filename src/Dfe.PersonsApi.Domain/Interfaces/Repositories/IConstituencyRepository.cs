using Dfe.PersonsApi.Domain.Constituencies;

namespace Dfe.PersonsApi.Domain.Interfaces.Repositories
{
    public interface IConstituencyRepository
    {
        Task<Constituency?> GetMemberOfParliamentByConstituencyAsync(string constituencyName, CancellationToken cancellationToken);
        IQueryable<Constituency> GetMembersOfParliamentByConstituenciesQueryable(List<string> constituencyNames);

    }
}
