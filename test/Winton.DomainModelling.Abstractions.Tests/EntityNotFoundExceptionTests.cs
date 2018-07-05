// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class EntityNotFoundExceptionTests
    {
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