using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dfe.PersonsApi.Application.Common.Interfaces;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Utils.Enums;
using Dfe.PersonsApi.Utils.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Application.Trust.Queries.GetAllPersonsAssociatedWithTrustByTrnOrUkprn
{
    public record GetAllPersonsAssociatedWithTrustByTrnOrUkprnQuery(string Id) : IRequest<Result<List<TrustGovernance>?>>;

    public class GetAllPersonsAssociatedWithTrustByTrnOrUkprnQueryHandler(
        ITrustQueryService trustQueryService,
        IMapper mapper,
        ICacheService<IMemoryCacheType> cacheService)
        : IRequestHandler<GetAllPersonsAssociatedWithTrustByTrnOrUkprnQuery, Result<List<TrustGovernance>?>>
    {
        public async Task<Result<List<TrustGovernance>?>> Handle(GetAllPersonsAssociatedWithTrustByTrnOrUkprnQuery request, CancellationToken cancellationToken)
        {
            var idType = IdentifierHelper<string, TrustIdType>.DetermineIdType(request.Id, TrustIdValidator.GetTrustIdValidators());

            var groupId = idType == TrustIdType.Trn ? request.Id : null;
            var ukPrn = idType == TrustIdType.UkPrn ? request.Id : null;

            var cacheKey = $"PersonsAssociatedWithTrust_{CacheKeyHelper.GenerateHashedCacheKey(request.Id)}";

            return await cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var query = trustQueryService.GetTrustGovernanceByGroupIdOrUkprn(groupId, ukPrn);
                if (query == null)
                {
                    return Result<List<TrustGovernance>?>.Failure("Trust not found.");
                }

                return Result<List<TrustGovernance>?>.Success(await query
                        .ProjectTo<TrustGovernance>(mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken));

            }, nameof(GetAllPersonsAssociatedWithTrustByTrnOrUkprnQueryHandler));
        }
    }
}
