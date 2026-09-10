using Dfe.PersonsApi.Domain.Constituencies;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Infrastructure.Repositories
{
    public class ConstituencyRepository(MopContext context) : IConstituencyRepository
    {
        public async Task<Constituency?> GetMemberOfParliamentByConstituencyAsync(string constituencyName, CancellationToken cancellationToken)
        {
            return await context.Constituencies
                .AsNoTracking()
                .Include(c => c.MemberContactDetails)
                .Where(c => c.ConstituencyName == constituencyName
                            && c.MemberContactDetails.TypeId == 1
                            && !c.EndDate.HasValue)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<Constituency> GetMembersOfParliamentByConstituenciesQueryable(List<string> constituencyNames)
        {
            return context.Constituencies
                .AsNoTracking()
                .Include(c => c.MemberContactDetails) 
                .Where(c => constituencyNames.Contains(c.ConstituencyName)
                            && c.MemberContactDetails.TypeId == 1
                            && !c.EndDate.HasValue);
        }

        public IQueryable<Constituency> SearchMembersOfParliamentQueryable(string searchTerm)
        {
            // Lowered on both sides so matching stays case insensitive regardless of database collation.
            var normalisedSearchTerm = searchTerm.Trim().ToLower();

            return context.Constituencies
                .AsNoTracking()
                .Include(c => c.MemberContactDetails)
                .Where(c => c.MemberContactDetails.TypeId == 1
                            && !c.EndDate.HasValue
                            && (c.ConstituencyName.ToLower().Contains(normalisedSearchTerm)
                                || c.NameDetails.NameDisplayAs.ToLower().Contains(normalisedSearchTerm)
                                || c.NameDetails.NameListAs.ToLower().Contains(normalisedSearchTerm)))
                .OrderBy(c => c.ConstituencyName);
        }
    }
}
