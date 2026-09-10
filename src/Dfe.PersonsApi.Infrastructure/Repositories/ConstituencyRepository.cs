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
            var pattern = $"%{EscapeLikeWildcards(searchTerm.Trim().ToLower())}%";

            return context.Constituencies
                .AsNoTracking()
                .Include(c => c.MemberContactDetails)
                .Where(c => c.MemberContactDetails.TypeId == 1
                            && !c.EndDate.HasValue
                            && (EF.Functions.Like(c.ConstituencyName.ToLower(), pattern, LikeEscapeCharacter)
                                || EF.Functions.Like(c.NameDetails.NameDisplayAs.ToLower(), pattern, LikeEscapeCharacter)
                                || EF.Functions.Like(c.NameDetails.NameListAs.ToLower(), pattern, LikeEscapeCharacter)))
                .OrderBy(c => c.ConstituencyName);
        }

        private const string LikeEscapeCharacter = "\\";

        // Wildcards typed by the caller are matched literally rather than treated as LIKE patterns.
        private static string EscapeLikeWildcards(string searchTerm) =>
            searchTerm
                .Replace(LikeEscapeCharacter, LikeEscapeCharacter + LikeEscapeCharacter, StringComparison.Ordinal)
                .Replace("%", $"{LikeEscapeCharacter}%", StringComparison.Ordinal)
                .Replace("_", $"{LikeEscapeCharacter}_", StringComparison.Ordinal);
    }
}
