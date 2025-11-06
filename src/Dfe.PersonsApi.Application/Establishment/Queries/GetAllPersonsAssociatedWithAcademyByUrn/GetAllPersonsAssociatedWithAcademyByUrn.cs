using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dfe.PersonsApi.Application.Common.Interfaces;
using Dfe.PersonsApi.Application.Common.Models;
using GovUK.Dfe.CoreLibs.Caching.Helpers;
using GovUK.Dfe.CoreLibs.Caching.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Application.Establishment.Queries.GetAllPersonsAssociatedWithAcademyByUrn
{
    public record GetAllPersonsAssociatedWithAcademyByUrnQuery(int Urn) : IRequest<Result<List<AcademyGovernance>?>>;

    public class GetAllPersonsAssociatedWithAcademyByUrnQueryHandler(
        IEstablishmentQueryService establishmentQueryService,
        IMapper mapper,
        ICacheService<IMemoryCacheType> cacheService)
        : IRequestHandler<GetAllPersonsAssociatedWithAcademyByUrnQuery, Result<List<AcademyGovernance>?>>
    {
        public async Task<Result<List<AcademyGovernance>?>> Handle(GetAllPersonsAssociatedWithAcademyByUrnQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"PersonsAssociatedWithAcademy_{CacheKeyHelper.GenerateHashedCacheKey(request.Urn.ToString())}";

            return await cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var query = establishmentQueryService.GetPersonsAssociatedWithAcademyByUrn(request.Urn);

                if (query == null)
                {
                    return Result<List<AcademyGovernance>?>.Failure("Academy not found.");
                }

                var result = Result<List<AcademyGovernance>?>.Success(await query
                    .ProjectTo<AcademyGovernance>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken));

                return result;
            }, nameof(GetAllPersonsAssociatedWithAcademyByUrnQueryHandler));
        }
    }
}
