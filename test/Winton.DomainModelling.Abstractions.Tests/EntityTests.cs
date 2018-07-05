// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class EntityTests
    {
        public sealed class Equality : EntityTests
        {
            [Fact]
            private void ShouldBeFalseWhenIdHasDefaultValue()
            {
                var entityA1 = new EntityA(default(int));
                var entityA2 = new EntityA(default(int));

                bool equal = entityA1.Equals(entityA2);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeFalseWhenIdsAreDifferentForDifferentReferences()
            {
                var entityA1 = new EntityA(1);
                var entityA2 = new EntityA(2);

                bool equal = entityA1.Equals(entityA2);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeFalseWhenOtherIsDifferentTypeOfEntity()
            {
                var entityA = new EntityA(1);
                var entityB = new EntityB(1);

                bool equal = entityA.Equals(entityB);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeFalseWhenOtherIsNull()
            {
                var entityA1 = new EntityA(1);

                bool equal = entityA1.Equals(null);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeTrueWhenIdsAreSameForDifferentReferences()
            {
                var entityA1 = new EntityA(1);
                var entityA2 = new EntityA(1);

                bool equal = entityA1.Equals(entityA2);

                equal.Should().BeTrue();
            }

            [Fact]
            private void ShouldBeTrueWhenReferencesAreSame()
            {
                var entityA1 = new EntityA(1);

                bool equal = entityA1.Equals(entityA1);

                equal.Should().BeTrue();
            }
        }

        public sealed class ObjectEquality : EntityTests
        {
            [Fact]
            private void ShouldBeFalseWhenIdsAreDifferentForDifferentReferences()
            {
                var entityA1 = new EntityA(1);
                var entityA2 = new EntityA(2);

                bool equal = entityA1.Equals((object)entityA2);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeFalseWhenOtherIsDifferentTypeOfEntity()
            {
                var entityA = new EntityA(1);
                var entityB = new EntityB(1);

                // ReSharper disable once SuspiciousTypeConversion.Global
                bool equal = entityA.Equals((object)entityB);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeFalseWhenOtherIsNull()
            {
                var entityA1 = new EntityA(1);

                bool equal = entityA1.Equals((object)null);

                equal.Should().BeFalse();
            }

            [Fact]
            private void ShouldBeTrueWhenIdsAreSameForDifferentReferences()
            {
                var entityA1 = new EntityA(1);
                var entityA2 = new EntityA(1);

                bool equal = entityA1.Equals((object)entityA2);

                equal.Should().BeTrue();
            }

            [Fact]
            private void ShouldBeTrueWhenReferencesAreSame()
            {
                var entityA1 = new EntityA(1);

                bool equal = entityA1.Equals((object)entityA1);

                equal.Should().BeTrue();
            }
        }

        private sealed class EntityA : Entity<int>
        {
            public EntityA(int id)
                : base(id)
            {
            }
        }

        private sealed class EntityB : Entity<int>
        {
            public EntityB(int id)
                : base(id)
            {
            }
        }
    }
}