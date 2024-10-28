using Dfe.PersonsApi.Application.Common.Models;
using Dfe.PersonsApi.Domain.Establishment;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Infrastructure.Repositories
{
    public class EstablishmentRepository(MstrContext context) : IEstablishmentRepository
    {
        public async Task<Establishment?> GetEstablishmentByUrn(string urn, CancellationToken cancellationToken)
        {
            var queryResult = await BaseQuery().SingleOrDefaultAsync(r => r.Establishment.URN.ToString() == urn);

            if (queryResult == null)
            {
                return null;
            }

            var result = ToEstablishment(queryResult);

            return result;
        }

        private IQueryable<EstablishmentQueryResult> BaseQuery()
        {
            var result =
                 from establishment in context.Establishments
                 from ifdPipeline in context.IfdPipelines.Where(i => i.GeneralDetailsUrn == establishment.PK_GIAS_URN).DefaultIfEmpty()
                 from establishmentType in context.EstablishmentTypes.Where(e => e.SK == establishment.EstablishmentTypeId).DefaultIfEmpty()
                 from localAuthority in context.LocalAuthorities.Where(l => l.SK == establishment.LocalAuthorityId).DefaultIfEmpty()
                 select new EstablishmentQueryResult { Establishment = establishment, IfdPipeline = ifdPipeline, LocalAuthority = localAuthority, EstablishmentType = establishmentType };

            return result;
        }

        private static Establishment ToEstablishment(EstablishmentQueryResult queryResult)
        {
            var result = queryResult.Establishment;
            result.IfdPipeline = queryResult.IfdPipeline;
            result.LocalAuthority = queryResult.LocalAuthority;
            result.EstablishmentType = queryResult.EstablishmentType;

            return result;
        }

    }

    internal record EstablishmentQueryResult
    {
        public Establishment Establishment { get; set; }
        public IfdPipeline IfdPipeline { get; set; }
        public LocalAuthority LocalAuthority { get; set; }
        public EstablishmentType EstablishmentType { get; set; }
    }
}
