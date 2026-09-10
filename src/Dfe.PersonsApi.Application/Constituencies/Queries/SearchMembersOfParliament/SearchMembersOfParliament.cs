using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Application.Constituencies.Queries.SearchMembersOfParliament
{
    public record SearchMembersOfParliamentQuery(string SearchTerm) : IRequest<Result<List<MemberOfParliament>>>;

    public class SearchMembersOfParliamentQueryHandler(
        IConstituencyRepository constituencyRepository,
        IMapper mapper,
        ICacheService<IMemoryCacheType> cacheService)
        : IRequestHandler<SearchMembersOfParliamentQuery, Result<List<MemberOfParliament>>>
    {
        public async Task<Result<List<MemberOfParliament>>> Handle(SearchMembersOfParliamentQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm.Trim().ToLowerInvariant();

            var cacheKey = $"MemberOfParliamentSearch_{CacheKeyHelper.GenerateHashedCacheKey(searchTerm)}";

            return await cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var membersOfParliament = await constituencyRepository
                    .SearchMembersOfParliamentQueryable(searchTerm)
                    .ProjectTo<MemberOfParliament>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<MemberOfParliament>>.Success(membersOfParliament);

            }, nameof(SearchMembersOfParliamentQueryHandler), cancellationToken);
        }
    }
}
