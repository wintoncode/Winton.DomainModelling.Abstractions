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