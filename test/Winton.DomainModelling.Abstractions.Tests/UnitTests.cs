// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class UnitTests
    {
        public sealed class CompareTo : UnitTests
        {
            [Fact]
            private void ShouldAlwaysReturn0BecauseAllUnitsAreEqual()
            {
                int comparable = Unit.Value.CompareTo(Unit.Value);

                comparable.Should().Be(0);
            }

            [Fact]
            private void ShouldAlwaysReturnZeroWhenComparedToAnyObject()
            {
                IComparable value = Unit.Value;

                int comparable = value.CompareTo(new object());

                comparable.Should().Be(0);
            }
        }

        public sealed class EqualityOperator : UnitTests
        {
            [Fact]
            private void ShouldAlwaysReturnTrueBecauseThereIsOnlyOneUnitValue()
            {
                // ReSharper disable once EqualExpressionComparison
                bool equal = Unit.Value == Unit.Value;

                equal.Should().BeTrue();
            }
        }

        public sealed class EqualsMethod : UnitTests
        {
            public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    Unit.Value,
                    true
                },
                new[]
                {
                    new object(),
                    false
                },
                new object[]
                {
                    "Unit",
                    false
                }
            };

            [Fact]
            private void ShouldAlwaysReturnTrueBecauseThereIsOnlyOneUnitValue()
            {
                bool equal = Unit.Value.Equals(Unit.Value);

                equal.Should().BeTrue();
            }

            [Theory]
            [MemberData(nameof(TestCases))]
            private void ShouldReturnTrueIfOtherObjectIsOfTypeUnit(object obj, bool expected)
            {
                bool equal = Unit.Value.Equals(obj);

                equal.Should().Be(expected);
            }
        }

        public sealed class GetHashCodeMethod : UnitTests
        {
            [Fact]
            private void ShouldReturnZero()
            {
                Unit unit = Unit.Value;

                int hashCode = unit.GetHashCode();

                hashCode.Should().Be(0);
            }
        }

        public sealed class NotEqualOperator : UnitTests
        {
            [Fact]
            private void ShouldAlwaysReturnFalseBecauseAllUnitsAreEqual()
            {
                // ReSharper disable once EqualExpressionComparison
                bool notEqual = Unit.Value != Unit.Value;

                notEqual.Should().BeFalse();
            }
        }

        public sealed class Value : UnitTests
        {
            [Fact]
            private void ShouldReturnADefaultUnit()
            {
                Unit value = Unit.Value;

                value.Should().BeEquivalentTo(default(Unit));
            }
        }
    }
}