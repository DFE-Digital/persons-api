using AutoMapper;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using MediatR;

namespace Dfe.PersonsApi.Application.Establishment.Queries.GetMemberOfParliamentBySchool
{
    public record GetMemberOfParliamentBySchoolQuery(int Urn) : IRequest<Result<MemberOfParliament?>>;

    public class GetMemberOfParliamentBySchoolQueryHandler(
        IEstablishmentRepository establishmentRepository,
        IConstituencyRepository constituencyRepository,
        IMapper mapper,
        ICacheService<IMemoryCacheType> cacheService)
        : IRequestHandler<GetMemberOfParliamentBySchoolQuery, Result<MemberOfParliament?>>
    {
        public async Task<Result<MemberOfParliament?>> Handle(GetMemberOfParliamentBySchoolQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"MPbySchool_{CacheKeyHelper.GenerateHashedCacheKey(request.Urn.ToString())}";

            return await cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var establishment = await establishmentRepository.GetEstablishmentByUrn(request.Urn.ToString(), cancellationToken);
                if (establishment == null)
                {
                    return Result<MemberOfParliament?>.Failure("School not found.");
                }

                var constituency = await constituencyRepository.GetMemberOfParliamentByConstituencyAsync(establishment.ParliamentaryConstituency!, cancellationToken);
                if (constituency == null)
                {
                    return Result<MemberOfParliament?>.Failure("Constituency not found for the given establishment.");
                }

                var mp = mapper.Map<MemberOfParliament?>(constituency);

                return Result<MemberOfParliament?>.Success(mp);

            }, nameof(GetMemberOfParliamentBySchoolQueryHandler));
        }
    }
}
