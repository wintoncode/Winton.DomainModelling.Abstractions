using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class UnauthorizedExceptionTests
    {
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