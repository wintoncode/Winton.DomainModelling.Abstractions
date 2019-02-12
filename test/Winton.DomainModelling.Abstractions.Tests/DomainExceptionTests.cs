// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;
#pragma warning disable 618

namespace Winton.DomainModelling
{
    public class DomainExceptionTests
    {
        public sealed class Message : DomainExceptionTests
        {
            [Fact]
            private void ShouldBeObsolete()
            {
                typeof(DomainException)
                    .Should()
                    .BeDecoratedWith<ObsoleteAttribute>(a => a.Message == "Prefer to return results with an Error instead.");
            }

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