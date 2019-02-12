// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using FluentAssertions;
using Xunit;
#pragma warning disable 618

namespace Winton.DomainModelling
{
    public class EntityNotFoundExceptionTests
    {
        [Fact]
        private void ShouldBeObsolete()
        {
            typeof(EntityNotFoundException)
                .Should()
                .BeDecoratedWith<ObsoleteAttribute>(a => a.Message == "Prefer to return results with a NotFoundError instead.");
        }

        public sealed class Message : EntityNotFoundExceptionTests
        {
            [Fact]
            private void ShouldSetMessageBasedOnTypeOfEntity()
            {
                EntityNotFoundException exception = EntityNotFoundException.Create<TestEntity, int>();

                exception.Message.Should().Be("The specified TestEntity could not be found.");
            }

            // ReSharper disable once ClassNeverInstantiated.Local
            private sealed class TestEntity : Entity<int>
            {
                public TestEntity(int id)
                    : base(id)
                {
                }
            }
        }
    }
}