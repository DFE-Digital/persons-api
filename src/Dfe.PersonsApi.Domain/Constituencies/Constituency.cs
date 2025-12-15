using Dfe.PersonsApi.Domain.Common;
using Dfe.PersonsApi.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.PersonsApi.Domain.Constituencies
{
    [ExcludeFromCodeCoverage]
    public class Constituency : IAggregateRoot
    {
        public ConstituencyId Id { get; set; }
        public MemberId MemberId { get; set; }
        public string ConstituencyName { get; set; }        
        public string? PartyName { get; set; }
        public NameDetails NameDetails { get; set; }
        public DateTime LastRefresh { get; set; }
        public DateOnly? EndDate { get; set; }
        public virtual MemberContactDetails MemberContactDetails { get; set; }       
    }
}
