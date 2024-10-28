namespace Dfe.PersonsApi.Application.Common.Models
{
    public class MemberOfParliament : Person
    {
        public required string ConstituencyName { get; set; }
    }
}
