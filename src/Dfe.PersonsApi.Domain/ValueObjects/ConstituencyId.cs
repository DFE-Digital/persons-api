using Dfe.PersonsApi.Domain.Common;

namespace Dfe.PersonsApi.Domain.ValueObjects
{
    public record ConstituencyId(int Value) : IStronglyTypedId;
}
