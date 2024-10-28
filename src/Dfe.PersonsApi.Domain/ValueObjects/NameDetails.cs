using Dfe.PersonsApi.Domain.Common;

namespace Dfe.PersonsApi.Domain.ValueObjects
{
    public record NameDetails(string NameListAs, string NameDisplayAs, string NameFullTitle) : IStronglyTypedId;
}