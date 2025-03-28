﻿using Dfe.PersonsApi.Application.Common.Interfaces;
using Dfe.PersonsApi.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Dfe.PersonsApi.Infrastructure.QueryServices
{
    internal class EstablishmentQueryService(MstrContext context) : IEstablishmentQueryService
    {
        public IQueryable<AcademyGovernanceQueryModel?>? GetPersonsAssociatedWithAcademyByUrn(int urn)
        {
            var establishmentExists = context.Establishments.AsNoTracking().Any(e => e.URN == urn);
            if (!establishmentExists)
            {
                return null;
            }

            var query = from ee in context.Establishments.AsNoTracking()
                join eeg in context.EducationEstablishmentGovernances.AsNoTracking()
                    on ee.SK equals eeg.EducationEstablishmentId
                join grt in context.GovernanceRoleTypes.AsNoTracking()
                    on eeg.GovernanceRoleTypeId equals grt.SK
                where ee.URN == urn
                select new AcademyGovernanceQueryModel(eeg, grt, ee);

            return query;
        }
    }
}
