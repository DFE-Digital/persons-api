﻿using Dfe.PersonsApi.Domain.Constituencies;
using Dfe.PersonsApi.Domain.ValueObjects;
using DfE.CoreLibs.Testing.AutoFixture.Attributes;

namespace Dfe.PersonsApi.Domain.Tests.Aggregates
{
    public class MemberContactDetailsTests
    {
        [Theory]
        [CustomAutoData]
        public void Constructor_ShouldThrowArgumentNullException_WhenMemberIdIsNull(
            int typeId,
            string? email,
            string? phone)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new MemberContactDetails(null!, typeId, email, phone));

            Assert.Equal("memberId", exception.ParamName);
        }

        [Theory]
        [CustomAutoData]
        public void Constructor_ShouldThrowArgumentException_WhenTypeIdIsNotPositive(
            MemberId memberId,
            string? email,
            string? phone)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new MemberContactDetails(memberId, -1, email, phone));

            Assert.Contains("TypeId must be positive", exception.Message);
            Assert.Equal("typeId", exception.ParamName);
        }

        [Theory]
        [CustomAutoData]
        public void Constructor_ShouldThrowArgumentException_WhenTypeIdIsZero(
            MemberId memberId,
            string? email,
            string? phone)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new MemberContactDetails(memberId, 0, email, phone));

            Assert.Contains("TypeId must be positive", exception.Message);
            Assert.Equal("typeId", exception.ParamName);
        }
    }
}
