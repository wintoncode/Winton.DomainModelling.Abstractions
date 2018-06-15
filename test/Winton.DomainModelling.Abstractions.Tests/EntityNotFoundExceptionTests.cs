using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class EntityNotFoundExceptionTests
    {
        [Fact]
        private void ShouldSetMessageBasedOnTypeOfEntity()
        {
            var exception = new EntityNotFoundException<TestEntity, int>();

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