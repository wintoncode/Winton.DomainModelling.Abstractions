// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using FluentAssertions;
using Xunit;
#pragma warning disable 618

namespace Winton.DomainModelling
{
    public class UnauthorizedExceptionTests
    {
        [Fact]
        private void ShouldBeObsolete()
        {
            typeof(UnauthorizedException)
                .Should()
                .BeDecoratedWith<ObsoleteAttribute>(a => a.Message == "Prefer to return results with an UnauthorizedError instead.");
        }

        public sealed class Message : UnauthorizedExceptionTests
        {
            [Theory]
            [InlineData("Test")]
            [InlineData("Foo")]
            private void ShouldSetMessageFromConstructor(string message)
            {
                var exception = new UnauthorizedException(message);

                exception.Message.Should().Be(message);
            }
        }
    }
}