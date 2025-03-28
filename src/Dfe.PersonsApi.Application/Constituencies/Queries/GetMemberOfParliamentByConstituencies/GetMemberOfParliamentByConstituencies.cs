﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using DfE.CoreLibs.Caching.Helpers;
using DfE.CoreLibs.Caching.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Application.Constituencies.Queries.GetMemberOfParliamentByConstituencies
{
    public record GetMembersOfParliamentByConstituenciesQuery(List<string> ConstituencyNames) : IRequest<Result<List<MemberOfParliament>>>;

    public class GetMembersOfParliamentByConstituenciesQueryHandler(
        IConstituencyRepository constituencyRepository,
        IMapper mapper,
        ICacheService<IMemoryCacheType> cacheService)
        : IRequestHandler<GetMembersOfParliamentByConstituenciesQuery, Result<List<MemberOfParliament>>>
    {
        public async Task<Result<List<MemberOfParliament>>> Handle(GetMembersOfParliamentByConstituenciesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"MemberOfParliament_{CacheKeyHelper.GenerateHashedCacheKey(request.ConstituencyNames)}";

            return await cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var constituenciesQuery = constituencyRepository
                    .GetMembersOfParliamentByConstituenciesQueryable(request.ConstituencyNames);

                var membersOfParliament = await constituenciesQuery
                    .ProjectTo<MemberOfParliament>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return Result<List<MemberOfParliament>>.Success(membersOfParliament);

            }, nameof(GetMembersOfParliamentByConstituenciesQueryHandler));
        }
    }

}
