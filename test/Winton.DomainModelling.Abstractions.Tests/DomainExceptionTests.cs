// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class DomainExceptionTests
    {
        public sealed class Message : DomainExceptionTests
        {
            [Theory]
            [InlineData("Test")]
            [InlineData("Foo")]
            private void ShouldSetMessageFromConstructor(string message)
            {
                var exception = new DomainException(message);

                exception.Message.Should().Be(message);
            }
        }
    }
}