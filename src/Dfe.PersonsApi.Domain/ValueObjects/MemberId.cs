using Dfe.PersonsApi.Domain.Common;

namespace Dfe.PersonsApi.Domain.ValueObjects
{
    public record MemberId(int Value) : IStronglyTypedId;
}
