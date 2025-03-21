﻿using Dfe.PersonsApi.Domain.Common;
using Dfe.PersonsApi.Domain.ValueObjects;

namespace Dfe.PersonsApi.Domain.Constituencies
{
#pragma warning disable CS8618
    public class Constituency : IAggregateRoot
    {
        public ConstituencyId Id { get; }
        public MemberId MemberId { get; private set; }
        public string ConstituencyName { get; private set; }
        public NameDetails NameDetails { get; private set; }
        public DateTime LastRefresh { get; private set; }
        public DateOnly? EndDate { get; private set; }

        public virtual MemberContactDetails MemberContactDetails { get; private set; }
        public Constituency() { }

        public Constituency(
            ConstituencyId constituencyId,
            MemberId memberId,
            string constituencyName,
            NameDetails nameDetails,
            DateTime lastRefresh,
            DateOnly? endDate,
            MemberContactDetails memberContactDetails)
        {
            Id = constituencyId ?? throw new ArgumentNullException(nameof(constituencyId));
            MemberId = memberId ?? throw new ArgumentNullException(nameof(memberId));
            ConstituencyName = constituencyName;
            NameDetails = nameDetails ?? throw new ArgumentNullException(nameof(nameDetails));
            LastRefresh = lastRefresh;
            EndDate = endDate;
            MemberContactDetails = memberContactDetails;
        }
    }
#pragma warning restore CS8618
}
