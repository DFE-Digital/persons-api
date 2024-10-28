using Dfe.PersonsApi.Application.Common.Models;

namespace Dfe.PersonsApi.Application.Common.Interfaces
{
    public interface ITrustQueryService
    {
        IQueryable<TrustGovernanceQueryModel?>? GetTrustGovernanceByGroupIdOrUkprn(string? groupId, string? ukPrn);
    }
}
