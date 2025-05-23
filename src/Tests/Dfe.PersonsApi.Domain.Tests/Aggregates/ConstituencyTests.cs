﻿using Dfe.PersonsApi.Domain.Constituencies;
using Dfe.PersonsApi.Domain.ValueObjects;
using Dfe.PersonsApi.Testing.Common.Customizations.Models;
using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using DfE.CoreLibs.Testing.AutoFixture.Customizations;

namespace Dfe.PersonsApi.Domain.Tests.Aggregates
{
    public class ConstituencyTests
    {
        [Theory]
        [CustomAutoData(typeof(MemberOfParliamentCustomization), typeof(DateOnlyCustomization))]
        public void Constructor_ShouldThrowArgumentNullException_WhenConstituencyIdIsNull(
            MemberId memberId,
            string constituencyName,
            NameDetails nameDetails,
            DateTime lastRefresh,
            DateOnly? endDate,
            MemberContactDetails memberContactDetails)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Constituency(null!, memberId, constituencyName, nameDetails, lastRefresh, endDate, memberContactDetails));

            Assert.Equal("constituencyId", exception.ParamName);
        }

        [Theory]
        [CustomAutoData(typeof(MemberOfParliamentCustomization), typeof(DateOnlyCustomization))]
        public void Constructor_ShouldThrowArgumentNullException_WhenMemberIdIsNull(
            ConstituencyId constituencyId,
            string constituencyName,
            NameDetails nameDetails,
            DateTime lastRefresh,
            DateOnly? endDate,
            MemberContactDetails memberContactDetails)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Constituency(constituencyId, null!, constituencyName, nameDetails, lastRefresh, endDate, memberContactDetails));

            Assert.Equal("memberId", exception.ParamName);
        }

        [Theory]
        [CustomAutoData(typeof(MemberOfParliamentCustomization), typeof(DateOnlyCustomization))]
        public void Constructor_ShouldThrowArgumentNullException_WhenNameDetailsIsNull(
            ConstituencyId constituencyId,
            MemberId memberId,
            string constituencyName,
            DateTime lastRefresh,
            DateOnly? endDate,
            MemberContactDetails memberContactDetails)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Constituency(constituencyId, memberId, constituencyName, null!, lastRefresh, endDate, memberContactDetails));

            Assert.Equal("nameDetails", exception.ParamName);
        }
    }
}
