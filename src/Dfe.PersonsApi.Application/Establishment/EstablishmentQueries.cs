using System.Diagnostics.CodeAnalysis;
using Dfe.PersonsApi.Domain.Interfaces.Repositories;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.PersonsApi.Application.Establishment
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentQueries(
        IEstablishmentRepository establishmentRepository,
        ITrustRepository trustRepository)
        : IEstablishmentQueries
    {
        private readonly ITrustRepository _trustRepository = trustRepository;

        public async Task<EstablishmentDto?> GetByUrn(string urn, CancellationToken cancellationToken)
        {
            var establishment = await establishmentRepository.GetEstablishmentByUrn(urn, cancellationToken);

            if (establishment == null)
            {
                return null;
            }

            return MapToEstablishmentDto(establishment);
        }

        private EstablishmentDto MapToEstablishmentDto(Domain.Establishment.Establishment establishment)
        {
            var result = new EstablishmentDtoBuilder()
                .WithBasicDetails(establishment)
                .WithLocalAuthority(establishment)
                .WithDiocese(establishment)
                .WithEstablishmentType(establishment)
                .WithGor(establishment)
                .WithPhaseOfEducation(establishment)
                .WithReligiousCharacter(establishment)
                .WithParliamentaryConstituency(establishment)
                .WithMISEstablishment(establishment)
                .WithAddress(establishment)
                .Build();

            return result;
        }
    }
}
