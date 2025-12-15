using AutoFixture;
using Dfe.PersonsApi.Domain.Constituencies;
using Dfe.PersonsApi.Domain.ValueObjects;

namespace Dfe.PersonsApi.Tests.Common.Customizations.Entities
{
    public class ConstituencyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var nameDetails = new NameDetails(
                    "Doe, John",
                    "John Doe",
                    "Mr. John Doe MP"
                );

                var memberId = fixture.Create<MemberId>();
                var memberContactDetails = new MemberContactDetails(
                    memberId,
                    1,
                    fixture.Create<string>(),
                    fixture.Create<string>()
                );

                return new Constituency
                {
                    Id = fixture.Create<ConstituencyId>(),
                    MemberId = memberId,
                    NameDetails = nameDetails,
                    ConstituencyName = fixture.Create<string>(),
                    PartyName = fixture.Create<string>(),
                    EndDate = DateOnly.FromDateTime(fixture.Create<DateTime>().Date),
                    LastRefresh = fixture.Create<DateTime>(),
                    MemberContactDetails = memberContactDetails
                };
            });
        }
    }
}
